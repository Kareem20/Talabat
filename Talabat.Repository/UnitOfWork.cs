using Talabat.Core;
using Talabat.Core.Models;
using Talabat.Core.Repositories;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ProductContext _dbContext;
        private readonly Dictionary<Type, object> _repositories = new();

        public UnitOfWork(ProductContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IGenericRepository<T> Repository<T>() where T : BaseEntity
        {
            var type = typeof(T);
            if (!_repositories.ContainsKey(type))
            {
                _repositories[type] = CreateRepository<T>();
            }
            return (IGenericRepository<T>)_repositories[type];
        }
        protected virtual IGenericRepository<T> CreateRepository<T>() where T : BaseEntity
        {
            return new GenericRepository<T>(_dbContext);
        }
        public async Task<int> CompleteAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
        public ValueTask DisposeAsync()
        {
            return _dbContext.DisposeAsync();
        }

    }
}
