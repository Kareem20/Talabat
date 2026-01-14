using Talabat.Core.Models.OrderValues;

namespace Talabat.Core.Specifications.OrderSpecifications
{
    public class OrderSpecificationBase : BaseSpecification<Order>
    {
        public OrderSpecificationBase() : base()
        {
            AddInclude(order => order.DeliveryMethod);
            AddInclude(order => order.Items);
        }
    }
}
