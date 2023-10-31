using VoxelGame.Engine.Data;

namespace VoxelGame.Engine.Components;

public class GameComponent
{
    protected Transform Transform;
    
    public virtual void Start() { }
    public virtual void Update() { }
    public virtual void Render() { }
    public virtual void RenderEditor() { }
    public virtual void Stop() { }
}