using System.Text.Json;
using Talabat.Core.Models;
using Talabat.Core.Models.OrderValues;

namespace Talabat.Repository.Data
{
    public static class ProductContextSeed
    {
        public static async Task SeedAsync(ProductContext context)
        {
            if (!context.ProductBrands.Any())
            {
                var BracndsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json");
                var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(BracndsData);
                if (Brands?.Count > 0)
                {
                    foreach (var brand in Brands)
                        context.ProductBrands.Add(brand);
                    await context.SaveChangesAsync();
                }
            }
            if (!context.ProductTypes.Any())
            {
                var TypesData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/types.json");
                var Types = JsonSerializer.Deserialize<List<ProductType>>(TypesData);
                if (Types?.Count > 0)
                {
                    foreach (var type in Types)
                        context.ProductTypes.Add(type);
                    await context.SaveChangesAsync();
                }
            }
            if (!context.Products.Any())
            {
                var ProductsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/products.json");
                var Products = JsonSerializer.Deserialize<List<Product>>(ProductsData);
                if (Products?.Count > 0)
                {
                    foreach (var product in Products)
                        context.Products.Add(product);
                    await context.SaveChangesAsync();
                }
            }
            if (!context.DeliveryMethods.Any())
            {
                var dmData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/delivery.json");
                var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(dmData);
                if (deliveryMethods?.Count > 0)
                {
                    foreach (var dm in deliveryMethods)
                        context.DeliveryMethods.Add(dm);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
