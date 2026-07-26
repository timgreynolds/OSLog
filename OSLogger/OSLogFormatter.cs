using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace com.mahonkin.tim.Logging.OSLog;

/// <summary>
/// Implementation of <see cref="IFormatProvider"/> and <see cref="ICustomFormatter"/> 
/// that adds "Priv" and "Mask" as custom format strings, allowing the user the redact
/// personal or other sensitive information from <see cref="OSLogger"/> log messages.
/// </summary>
public class OSLogFormatter : IFormatProvider, ICustomFormatter
{
    /// <inheritdoc cref="ICustomFormatter.Format(string?, object?, IFormatProvider?)"/>
    /// <remarks></remarks>
    public string Format(string? format, object? arg, IFormatProvider? provider = null)
    {
        if (arg is null) return "<null>";
        switch (format)
        {
            case "Priv": return "<Private>";
            case "Mask": return GenerateHash(arg);
            default: return HandleOtherFormats(format, arg);
        }
    }

    /// <inheritdoc cref="IFormatProvider.GetFormat(Type)"/>
    /// <remarks></remarks>
    public object? GetFormat(Type? formatType)
    {
        if (formatType == typeof(ICustomFormatter))
        {
            return this;
        }
        else
        {
            return null;
        }
    }

    private string GenerateHash(object arg)
    {
        byte[] bytes = (Type.GetTypeCode(arg.GetType()))
        switch
        {
            TypeCode.Char => BitConverter.GetBytes((char)arg),
            TypeCode.Byte => BitConverter.GetBytes((ushort)arg),
            TypeCode.SByte => BitConverter.GetBytes((short)arg),
            TypeCode.Int16 => BitConverter.GetBytes((short)arg),
            TypeCode.Int32 => BitConverter.GetBytes((int)arg),
            TypeCode.Int64 => BitConverter.GetBytes((long)arg),
            TypeCode.String => Encoding.Default.GetBytes((string)arg),
            TypeCode.UInt16 => BitConverter.GetBytes((ushort)arg),
            TypeCode.UInt32 => BitConverter.GetBytes((uint)arg),
            TypeCode.UInt64 => BitConverter.GetBytes((ulong)arg),
            TypeCode.Single => BitConverter.GetBytes((float)arg),
            TypeCode.Double => BitConverter.GetBytes((double)arg),
            TypeCode.Object => JsonSerializer.SerializeToUtf8Bytes(arg, JsonSerializerOptions.Default),
            TypeCode.Decimal => BitConverter.GetBytes((double)arg),
            TypeCode.Boolean => BitConverter.GetBytes((bool)arg),
            TypeCode.DateTime => BitConverter.GetBytes(((DateTime)arg).Ticks),
            _ => BitConverter.GetBytes(DateTimeOffset.Now.Ticks)
        };
        byte[] hash = SHA1.HashData(bytes);
        Span<byte> slice = hash.AsSpan<byte>().Slice(1, 18);
        return "<" + Convert.ToBase64String(slice) + ">";
    }

    private string HandleOtherFormats(string? format, object arg)
    {
        try
        {
            if (arg is IFormattable formattable)
            {
                return formattable.ToString(format, CultureInfo.CurrentCulture);
            }
            return arg.ToString() ?? "<null>";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
}