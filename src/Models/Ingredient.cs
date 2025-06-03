namespace OrderProcessingApp.Models
{
    public class Ingredient
    {
        public string Name { get; }
        public decimal Amount { get; }

        public Ingredient(string name, decimal amount)
        {
            Name = name;
            Amount = amount;
        }
    }
}
