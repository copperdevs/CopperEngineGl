using System.Numerics;
using VoxelGame.Engine.Logs;

namespace VoxelGame.Engine.Utils;

public static class ColorUtil
{
    internal static ConsoleColor ToConsoleColor(this CopperLogger.LogType logType)
    {
        return logType switch
        {
            CopperLogger.LogType.DeepInfo => ConsoleColor.DarkCyan,
            CopperLogger.LogType.Info => ConsoleColor.DarkGray,
            CopperLogger.LogType.Warning => ConsoleColor.DarkYellow,
            CopperLogger.LogType.Error => ConsoleColor.DarkRed,
            _ => ConsoleColor.Black
        };
    }
    
    internal static Vector4 ToImGuiColor(this CopperLogger.LogType logType)
    {
        return logType switch
        {
            CopperLogger.LogType.DeepInfo => new Vector4(0, 1, 1, 1),
            CopperLogger.LogType.Info => new Vector4(0.37f, 0.37f, 0.37f, 1),
            CopperLogger.LogType.Warning => new Vector4(1, 1, 0, 1),
            CopperLogger.LogType.Error => new Vector4(1, 0, 0, 1),
            _ => new Vector4(1, 1, 1, 1)
        };
    }
}