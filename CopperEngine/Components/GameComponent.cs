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
    
    public void AddComponent<T>() where T : GameComponent, new() => Parent?.AddComponent<T>();
    public GameComponent[] GetComponents<T>() where T : GameComponent => Parent?.GetComponents<T>() ?? Array.Empty<GameComponent>();
    public GameComponent GetFirstComponent<T>() where T : GameComponent => Parent?.GetFirstComponent<T>() ?? new GameComponent();
    public bool TryGetComponents<T>(out GameComponent[] components) where T : GameComponent => Parent!.TryGetComponents<T>(out components);
    public bool TryGetFirstComponent<T>(out GameComponent component) where T : GameComponent => Parent!.TryGetFirstComponent<T>(out component);
    public void RemoveComponents<T>() where T : GameComponent, new() => Parent?.RemoveComponents<T>();
}