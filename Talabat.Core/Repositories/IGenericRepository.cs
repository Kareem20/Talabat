using Talabat.Core.Models;
using Talabat.Core.Specifications;

namespace Talabat.Core.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(int Id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetByIdWithSpecificationAsync(ISpecification<T>? specification = null);
        Task<IReadOnlyList<T>> GetAllWithSpecificationAsync(ISpecification<T>? specification = null);
        Task<int> GetCountWithSpecificationAsync(ISpecification<T>? specification = null);

        Task AddAsync(T item);
        void Update(T item);
        void Delete(T item);
    }
}
