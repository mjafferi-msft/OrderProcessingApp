using Microsoft.Extensions.Logging;
using OrderProcessingApp.Models;
using OrderProcessingApp.Validators;

namespace OrderProcessingApp.Utils
{
    public static class DataRefiner
    {
        /// <summary>
        /// Removes orders and products that reference invalid or duplicate ingredients.
        /// </summary>
        public static void RemoveInvalidIngredientRefs(
            List<Order> orders,
            List<Product> products,
            Dictionary<string, List<Ingredient>> ingredientProductMap,
            IValidator<Ingredient> ingredientValidator,
            ILogger logger)
        {
            var validProductIds = new HashSet<string>(products.Select(p => p.ProductId));

            foreach (var (productId, ingredients) in ingredientProductMap)
            {
                if (ingredients == null || ingredients.Count == 0 || ingredients.Any(i => !ingredientValidator.Validate(i)))
                {
                    logger?.LogWarning("Product '{ProductId}' dropped due to invalid or missing ingredients.", productId);
                    validProductIds.Remove(productId);
                }
                else if (ingredients.Select(i => i.Name).Distinct().Count() != ingredients.Count)
                {
                    logger?.LogWarning("Product '{ProductId}' dropped due to duplicate ingredient names.", productId);
                    validProductIds.Remove(productId);
                }
            }

            orders.RemoveAll(o => !validProductIds.Contains(o.ProductId));
            products.RemoveAll(p => !validProductIds.Contains(p.ProductId));
        }
    }
}