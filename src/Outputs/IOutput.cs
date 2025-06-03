namespace OrderProcessingApp.Outputs
{
    public interface IOutput
    {
        void OutputOrderTotals(Dictionary<string, decimal> orderTotals);
        void OutputTotalIngredients(Dictionary<string, decimal> totalIngredients);
        void OutputNoValidOrders();

    }
}