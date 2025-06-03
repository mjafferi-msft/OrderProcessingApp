using OrderProcessingApp.Models;

namespace OrderProcessingApp.Processors
{
    public interface IOrderProcessor
    {
        Dictionary<string, decimal> CalculateOrderTotals(IEnumerable<Order> orders, Dictionary<string, Product> productMap);
    }
}