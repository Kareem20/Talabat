using System.Linq.Expressions;
using Talabat.Core.Models;

namespace Talabat.Core.Specifications
{
    public interface ISpecification<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Criteria { get; }
        public IReadOnlyList<Expression<Func<T, object>>> Includes { get; }
        public IReadOnlyList<Expression<Func<T, object>>> OrderBys { get; }
        public IReadOnlyList<Expression<Func<T, object>>> OrderByDescendings { get; }
        public int? Take { get; }
        public int? Skip { get; }

    }
}
