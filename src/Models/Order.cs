namespace OrderProcessingApp.Models
{
    public class Order
    {
        public string OrderId { get; }
        public string ProductId { get; }
        public int Quantity { get; }
        public DateTime CreatedAt { get; }
        public DateTime DeliveryAt { get; }
        public string DeliveryAddress { get; }

        public Order(string orderId, string productId, int quantity, DateTime createdAt, DateTime deliveryAt, string deliveryAddress)
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
