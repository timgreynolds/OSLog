using System;
using System.Collections;
using com.mahonkin.tim.extensions.Logging;
using com.mahonkin.tim.logging.OSLogger;
using Microsoft.Extensions.Logging;

namespace com.mahonkin.tim.LoggingTest;

public static class LoggingTest
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        IntPtr logPtr = OSLogger.Create(nameof(LoggingTest), nameof(Main));
        IEnumerator logLevels = Enum.GetValues(typeof(LogLevel)).GetEnumerator();
        while (logLevels.MoveNext())
        {
            LogLevel currentLevel = (LogLevel)logLevels.Current;
            Console.WriteLine($"Logging {currentLevel} as {currentLevel.LogType()}");
            Console.WriteLine($"{currentLevel} is enabled: {OSLogger.IsEnabled(logPtr, currentLevel.LogType())}");
            OSLogger.Log(logPtr, currentLevel.LogType(), $"{currentLevel}");
        }
        OSLogger.LogTrace(logPtr, LogLevel.Trace.ToString());
        OSLogger.LogDebug(logPtr, LogLevel.Debug.ToString());
        OSLogger.LogInformation(logPtr, LogLevel.Information.ToString());
        OSLogger.LogWarning(logPtr, LogLevel.Warning.ToString());
        OSLogger.LogError(logPtr, LogLevel.Error.ToString());
        OSLogger.LogCritical(logPtr, LogLevel.Critical.ToString());
        OSLogger.LogNone(logPtr, LogLevel.None.ToString());
    }
}