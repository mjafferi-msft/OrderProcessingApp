using Newtonsoft.Json;

namespace OrderProcessingApp.Loaders
{
    public class JsonFileLoader<T> : ILoader<T>
    {
        /// <summary>
        /// Loads data from a JSON file and deserializes it into an object of type T.
        /// </summary>
        public T Load(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File not found: {filePath}");
            }

            try
            {
                var json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<T>(json)
                    ?? throw new InvalidOperationException($"Failed to deserialize data from {filePath}");
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException($"Malformed JSON in file {filePath}: {ex.Message}");
            }
        }
    }
}