using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using com.mahonkin.tim.logging;
using Microsoft.Extensions.Logging;

namespace com.mahonkin.tim.logging
{
    /// <summary>
    /// Apple Unified Logging <see href="https://developer.apple.com/documentation/os/oslogtype">LogType</see> enum.
    /// </summary>
    public enum OSLogType : uint
    {
        /// Apple Unified Logging OSLogType <see href="https://developer.apple.com/documentation/os/oslogtype/2320721-default">default.</see>
        OS_LOG_TYPE_DEFAULT = 00,
        /// Apple Unified Logging OSLogType <see href="https://developer.apple.com/documentation/os/oslogtype/2320719-info">info.</see>
        OS_LOG_TYPE_INFO = 01,
        /// Apple Unified Logging OSLogType <see href="https://developer.apple.com/documentation/os/oslogtype/2320717-debug">debug.</see>
        OS_LOG_TYPE_DEBUG = 02,
        /// Apple Unified Logging OSLogType <see href="https://developer.apple.com/documentation/os/oslogtype/2320727-error">error.</see>
        OS_LOG_TYPE_ERROR = 10,
        /// Apple Unified Logging OSLogType <see href="https://developer.apple.com/documentation/os/oslogtype/2320725-fault">fault.</see>
        OS_LOG_TYPE_FAULT = 11
    }

    /// <summary>
    /// Class the represents an Apple Unified Logging OSLog object.
    /// </summary>
    public static class OSLogger
    {
        #region Private Fields
        private static readonly Dictionary<LogLevel, OSLogType> _logTypes = new Dictionary<LogLevel, OSLogType>
        {
            {LogLevel.None, OSLogType.OS_LOG_TYPE_DEFAULT},
            {LogLevel.Trace, OSLogType.OS_LOG_TYPE_DEFAULT},
            {LogLevel.Debug, OSLogType.OS_LOG_TYPE_DEBUG},
            {LogLevel.Information, OSLogType.OS_LOG_TYPE_INFO},
            {LogLevel.Warning, OSLogType.OS_LOG_TYPE_ERROR},
            {LogLevel.Error, OSLogType.OS_LOG_TYPE_ERROR},
            {LogLevel.Critical, OSLogType.OS_LOG_TYPE_FAULT}
        };
        #endregion private Fields

