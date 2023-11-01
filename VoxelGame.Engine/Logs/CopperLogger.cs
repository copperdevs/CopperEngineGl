namespace VoxelGame.Engine.Logs;

public static class Log
{
    public static void DeepInfo(object message) => CopperLogger.LogDeepInfo(message);
    public static void Info(object message) => CopperLogger.LogInfo(message);
    public static void Warning(object message) => CopperLogger.LogWarning(message);
    public static void Error(object message) => CopperLogger.LogError(message);
}

public static class CopperLogger
{
    public static bool DeepInfoLogsEnabled = false;
    public static bool InfoLogsEnabled = true;
    public static bool WarningLogsEnabled = true;
    public static bool ErrorLogsEnabled = true;
    
    public static void LogDeepInfo(object message)
    {
        if(DeepInfoLogsEnabled)
            BaseLog("Info", message, ConsoleColor.DarkCyan);
    }
    
    public static void LogInfo(object message)
    {
        if(InfoLogsEnabled)
            BaseLog("Info", message, ConsoleColor.DarkGray);
    }

    public static void LogWarning(object message)
    {
        if(WarningLogsEnabled)
            BaseLog("Warning", message, ConsoleColor.DarkYellow);
    }

    public static void LogError(object message)
    {
        if(ErrorLogsEnabled)
            BaseLog("Error", message, ConsoleColor.DarkRed);
    }

    private static void BaseLog(string prefix, object message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine($"[{prefix}] {message}");
        Console.ResetColor();
    }
}