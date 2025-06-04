namespace OrderProcessingApp.Filters
{
    public interface IFilter<T>
    {
        IList<T> Filter(IEnumerable<T> items);
    }
}