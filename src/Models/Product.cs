namespace OrderProcessingApp.Models
{
    public class Product
    {
        public string ProductId { get; }
        public string ProductName { get; }
        public decimal Price { get; }

        public Product(string productId, string productName, decimal price)
        {
            ProductId = productId;
            ProductName = productName;
            Price = price;
        }

    }
}
