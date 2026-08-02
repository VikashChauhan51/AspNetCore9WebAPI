using Microsoft.Extensions.Compliance.Redaction;

namespace CourseLibrary.Infrastructure.Observability.Logging.Redaction;

public sealed class PartialMaskingRedactor : Redactor
{
    private readonly int _prefixLength;
    private readonly int _suffixLength;
    private readonly char _maskCharacter;

    public PartialMaskingRedactor(
        int prefixLength = 2,
        int suffixLength = 2,
        char maskCharacter = '*')
    {
        _prefixLength = prefixLength;
        _suffixLength = suffixLength;
        _maskCharacter = maskCharacter;
    }


    public override int GetRedactedLength(
        ReadOnlySpan<char> input)
    {
        if (input.Length <= _prefixLength + _suffixLength)
        {
            return input.Length;
        }

        return input.Length;
    }


    public override int Redact(
        ReadOnlySpan<char> source,
        Span<char> destination)
    {
        if (source.Length <= _prefixLength + _suffixLength)
        {
            source.CopyTo(destination);
            return source.Length;
        }


        var index = 0;


        source[.._prefixLength]
            .CopyTo(destination);

        index += _prefixLength;


        var maskLength =
            source.Length -
            (_prefixLength + _suffixLength);


        for (var i = 0; i < maskLength; i++)
        {
            destination[index++] = _maskCharacter;
        }


        source[^_suffixLength..]
            .CopyTo(destination[index..]);

        index += _suffixLength;


        return index;
    }
}