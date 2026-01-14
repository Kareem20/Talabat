using Microsoft.EntityFrameworkCore;
using Talabat.Core.Models;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly ProductContext _dbContext;

        public GenericRepository(ProductContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllWithSpecificationAsync(ISpecification<T> specification)
        {
            return await ApplySpecification(specification).ToListAsync();
        }

        public async Task<T> GetByIdWithSpecificationAsync(ISpecification<T> specification)
        {
            return await ApplySpecification(specification).FirstOrDefaultAsync();
        }

        public async Task<int> GetCountWithSpecificationAsync(ISpecification<T>? specification = null)
        {
            return await ApplySpecification(specification).CountAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> specification)
        {
            return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>(), specification);
        }

        public async Task AddAsync(T item)
        {
            await _dbContext.Set<T>().AddAsync(item);
        }
        public void Update(T item)
        {
            _dbContext.Set<T>().Update(item);
        }

        public void Delete(T item)
        {
            _dbContext.Set<T>().Remove(item);
        }
    }
}
