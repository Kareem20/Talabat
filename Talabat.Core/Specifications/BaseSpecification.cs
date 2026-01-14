using System.Linq.Expressions;
using Talabat.Core.Models;

namespace Talabat.Core.Specifications
{
    public abstract class BaseSpecification<T> : ISpecification<T> where T : BaseEntity
    {
        // The filter criteria (where condition)
        public Expression<Func<T, bool>> Criteria { get; protected set; }

        // Includes (navigation properties to eager load)
        private readonly List<Expression<Func<T, object>>> _includes = new();
        public IReadOnlyList<Expression<Func<T, object>>> Includes => _includes;

        // OrderBy / OrderByDescending expressions
        private readonly List<Expression<Func<T, object>>> _orderBys = new();
        private readonly List<Expression<Func<T, object>>> _orderByDescendings = new();
        public IReadOnlyList<Expression<Func<T, object>>> OrderBys => _orderBys;
        public IReadOnlyList<Expression<Func<T, object>>> OrderByDescendings => _orderByDescendings;

        public int? Take { get; private set; }

        public int? Skip { get; private set; }

        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            _includes.Add(includeExpression);
        }
        protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            _orderBys.Add(orderByExpression);
        }
        protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
        {
            _orderByDescendings.Add(orderByDescExpression);
        }
        protected void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
        }
    }
}
