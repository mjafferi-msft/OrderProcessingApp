using Newtonsoft.Json;

using static OrderProcessingApp.Constants.AppConstants;

namespace OrderProcessingApp.Loaders
{
    /// <summary>
    /// Loads data from a JSON file and deserializes it into an object of type T.
    /// </summary>
    public class JsonFileLoader<T> : ILoader<T>
    {
        public T Load(string filePath)
        {
            try
            {
                filePath = Path.Combine(AppContext.BaseDirectory, InputDirectory, filePath);
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException($"File not found: {filePath}");
                }

                var json = File.ReadAllText(filePath);
                if (string.IsNullOrWhiteSpace(json) || json.Trim() == "[]" || json.Trim() == "{}")
                {
                    throw new InvalidDataException($"File is empty: {filePath}");
                }

                var result = JsonConvert.DeserializeObject<T>(json);
                if (result == null)
                {
                    throw new InvalidDataException($"Deserialization returned null for file: {filePath}");
                }

                return result;
            }
            catch (JsonException)
            {
                throw new InvalidDataException($"Invalid JSON format in file: {filePath}");
            }
        }
    }
}