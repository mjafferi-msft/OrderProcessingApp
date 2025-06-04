using Microsoft.Extensions.Logging;
using OrderProcessingApp.Models;
using OrderProcessingApp.Validators;

namespace OrderProcessingApp.Utils
{
    public static class DataRefiner
    {
        /// <summary>
        /// Gets products that have valid ingredients based on the provided ingredient product map and ingredient validator.
        /// </summary>
        public static IList<Product> GetProductsWithValidIngredients(
            IList<Product> products,
            Dictionary<string, IList<Ingredient>> ingredientProductMap,
            IValidator<Ingredient> ingredientValidator,
            ILogger logger)
        {
            var validProducts = new List<Product>();

            foreach (var product in products)
            {
                if (ingredientProductMap.TryGetValue(product.ProductId, out var ingredients) &&
                    ingredients != null &&
                    ingredients.Count > 0 &&
                    ingredients.All(ingredientValidator.Validate) &&
                    ingredients.Select(i => i.Name).Distinct().Count() == ingredients.Count)
                {
                    validProducts.Add(product);
                }
                else
                {
                    logger?.LogWarning("Product '{ProductId}' dropped due to invalid, missing, or duplicate ingredients.", product.ProductId);
                }
            }

            return validProducts;
        }
    }
}