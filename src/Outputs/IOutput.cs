using OrderProcessingApp.Models;

namespace OrderProcessingApp.Outputs
{
    public interface IOutput
    {
        void OutputCompleteOrderDetails(IEnumerable<Order> orders, Dictionary<string, Product> productMap);
        void OutputOrderTotals(Dictionary<string, decimal> orderTotals);
        void OutputTotalIngredients(Dictionary<string, decimal> totalIngredients);
    }
}