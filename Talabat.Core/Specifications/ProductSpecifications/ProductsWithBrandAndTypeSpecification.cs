using Talabat.Core.Models;
using Talabat.Core.Specifications.Paramters;

namespace Talabat.Core.Specifications.ProductSpecifications
{
    public class ProductsWithBrandAndTypeSpecification : BaseSpecification<Product>
    {
        public ProductsWithBrandAndTypeSpecification()
        {
        }
        public ProductsWithBrandAndTypeSpecification(ProductSpecificationParamters paramters)
        {
            Criteria = p =>
                (string.IsNullOrEmpty(paramters.Search) || p.Name.ToLower().Contains(paramters.Search)) &&
                (paramters.BrandId == null || p.ProductBrandID == paramters.BrandId) &&
                (paramters.TypeId == null || p.ProductTypeId == paramters.TypeId);
            AddInclude(Product => Product.ProductBrand);
            AddInclude(Product => Product.ProductType);
            if (!string.IsNullOrEmpty(paramters.Sort))
            {
                switch (paramters.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(Product => Product.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDescending(Product => Product.Price);
                        break;
                    default:
                        AddOrderBy(Product => Product.Name);
                        break;
                }
            }
            ApplyPaging(paramters.PageSize * (paramters.PageIndex - 1), paramters.PageSize);
        }
    }
}
