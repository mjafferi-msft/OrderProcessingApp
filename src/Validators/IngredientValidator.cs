using Microsoft.Extensions.Logging;
using OrderProcessingApp.Models;

namespace OrderProcessingApp.Validators
{
    /// <summary>
    /// Validates the properties of an Ingredient object to ensure it meets the required constraints.
    /// </summary>
    public class IngredientValidator : IValidator<Ingredient>
    {
        private readonly ILogger<IngredientValidator> _logger;

        public IngredientValidator(ILogger<IngredientValidator> logger)
        {
            _logger = logger;
        }

        public bool Validate(Ingredient ingredient)
        {
            bool isValid = !string.IsNullOrWhiteSpace(ingredient.Name)
               && ingredient.Amount > 0;

            if (!isValid)
            {
                _logger.LogWarning("Ingredient '{IngredientName}' failed basic constraints validation and was dropped.", ingredient.Name);
            }

            return isValid;
        }
    }
}