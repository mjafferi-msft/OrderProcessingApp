using OrderProcessingApp.Models;

namespace OrderProcessingApp.Outputs
{
    /// <summary>
    /// Provides methods to output order totals and ingredient requirements to the console.
    /// </summary>
    public class ConsoleOutput : IOutput
    {
        public void OutputCompleteOrderDetails(IEnumerable<Order> orders, Dictionary<string, Product> productMap)
        {
            Console.WriteLine("\n\nValid Orders Summary:");
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("{0,-7} | {1,-17} | {2,-8} | {3,-6:C} | {4,-6:C} | {5,-20} | {6,-20} | {7,-25}",
                "OrderId", "Product", "Quantity", "Price", "Total", "CreatedAt", "DeliveryAt", "Delivery Address");
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------");

            var groupedOrders = orders.GroupBy(o => o.OrderId);

            foreach (var group in groupedOrders)
            {
                bool first = true;
                foreach (var order in group)
                {
                    var product = productMap.TryGetValue(order.ProductId, out var p) ? p : null;

                    Console.WriteLine("{0,-7} | {1,-17} | {2,-8} | {3,-6:C} | {4,-6:C} | {5,-20} | {6,-20} | {7,-25}",
                        first ? order.OrderId : "",
                        product?.ProductName,
                        order.Quantity,
                        product?.Price,
                        product?.Price * order.Quantity,
                        order.CreatedAt.DateTime,
                        order.DeliveryAt.DateTime,
                        order.DeliveryAddress
                    );
                    first = false;
                }
            }
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------");
        }

        public void OutputOrderTotals(Dictionary<string, decimal> orderTotals)
        {
            Console.WriteLine("\n\nValid Order Totals:");
            Console.WriteLine("----------------------------");
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
            Console.WriteLine("----------------------------");
        }

        public void OutputTotalIngredients(Dictionary<string, decimal> totalIngredients)
        {
            Console.WriteLine("\n\nTotal Ingredients Required:");
            Console.WriteLine("------------------------------------");
            Console.WriteLine("{0,-20} | {1,12}", "Ingredient", "Amount (grams)");
            Console.WriteLine("---------------------|--------------");
            foreach (var ingredient in totalIngredients)
            {
                Console.WriteLine("{0,-20} | {1,12:N2}", ingredient.Key, ingredient.Value);
            }
            Console.WriteLine("------------------------------------");
        }
    }
}