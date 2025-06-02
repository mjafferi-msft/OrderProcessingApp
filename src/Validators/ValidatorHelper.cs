using OrderProcessingApp.Models;

namespace OrderProcessingApp.Validators
{
    public static class ValidatorHelper
    {
        public static List<Order> GetValidOrders(List<Order> orders)
        {
            // Filter out invalid orders based on individual constraints
            var validOrders = orders.Where(IsValidOrder).ToList();

            // Apply group-level constraints
            return FilterGroupValidOrders(validOrders);
        }

        private static bool IsValidOrder(Order order)
        {
            return !string.IsNullOrWhiteSpace(order.OrderId)
                && !string.IsNullOrWhiteSpace(order.ProductId)
                && !string.IsNullOrWhiteSpace(order.DeliveryAddress)
                && order.CreatedAt < order.DeliveryAt
                && order.Quantity > 0;
        }

        private static List<Order> FilterGroupValidOrders(List<Order> orders)
        {
            var validOrders = new List<Order>();

            // Group by OrderId
            foreach (var group in orders.GroupBy(o => o.OrderId))
            {
                var orderList = group.ToList();

                // All addresses must be the same for this OrderId
                if (orderList.Select(o => o.DeliveryAddress).Distinct().Count() > 1)
                {
                    continue;
                }

                // No duplicate ProductId for the same OrderId
                if (orderList.Select(o => o.ProductId).Distinct().Count() != orderList.Count)
                {
                    continue;
                }

                // All orders in group must have the same CreatedAt and DeliveryAt
                if (orderList.Select(o => o.CreatedAt).Distinct().Count() > 1 ||
                    orderList.Select(o => o.DeliveryAt).Distinct().Count() > 1)
                {
                    continue;
                }

                // If all checks pass, add all orders in this group
                validOrders.AddRange(orderList);
            }

            return validOrders;
        }

        public static List<Product> GetValidProducts(List<Product> products)
        {
            // Filter out invalid products based on individual constraints
            var validProducts = products.Where(IsValidProduct).ToList();

            // Remove duplicates by ProductId, keeping the first occurrence
            var distinctProducts = validProducts
                .GroupBy(p => p.ProductId)
                .Select(g => g.First())
                .ToList();

            return distinctProducts;
        }

        private static bool IsValidProduct(Product product)
        {
            return !string.IsNullOrWhiteSpace(product.ProductId)
                && product.Price > 0;
        }
    }
}
