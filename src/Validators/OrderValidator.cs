using Microsoft.Extensions.Logging;
using OrderProcessingApp.Models;

namespace OrderProcessingApp.Validators
{
    /// <summary>
    /// Validates the properties of an Order object to ensure it meets the required constraints.
    /// </summary>
    public class OrderValidator : IValidator<Order>
    {
        private readonly ILogger<OrderValidator> _logger;

        public OrderValidator(ILogger<OrderValidator> logger)
        {
            _logger = logger;
        }

        public bool Validate(Order order)
        {
            bool isValid = !string.IsNullOrWhiteSpace(order.OrderId)
                && !string.IsNullOrWhiteSpace(order.ProductId)
                && !string.IsNullOrWhiteSpace(order.DeliveryAddress)
                && order.CreatedAt != default
                && order.DeliveryAt != default
                && order.CreatedAt < order.DeliveryAt
                && order.DeliveryAt < DateTimeOffset.UtcNow
                && order.Quantity > 0;

            if (!isValid)
            {
                _logger.LogWarning("Order '{OrderId}' failed basic constraints validation and was dropped.", order.OrderId);
            }

            return isValid;
        }
    }
}