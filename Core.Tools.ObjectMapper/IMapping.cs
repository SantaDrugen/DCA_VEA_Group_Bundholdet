namespace Core.Tools.ObjectMapper;

public interface IMapping<TSource,TDst>
{
    /// Configure mapping from source to destination.
    /// The implementation will be invoked by the ObjectMapper to apply special rules.
    /// <param name="source">The source object.</param>
    /// <param name="destination">The destination object to populate.</param>
    void Configure(TSource source, TDst destination);
}