        #region Public Methods
        /// <summary>
        /// Constructor/Initializer for an <see href="https://developer.apple.com/documentation/os/logger/3551621-init">OSLog</see> object.
        /// </summary>
        /// <remarks>
        /// Creates a native object that can be used to write messsages to the Unified Logging framework with the specified parameters.
        /// <br />The Apple documentation defines <paramref name="subsystem"/> and <paramref name="category"/> in a way that seems backwards. Traditionally when used with an ILoggerProvider it is the category that is the reverse DNS notation name.
        /// </remarks>
        /// <param name="subsystem">Apple documentation defines subsystem this way:<br />An identifier string, in reverse DNS notation, that represents the app subsystem that’s logging information, such as com.your_company.your_subsystem_name. The logging system uses this information to categorize and filter related log messages, and to group related logging settings.</param>
        /// <param name="category">Apple documentation defines category this way:<br />A category within the specified subsystem. The system uses this value to categorize and filter related log messages, and to group related logging settings within the subsystem. A category’s logging settings override those of the containing subsystem.</param>
        [DllImport("libOSLogNative", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Create")]
        public static extern IntPtr Create(string subsystem, string category);

        /// <summary>
        /// Returns the mapped <see cref="OSLogType">LogType</see> for a given LogLevel. 
        /// </summary>
        /// <param name="level">The LogLevel to be mapped.</param>
        /// <returns>The LogType that corresponds to the given LogLevel.</returns>
        public static OSLogType GetOsLogType(LogLevel level)
        {
            return _logTypes[level];
        }

        /// <summary>
        /// <see href="https://developer.apple.com/documentation/os/oslog/1643749-isenabled">IsEnabled()</see> 
        /// </summary>
        /// <param name="logPtr" />
        /// <param name="type">The <see cref="OSLogType">LogType</see> to check.</param>
        [DllImport("libOSLogNative", CallingConvention = CallingConvention.Cdecl, EntryPoint = "IsEnabled")]
        public static extern bool IsEnabled(IntPtr logPtr, OSLogType type);

        /// <summary>
        /// <see href="https://developer.apple.com/documentation/os/logger/3551622-log">Log()</see> 
        /// </summary>
        /// <param name="logPtr" />
        /// <param name="type">The message’s <see cref="OSLogType">LogType,</see> which determines the severity of the message and whether the system persists it to disk.</param>
        /// <param name="message">The string that the logger writes to the log. Interpolation and privacy options are not supported in this context.</param>
        [DllImport("libOSLogNative", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Log")]
        public static extern void Log(IntPtr logPtr, OSLogType type, string message);

        /// <summary>
        /// <see href="https://developer.apple.com/documentation/os/logger/3551624-trace">LogTrace()</see> 
        /// </summary>
        /// <param name="logPtr" />
        /// <param name="message">The string that the logger writes to the log. Interpolation and privacy options are not supported in this context.</param>
        [DllImport("libOSLogNative", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogTrace")]
        public static extern void LogTrace(IntPtr logPtr, string message);

        /// <summary>
        /// <see href="https://developer.apple.com/documentation/os/logger/3551615-debug">LogDebug()</see> 
        /// </summary>
        /// <param name="logPtr" />
        /// <param name="message">The string that the logger writes to the log. Interpolation and privacy options are not supported in this context.</param>
        [DllImport("libOSLogNative", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogDebug")]
        public static extern void LogDebug(IntPtr logPtr, string message);

        /// <summary>
        /// <see href="https://developer.apple.com/documentation/os/logger/3551618-info">LogInformation()</see> 
        /// </summary>
        /// <param name="logPtr" />
        /// <param name="message">The string that the logger writes to the log. Interpolation and privacy options are not supported in this context.</param>
        [DllImport("libOSLogNative", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogInfo")]
        public static extern void LogInformation(IntPtr logPtr, string message);

        /// <summary>
        /// <see href="https://developer.apple.com/documentation/os/logger/3551625-warning">LogWarning()</see> 
        /// </summary>
        /// <param name="logPtr" />
        /// <param name="message">The string that the logger writes to the log. Interpolation and privacy options are not supported in this context.</param>
        [DllImport("libOSLogNative", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogWarning")]
        public static extern void LogWarning(IntPtr logPtr, string message);

        /// <summary>
        /// <see href="https://developer.apple.com/documentation/os/logger/3551616-error">LogError()</see> 
        /// </summary>
        /// <param name="logPtr" />
        /// <param name="message">The string that the logger writes to the log. Interpolation and privacy options are not supported in this context.</param>
        [DllImport("libOSLogNative", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogError")]
        public static extern void LogError(IntPtr logPtr, string message);

        /// <summary>
        /// <see href="https://developer.apple.com/documentation/os/logger/3551614-critical">LogCritical()</see> 
        /// </summary>
        /// <param name="logPtr" />
        /// <param name="message">The string that the logger writes to the log. Interpolation and privacy options are not supported in this context.</param>
        [DllImport("libOSLogNative", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogCritical")]
        public static extern void LogCritical(IntPtr logPtr, string message);

        /// <summary>
        /// <see href="https://developer.apple.com/documentation/os/logger/3580304-log">LogNone()</see> 
        /// </summary>
        /// <param name="logPtr" />
        /// <param name="message">The string that the logger writes to the log. Interpolation and privacy options are not supported in this context.</param>
        [DllImport("libOSLogNative", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogDefault")]
        public static extern void LogNone(IntPtr logPtr, string message);
    }
    #endregion Public Methods
}

namespace com.mahonkin.tim.extensions.Logging
{
    /// <summary>
    /// Potentially helpful extension methods for LogType.
    /// </summary>
    public static class LogTypeExtensions
    {
        /// <summary>
        /// Extension method that returns the <see cref="OSLogType">LogType</see> corresponding to the LogLevel.
        /// </summary>
        public static OSLogType LogType(this LogLevel logLevel)
        {
            return OSLogger.GetOsLogType(logLevel);
        }
    }
}