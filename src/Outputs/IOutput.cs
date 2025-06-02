namespace OrderProcessingApp.Outputs
{
    public interface IOutput
    {
        void OutputOrderTotals(Dictionary<string, double> orderTotals);
        void OutputTotalIngredients(Dictionary<string, double> totalIngredients);
        void OutputNoValidOrders();

    }
}