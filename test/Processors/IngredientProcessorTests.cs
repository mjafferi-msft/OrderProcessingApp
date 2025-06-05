using OrderProcessingApp.Models;
using OrderProcessingApp.Processors;

namespace OrderProcessingApp.Tests.Processors
{
    [TestFixture]
    public class IngredientProcessorTests
    {
        private DateTimeOffset _dateTimeNow;
        private IngredientProcessor _processor;

        [SetUp]
        public void SetUp()
        {
            _dateTimeNow = DateTimeOffset.UtcNow;
            _processor = new IngredientProcessor();
        }

        [Test]
        public void CalculateTotalIngredients_WithValidOrders_ReturnsCorrectTotals()
        {
            var orders = new List<Order>
            {
                new Order("O1", "P1", 2, _dateTimeNow.AddHours(-2), _dateTimeNow.AddHours(-1), "123 Main St"),
                new Order("O2", "P2", 1, _dateTimeNow.AddHours(-4), _dateTimeNow.AddHours(-3), "123 Opp St")
            };
            var productMap = new Dictionary<string, Product>
            {
                ["P1"] = new Product("P1", "Pizza", 10M),
                ["P2"] = new Product("P2", "Pasta", 5M)
            };
            var ingredientProductMap = new Dictionary<string, IList<Ingredient>>
            {
                ["P1"] = new List<Ingredient> { new("Cheese", 0.5M), new("Tomato", 0.2M) },
                ["P2"] = new List<Ingredient> { new("Cheese", 0.3M) }
            };

            var result = _processor.CalculateTotalIngredients(orders, productMap, ingredientProductMap);

            Assert.That(result["Cheese"], Is.EqualTo(1.3M));
            Assert.That(result["Tomato"], Is.EqualTo(0.4M));
        }

        [Test]
        public void CalculateTotalIngredients_OrderWithUnknownProduct_SkipsOrder()
        {
            var orders = new List<Order>
            {
                new Order("O2", "P3", 1, _dateTimeNow.AddHours(-4), _dateTimeNow.AddHours(-3), "123 Main St")
            };
            var productMap = new Dictionary<string, Product>
            {
                ["P1"] = new Product("P1", "Pizza", 10M)
            };
            var ingredientProductMap = new Dictionary<string, IList<Ingredient>>
            {
                ["P1"] = new List<Ingredient> { new("Cheese", 0.5M) }
            };

            var result = _processor.CalculateTotalIngredients(orders, productMap, ingredientProductMap);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void CalculateTotalIngredients_ProductWithNoIngredient_SkipsOrder()
        {
            var orders = new List<Order>
            {
                new Order("O1", "P1", 1, _dateTimeNow.AddHours(-2), _dateTimeNow.AddHours(-1), "123 Main St")
            };
            var productMap = new Dictionary<string, Product>
            {
                ["P1"] = new Product("P1", "Pizza", 10M)
            };
            var ingredientProductMap = new Dictionary<string, IList<Ingredient>>
            {
                ["P2"] = new List<Ingredient> { new("Cheese", 0.5M) }
            };

            var result = _processor.CalculateTotalIngredients(orders, productMap, ingredientProductMap);

            Assert.That(result, Is.Empty);
        }
    }
}