using VoxelGame.Engine.Utils;

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

    internal static List<(string, LogType)> Logs = new();

    internal enum LogType
    {
        DeepInfo,
        Info,
        Warning,
        Error
    }
    
    public static void LogDeepInfo(object message)
    {
        if(DeepInfoLogsEnabled)
            BaseLog("Info", message, LogType.DeepInfo);
    }
    
    public static void LogInfo(object message)
    {
        if(InfoLogsEnabled)
            BaseLog("Info", message, LogType.Info);
    }

    public static void LogWarning(object message)
    {
        if(WarningLogsEnabled)
            BaseLog("Warning", message, LogType.Warning);
    }

    public static void LogError(object message)
    {
        if(ErrorLogsEnabled)
            BaseLog("Error", message, LogType.Error);
    }

    private static void BaseLog(string prefix, object message, LogType type)
    {
        Console.ForegroundColor = type.ToConsoleColor();
        
        var logMessage = $"[{DateTime.Now.ToLongTimeString()}] [{prefix}] {message}";
        Console.WriteLine(logMessage);
        Logs.Add((logMessage, type));
        
        Console.ResetColor();
    }
}