//
//  Native.c
//  OSLogNative
//
//  Created by Timothy Reynolds on 2/9/24.
//
// Many thanks to Soren Nils Kuklau and Stephan Schlecht for inspiration.
// https://github.com/chucker/serilog-sinks-apple-unified-logging
// https://stackoverflow.com/questions/53711865/how-to-p-invoke-os-log/53795536#53795536
//

#include <os/log.h>

extern os_log_t Create(char *subsystem, char *category)
{
    return os_log_create(subsystem, category);
}

extern bool IsEnabled(os_log_t log, os_log_type_t type)
{
    return os_log_type_enabled(log, type);
}

extern void Log(os_log_t log, os_log_type_t type, char *message)
{
    os_log_with_type(log, type, "%{public}s", message);
}

extern void LogDefault(os_log_t log, char *mesage)
{
    os_log(log, "%{public}s", mesage);
}

extern void LogTrace(os_log_t log, char *message)
{
    os_log(log, "%{public}s", message);
}

extern void LogDebug(os_log_t log, char *message)
{
    os_log_debug(log, "%{public}s", message);
}

extern void LogInfo(os_log_t log, char *message)
{
    os_log_info(log, "%{public}s", message);
}

extern void LogWarning(os_log_t log, char *message)
{
    os_log_info(log, "%{public}s", message);
}

extern void LogError(os_log_t log, char *message)
{
    os_log_error(log, "%{public}s", message);
}

extern void LogFault(os_log_t log, char *message)
{
    os_log_error(log, "%{public}s", message);
}

extern void LogCritical(os_log_t log, char *message)
{
    LogFault(log, message);
}
