using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using PersonsHandBook.Domain.Models;
using PersonsHandBook.Domain.Models.Entity;
using PersonsHandBook.Domain.Models.Input;
using PersonsHandBook.Infrastructure;
using PersonsHandBook.Models;
using PersonsHandBook.Resources.Locallizer;
using PersonsHandBook.Services;

namespace PersonsHandBook.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly HandBookManager _handBookManager;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;

        public ContactsController(ILoggerFactory loggerFactory, IMapper mapper, HandBookManager handBookManager, IStringLocalizer<SharedResources> stringLocalizer)
        {
            _logger = loggerFactory.CreateLogger<PersonsController>();
            _mapper = mapper;
            _handBookManager = handBookManager;
           
            _stringLocalizer = stringLocalizer;
        }

        [HttpPost("GetContacts")]
        public IActionResult GetContacts(GetContacts model)
        {
            var contacts =  _handBookManager.GetPersonsContacts(model);
            
            if (contacts != null && contacts.Any())
                return Ok(contacts);
            
            return BadRequest(_stringLocalizer[Constants.ContactsNotFound, model.PersonId].Value);
        }


        [HttpPost("AddNewContact")]
        public async Task<IActionResult> AddNewContact(AddContactModel contactModel)
        {
            var model = _mapper.Map<Contact>(contactModel);

            var result = await _handBookManager.AddNewContact(model);

            return result ? NoContent() : BadRequest();
        }


        [HttpPatch("UpdateContact")]
        public async Task<IActionResult> UpdateContact(UpdateContactModel model)
        {
            var contact = _mapper.Map<Contact>(model);

            var result = await _handBookManager.UpdateContact(contact);

            if (result.Success)
            {
                return NoContent();
            }

            return BadRequest(_stringLocalizer[Constants.ErrorUpdateContact].Value);
        }


        [HttpGet("GetReport/{personId}")]
        public async Task<IActionResult> GetContactsReport(int personId)
        {
            var model = await _handBookManager.GetPersonContactsReport(personId);

            if (model.Success == false)
            {
                _logger.LogError($"Cant Get Report For {personId}");
                return BadRequest(_stringLocalizer[Constants.CantGetReport, personId].Value);
            }
                

            return Ok(model.Data);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteContact(DeleteContact model)
        {
            var result = await _handBookManager.DeleteContact(model.ContactId);

            if (result.Success == false)
                return BadRequest(_stringLocalizer[result.ErrorMessage].Value);

            return NoContent();

        }

    }
}
