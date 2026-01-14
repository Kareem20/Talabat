namespace Talabat.Core.Specifications.OrderSpecifications
{
    public class OrderByPaymentIntentSpecification : OrderSpecificationBase
    {
        public OrderByPaymentIntentSpecification(string PaymentIntentId) : base()
        {
            Criteria = (order => order.PaymentIntentId == PaymentIntentId);
        }
    }
}
