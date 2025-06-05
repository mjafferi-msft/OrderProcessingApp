using Microsoft.Extensions.Logging;
using Moq;
using OrderProcessingApp.Models;
using OrderProcessingApp.Tests.Helpers;
using OrderProcessingApp.Utils;
using OrderProcessingApp.Validators;

namespace OrderProcessingApp.Tests.Utils
{
    [TestFixture]
    public class DataRefinerTests
    {
        private Mock<IValidator<Ingredient>> _mockIngredientValidator;
        private Mock<ILogger> _mockLogger;

        [SetUp]
        public void SetUp()
        {
            _mockIngredientValidator = new Mock<IValidator<Ingredient>>();
            _mockLogger = new Mock<ILogger>();
        }

        [Test]
        public void GetProductsWithValidIngredients_AllValid_ReturnsAllProducts()
        {
            var products = new List<Product>
            {
                new Product("P1", "Pizza", 10M),
                new Product("P2", "Pasta", 5M)
            };
            var ingredientProductMap = new Dictionary<string, IList<Ingredient>>
            {
                ["P1"] = new List<Ingredient> { new("Cheese", 0.5M), new("Tomato", 0.2M) },
                ["P2"] = new List<Ingredient> { new("Cheese", 0.3M) }
            };
            _mockIngredientValidator.Setup(v => v.Validate(It.IsAny<Ingredient>())).Returns(true);

            var result = DataRefiner.GetProductsWithValidIngredients(products, ingredientProductMap, _mockIngredientValidator.Object, _mockLogger.Object);

            Assert.That(result, Is.EquivalentTo(products));
        }

        [Test]
        public void GetProductsWithValidIngredients_InvalidIngredient_SkipsProduct()
        {
            var products = new List<Product>
            {
                new Product("P1", "Pizza", 10M)
            };
            var ingredientProductMap = new Dictionary<string, IList<Ingredient>>
            {
                ["P1"] = new List<Ingredient> { new("Cheese", 0.5M), new("Tomato", 0.2M) }
            };
            _mockIngredientValidator.Setup(v => v.Validate(It.Is<Ingredient>(i => i.Name == "Cheese"))).Returns(false);
            _mockIngredientValidator.Setup(v => v.Validate(It.Is<Ingredient>(i => i.Name == "Tomato"))).Returns(true);

            var result = DataRefiner.GetProductsWithValidIngredients(products, ingredientProductMap, _mockIngredientValidator.Object, _mockLogger.Object);

            Assert.That(result, Is.Empty);
            _mockLogger.VerifyLogging(LogLevel.Warning, Times.Once());
        }

        [Test]
        public void GetProductsWithValidIngredients_MissingIngredients_SkipsProduct()
        {
            var products = new List<Product>
            {
                new Product("P1", "Pizza", 10M)
            };
            var ingredientProductMap = new Dictionary<string, IList<Ingredient>>();

            var result = DataRefiner.GetProductsWithValidIngredients(products, ingredientProductMap, _mockIngredientValidator.Object, _mockLogger.Object);

            Assert.That(result, Is.Empty);
            _mockLogger.VerifyLogging(LogLevel.Warning, Times.Once());
        }

        [Test]
        public void GetProductsWithValidIngredients_EmptyIngredientList_SkipsProduct()
        {
            var products = new List<Product>
            {
                new Product("P1", "Pizza", 10M)
            };
            var ingredientProductMap = new Dictionary<string, IList<Ingredient>>
            {
                ["P1"] = new List<Ingredient>()
            };

            var result = DataRefiner.GetProductsWithValidIngredients(products, ingredientProductMap, _mockIngredientValidator.Object, _mockLogger.Object);

            Assert.That(result, Is.Empty);
            _mockLogger.VerifyLogging(LogLevel.Warning, Times.Once());
        }

        [Test]
        public void GetProductsWithValidIngredients_NullIngredientList_SkipsProduct()
        {
            var products = new List<Product>
            {
                new Product("P1", "Pizza", 10M)
            };
            var ingredientProductMap = new Dictionary<string, IList<Ingredient>>
            {
                ["P1"] = null
            };

            var result = DataRefiner.GetProductsWithValidIngredients(products, ingredientProductMap, _mockIngredientValidator.Object, _mockLogger.Object);

            Assert.That(result, Is.Empty);
            _mockLogger.VerifyLogging(LogLevel.Warning, Times.Once());
        }

        [Test]
        public void GetProductsWithValidIngredients_DuplicateIngredientNames_SkipsProduct()
        {
            var products = new List<Product>
            {
                new Product("P1", "Pizza", 10M)
            };
            var ingredientProductMap = new Dictionary<string, IList<Ingredient>>
            {
                ["P1"] = new List<Ingredient> { new("Cheese", 0.5M), new("Cheese", 2M) }
            };
            _mockIngredientValidator.Setup(v => v.Validate(It.IsAny<Ingredient>())).Returns(true);

            var result = DataRefiner.GetProductsWithValidIngredients(products, ingredientProductMap, _mockIngredientValidator.Object, _mockLogger.Object);

            Assert.That(result, Is.Empty);
            _mockLogger.VerifyLogging(LogLevel.Warning, Times.Once());
        }
    }
}