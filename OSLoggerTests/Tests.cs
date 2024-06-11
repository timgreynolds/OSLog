using System;

namespace com.mahonkin.tim.logging.OSLoggerTests;

public class Tests
{
    public static void Main(string[] args)  
    {
        IntPtr logPtr = OSLogger.Create(nameof(Program), nameof(Main));
        Console.WriteLine(OSLogger.IsEnabled(logPtr, OSLogType.OS_LOG_TYPE_DEFAULT));
        OSLogger.Log(logPtr, OSLogType.OS_LOG_TYPE_ERROR, "OSLogger Log(Error)");
    }
}
