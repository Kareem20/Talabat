using Talabat.Core.Models;
using Talabat.Core.Specifications.Paramters;

namespace Talabat.Core.Specifications.ProductSpecifications
{
    public class ProductsWithFiltersForCountSpecification : BaseSpecification<Product>
    {
        public ProductsWithFiltersForCountSpecification(ProductSpecificationParamters paramters)
        {
            Criteria = p =>
                (string.IsNullOrEmpty(paramters.Search) || p.Name.ToLower().Contains(paramters.Search)) &&
                (paramters.BrandId == null || p.ProductBrandID == paramters.BrandId) &&
                (paramters.TypeId == null || p.ProductTypeId == paramters.TypeId);
        }
    }
}
