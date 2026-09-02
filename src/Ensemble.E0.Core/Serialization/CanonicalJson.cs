using System.Text;

namespace Ensemble.E0.Core.Serialization;

internal static class CanonicalJson
{
    private const string HexDigits = "0123456789abcdef";
    private static readonly Encoding Utf8 = new UTF8Encoding(
        encoderShouldEmitUTF8Identifier: false,
        throwOnInvalidBytes: true);

    public static byte[] EncodeUtf8(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return Utf8.GetBytes(value);
    }

    public static void AppendString(StringBuilder builder, string value)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(value);

        builder.Append('"');
        for (var index = 0; index < value.Length; index++)
        {
            var character = value[index];
            switch (character)
            {
                case '"':
                    builder.Append("\\\"");
                    break;
                case '\\':
                    builder.Append("\\\\");
                    break;
                case '\b':
                    builder.Append("\\b");
                    break;
                case '\t':
                    builder.Append("\\t");
                    break;
                case '\n':
                    builder.Append("\\n");
                    break;
                case '\f':
                    builder.Append("\\f");
                    break;
                case '\r':
                    throw new CanonicalJsonException(
                        "Canonical JSON string contains a carriage return.");
                default:
                    if (character <= '\u001F')
                    {
                        builder.Append("\\u00");
                        builder.Append(HexDigits[(character >> 4) & 0x0F]);
                        builder.Append(HexDigits[character & 0x0F]);
                    }
                    else if (char.IsHighSurrogate(character))
                    {
                        if (index + 1 >= value.Length || !char.IsLowSurrogate(value[index + 1]))
                        {
                            throw new CanonicalJsonException(
                                "Canonical JSON string contains an invalid Unicode surrogate sequence.");
                        }

                        builder.Append(character);
                        builder.Append(value[++index]);
                    }
                    else if (char.IsLowSurrogate(character))
                    {
                        throw new CanonicalJsonException(
                            "Canonical JSON string contains an invalid Unicode surrogate sequence.");
                    }
                    else
                    {
                        builder.Append(character);
                    }

                    break;
            }
        }

        builder.Append('"');
    }

    public static void AppendPropertyName(StringBuilder builder, string name)
    {
        AppendString(builder, name);
        builder.Append(':');
    }

    public static void AppendSeparator(StringBuilder builder, ref bool first)
    {
        if (!first)
        {
            builder.Append(',');
        }

        first = false;
    }
}

internal sealed class CanonicalJsonException : Exception
{
    public CanonicalJsonException(string message)
        : base(message)
    {
    }
}
