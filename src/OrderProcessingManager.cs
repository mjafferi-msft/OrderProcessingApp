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
        private readonly ILoader<List<Order>> _orderLoader;
        private readonly ILoader<List<Product>> _productLoader;
        private readonly ILoader<Dictionary<string, List<Ingredient>>> _ingredientProductMapLoader;
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
            ILoader<List<Order>> orderLoader,
            ILoader<List<Product>> productLoader,
            ILoader<Dictionary<string, List<Ingredient>>> ingredientProductMapLoader,
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
            // Load JSON data from files
            var orders = _orderLoader.Load(OrdersJsonPath) ?? [];
            var products = _productLoader.Load(ProductsJsonPath) ?? [];
            var ingredientProductMap = _ingredientProductMapLoader.Load(IngredientsJsonPath) ?? [];
            Console.WriteLine($"Total orders loaded from JSON: {orders.Count}");
            Console.WriteLine($"Total product loaded from JSON: {products.Count}");

            // Validating orders and products
            orders = orders.Where(_orderValidator.Validate).ToList();
            products = products.Where(_productValidator.Validate).ToList();
            Console.WriteLine($"Total product after validation: {products.Count}");

            // Filtering orders and products
            orders = _orderFilter.Filter(orders);
            products = _productFilter.Filter(products);

            // Remove orders and products based on ingredient validation
            DataRefiner.RemoveInvalidIngredientRefs(orders, products, ingredientProductMap, _ingredientValidator, _logger);

            var productMap = products.ToDictionary(p => p.ProductId);
            Console.WriteLine($"Valid orders after validation and filtering: {orders.Count}");
            Console.WriteLine($"Valid product after validation and filtering: {products.Count}");


            // Process orders and calculate totals
            var orderTotals = _orderProcessor.CalculateOrderTotals(orders, productMap);
            var totalIngredients = _ingredientProcessor.CalculateTotalIngredients(orders, productMap, ingredientProductMap);

            // Output results
            if (orderTotals.Count == 0)
            {
                _output.OutputNoValidOrders();
            }
            else
            {
                _output.OutputOrderTotals(orderTotals);
                _output.OutputTotalIngredients(totalIngredients);
            }
        }
    }
}