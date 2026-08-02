using Microsoft.Extensions.Compliance.Redaction;

namespace CourseLibrary.Infrastructure.Observability.Logging.Redaction;

public sealed class EmailRedactor : Redactor
{
    private const string Mask = "****";


    public override int GetRedactedLength(
        ReadOnlySpan<char> input)
    {
        var atIndex = input.IndexOf('@');

        if (atIndex <= 3)
        {
            return Mask.Length;
        }

        return 3 + Mask.Length + input.Length - atIndex;
    }


    public override int Redact(
        ReadOnlySpan<char> source,
        Span<char> destination)
    {
        var atIndex = source.IndexOf('@');


        if (atIndex <= 3)
        {
            Mask.AsSpan()
                .CopyTo(destination);

            return Mask.Length;
        }


        var index = 0;


        // Keep first 3 chars
        source[..3]
            .CopyTo(destination);

        index += 3;


        // Mask middle
        Mask.AsSpan()
            .CopyTo(destination[index..]);

        index += Mask.Length;


        // Keep domain
        source[atIndex..]
            .CopyTo(destination[index..]);

        index += source.Length - atIndex;


        return index;
    }
}