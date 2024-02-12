using System;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;

namespace com.mahonkin.tim.UnifiedLogging;

public enum OSLogType : uint
{
    OS_LOG_TYPE_DEFAULT = 00,
    OS_LOG_TYPE_INFO = 01,
    OS_LOG_TYPE_DEBUG = 02,
    OS_LOG_TYPE_ERROR = 10,
    OS_LOG_TYPE_FAULT = 11
}

public static class OSLog
{
    public static OSLogType GetOSLogType(LogLevel level)
    {
        switch (level)
        {
            case LogLevel.Trace: return OSLogType.OS_LOG_TYPE_DEFAULT;
            case LogLevel.Debug: return OSLogType.OS_LOG_TYPE_DEBUG;
            case LogLevel.Information: return OSLogType.OS_LOG_TYPE_INFO;
            case LogLevel.Warning: return OSLogType.OS_LOG_TYPE_INFO;
            case LogLevel.Error: return OSLogType.OS_LOG_TYPE_ERROR;
            case LogLevel.Critical: return OSLogType.OS_LOG_TYPE_FAULT;
            case LogLevel.None: return OSLogType.OS_LOG_TYPE_DEFAULT;
            default: return OSLogType.OS_LOG_TYPE_DEFAULT;
        }
    }

    [DllImport("libOSLogNative.dylib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Create")]
    public static extern IntPtr Create(string subsytem, string category);

    [DllImport("libOSLogNative.dylib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogTypeEnabled")]
    public static extern bool LogTypeEnabled(IntPtr logPtr, OSLogType type);

    [DllImport("libOSLogNative.dylib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Log")]
    public static extern void Log(IntPtr logPtr, OSLogType type, string message);

    [DllImport("libOSLogNative.dylib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogTrace")]
    public static extern void LogTrace(IntPtr logPtr, string message);

    [DllImport("libOSLogNative.dylib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogDebug")]
    public static extern void LogDebug(IntPtr logPtr, string message);

    [DllImport("libOSLogNative.dylib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogInfo")]
    public static extern void LogInformation(IntPtr logPtr, string message);

    [DllImport("libOSLogNative.dylib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogWarning")]
    public static extern void LogWarning(IntPtr logPtr, string message);

    [DllImport("libOSLogNative.dylib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogError")]
    public static extern void LogError(IntPtr logPtr, string message);

    [DllImport("libOSLogNative.dylib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogCritical")]
    public static extern void LogCritical(IntPtr logPtr, string message);

    [DllImport("libOSLogNative.dylib", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogDefault")]
    public static extern void LogNone(IntPtr logPtr, string message);
}