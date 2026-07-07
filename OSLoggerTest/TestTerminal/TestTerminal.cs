using System;
using com.mahonkin.tim.extensions.Logging;
using com.mahonkin.tim.Logging;
using Microsoft.Extensions.Logging;

namespace com.mahonkin.tim.LoggingTest;

public static class LoggingTest
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        IntPtr logPtr = OSLogger.Create(nameof(LoggingTest), nameof(Main));
        foreach (LogLevel currentLevel in Enum.GetValues(typeof(LogLevel)))
        {
            OSLogger.Log(logPtr, currentLevel.LogType(), $"{currentLevel} is enabled: {OSLogger.IsEnabled(logPtr, currentLevel.LogType())}");
            OSLogger.Log(logPtr, currentLevel.LogType(), $"Logging {currentLevel} as {currentLevel.LogType()}");
        }
        OSLogger.LogTrace(logPtr, $"{LogLevel.Trace}");
        OSLogger.LogDebug(logPtr, $"{LogLevel.Debug}");
        OSLogger.LogInformation(logPtr, $"{LogLevel.Information}");
        OSLogger.LogWarning(logPtr, $"{LogLevel.Warning}");
        OSLogger.LogError(logPtr, $"{LogLevel.Error}");
        OSLogger.LogCritical(logPtr, $"{LogLevel.Critical}");
        OSLogger.LogNone(logPtr, $"{LogLevel.None}");
    }
}
