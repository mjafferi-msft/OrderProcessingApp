namespace OrderProcessingApp.Loaders
{
    public interface ILoader<T>
    {
        T Load(string filePath);
    }
}