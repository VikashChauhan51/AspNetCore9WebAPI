namespace CourseLibrary.Caching;

using System.Text.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class JsonEntitySerializer : IEntitySerializer
{
    private readonly JsonSerializerOptions _options;

    public JsonEntitySerializer(JsonSerializerOptions? options = null)
    {
        _options = options ?? new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
    }

    public async Task<byte[]> SerializeToBytesAsync<T>(T entity, CancellationToken cancellationToken = default)
    {
        using var memoryStream = new MemoryStream();
        await JsonSerializer.SerializeAsync(memoryStream, entity, _options, cancellationToken);
        return memoryStream.ToArray();
    }

    public async Task<string> SerializeToStringAsync<T>(T entity, CancellationToken cancellationToken = default)
    {
        using var memoryStream = new MemoryStream();
        await JsonSerializer.SerializeAsync(memoryStream, entity, _options, cancellationToken);
        memoryStream.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(memoryStream, Encoding.UTF8);
        return await reader.ReadToEndAsync();
    }

    public async Task<T?> DeserializeFromBytesAsync<T>(byte[] data, CancellationToken cancellationToken = default)
    {
        using var memoryStream = new MemoryStream(data);
        return await JsonSerializer.DeserializeAsync<T>(memoryStream, _options, cancellationToken);
    }

    public async Task<T?> DeserializeFromStringAsync<T>(string json, CancellationToken cancellationToken = default)
    {
        var bytes = Encoding.UTF8.GetBytes(json);
        return await DeserializeFromBytesAsync<T>(bytes, cancellationToken);
    }
}