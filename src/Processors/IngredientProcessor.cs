using OrderProcessingApp.Models;

namespace OrderProcessingApp.Processors
{
    /// <summary>
    /// Processes total amount of each ingredient required for a set of orders.
    /// </summary>
    public class IngredientProcessor : IIngredientProcessor
    { 
        public Dictionary<string, double> CalculateTotalIngredients(
            IEnumerable<Order> orders,
            Dictionary<string, Product> productMap,
            Dictionary<string, List<Ingredient>> ingredientProductMap)
        {
            var totalIngredients = new Dictionary<string, double>();

            foreach (var order in orders)
            {
                if (!productMap.ContainsKey(order.ProductId))
                {
                    continue;
                }

                if (ingredientProductMap.TryGetValue(order.ProductId, out var ingredients))
                {
                    foreach (var ingredient in ingredients)
                    {
                        if (!totalIngredients.ContainsKey(ingredient.Name))
                        {
                            totalIngredients[ingredient.Name] = 0;
                        }
                        totalIngredients[ingredient.Name] += ingredient.Amount * order.Quantity;
                    }
                }
            }
            return totalIngredients;
        }
    }
}