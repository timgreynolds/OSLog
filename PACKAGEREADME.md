# OSLog Utility Class

A simple utility class providing "native interoperability" with the Apple Unified Logging framework. 

## Getting started

### Installation

* Install the NuGet package

## Usage

* Get a pointer to the OSLog object by calling the OSLogger.Create() method.
  `IntPtr logPtr = OSlogger.Create(subsystem, category);`
* Pass the pointer to any of the OSLogger logging methods along with the log message text.
  `OSLogger.LogDebug(logPtr, "Debug Message")`

## Notes

* There is not a direct, one-to-one correlation between .Net `LogLevel` and Unified Logging `LogType` so I've chosen to map both .Net `LogLevel.Warning` and `LogLevel.Error` to `LogType_Error`.
* Interpolation on the message supplied is performed before the message is passed to the OSLog object. This means that you cannot take advantage of the Unified Logging option to redact private data.
* Also because of the interpolation this logger cannot support structured logging. Runtime variables cannot be passed to the log method as parameters.
* The package has been created with the intent of support for .Net versions 8 and 9-preview on iOS, MacCatalyst, and macOS (Cocoa). 

## Additional documentation

* [Github repository](https://github.com/timgreynolds/OSLog/)
* [Logging in C# and .NET](https://learn.microsoft.com/en-us/dotnet/core/extensions/logging?tabs=command-line)
* [Apple Unified Logging](https://developer.apple.com/documentation/os/logging?language=objc)

## Feedback
[Github respository issues](https://github.com/timgreynolds/OSLog/issues)
