using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace OrderProcessingApp.Loaders
{
    /// <summary>
    /// Loads data from a JSON file and deserializes it into an object of type T.
    /// </summary>
    public class JsonFileLoader<T> : ILoader<T>
    {
        private readonly ILogger<JsonFileLoader<T>> _logger;

        public JsonFileLoader(ILogger<JsonFileLoader<T>> logger)
        {
            _logger = logger;
        }

        public T? Load(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    _logger.LogError("File not found: {FilePath}", filePath);
                    return default;
                }
                var json = File.ReadAllText(filePath);
                var result = JsonConvert.DeserializeObject<T>(json);
                if (result == null)
                {
                    _logger.LogError("Deserialization returned null for file: {FilePath}", filePath);
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load or deserialize file: {FilePath}", filePath);
                return default;
            }
        }
    }
}