using Microsoft.Extensions.Logging;
using PersonsHandBook.Domain.Interfaces;
using PersonsHandBook.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using PersonsHandBook.Domain.Models.Entity;
using PersonsHandBook.Domain.Models.Enum;
using PersonsHandBook.Domain.Models.Input;
using PersonsHandBook.Domain.Models.Output;

namespace PersonsHandBook.Infrastructure
{
    public class HandBookManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPersonRepository _personRepository;
        private readonly IPhotoRepository _photoRepository;
        private readonly IContactRepository _contactRepository;
        private readonly ILogger _logger;

        public HandBookManager(IUnitOfWork unitOfWork, IPersonRepository personRepository, IPhotoRepository photoRepository, IContactRepository contactRepository, ILoggerFactory loggerFactory)
        {
            _unitOfWork = unitOfWork;

            _personRepository = personRepository;
            _photoRepository = photoRepository;
            _contactRepository = contactRepository;

            _logger = loggerFactory.CreateLogger<HandBookManager>();
        }

        public async Task<Result<Person>> GetPersonById(int personId)
        {
            var person = await _personRepository.GetPersonListBy(x => x.Id == personId).Include(c => c.Contacts).Include(p => p.Photo).FirstOrDefaultAsync();

            if (person == null)
                return new Result<Person> { Success = false, ErrorMessage = Constants.PersonNotFound };

            foreach (var contact in person.Contacts)
            {
                contact.Photo = await _photoRepository.GetPhotosBy(x => x.ContactId == contact.Id).FirstOrDefaultAsync();
            }


            return new Result<Person> { Success = true, Data = person };
        }

        public IEnumerable<Person> DetailSearchPerson(DetailSearchPerson detailSearch)
        {
            var result = _personRepository.GetPersonListBy(p =>
            p.DateOfBirth == detailSearch.DateOfBirth ||
            p.LastName == detailSearch.LastName ||
            p.Name == detailSearch.Name ||
            p.Sex == detailSearch.Sex ||
            p.PersonalNumber == detailSearch.PersonalNumber).Include(p => p.Contacts).Include(p => p.Photo);

            return GetPagedResult<Person>(result, detailSearch.PageSize, detailSearch.PageNumber);
        }

        public async Task<Result> EditPerson(Person person)
        {
            var foundPerson = await _personRepository.GetById(person.Id);
            if (foundPerson == null)
                return new Result { Success = false, ErrorMessage = Constants.PersonNotFound };

            _personRepository.Update(person);

            if (await _unitOfWork.CommitAsync())
                return new Result { Success = true };

            return new Result { Success = false, ErrorMessage = Constants.ErrorEditPerson };
        }

        public Photo GetPhoto(int? personId, int? contactId)
        {
            return _photoRepository.GetPhoto(personId, contactId);
        }

        public async Task<Result> UploadPhoto(Photo photo, IFormFile file)
        {
            var result = new Result() { Success = false };

            try
            {
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), Constants.PhotosFolderPath);

                var generatedName = Guid.NewGuid().ToString();

                var extension = Path.GetExtension(string.Concat(folderPath, "//", file.FileName));

                var newGeneratedName = string.Concat(generatedName, extension);


                await SavePhoto(file, newGeneratedName);


                photo.Url = string.Concat(Constants.PhotosFolderPath, "//", newGeneratedName);

                var oldPhoto = GetPhoto(photo.PersonId, photo.ContactId);

                if (oldPhoto != null)
                {
                    _photoRepository.DeletePhoto(oldPhoto);
                }


                await _photoRepository.AddPhoto(photo);

