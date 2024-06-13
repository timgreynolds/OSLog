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
    public static partial class OSLogger
    {
        #region Private Fields
        private static readonly Dictionary<LogLevel, OSLogType> _logTypes = new Dictionary<LogLevel, OSLogType>
        {
            {LogLevel.None, OSLogType.OS_LOG_TYPE_DEFAULT},
            {LogLevel.Trace, OSLogType.OS_LOG_TYPE_DEFAULT},
            {LogLevel.Debug, OSLogType.OS_LOG_TYPE_DEBUG},
            {LogLevel.Information, OSLogType.OS_LOG_TYPE_INFO},
            {LogLevel.Warning, OSLogType.OS_LOG_TYPE_INFO},
            {LogLevel.Error, OSLogType.OS_LOG_TYPE_ERROR},
            {LogLevel.Critical, OSLogType.OS_LOG_TYPE_FAULT}
        };
        #endregion private Fields

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
        /// Constructor/Initializer for an <see href="https://developer.apple.com/documentation/os/logger/3551621-init">OSLog</see> object.
        /// </summary>
        /// <remarks>
        /// Creates a native object that can be used to write messsages to the Unified Logging framework with the specified parameters.
        /// <br />The Apple documentation defines <paramref name="subsystem"/> and <paramref name="category"/> in a way that seems backwards. Traditionally when used with an ILoggerProvider it is the category that is the reverse DNS notation name.
        /// </remarks>
        /// <param name="subsystem">Apple documentation defines subsystem this way:<br />An identifier string, in reverse DNS notation, that represents the app subsystem that’s logging information, such as com.your_company.your_subsystem_name. The logging system uses this information to categorize and filter related log messages, and to group related logging settings.</param>
        /// <param name="category">Apple documentation defines category this way:<br />A category within the specified subsystem. The system uses this value to categorize and filter related log messages, and to group related logging settings within the subsystem. A category’s logging settings override those of the containing subsystem.</param>
        public static IntPtr Create(string subsystem, string category) => create(subsystem, category);
        [LibraryImport("libOSLogNative", EntryPoint = "Create")]
        private static partial IntPtr create([MarshalAs(UnmanagedType.LPStr)] string subsystem, [MarshalAs(UnmanagedType.LPStr)] string category);

        /// <summary>
        /// <see href="https://developer.apple.com/documentation/os/oslog/1643749-isenabled">IsEnabled()</see> 
        /// </summary>
        /// <param name="logPtr" />
        /// <param name="type">The <see cref="OSLogType">LogType</see> to check.</param>
        public static bool IsEnabled(IntPtr logPtr, OSLogType type) => isEnabled(logPtr, type);
        [DllImport("libOSLogNative", EntryPoint = "IsEnabled")]
        private static extern bool isEnabled(IntPtr logPtr, OSLogType type);

        /// <summary>
        /// <see href="https://developer.apple.com/documentation/os/logger/3551622-log">Log()</see> 
        /// </summary>
        /// <param name="logPtr" />
        /// <param name="type">The message’s <see cref="OSLogType">LogType,</see> which determines the severity of the message and whether the system persists it to disk.</param>
        /// <param name="message">The string that the logger writes to the log. Interpolation and privacy options are not supported in this context.</param>
        public static void Log(IntPtr logPtr, OSLogType type, string message)
        {
            if (type == OSLogType.OS_LOG_TYPE_ERROR)
            {
                logError(logPtr, message);
            }
            else if (type == OSLogType.OS_LOG_TYPE_FAULT)
            {
                logCritical(logPtr, message);
            }
            else
            {
                log(logPtr, type, message);
            }
        }
        [LibraryImport("libOSLogNative", EntryPoint = "Log")]
        private static partial void log(IntPtr logPtr, OSLogType type, [MarshalAs(UnmanagedType.LPStr)] string message);

        /// <summary>
        /// <see href="https://developer.apple.com/documentation/os/logger/3551624-trace">LogTrace()</see> 
        /// </summary>
        /// <param name="logPtr" />
        /// <param name="message">The string that the logger writes to the log. Interpolation and privacy options are not supported in this context.</param>
        public static void LogTrace(IntPtr logPtr, string message) => logTrace(logPtr, message);
        [LibraryImport("libOSLogNative", EntryPoint = "LogTrace")]
        private static partial void logTrace(IntPtr logPtr, [MarshalAs(UnmanagedType.LPStr)] string message);

        /// <summary>
        /// <see href="https://developer.apple.com/documentation/os/logger/3551615-debug">LogDebug()</see> 
        /// </summary>
        /// <param name="logPtr" />
        /// <param name="message">The string that the logger writes to the log. Interpolation and privacy options are not supported in this context.</param>
        public static void LogDebug(IntPtr logPtr, string message) => logDebug(logPtr, message);
        [LibraryImport("libOSLogNative", EntryPoint = "LogDebug")]
        private static partial void logDebug(IntPtr logPtr, [MarshalAs(UnmanagedType.LPStr)] string message);

        /// <summary>
        /// <see href="https://developer.apple.com/documentation/os/logger/3551618-info">LogInformation()</see> 
        /// </summary>
        /// <param name="logPtr" />
        /// <param name="message">The string that the logger writes to the log. Interpolation and privacy options are not supported in this context.</param>
        public static void LogInformation(IntPtr logPtr, string message) => logInformation(logPtr, message);
        [LibraryImport("libOSLogNative", EntryPoint = "LogInfo")]
        private static partial void logInformation(IntPtr logPtr, [MarshalAs(UnmanagedType.LPStr)] string message);

        /// <summary>
        /// <see href="https://developer.apple.com/documentation/os/logger/3551625-warning">LogWarning()</see> 
        /// </summary>
        /// <param name="logPtr" />
        /// <param name="message">The string that the logger writes to the log. Interpolation and privacy options are not supported in this context.</param>
        public static void LogWarning(IntPtr logPtr, string message) => logWarning(logPtr, message);
        [LibraryImport("libOSLogNative", EntryPoint = "LogWarning")]
        private static partial void logWarning(IntPtr logPtr, [MarshalAs(UnmanagedType.LPStr)] string message);

        /// <summary>
        /// <see href="https://developer.apple.com/documentation/os/logger/3551616-error">LogError()</see> 
        /// </summary>
        /// <param name="logPtr" />
        /// <param name="message">The string that the logger writes to the log. Interpolation and privacy options are not supported in this context.</param>
        public static void LogError(IntPtr logPtr, string message) => logError(logPtr, message);
        [LibraryImport("libOSLogNative", EntryPoint = "LogError")]
        private static partial void logError(IntPtr logPtr, [MarshalAs(UnmanagedType.LPStr)] string message);

        /// <summary>
        /// <see href="https://developer.apple.com/documentation/os/logger/3551614-critical">LogCritical()</see> 
        /// </summary>
        /// <param name="logPtr" />
        /// <param name="message">The string that the logger writes to the log. Interpolation and privacy options are not supported in this context.</param>
        public static void LogCritical(IntPtr logPtr, string message) => logCritical(logPtr, message);
        [LibraryImport("libOSLogNative", EntryPoint = "LogFault")]
        private static partial void logCritical(IntPtr logPtr, [MarshalAs(UnmanagedType.LPStr)] string message);

        /// <summary>
        /// <see href="https://developer.apple.com/documentation/os/logger/3580304-log">LogNone()</see> 
        /// </summary>
        /// <param name="logPtr" />
        /// <param name="message">The string that the logger writes to the log. Interpolation and privacy options are not supported in this context.</param>
        public static void LogNone(IntPtr logPtr, string message) => logNone(logPtr, message);
        [LibraryImport("libOSLogNative", EntryPoint = "LogDefault")]
        private static partial void logNone(IntPtr logPtr, [MarshalAs(UnmanagedType.LPStr)] string message);
    }
}

namespace com.mahonkin.tim.extensions.Logging
{
    /// <summary>
    /// Some potentially helpful extension methods.
    /// </summary>
    public static class LogExtensions
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