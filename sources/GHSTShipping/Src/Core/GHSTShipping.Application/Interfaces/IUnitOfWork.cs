using System.Threading.Tasks;

namespace GHSTShipping.Application.Interfaces
{
    public interface IUnitOfWork
    {
        Task<bool> SaveChangesAsync();
    }
}
