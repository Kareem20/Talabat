namespace Talabat.APIs.DTOs
{
    public class ProductDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
        public int ProductBrandID { get; set; }
        public string ProductBrand { get; set; }
        public int ProductTypeID { get; set; }
        public string ProductType { get; set; }
    }
}
