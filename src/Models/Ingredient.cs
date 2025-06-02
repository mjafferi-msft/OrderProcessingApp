namespace OrderProcessingApp.Models
{
    public class Ingredient
    {
        public string Name { get; }
        public double Amount { get; }

        public Ingredient(string name, double amount)
        {
            Name = name;
            Amount = amount;
        }
    }
}
