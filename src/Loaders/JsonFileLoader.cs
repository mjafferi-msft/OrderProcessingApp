using Newtonsoft.Json;

namespace OrderProcessingApp.Loaders
{
    public class JsonFileLoader<T> : ILoader<T>
    {
        public T Load(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File not found: {filePath}");
            }

            var json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<T>(json) ?? throw new InvalidOperationException($"Failed to deserialize data from {filePath}");
        }
    }
}