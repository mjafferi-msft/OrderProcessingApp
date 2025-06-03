namespace OrderProcessingApp.Filters
{
    public interface IFilter<T>
    {
        List<T> Filter(List<T> items);
    }
}