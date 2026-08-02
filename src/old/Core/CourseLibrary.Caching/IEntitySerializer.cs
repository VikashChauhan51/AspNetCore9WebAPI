namespace CourseLibrary.Caching;

using System.Threading;
using System.Threading.Tasks;

public interface IEntitySerializer
{
    /// <summary>
    /// Asynchronously serializes an entity to a byte array.
    /// </summary>
    Task<byte[]> SerializeToBytesAsync<T>(T entity, CancellationToken cancellationToken);

    /// <summary>
    /// Asynchronously deserializes a byte array to an entity.
    /// </summary>
    Task<T?> DeserializeFromBytesAsync<T>(byte[] data, CancellationToken cancellationToken);

    /// <summary>
    /// Asynchronously serializes an entity to a string.
    /// </summary>
    Task<string> SerializeToStringAsync<T>(T entity, CancellationToken cancellationToken);

    /// <summary>
    /// Asynchronously deserializes a string to an entity.
    /// </summary>
    Task<T?> DeserializeFromStringAsync<T>(string data, CancellationToken cancellationToken);
}

