using Microsoft.Extensions.Logging;
using Moq;
using OrderProcessingApp.Filters;
using OrderProcessingApp.Models;
using OrderProcessingApp.Tests.Helpers;

namespace OrderProcessingApp.Tests.Filters
{
    [TestFixture]
    public class OrderFilterTests
    {
        private DateTimeOffset _dateTimeNow;
        private Mock<ILogger<OrderFilter>> _mockLogger;
        private OrderFilter _filter;

        [SetUp]
        public void SetUp()
        {
            _dateTimeNow = DateTimeOffset.UtcNow;
            _mockLogger = new Mock<ILogger<OrderFilter>>();
            _filter = new OrderFilter(_mockLogger.Object);
        }

        [Test]
        public void Filter_ValidGroup_ReturnsAllOrders()
        {
            var orders = new List<Order>
            {
                new Order("O1", "P1", 1, _dateTimeNow.AddHours(-2), _dateTimeNow.AddHours(-1), "123 Main St"),
                new Order("O1", "P2", 2, _dateTimeNow.AddHours(-2), _dateTimeNow.AddHours(-1), "123 Main St")
            };

            var result = _filter.Filter(orders);

            Assert.That(result, Is.EquivalentTo(orders));
        }

        [Test]
        public void Filter_MultipleAddresses_RemovesGroup()
        {
            var orders = new List<Order>
            {
                new Order("O1", "P1", 1, _dateTimeNow.AddHours(-2), _dateTimeNow.AddHours(-1), "123 Main St"),
                new Order("O1", "P2", 2, _dateTimeNow.AddHours(-2), _dateTimeNow.AddHours(-1), "456 Main St")
            };

            var result = _filter.Filter(orders);

            Assert.That(result, Is.Empty);
            _mockLogger.VerifyLogging(LogLevel.Warning, Times.Once());
        }

        [Test]
        public void Filter_DuplicateProductIds_RemovesGroup()
        {
            var orders = new List<Order>
            {
                new Order("O1", "P1", 1, _dateTimeNow.AddHours(-2), _dateTimeNow.AddHours(-1), "123 Main St"),
                new Order("O1", "P1", 2, _dateTimeNow.AddHours(-2), _dateTimeNow.AddHours(-1), "123 Main St")
            };

            var result = _filter.Filter(orders);

            Assert.That(result, Is.Empty);
            _mockLogger.VerifyLogging(LogLevel.Warning, Times.Once());
        }

        [Test]
        public void Filter_MultipleCreatedAt_RemovesGroup()
        {
            var orders = new List<Order>
            {
                new Order("O1", "P1", 1, _dateTimeNow.AddHours(-2), _dateTimeNow.AddHours(-2), "123 Main St"),
                new Order("O1", "P2", 2, _dateTimeNow.AddHours(-3), _dateTimeNow.AddHours(-2), "123 Main St")
            };

            var result = _filter.Filter(orders);

            Assert.That(result, Is.Empty);
            _mockLogger.VerifyLogging(LogLevel.Warning, Times.Once());
        }

        [Test]
        public void Filter_MultipleDeliveryAt_RemovesGroup()
        {
            var orders = new List<Order>
            {
                new Order("O1", "P1", 1, _dateTimeNow.AddHours(-3), _dateTimeNow.AddHours(-2), "123 Main St"),
                new Order("O1", "P2", 2, _dateTimeNow.AddHours(-3), _dateTimeNow.AddHours(-1), "123 Main St")
            };

            var result = _filter.Filter(orders);

            Assert.That(result, Is.Empty);
            _mockLogger.VerifyLogging(LogLevel.Warning, Times.Once());
        }

        [Test]
        public void Filter_EmptyList_ReturnsEmptyList()
        {
            var orders = new List<Order>();

            var result = _filter.Filter(orders);

            Assert.That(result, Is.Empty);
        }
    }
}