namespace OrderProcessingApp.Outputs
{
    /// <summary>
    /// Provides methods to output order totals and ingredient requirements to the console.
    /// </summary>
    public class ConsoleOutput : IOutput
    {
        public void OutputOrderTotals(Dictionary<string, double> orderTotals)
        {
            Console.WriteLine("Order Totals: ");
            foreach (var order in orderTotals)
            {
                Console.WriteLine($"Order {order.Key}: €{order.Value:F2}");
            }
        }

        public void OutputTotalIngredients(Dictionary<string, double> totalIngredients)
        {
            Console.WriteLine("\nTotal Ingredients Required:");
            foreach (var ingredient in totalIngredients)
            {
                Console.WriteLine($"{ingredient.Key}: {ingredient.Value:F2} grams.");
            }
        }

        public void OutputNoValidOrders()
        {
            Console.WriteLine("No valid orders found!");
        }
    }
}