using Microsoft.EntityFrameworkCore;
using Talabat.Core.Models;
using Talabat.Core.Specifications;

namespace Talabat.Repository
{
    public class SpecificationEvaluator<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery,
                                            ISpecification<T> spec)
        {
            var query = inputQuery;
            if (spec == null)
                return query;
            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }
            foreach (var include in spec.Includes)
            {
                query = query.Include(include);
            }
            if (spec.OrderBys is not null && spec.OrderBys.Any())
            {
                foreach (var orderBy in spec.OrderBys)
                {
                    query = query.OrderBy(orderBy);
                }
            }
            if (spec.OrderByDescendings != null && spec.OrderByDescendings.Any())
            {
                foreach (var orderByDescending in spec.OrderByDescendings)
                {
                    query = query.OrderByDescending(orderByDescending);
                }
            }
            if (spec.Skip.HasValue)
            {
                query = query.Skip(spec.Skip.Value);
            }
            if (spec.Take.HasValue)
            {
                query = query.Take(spec.Take.Value);
            }
            return query;
        }
    }
}
