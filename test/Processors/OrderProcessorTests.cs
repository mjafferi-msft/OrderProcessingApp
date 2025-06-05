using OrderProcessingApp.Models;
using OrderProcessingApp.Processors;

namespace OrderProcessingApp.Tests.Processors
{
    [TestFixture]
    public class OrderProcessorTests
    {
        private DateTimeOffset _dateTimeNow;
        private OrderProcessor _processor;

        [SetUp]
        public void SetUp()
        {
            _dateTimeNow = DateTimeOffset.UtcNow;
            _processor = new OrderProcessor();
        }

        [Test]
        public void CalculateOrderTotals_WithValidOrders_ReturnsCorrectTotals()
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

            var result = _processor.CalculateOrderTotals(orders, productMap);

            Assert.That(result["O1"], Is.EqualTo(20M));
            Assert.That(result["O2"], Is.EqualTo(5M));
        }

        [Test]
        public void CalculateOrderTotals_OrderWithUnknownProduct_SkipsOrder()
        {
            var orders = new List<Order>
            {
                new Order("O1", "P1", 2, _dateTimeNow.AddHours(-2), _dateTimeNow.AddHours(-1), "123 Main St"),
                new Order("O2", "P3", 1, _dateTimeNow.AddHours(-4), _dateTimeNow.AddHours(-3), "123 Opp St")
            };
            var productMap = new Dictionary<string, Product>
            {
                ["P1"] = new Product("P1", "Pizza", 10M)
            };

            var result = _processor.CalculateOrderTotals(orders, productMap);

            Assert.That(result.ContainsKey("O1"), Is.True);
            Assert.That(result.ContainsKey("O2"), Is.False);
        }
    }
}