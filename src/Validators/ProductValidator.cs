using Microsoft.Extensions.Logging;
using OrderProcessingApp.Models;

namespace OrderProcessingApp.Validators
{
    /// <summary>
    /// Validates the properties of an Product object to ensure it meets the required constraints.
    /// </summary>
    public class ProductValidator : IValidator<Product>
    {
        private readonly ILogger<ProductValidator> _logger;

        public ProductValidator(ILogger<ProductValidator> logger)
        {
            _logger = logger;
        }

        public bool Validate(Product product)
        {
            bool isValid = !string.IsNullOrWhiteSpace(product.ProductId)
                && !string.IsNullOrWhiteSpace(product.ProductName)
                && product.Price > 0;

            if (!isValid)
            {
                _logger.LogWarning("Product '{ProductId}' failed basic constraints validation and was dropped.", product.ProductId);
            }

            return isValid;
        }
    }
}