                result.Success = await _unitOfWork.CommitAsync();

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"UploadPhoto Person: {photo.PersonId}, contact?={photo.ContactId}");

                return result;
            }

        }

        public async Task<Result> DeletePhoto(int id)
        {
            try
            {
                var photo = await _photoRepository.GetById(id);
                if (photo == null)
                    return new Result { Success = false, ErrorMessage = Constants.PhotoNotFound } ;

                 _photoRepository.DeletePhoto(photo);

                if (await _unitOfWork.CommitAsync())
                    return new Result { Success = true };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"HandBookManager.DeletePhoto(int {id})");
            }

            return new Result { Success = false, ErrorMessage = Constants.ErrorRemovePhoto };
        }

        public async Task<Result> DeleteContact(int contactId)
        {
            var result = new Result();

            var contact = await _contactRepository.GetContactListBy(x => x.Id == contactId).Include(p => p.Photo).FirstOrDefaultAsync();

            if (contact == null)
            {
                result.Success = false;
                result.ErrorMessage = Constants.ContactNotFound;
                return result;
            }

            _contactRepository.Delete(contact);

            if(contact.Photo != null)
                _photoRepository.DeletePhoto(contact.Photo);

            result.Success = await _unitOfWork.CommitAsync();

            if (result.Success == false)
                result.ErrorMessage = Constants.CouldntDeleteContact;

            return result;
        }

        public async Task<Result> UpdateContact(Contact contact)
        {
            var result = new Result()
            {
                Success = false
            };

            try
            {
                _contactRepository.UpdateContact(contact);
                result.Success = await _unitOfWork.CommitAsync();

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception Update Contact");
                return result;
            }

        }

        public async Task<Result<PersonsContactReport>> GetPersonContactsReport(int personId)
        {
            var contacts = _contactRepository.GetContactListBy(c => c.PersonId == personId);

            if (contacts.Any() == false)
            {
                return new Result<PersonsContactReport>()
                {
                    Success = false,
                    Data = null,
                    ErrorMessage = "Cant Find Contacts With PersonId"
                };
            }

            return new Result<PersonsContactReport>()
            {
                Success = true,
                Data = new PersonsContactReport
                {
                    Colleges = await contacts.CountAsync(x => x.RelationType == Relation.College),
                    Ect = await contacts.CountAsync(x => x.RelationType == Relation.Ect),
                    Friends = await contacts.CountAsync(x => x.RelationType == Relation.Friend),
                    Relatives = await contacts.CountAsync(x => x.RelationType == Relation.Relative),
                }
            };
        }

        public async Task<Result> RemovePerson(int personId)
        {
            var result = _personRepository.RemovePerson(personId);

            if (result.Success == false)
                return new Result { ErrorMessage = result.ErrorMessage, Success = result.Success };

            foreach(var contact in result.Data.Contacts)
            {
                _contactRepository.Delete(contact);
                if (contact.Photo != null)
                    _photoRepository.DeletePhoto(contact.Photo);
            }

            _photoRepository.DeletePhoto(result.Data.Photo);

            if (await _unitOfWork.CommitAsync())
                return new Result { Success = true };


            return new Result
            {
                Success = false,
                ErrorMessage = Constants.ErrorRemovePerson
            };
        }

        public async Task<bool> AddNewContact(Contact model)
        {
            try
            {
                await _contactRepository.AddContact(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception AddNewContact");
                return false;
            }

            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task<Result> CreateNewPerson(Person newPerson)
        {
            try
            {
                await _personRepository.AddNewPerson(newPerson);

                if(await _unitOfWork.CommitAsync());
                    return new Result { Success = true };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception CreateNewPerson");
            }

            return new Result { Success = false, ErrorMessage = Constants.CreatePeronsError };

        }



        public IEnumerable<Person> FindPersonByParam(SearchParams param)
        {
            var result = _personRepository.GetPersonListBy(x => x.Name.Contains(param.Name) && x.LastName.Contains(param.LastName)).Include(p=>p.Contacts).Include(p=>p.Photo);

            return GetPagedResult(result, param.PageSize, param.PageNumber);
        }

        public IEnumerable<Contact> GetPersonsContacts(GetContacts model)
        {
            var result = _contactRepository.GetContactListBy(x => x.PersonId == model.PersonId);

            return GetPagedResult(result, model.PageSize, model.PageNumber);
        }


        private IEnumerable<T> GetPagedResult<T>(IQueryable<T> data, int pageSize, int pageNumber)
        {
            return data.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }


        private async Task SavePhoto(IFormFile photo, string generatedName)
        {
            var address = Path.Combine(Directory.GetCurrentDirectory(), Constants.PhotosFolderPath);

            if (!Directory.Exists(address))
                Directory.CreateDirectory(address);

            await using var fileContentStream = new MemoryStream();
            await photo.CopyToAsync(fileContentStream);
            await File.WriteAllBytesAsync(Path.Combine(address, generatedName), fileContentStream.ToArray());
        }

    }
}
