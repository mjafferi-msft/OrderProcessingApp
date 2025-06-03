namespace OrderProcessingApp.Models
{
    public class Order
    {
        public string OrderId { get; }
        public string ProductId { get; }
        public int Quantity { get; }
        public DateTimeOffset CreatedAt { get; }
        public DateTimeOffset DeliveryAt { get; }
        public string DeliveryAddress { get; }

        public Order(string orderId, string productId, int quantity, DateTimeOffset createdAt, DateTimeOffset deliveryAt, string deliveryAddress)
        {
            OrderId = orderId;
            ProductId = productId;
            Quantity = quantity;
            CreatedAt = createdAt;
            DeliveryAt = deliveryAt;
            DeliveryAddress = deliveryAddress;
        }
    }
}
