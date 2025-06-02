using OrderProcessingApp.Loaders;
using OrderProcessingApp.Models;
using OrderProcessingApp.Processors;
using OrderProcessingApp.Validators;
using OrderProcessingApp.Outputs;

using static OrderProcessingApp.Constants.AppConstants;

namespace OrderProcessingApp
{
    class Program
    {
        public static void Main()
        {
            // Load data
            var orders = new JsonFileLoader<List<Order>>().Load(OrdersJsonPath) ?? [];
            var products = new JsonFileLoader<List<Product>>().Load(ProductsJsonPath) ?? [];
            var ingredientProductMap = new JsonFileLoader<Dictionary<string, List<Ingredient>>>().Load(IngredientsJsonPath) ?? [];
            Console.WriteLine($"Total orders loaded from JSON: {orders.Count}");


            // Filter valid orders and products
            orders = ValidatorHelper.GetValidOrders(orders);
            products = ValidatorHelper.GetValidProducts(products);
            Console.WriteLine($"Valid orders after validation: {orders.Count}");

            var productMap = products.ToDictionary(p => p.ProductId);


            // Process
            var orderTotals = new OrderProcessor().CalculateOrderTotals(orders, productMap);
            var totalIngredients = new IngredientProcessor().CalculateTotalIngredients(orders, productMap, ingredientProductMap);


            // Print
            var output = new ConsoleOutput();
            if (orderTotals.Count == 0)
            {
                output.OutputNoValidOrders();
            }
            else
            {
                output.OutputOrderTotals(orderTotals);
                output.OutputTotalIngredients(totalIngredients);
            }
        }
    }
}