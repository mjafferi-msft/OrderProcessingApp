using Microsoft.Extensions.Logging;
using OrderProcessingApp.Filters;
using OrderProcessingApp.Loaders;
using OrderProcessingApp.Models;
using OrderProcessingApp.Outputs;
using OrderProcessingApp.Processors;
using OrderProcessingApp.Utils;
using OrderProcessingApp.Validators;

using static OrderProcessingApp.Constants.AppConstants;


namespace OrderProcessingApp
{
    /// <summary>
    /// Manages the end-to-end processing of orders, including loading, validation, filtering, processing, and output.
    /// </summary>
    public class OrderProcessingManager
    {
        private readonly ILoader<IList<Order>> _orderLoader;
        private readonly ILoader<IList<Product>> _productLoader;
        private readonly ILoader<Dictionary<string, IList<Ingredient>>> _ingredientProductMapLoader;
        private readonly IValidator<Order> _orderValidator;
        private readonly IValidator<Product> _productValidator;
        private readonly IValidator<Ingredient> _ingredientValidator;
        private readonly IFilter<Order> _orderFilter;
        private readonly IFilter<Product> _productFilter;
        private readonly IOrderProcessor _orderProcessor;
        private readonly IIngredientProcessor _ingredientProcessor;
        private readonly IOutput _output;
        private readonly ILogger<OrderProcessingManager> _logger;

        public OrderProcessingManager(
            ILoader<IList<Order>> orderLoader,
            ILoader<IList<Product>> productLoader,
            ILoader<Dictionary<string, IList<Ingredient>>> ingredientProductMapLoader,
            IValidator<Order> orderValidator,
            IValidator<Product> productValidator,
            IValidator<Ingredient> ingredientValidator,
            IFilter<Order> orderFilter,
            IFilter<Product> productFilter,
            IOrderProcessor orderProcessor,
            IIngredientProcessor ingredientProcessor,
            IOutput output,
            ILogger<OrderProcessingManager> logger)
        {
            _orderLoader = orderLoader;
            _productLoader = productLoader;
            _ingredientProductMapLoader = ingredientProductMapLoader;
            _orderValidator = orderValidator;
            _productValidator = productValidator;
            _ingredientValidator = ingredientValidator;
            _orderFilter = orderFilter;
            _productFilter = productFilter;
            _orderProcessor = orderProcessor;
            _ingredientProcessor = ingredientProcessor;
            _output = output;
            _logger = logger;
        }

        public void Run()
        {
            try
            {
                // Load JSON data from files
                var orders = _orderLoader.Load(OrdersJsonPath);
                var products = _productLoader.Load(ProductsJsonPath);
                var ingredientProductMap = _ingredientProductMapLoader.Load(IngredientsJsonPath);

                // Validate and filter products
                products = products.Where(_productValidator.Validate).ToList();
                products = _productFilter.Filter(products);

                // Get only products with valid ingredients
                products = DataRefiner.GetProductsWithValidIngredients(
                    products, ingredientProductMap, _ingredientValidator, _logger).ToList();

                if (products.Count == 0)
                {
                    _logger.LogWarning("No valid products remain after validation and filtering!");
                    return;
                }

                var validProductIds = new HashSet<string>(products.Select(p => p.ProductId));

                // Filter orders to only those where all products are valid
                orders = orders.GroupBy(o => o.OrderId)
                    .Where(g => g.All(o => validProductIds.Contains(o.ProductId)))
                    .SelectMany(g => g).ToList();

                // Validate and filter orders
                orders = orders.Where(_orderValidator.Validate).ToList();
                orders = _orderFilter.Filter(orders);

                if (orders.Count == 0)
                {
                    _logger.LogWarning("No valid orders remain after validation and filtering!");
                    return;
                }

                var productMap = products.ToDictionary(p => p.ProductId);

                // Process orders and calculate totals
                var orderTotals = _orderProcessor.CalculateOrderTotals(orders, productMap);
                var totalIngredients = _ingredientProcessor.CalculateTotalIngredients(orders, productMap, ingredientProductMap);

                // Output results
                _output.OutputCompleteOrderDetails(orders, productMap);
                _output.OutputOrderTotals(orderTotals);
                _output.OutputTotalIngredients(totalIngredients);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return;
            }
        }
    }
}