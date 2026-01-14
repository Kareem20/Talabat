namespace Talabat.Core.Specifications.OrderSpecifications
{
    public class OrderByIdAndBuyerSpecification : OrderSpecificationBase
    {
        public OrderByIdAndBuyerSpecification(int OrderId, string BuyerEmail) : base()
        {
            Criteria = (order => order.Id == OrderId && order.BuyerEmail == BuyerEmail);
        }
    }
}
