using System.Text.Json;

namespace UnitTests.QueryTests
{
    public static class JsonTestDataLoader
    {
        public static IReadOnlyList<T>? LoadEntities<T>(string fileName)
        {
            var path = Path.Combine(AppContext.BaseDirectory, "TestData", fileName);
            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<List<T>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
}
