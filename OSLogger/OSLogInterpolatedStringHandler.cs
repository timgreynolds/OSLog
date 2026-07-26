using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace com.mahonkin.tim.Logging.OSLog;

/// <inheritdoc cref="DefaultInterpolatedStringHandler"/>
/// <remarks></remarks>
[InterpolatedStringHandler]
public struct OSLogInterpolatedStringHandler
{
    private StringBuilder _builder;
    private OSLogFormatter _formatter;

    /// <inheritdoc cref="DefaultInterpolatedStringHandler(int, int)"/>
    /// <remarks></remarks>
    public OSLogInterpolatedStringHandler(int literalLength, int formattedCount)
    {
        _builder = new StringBuilder(literalLength + (formattedCount * 16));
        _formatter = new OSLogFormatter();
    }

    /// <inheritdoc cref="DefaultInterpolatedStringHandler.AppendLiteral(string)"/>
    /// <remarks></remarks>
    public void AppendLiteral(string s) => _builder.Append(s);

    /// <inheritdoc cref="DefaultInterpolatedStringHandler.AppendFormatted(object?, int, string?)"/>
    /// <remarks></remarks>
    public void AppendFormatted<T>(T value) => _builder.Append(value);

    /// <inheritdoc cref="DefaultInterpolatedStringHandler.AppendFormatted(object?, int, string?)"/>
    /// <remarks></remarks>
    public void AppendFormatted<T>(T value, string format)
    {
        string formatted = _formatter.Format(format, value);
        _builder.Append(formatted);
    }

    /// <inheritdoc cref="DefaultInterpolatedStringHandler.AppendFormatted(object?, int, string?)"/>
    /// <remarks></remarks>
    public void AppendFormatted<T>(T value, int alignment, string format)
    {
        string formatted = _formatter.Format(format, value);
        int padding = Math.Max(0, Math.Abs(alignment) - formatted.Length);
        if (alignment > 0)
        {
            _builder.Append(' ', padding);
            _builder.Append(formatted);
        }
        else
        {
            _builder.Append(formatted);
            _builder.Append(' ', padding);
        }
    }

    /// <inheritdoc cref="StringBuilder.ToString()" />
    /// <remarks></remarks>
    public override string ToString() => _builder.ToString();
}