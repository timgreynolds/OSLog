using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;

namespace com.mahonkin.tim.Logging.OSLog
{
    /// <summary>
    /// Apple Unified Logging <see href="https://developer.apple.com/documentation/os/oslogtype">LogType</see> enum.
    /// Defined in log.h
    /// </summary>
    public enum OSLogType : uint
    {
        /// Defined in log.h
        OS_LOG_TYPE_DEFAULT = 00,
        /// Defined in log.h
        OS_LOG_TYPE_INFO = 01,
        /// Defined in log.h
        OS_LOG_TYPE_DEBUG = 02,
        /// Defined in log.h
        OS_LOG_TYPE_ERROR = 10,
        /// Defined in log.h
        OS_LOG_TYPE_FAULT = 11
    }

    /// <summary>
    /// Class that provides access to an Apple Unified Logging OSLog object.
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
        /// Returns the mapped OSLogType for a given LogLevel. 
        /// </summary>
        /// <param name="level">The LogLevel to be mapped.</param>
        /// <returns>The LogType that corresponds to the given LogLevel.</returns>
        public static OSLogType GetOsLogType(LogLevel level)
        {
            return _logTypes[level];
        }

        /// <summary>
        /// Creates a logger using the specified subsystem and category.<br/>
        /// <see href="https://developer.apple.com/documentation/os/logger/3551621-init">OSLog</see>
        /// </summary>
        /// <remarks>
        /// Creates a native object that can be used to write messsages to the Unified Logging framework with the specified parameters.
        /// </remarks>
        /// <param name="subsystem">Apple documentation defines subsystem this way:<br />An identifier string, in reverse DNS notation, that represents the app subsystem that’s logging information, such as com.your_company.your_subsystem_name. The logging system uses this information to categorize and filter related log messages, and to group related logging settings.</param>
        /// <param name="category">Apple documentation defines category this way:<br />A category within the specified subsystem. The system uses this value to categorize and filter related log messages, and to group related logging settings within the subsystem. A category’s logging settings override those of the containing subsystem.</param>
        /// <returns>A pointer to the created OSLog instance.</returns>
        public static IntPtr Create(string subsystem, string category) => create(subsystem, category);
        [LibraryImport("libOSLogNative", EntryPoint = "Create")]
        private static partial IntPtr create([MarshalAs(UnmanagedType.LPStr)] string subsystem, [MarshalAs(UnmanagedType.LPStr)] string category);

        /// <summary>
        /// Returns a Boolean value that indicates whether the log can write messages with the specified log type.<br/>
        /// <see href="https://developer.apple.com/documentation/os/oslog/1643749-isenabled">IsEnabled()</see> 
        /// </summary>
        /// <param name="logPtr">Pointer to the OSLog instance.</param>
        /// <param name="type">The LogType to check.</param>
        /// <returns>True if logging at the specified level is in an enabled state; otherwise, False.</returns>
        public static bool IsEnabled(IntPtr logPtr, OSLogType type) => isEnabled(logPtr, type);
        [DllImport("libOSLogNative", EntryPoint = "IsEnabled")]
        private static extern bool isEnabled(IntPtr logPtr, OSLogType type);

