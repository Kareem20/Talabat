namespace Talabat.Core.Models.OrderValues
{
    public class OrderItem : BaseEntity
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public OrderItem()
        {

        }
        public OrderItem(int productID, string productName, string pictureUrl, int quantity, decimal price)
        {
            ProductId = productID;
            ProductName = productName;
            PictureUrl = pictureUrl;
            Quantity = quantity;
            Price = price;
        }
    }
}
