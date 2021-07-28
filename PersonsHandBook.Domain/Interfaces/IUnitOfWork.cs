using System.Threading.Tasks;

namespace PersonsHandBook.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        Task<bool> CommitAsync();
    }
}
