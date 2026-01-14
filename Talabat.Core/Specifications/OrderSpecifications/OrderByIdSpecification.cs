namespace Talabat.Core.Specifications.OrderSpecifications
{
    public class OrderByIdSpecification : OrderSpecificationBase
    {
        public OrderByIdSpecification(int OrderId) : base()
        {
            Criteria = (order => order.Id == OrderId);
        }
    }
}
