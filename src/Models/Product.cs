namespace OrderProcessingApp.Models
{
    public class Product
    {
        public string ProductId { get; }
        public string ProductName { get; }
        public double Price { get; }

        public Product(string productId, string productName, double price)
        {
            ProductId = productId;
            ProductName = productName;
            Price = price;
        }

    }
}
