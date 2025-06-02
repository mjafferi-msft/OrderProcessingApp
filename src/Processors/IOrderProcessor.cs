using OrderProcessingApp.Models;

namespace OrderProcessingApp.Processors
{
    public interface IOrderProcessor
    {
        Dictionary<string, double> CalculateOrderTotals(IEnumerable<Order> orders, Dictionary<string, Product> productMap);
    }
}