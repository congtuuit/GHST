using System.Threading;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Interfaces
{
    public interface ICronJobService
    {
        Task DoWorkAsync(CancellationToken cancellationToken);
    }
}
