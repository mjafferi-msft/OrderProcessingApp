using Microsoft.Extensions.DependencyInjection;

namespace OrderProcessingApp
{

    /// <summary>
    /// Entry point for the OrderProcessingApp application. It configures services and starts the order processing.
    /// </summary>
    class Program
    {
        public static void Main()
        {
            var serviceProvider = Startup.ConfigureServices();
            var app = serviceProvider.GetRequiredService<OrderProcessingManager>();
            app.Run();
        }
    }
}