using Core.Tools.ObjectMapper;
using Microsoft.Extensions.DependencyInjection;

namespace UnitTests.ObjectMapper;

public class ObjectMapperDefaultTests
{
    private readonly IObjectMapper _mapper;

    public ObjectMapperDefaultTests()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IObjectMapper, Core.Tools.ObjectMapper.ObjectMapper>();
        var provider = services.BuildServiceProvider();
        _mapper = provider.GetRequiredService<IObjectMapper>();
    }

    private class SourceDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    private class DestEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        // Extra property not in Source: should remain default
        public bool IsActive { get; set; }
    }

    [Fact]
    public void Map_DefaultProperties_CopiesValuesProperly()
    {
        // Arrange
        var source = new SourceDto
        {
            Id = 42,
            Name = "TestName",
            CreatedAt = new DateTime(2021, 1, 1)
        };

        // Act
        var result = _mapper.Map<DestEntity>(source);

        // Assert
        Assert.Equal(42, result.Id);
        Assert.Equal("TestName", result.Name);
        Assert.Equal(new DateTime(2021, 1, 1), result.CreatedAt);
        Assert.False(result.IsActive); // default(bool)
    }
}