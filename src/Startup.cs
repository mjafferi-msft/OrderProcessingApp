using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrderProcessingApp.Filters;
using OrderProcessingApp.Loaders;
using OrderProcessingApp.Models;
using OrderProcessingApp.Outputs;
using OrderProcessingApp.Processors;
using OrderProcessingApp.Validators;

namespace OrderProcessingApp
{
    /// <summary>
    /// Configures the services for the OrderProcessingApp application.
    /// </summary>
    public static class Startup
    {
        public static ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddTransient<OrderProcessingManager>();

            // Loaders
            services.AddTransient(typeof(ILoader<>), typeof(JsonFileLoader<>));

            // Validators
            services.AddTransient<IValidator<Order>, OrderValidator>();
            services.AddTransient<IValidator<Product>, ProductValidator>();
            services.AddTransient<IValidator<Ingredient>, IngredientValidator>();

            // Filters
            services.AddTransient<IFilter<Order>, OrderFilter>();
            services.AddTransient<IFilter<Product>, ProductFilter>();

            // Processors
            services.AddTransient<IOrderProcessor, OrderProcessor>();
            services.AddTransient<IIngredientProcessor, IngredientProcessor>();

            // Output
            services.AddTransient<IOutput, ConsoleOutput>();

            services.AddLogging(configure => configure.AddConsole());

            return services.BuildServiceProvider();
        }
    }
}