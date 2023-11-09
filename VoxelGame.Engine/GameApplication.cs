using System.Numerics;
using Silk.NET.Windowing;

namespace CopperEngine;

public class GameApplication
{
    public virtual void Load() { }
    public virtual void Update(float delta) { }
    public virtual void Render(float delta) { }
    public virtual void EditorRender() { }
    public virtual void Close() { } 
    
    public virtual void WindowMove(Vector2 position) { }
    public virtual void WindowResize(Vector2 size) { }
    public virtual void WindowStateChange(WindowState state) { }
    public virtual void WindowFrameBufferResize(Vector2 size) { }
}