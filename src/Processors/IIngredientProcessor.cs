using OrderProcessingApp.Models;

namespace OrderProcessingApp.Processors
{
    public interface IIngredientProcessor
    {
        Dictionary<string, decimal> CalculateTotalIngredients(IEnumerable<Order> orders, Dictionary<string, Product> productMap, Dictionary<string, List<Ingredient>> ingredientProductMap);
    }
}