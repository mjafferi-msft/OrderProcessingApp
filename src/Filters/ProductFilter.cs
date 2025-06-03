using Microsoft.Extensions.Logging;
using OrderProcessingApp.Models;

namespace OrderProcessingApp.Filters
{
    /// <summary>
    /// Filters products based on some group-level constraints.
    /// </summary>
    public class ProductFilter : IFilter<Product>
    {
        private readonly ILogger<ProductFilter> _logger;

        public ProductFilter(ILogger<ProductFilter> logger)
        {
            _logger = logger;
        }

        public List<Product> Filter(List<Product> products)
        {
            var distinctProducts = products
                .GroupBy(p => p.ProductId)
                .Select(g => g.First())
                .ToList();

            if (distinctProducts.Count != products.Count)
            {
                _logger.LogWarning("Duplicate products found and removed during filtering.");
            }

            return distinctProducts;
        }
    }
}