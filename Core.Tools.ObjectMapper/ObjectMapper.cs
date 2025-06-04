using System.Collections.Concurrent;
using System.Reflection;

namespace Core.Tools.ObjectMapper;


/// A simple reflection-based object mapper.
/// It will:
///  1. Check if a custom IMapping<TSource, TDst> exists in the DI container;
///  2. If so, use that mapping to configure a fresh instance of TDst;
///  3. Otherwise, copy all public, writable properties from source to TDst by name if types are compatible.
public class ObjectMapper : IObjectMapper
{
    private readonly IServiceProvider _serviceProvider;
    private static readonly ConcurrentDictionary<(Type, Type), PropertyInfo[]> _cachedProperties
        = new();

    public ObjectMapper(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public TDst Map<TDst>(object source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        // Create a new TDst using its parameterless constructor
        var destination = Activator.CreateInstance<TDst>();

        // Try to find a custom IMapping<S, D> for this pair
        var mappingInterface = typeof(IMapping<,>).MakeGenericType(source.GetType(), typeof(TDst));
        var customMapping = _serviceProvider.GetService(mappingInterface);
        if (customMapping != null)
        {
            // Invoke Configure(source, destination)
            var configureMethod = mappingInterface.GetMethod(nameof(IMapping<object, object>.Configure))!;
            configureMethod.Invoke(customMapping, new[] { source, destination! });
            return destination;
        }

        // Otherwise, do default reflection-based property copy
        CopyProperties(source, destination!);
        return destination!;
    }
    
    public void Map<TSource, TDst>(TSource source, TDst destination)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (destination == null) throw new ArgumentNullException(nameof(destination));

        var mappingInterface = typeof(IMapping<,>).MakeGenericType(typeof(TSource), typeof(TDst));
        var customMapping = _serviceProvider.GetService(mappingInterface);
        if (customMapping != null)
        {
            var configureMethod = mappingInterface.GetMethod(nameof(IMapping<object, object>.Configure))!;
            configureMethod.Invoke(customMapping, new object[] { source, destination! });
            return;
        }

        CopyProperties(source!, destination!);
    }

    private static void CopyProperties(object source, object destination)
    {
        var key = (source.GetType(), destination.GetType());
        var props = _cachedProperties.GetOrAdd(key, _ =>
        {
            return source.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead)
                .Join(
                    destination.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .Where(q => q.CanWrite),
                    p => p.Name,
                    q => q.Name,
                    (p, q) => new { SourceProp = p, DestProp = q }
                )
                .Where(x => x.DestProp.PropertyType.IsAssignableFrom(x.SourceProp.PropertyType))
                .Select(x => x.SourceProp)
                .ToArray();
        });

        foreach (var prop in props)
        {
            var destProp = destination.GetType().GetProperty(prop.Name)!;
            var value = prop.GetValue(source);
            destProp.SetValue(destination, value);
        }
    }
}