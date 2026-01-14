namespace Talabat.Core.Specifications.OrderSpecifications
{
    public class OrdersForBuyerSpecification : OrderSpecificationBase
    {
        public OrdersForBuyerSpecification(string BuyerEmail) : base()
        {
            Criteria = (order => order.BuyerEmail == BuyerEmail);
            AddOrderByDescending(order => order.Date);
        }
    }
}
