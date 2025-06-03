using Microsoft.Extensions.Logging;
using OrderProcessingApp.Models;

namespace OrderProcessingApp.Filters
{
    /// <summary>
    /// Filters orders based on some group-level constraints.
    /// </summary>
    public class OrderFilter : IFilter<Order>
    {
        private readonly ILogger<OrderFilter> _logger;

        public OrderFilter(ILogger<OrderFilter> logger)
        {
            _logger = logger;
        }

        public List<Order> Filter(List<Order> orders)
        {
            var validOrders = new List<Order>();

            foreach (var group in orders.GroupBy(o => o.OrderId))
            {
                var orderList = group.ToList();

                bool hasMultipleAddresses = orderList.Select(o => o.DeliveryAddress).Distinct().Count() > 1;
                bool hasDuplicateProducts = orderList.Select(o => o.ProductId).Distinct().Count() != orderList.Count;
                bool hasMultipleCreatedAt = orderList.Select(o => o.CreatedAt).Distinct().Count() > 1;
                bool hasMultipleDeliveryAt = orderList.Select(o => o.DeliveryAt).Distinct().Count() > 1;

                bool isValidGroup = !(hasMultipleAddresses || hasDuplicateProducts || hasMultipleCreatedAt || hasMultipleDeliveryAt);

                if (isValidGroup)
                {
                    validOrders.AddRange(orderList);
                }
                else
                {
                    _logger.LogWarning("Order '{OrderId}' failed group-level constraints validation and was dropped.", group.Key);
                }
            }

            return validOrders;
        }
    }
}