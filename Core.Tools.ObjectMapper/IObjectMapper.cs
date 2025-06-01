namespace Core.Tools.ObjectMapper;

public interface IObjectMapper
{
    /// <typeparam name="TDst">The destination type.</typeparam>
    /// <param name="source">The source object.</param>
    TDst Map<TDst>(object source);
    
    /// <typeparam name="TSource">The source type.</typeparam>
    /// <typeparam name="TDst">The destination type.</typeparam>
    /// <param name="source">The source object.</param>
    void Map<TSource, TDst>(TSource source, TDst destination);
}