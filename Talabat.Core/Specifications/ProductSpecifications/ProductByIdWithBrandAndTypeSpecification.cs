namespace Talabat.Core.Specifications.ProductSpecifications
{
    public class ProductByIdWithBrandAndTypeSpecification : ProductsWithBrandAndTypeSpecification
    {
        public ProductByIdWithBrandAndTypeSpecification(int Id) : base()
        {
            Criteria = product => product.Id == Id;
        }
    }
}