        /// <summary>
        /// Writes a message to the log using the specified log type.<br/>
        /// <see href="https://developer.apple.com/documentation/os/logger/3551622-log">Log()</see> 
        /// </summary>
        /// <param name="logPtr">Pointer to the OSLog instance.</param>
        /// <param name="type">The message’s LogType, which determines the severity of the message and whether the system persists it to disk.</param>
        /// <param name="message">The string that the logger writes to the log. Interpolated arguments can be decorated with either ":Priv" or ":Mask" format strings in order to obscure sensitive information.</param>
        public static void Log(IntPtr logPtr, OSLogType type, string message)
        {
            if (OSLogger.IsEnabled(logPtr, type) == false) return;
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
        /// Writes a Trace message to the log.<br/>
        /// <see href="https://developer.apple.com/documentation/os/logger/3551624-trace">LogTrace()</see> 
        /// </summary>
        /// <param name="logPtr">Pointer to the OSLog instance.</param>
        /// <param name="message">The string that the logger writes to the log. Interpolated arguments can be decorated with either ":Priv" or ":Mask" format strings in order to obscure sensitive information.</param>
        public static void LogTrace(IntPtr logPtr, string message)
        {
            if (OSLogger.isEnabled(logPtr, OSLogType.OS_LOG_TYPE_DEFAULT) == false) return;
            logTrace(logPtr, message);
        }
        [LibraryImport("libOSLogNative", EntryPoint = "LogTrace")]
        private static partial void logTrace(IntPtr logPtr, [MarshalAs(UnmanagedType.LPStr)] string message);

        /// <summary>
        /// Writes a Debug message to the log.<br/>
        /// <see href="https://developer.apple.com/documentation/os/logger/3551615-debug">LogDebug()</see> 
        /// </summary>
        /// <param name="logPtr">Pointer to the OSLog instance.</param>
        /// <param name="message">The string that the logger writes to the log. Interpolated arguments can be decorated with either ":Priv" or ":Mask" format strings in order to obscure sensitive information.</param>
        public static void LogDebug(IntPtr logPtr, string message)
        {
            if (OSLogger.isEnabled(logPtr, OSLogType.OS_LOG_TYPE_DEBUG) == false) return;
            logDebug(logPtr, message);
        }
        [LibraryImport("libOSLogNative", EntryPoint = "LogDebug")]
        private static partial void logDebug(IntPtr logPtr, [MarshalAs(UnmanagedType.LPStr)] string message);

        /// <summary>
        /// Writes an Informational message to the log.<br/>
        /// <see href="https://developer.apple.com/documentation/os/logger/3551618-info">LogInformation()</see> 
        /// </summary>
        /// <param name="logPtr">Pointer to the OSLog instance.</param>
        /// <param name="message">The string that the logger writes to the log. Interpolated arguments can be decorated with either ":Priv" or ":Mask" format strings in order to obscure sensitive information.</param>
        public static void LogInformation(IntPtr logPtr, string message)
        {
            if (OSLogger.isEnabled(logPtr, OSLogType.OS_LOG_TYPE_INFO) == false) return;
            logInformation(logPtr, message);
        }
        [LibraryImport("libOSLogNative", EntryPoint = "LogInfo")]
        private static partial void logInformation(IntPtr logPtr, [MarshalAs(UnmanagedType.LPStr)] string message);

        /// <summary>
        /// Writes a Warning message to the log.<br/>
        /// <see href="https://developer.apple.com/documentation/os/logger/3551625-warning">LogWarning()</see> 
        /// </summary>
        /// <param name="logPtr">Pointer to the OSLog instance.</param>
        /// <param name="message">The string that the logger writes to the log. Interpolated arguments can be decorated with either ":Priv" or ":Mask" format strings in order to obscure sensitive information.</param>
        public static void LogWarning(IntPtr logPtr, string message)
        {
            if (OSLogger.isEnabled(logPtr, OSLogType.OS_LOG_TYPE_INFO) == false) return;
            logWarning(logPtr, message);
        }
        [LibraryImport("libOSLogNative", EntryPoint = "LogWarning")]
        private static partial void logWarning(IntPtr logPtr, [MarshalAs(UnmanagedType.LPStr)] string message);

        /// <summary>
        /// Writes an Error message to the log.<br/>
        /// <see href="https://developer.apple.com/documentation/os/logger/3551616-error">LogError()</see> 
        /// </summary>
        /// <param name="logPtr">Pointer to the OSLog instance.</param>
        /// <param name="message">The string that the logger writes to the log. Interpolated arguments can be decorated with either ":Priv" or ":Mask" format strings in order to obscure sensitive information.</param>
        public static void LogError(IntPtr logPtr, string message)
        {
            if (OSLogger.isEnabled(logPtr, OSLogType.OS_LOG_TYPE_ERROR) == false) return;
            logError(logPtr, message);
        }
        [LibraryImport("libOSLogNative", EntryPoint = "LogError")]
        private static partial void logError(IntPtr logPtr, [MarshalAs(UnmanagedType.LPStr)] string message);

        /// <summary>
        /// Writes a Critical/Failure message to the log.<br/>
        /// <see href="https://developer.apple.com/documentation/os/logger/3551614-critical">LogCritical()</see> 
        /// </summary>
        /// <param name="logPtr">Pointer to the OSLog instance.</param>
        /// <param name="message">The string that the logger writes to the log. Interpolated arguments can be decorated with either ":Priv" or ":Mask" format strings in order to obscure sensitive information.</param>
        public static void LogCritical(IntPtr logPtr, string message)
        {
            if (OSLogger.isEnabled(logPtr, OSLogType.OS_LOG_TYPE_FAULT) == false) return;
            logCritical(logPtr, message);
        }
        [LibraryImport("libOSLogNative", EntryPoint = "LogFault")]
        private static partial void logCritical(IntPtr logPtr, [MarshalAs(UnmanagedType.LPStr)] string message);

        /// <summary>
        /// Writes a Default message to the log.<br/>
        /// <see href="https://developer.apple.com/documentation/os/logger/3580304-log">LogNone()</see> 
        /// </summary>
        /// <param name="logPtr">Pointer to the OSLog instance.</param>
        /// <param name="message">The string that the logger writes to the log. Interpolated arguments can be decorated with either ":Priv" or ":Mask" format strings in order to obscure sensitive information.</param>
        public static void LogNone(IntPtr logPtr, string message)
        {
            if (OSLogger.isEnabled(logPtr, OSLogType.OS_LOG_TYPE_DEFAULT) == false) return;
            logNone(logPtr, message);
        }
        [LibraryImport("libOSLogNative", EntryPoint = "LogDefault")]
        private static partial void logNone(IntPtr logPtr, [MarshalAs(UnmanagedType.LPStr)] string message);

        /// 
        public static void Log(IntPtr logPtr, OSLogType logType, OSLogInterpolatedStringHandler builder)
        {
            string message = builder.ToString() ?? string.Empty;
            log(logPtr, logType, message);
        }

        ///
        public static void LogTrace(IntPtr logPtr, OSLogInterpolatedStringHandler builder)
        {
            string message = builder.ToString() ?? string.Empty;
            logTrace(logPtr, message);
        }

        ///
        public static void LogDebug(IntPtr logPtr, OSLogInterpolatedStringHandler builder)
        {
            string message = builder.ToString() ?? string.Empty;
            logDebug(logPtr, message);
        }

        ///
        public static void LogInformation(IntPtr logPtr, OSLogInterpolatedStringHandler builder)
        {
            string message = builder.ToString() ?? string.Empty;
            logInformation(logPtr, message);
        }

        ///
        public static void LogWarning(IntPtr logPtr, OSLogInterpolatedStringHandler builder)
        {
            string message = builder.ToString() ?? string.Empty;
            logWarning(logPtr, message);
        }

        ///
        public static void LogError(IntPtr logPtr, OSLogInterpolatedStringHandler builder)
        {
            string message = builder.ToString() ?? string.Empty;
            logError(logPtr, message);
        }

        ///
        public static void LogCritical(IntPtr logPtr, OSLogInterpolatedStringHandler builder)
        {
            string message = builder.ToString() ?? string.Empty;
            logCritical(logPtr, message);
        }

        ///
        public static void LogNone(IntPtr logPtr, OSLogInterpolatedStringHandler builder)
        {
            string message = builder.ToString() ?? string.Empty;
            logNone(logPtr, message);
        }
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
        /// Extension method that returns the LogType corresponding to the LogLevel.
        /// </summary>
        public static tim.Logging.OSLog.OSLogType LogType(this LogLevel logLevel)
        {
            return tim.Logging.OSLog.OSLogger.GetOsLogType(logLevel);
        }
    }
}
