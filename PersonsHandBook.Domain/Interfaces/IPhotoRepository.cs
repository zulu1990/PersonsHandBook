using System;
using PersonsHandBook.Domain.Models.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PersonsHandBook.Domain.Interfaces
{
    public interface IPhotoRepository : IRepository<Photo>
    {
        Task AddPhoto(Photo photo);
        Photo GetPhoto(int? personId, int? contactId);
        IQueryable<Photo> GetPhotosBy(Expression<Func<Photo, bool>> func);

        void DeletePhoto(Photo photo);

    }
}
