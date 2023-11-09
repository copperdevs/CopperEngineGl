using CopperEngine.Data;
using CopperEngine.Scenes;

namespace CopperEngine.Components;

public class GameComponent
{
    internal GameObject? Parent;
    internal Transform? Transform { get => Parent.Transform; set => Parent.Transform = value; }
    internal Scene? ParentScene;
    
    public virtual void Start() { }
    public virtual void PreUpdate() { }
    public virtual void Update() { }
    public virtual void PostUpdate() { }
    public virtual void PreFixedUpdate() { }
    public virtual void FixedUpdate() { }
    public virtual void PostFixedUpdate() { }
    public virtual void Render() { }
    public virtual void RenderEditor() { }
    public virtual void Stop() { }
}