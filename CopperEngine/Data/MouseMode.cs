using Silk.NET.Input;

namespace CopperEngine.Data;

public enum MouseMode
{
    /// <inheritdoc cref="CursorMode.Normal"/>
    Normal = CursorMode.Normal,
    
    /// <inheritdoc cref="CursorMode.Hidden"/>
    Hidden = CursorMode.Hidden,
    
    /// <inheritdoc cref="CursorMode.Raw"/>
    Disabled = CursorMode.Raw
}