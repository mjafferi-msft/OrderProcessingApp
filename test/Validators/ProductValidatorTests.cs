using Microsoft.Extensions.Logging;
using Moq;
using OrderProcessingApp.Models;
using OrderProcessingApp.Tests.Helpers;
using OrderProcessingApp.Validators;

namespace OrderProcessingApp.Tests.Validators
{
    [TestFixture]
    public class ProductValidatorTests
    {
        private static readonly string[] NullOrEmptyValues = [null, string.Empty];

        private Mock<ILogger<ProductValidator>> _mockLogger;
        private ProductValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _mockLogger = new Mock<ILogger<ProductValidator>>();
            _validator = new ProductValidator(_mockLogger.Object);
        }

        [Test]
        public void Validate_WithValidProduct_ReturnsTrue()
        {
            var product = new Product("P1", "Pizza", 10.0M);

            bool result = _validator.Validate(product);

            Assert.That(result, Is.True);
        }

        [TestCaseSource(nameof(NullOrEmptyValues))]
        public void Validate_WithNullOrEmptyProductId_ReturnsFalse(string invalidValue)
        {
            var product = new Product(invalidValue, "Pizza", 10.0M);

            bool result = _validator.Validate(product);

            Assert.That(result, Is.False);
            _mockLogger.VerifyLogging(LogLevel.Warning, Times.Once());
        }

        [TestCaseSource(nameof(NullOrEmptyValues))]
        public void Validate_WithNullOrEmptyProductName_ReturnsFalse(string invalidValue)
        {
            var product = new Product("P1", invalidValue, 10.0M);

            bool result = _validator.Validate(product);

            Assert.That(result, Is.False);
            _mockLogger.VerifyLogging(LogLevel.Warning, Times.Once());
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void Validate_WithNonPositivePrice_ReturnsFalse(decimal invalidPrice)
        {
            var product = new Product("P1", "Pizza", invalidPrice);

            bool result = _validator.Validate(product);

            Assert.That(result, Is.False);
            _mockLogger.VerifyLogging(LogLevel.Warning, Times.Once());
        }
    }
}