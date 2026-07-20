# OSLog Utility Class

A simple utility class providing "native interoperability" with the Apple Unified Logging framework.

## Getting started

### Prerequisites

- net9.0 or net8.0 SDK
- maui-maccatalyst and maui-ios workloads
- Visual Studio for Mac or Visual Studio Code

### Installation

- Install the NuGet package

## Usage

- Get a pointer to an OSLog object by calling the OSLogger.Create() method.
  `IntPtr logPtr = OSlogger.Create(subsystem, category);`
- Pass the pointer to any of the OSLogger logging methods along with the log message text.
  `OSLogger.LogDebug(logPtr, "Debug Message")`
  - Runtime variables are supported for interpolated strings in the log message. See Notes below.
- View the logging output in the MacOS `Console` application.
  - Use the values of `subsystem` and `category` to filter the log output in `Console`.

## Notes

- There is not a direct, one-to-one correlation between .Net `LogLevel` and Unified Logging `LogType`. This package maps both `LogLevel.None` and `LogLevel.Trace` to `OS_LOG_TYPE_DEFAULT` and both `LogLevel.Information` and `LogLevel.Warning` to `OS_LOG_TYPE_INFO`.
- The log message, when passed from .Net Managed Code to the native OSLog instance, is passed as a pointer to a simple character array.
  - Starting with v2.1 a custom interpolated string handler provides functionality to mimic [OSLogPrivacy](https://developer.apple.com/documentation/os/oslogprivacy) options. Interpolated strings can be decorated with custom format specifiers, `Priv` and `Mask`, to redact sensitive information before it is passed to the native logger.
- The package has been created with the intent of support for .Net versions 8 and 9 on iOS, MacCatalyst, and macOS (Cocoa).

## Additional documentation

- [Github repository](https://github.com/timgreynolds/OSLog/)
- [Logging in C# and .NET](https://learn.microsoft.com/en-us/dotnet/core/extensions/logging?tabs=command-line)
- [Apple Unified Logging](https://developer.apple.com/documentation/os/logging?language=objc)

## Feedback

[Github respository issues](https://github.com/timgreynolds/OSLog/issues)
