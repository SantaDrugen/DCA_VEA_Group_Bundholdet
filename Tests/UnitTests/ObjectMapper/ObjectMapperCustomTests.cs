using Core.Tools.ObjectMapper;
using Microsoft.Extensions.DependencyInjection;

namespace UnitTests.ObjectMapper;

public class ObjectMapperCustomTests
{
    private readonly IObjectMapper _mapper;

    public ObjectMapperCustomTests()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IObjectMapper, Core.Tools.ObjectMapper.ObjectMapper>();

        // Register a custom mapping: map SourceCustom → DestCustom
        services.AddSingleton<IMapping<SourceCustom, DestCustom>, SourceCustomToDestCustomMapping>();

        var provider = services.BuildServiceProvider();
        _mapper = provider.GetRequiredService<IObjectMapper>();
    }

    private class SourceCustom
    {
        public string FullName { get; set; } = string.Empty;
        public int Age { get; set; }
    }

    private class DestCustom
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int Age { get; set; }
    }

    private class SourceCustomToDestCustomMapping : IMapping<SourceCustom, DestCustom>
    {
        public void Configure(SourceCustom source, DestCustom destination)
        {
            // Suppose FullName = "First Last"
            var parts = source.FullName.Split(' ', 2);
            destination.FirstName = parts[0];
            destination.LastName = parts.Length > 1 ? parts[1] : "";
            destination.Age = source.Age + 1; // arbitrary logic
        }
    }

    [Fact]
    public void Map_WithCustomMapping_InvokesCustomLogic()
    {
        // Arrange
        var source = new SourceCustom
        {
            FullName = "Alice Wonderland",
            Age = 30
        };

        // Act
        var dest = _mapper.Map<DestCustom>(source);

        // Assert
        Assert.Equal("Alice", dest.FirstName);
        Assert.Equal("Wonderland", dest.LastName);
        Assert.Equal(31, dest.Age);
    }
}