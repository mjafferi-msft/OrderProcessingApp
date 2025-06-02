using OrderProcessingApp.Models;

namespace OrderProcessingApp.Processors
{
    public class OrderProcessor : IOrderProcessor
    {
        public Dictionary<string, double> CalculateOrderTotals(
            IEnumerable<Order> orders, 
            Dictionary<string, Product> productMap)
        {
            var orderTotals = new Dictionary<string, double>();

            foreach (var order in orders)
            {
                if (!productMap.ContainsKey(order.ProductId))
                {
                    continue;
                }

                var product = productMap[order.ProductId];
                double total = product.Price * order.Quantity;

                if (!orderTotals.ContainsKey(order.OrderId))
                {
                    orderTotals[order.OrderId] = 0;
                }
                orderTotals[order.OrderId] += total;
            }

            return orderTotals;
        }
    }
}