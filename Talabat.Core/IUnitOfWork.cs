using Talabat.Core.Models;
using Talabat.Core.Repositories;

namespace Talabat.Core
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IGenericRepository<T> Repository<T>() where T : BaseEntity;
        Task<int> CompleteAsync();
    }
}
