namespace OrderProcessingApp.Outputs
{
    /// <summary>
    /// Provides methods to output order totals and ingredient requirements to the console.
    /// </summary>
    public class ConsoleOutput : IOutput
    {
        public void OutputOrderTotals(Dictionary<string, decimal> orderTotals)
        {
            Console.WriteLine("Order Totals");
            Console.WriteLine("-------------");
            Console.WriteLine("{0,-12} | {1,12}", "Order ID", "Total Amount");
            Console.WriteLine("-------------|--------------");
            decimal grandTotal = 0;
            foreach (var order in orderTotals)
            {
                Console.WriteLine("{0,-12} | {1,12:C}", order.Key, order.Value);
                grandTotal += order.Value;
            }
            Console.WriteLine("-------------|--------------");
            Console.WriteLine("{0,-12} | {1,12:C}", "Total", grandTotal);
        }

        public void OutputTotalIngredients(Dictionary<string, decimal> totalIngredients)
        {
            Console.WriteLine("\nTotal Ingredients Required:");
            Console.WriteLine("{0,-20} | {1,12}", "Ingredient", "Amount (grams)");
            Console.WriteLine("---------------------|--------------");
            foreach (var ingredient in totalIngredients)
            {
                Console.WriteLine("{0,-20} | {1,12:N2}", ingredient.Key, ingredient.Value);
            }
        }

        public void OutputNoValidOrders()
        {
            Console.WriteLine("No valid orders found!");
        }
    }
}