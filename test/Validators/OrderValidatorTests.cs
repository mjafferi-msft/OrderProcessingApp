using Microsoft.Extensions.Logging;
using Moq;
using OrderProcessingApp.Models;
using OrderProcessingApp.Tests.Helpers;
using OrderProcessingApp.Validators;
using System;

namespace OrderProcessingApp.Tests.Validators
{
    [TestFixture]
    public class OrderValidatorTests
    {
        private static readonly string[] NullOrEmptyValues = [null, string.Empty];

        private DateTimeOffset _dateTimeNow;
        private Mock<ILogger<OrderValidator>> _mockLogger;
        private OrderValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _dateTimeNow = DateTimeOffset.Now;
            _mockLogger = new Mock<ILogger<OrderValidator>>();
            _validator = new OrderValidator(_mockLogger.Object);
        }

        [Test]
        public void Validate_WithValidOrder_ReturnsTrue()
        {
            var order = new Order("O1", "P1", 2, _dateTimeNow.AddHours(-2), _dateTimeNow.AddHours(-1), "123 Main St");

            bool result = _validator.Validate(order);

            Assert.That(result, Is.True);
        }

        [TestCaseSource(nameof(NullOrEmptyValues))]
        public void Validate_WithNullOrEmptyOrderId_ReturnsFalse(string invalidValue)
        {
            var order = new Order(invalidValue, "P1", 2, _dateTimeNow.AddHours(-2), _dateTimeNow.AddHours(-1), "123 Main St");

            bool result = _validator.Validate(order);

            Assert.That(result, Is.False);
            _mockLogger.VerifyLogging(LogLevel.Warning, Times.Once());
        }

        [TestCaseSource(nameof(NullOrEmptyValues))]
        public void Validate_WithNullOrEmptyProductId_ReturnsFalse(string invalidValue)
        {
            var order = new Order("O1", invalidValue, 2, _dateTimeNow.AddHours(-2), _dateTimeNow.AddHours(-1), "123 Main St");

            bool result = _validator.Validate(order);

            Assert.That(result, Is.False);
            _mockLogger.VerifyLogging(LogLevel.Warning, Times.Once());
        }

        [TestCaseSource(nameof(NullOrEmptyValues))]
        public void Validate_WithNullOrEmptyDeliveryAddress_ReturnsFalse(string invalidValue)
        {
            var order = new Order("O1", "P1", 2, _dateTimeNow.AddHours(-2), _dateTimeNow.AddHours(-1), invalidValue);

            bool result = _validator.Validate(order);

            Assert.That(result, Is.False);
            _mockLogger.VerifyLogging(LogLevel.Warning, Times.Once());
        }

        [Test]
        public void Validate_WithDefaultCreatedAt_ReturnsFalse()
        {
            var order = new Order("O1", "P1", 2, default, _dateTimeNow.AddHours(-1), "123 Main St");

            bool result = _validator.Validate(order);

            Assert.That(result, Is.False);
            _mockLogger.VerifyLogging(LogLevel.Warning, Times.Once());
        }

        [Test]
        public void Validate_WithDefaultDeliveryAt_ReturnsFalse()
        {
            var order = new Order("O1", "P1", 2, _dateTimeNow.AddHours(-2), default, "123 Main St");

            bool result = _validator.Validate(order);

            Assert.That(result, Is.False);
            _mockLogger.VerifyLogging(LogLevel.Warning, Times.Once());
        }

        [Test]
        public void Validate_WithCreatedAtAfterDeliveryAt_ReturnsFalse()
        {
            var order = new Order("O1", "P1", 2, _dateTimeNow.AddHours(-1), _dateTimeNow.AddHours(-2), "123 Main St");

            bool result = _validator.Validate(order);

            Assert.That(result, Is.False);
            _mockLogger.VerifyLogging(LogLevel.Warning, Times.Once());
        }

        [Test]
        public void Validate_WithDeliveryAtInFuture_ReturnsFalse()
        {
            var order = new Order("O1", "P1", 2, _dateTimeNow.AddHours(-2), _dateTimeNow.AddHours(1), "123 Main St");

            bool result = _validator.Validate(order);

            Assert.That(result, Is.False);
            _mockLogger.VerifyLogging(LogLevel.Warning, Times.Once());
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void Validate_WithNonPositiveQuantity_ReturnsFalse(int invalidQuantity)
        {
            var order = new Order("O1", "P1", invalidQuantity, _dateTimeNow.AddHours(-2), _dateTimeNow.AddHours(-1), "123 Main St");

            bool result = _validator.Validate(order);

            Assert.That(result, Is.False);
            _mockLogger.VerifyLogging(LogLevel.Warning, Times.Once());
        }
    }
}