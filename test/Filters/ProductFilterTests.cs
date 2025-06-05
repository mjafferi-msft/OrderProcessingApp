using Microsoft.Extensions.Logging;
using Moq;
using OrderProcessingApp.Filters;
using OrderProcessingApp.Models;
using OrderProcessingApp.Tests.Helpers;

namespace OrderProcessingApp.Tests.Filters
{
    [TestFixture]
    public class ProductFilterTests
    {
        private Mock<ILogger<ProductFilter>> _mockLogger;
        private ProductFilter _filter;

        [SetUp]
        public void SetUp()
        {
            _mockLogger = new Mock<ILogger<ProductFilter>>();
            _filter = new ProductFilter(_mockLogger.Object);
        }

        [Test]
        public void Filter_ValidGroup_ReturnsAllProducts()
        {
            var products = new List<Product>
            {
                new Product("P1", "Pizza", 10M),
                new Product("P2", "Pasta", 5M)
            };

            var result = _filter.Filter(products);

            Assert.That(result, Is.EquivalentTo(products));
        }

        [Test]
        public void Filter_WithDuplicates_RemovesDuplicates()
        {
            var products = new List<Product>
            {
                new Product("P1", "Pizza", 10M),
                new Product("P1", "Pizza", 12M),
                new Product("P2", "Pasta", 5M)
            };

            var result = _filter.Filter(products);

            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.Any(p => p.ProductId == "P1"));
            Assert.That(result.Single(p => p.ProductId == "P1").Price, Is.EqualTo(10M));
            Assert.That(result.Any(p => p.ProductId == "P2"));
            _mockLogger.VerifyLogging(LogLevel.Warning, Times.Once());
        }

        [Test]
        public void Filter_EmptyList_ReturnsEmptyList()
        {
            var products = new List<Product>();

            var result = _filter.Filter(products);

            Assert.That(result, Is.Empty);
        }
    }
}