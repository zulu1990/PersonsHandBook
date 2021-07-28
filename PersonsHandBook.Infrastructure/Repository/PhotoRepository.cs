using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PersonsHandBook.Domain.Interfaces;
using PersonsHandBook.Domain.Models;
using PersonsHandBook.Domain.Models.Entity;

namespace PersonsHandBook.Infrastructure.Repository
{
    public class PhotoRepository : Repository<Photo>, IPhotoRepository
    {
        public PhotoRepository(DbFactory factory, ILoggerFactory loggerFactory) : base(factory, loggerFactory)
        {
        }


        public async Task AddPhoto(Photo photo)
        {
            await Add(photo);
        }

        public void DeletePhoto(Photo photo)
        {
            try
            {
                Delete(photo);

                File.Delete(photo.Url);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error Delete Photo {photo.Id}");
            }
           
        }

        public Photo GetPhoto(int? personId, int? contactId)
        {
            var list = GetPhotosBy(x => x.PersonId == personId && x.ContactId == contactId);

            return list.FirstOrDefault();
        }

        public IQueryable<Photo> GetPhotosBy(Expression<Func<Photo, bool>> func)
        {
            return List(func);
        }




    }
}
