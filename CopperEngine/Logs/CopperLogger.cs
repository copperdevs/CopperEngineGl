using System.Diagnostics;
using CopperEngine.Utils;
using Silk.NET.OpenGL;

namespace CopperEngine.Logs;

public static class Log
{
    /// <summary> Log message to the console that is called very often </summary>
    /// <param name="message">Target Message</param>
    public static void DeepInfo(object message) => CopperLogger.LogDeepInfo(message);
    
    /// <summary> Log message to the console </summary>
    /// <param name="message">Target Message</param>
    public static void Info(object message) => CopperLogger.LogInfo(message);
    
    /// <summary> Log a warning to the console </summary>
    /// <param name="message">Target Message</param>
    public static void Warning(object message) => CopperLogger.LogWarning(message);
    
    /// <summary> Log an error to the console </summary>
    /// <param name="message">Target Message</param>
    public static void Error(object message) => CopperLogger.LogError(message);
    
    /// <summary> Logs an exception to the console </summary>
    /// <param name="message">Target Exception</param>
    public static void Error(Exception message) => CopperLogger.LogError(message);
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
    
    /// <inheritdoc cref="Log.DeepInfo"/>
    public static void LogDeepInfo(object message)
    {
        if(DeepInfoLogsEnabled)
            BaseLog("Info", message, LogType.DeepInfo);
    }
    
    /// <inheritdoc cref="Log.Info"/>
    public static void LogInfo(object message)
    {
        if(InfoLogsEnabled)
            BaseLog("Info", message, LogType.Info);
    }

    /// <inheritdoc cref="Log.Warning"/>
    public static void LogWarning(object message)
    {
        if(WarningLogsEnabled)
            BaseLog("Warning", message, LogType.Warning);
    }

    /// <inheritdoc cref="Log.Error"/>
    public static void LogError(object message)
    {
        if(ErrorLogsEnabled)
            BaseLog("Error", message, LogType.Error);
    }
    
    /// <inheritdoc cref="Log.Error"/>
    public static void LogError(Exception message)
    {
        if(ErrorLogsEnabled)
            BaseLog("Error", $"Exception Error. | Source -  {message.Source}\n{message.StackTrace}", LogType.Error);
    }

    private static void BaseLog(string prefix, object message, LogType type)
    {
        Console.ForegroundColor = type.ToConsoleColor();
        
        var logMessage = $"[{DateTime.Now.ToLongTimeString()}] [{prefix}] {message}";
        Console.WriteLine(logMessage);
        Logs.Add((logMessage, type));
        
        Console.ResetColor();
    }
    
    [Conditional("DEBUG")]
    internal static void CheckGlError(this GL gl, string title)
    {
        var error = gl.GetError();
        if (error != GLEnum.NoError)
        {
            Debug.Print($"{title}: {error}");
        }
    }
}