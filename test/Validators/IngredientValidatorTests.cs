using Microsoft.Extensions.Logging;
using Moq;
using OrderProcessingApp.Models;
using OrderProcessingApp.Tests.Helpers;
using OrderProcessingApp.Validators;

namespace OrderProcessingApp.Tests.Validators
{
    [TestFixture]
    public class IngredientValidatorTests
    {
        private static readonly string[] NullOrEmptyValues = [null, string.Empty];

        private Mock<ILogger<IngredientValidator>> _mockLogger;
        private IngredientValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _mockLogger = new Mock<ILogger<IngredientValidator>>();
            _validator = new IngredientValidator(_mockLogger.Object);
        }

        [Test]
        public void Validate_WithValidIngredient_ReturnsTrue()
        {
            var ingredient = new Ingredient("Cheese", 1.0M);

            bool result = _validator.Validate(ingredient);

            Assert.That(result, Is.True);
        }

        [TestCaseSource(nameof(NullOrEmptyValues))]
        public void Validate_WithNullOrEmptyName_ReturnsFalse(string invalidValue)
        {
            var ingredient = new Ingredient(invalidValue, 1.0M);

            bool result = _validator.Validate(ingredient);

            Assert.That(result, Is.False);
            _mockLogger.VerifyLogging(LogLevel.Warning, Times.Once());
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void Validate_WithNonPositiveAmount_ReturnsFalse(decimal invalidAmount)
        {
            var ingredient = new Ingredient("Cheese", invalidAmount);

            bool result = _validator.Validate(ingredient);

            Assert.That(result, Is.False);
            _mockLogger.VerifyLogging(LogLevel.Warning, Times.Once());
        }
    }
}