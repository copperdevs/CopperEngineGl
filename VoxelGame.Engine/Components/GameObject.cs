using System.Numerics;
using CopperEngine.Components;
using CopperEngine.Data;
using CopperEngine.Scenes;

namespace CopperEngine.Components;

public class GameObject
{
    internal GameObject() {}
    
    internal GameObject(Scene parentScene)
    {
        ParentScene = parentScene;
    }

    public Transform Transform { get; internal set; } = new()
    {
        Position = Vector3.Zero,
        Scale = Vector3.One,
        Rotation = Quaternion.Identity
    };

    internal readonly List<GameComponent> Components = new();
    internal Scene ParentScene;

    public void AddComponent(GameComponent component)
    {
        Components.Add(component);
        component.Parent = this;
        component.ParentScene = ParentScene;
        component.Start();
    }
    
    public void AddComponent<T>() where T : GameComponent, new()
    {
        AddComponent(new T());
    }

    public GameComponent[] GetComponents<T>() where T : GameComponent
    {
        return Components.Where(c => c.GetType() == typeof(T)).ToArray();
    }

    public GameComponent GetFirstComponent<T>() where T : GameComponent
    {
        return GetComponents<T>()[0];
    }

    public bool TryGetComponents<T>(out GameComponent[] components) where T : GameComponent
    {
        components = GetComponents<T>();
        return components.Length is not 0;
    }

    public bool TryGetFirstComponent<T>(out GameComponent component) where T : GameComponent
    {
        var components = TryGetComponents<T>(out var c);
        component = c[0];
        return components;
    }

    public void RemoveComponents<T>() where T : GameComponent, new()
    {
        Components.RemoveAll(gm => gm.GetType() == typeof(T));
    }

    internal void PreUpdate()
    {
        var targetComponents = Components;
        targetComponents.ForEach(c => c.PreUpdate());
    }

    internal void Update()
    {
        var targetComponents = Components;
        targetComponents.ForEach(c => c.Update());
    }
    
    internal void PostUpdate()
    {
        var targetComponents = Components;
        targetComponents.ForEach(c => c.PostUpdate());
    }
    
    internal void PreFixedUpdate()
    {
        var targetComponents = Components;
        targetComponents.ForEach(c => c.PreFixedUpdate());
    }
    
    internal void FixedUpdate()
    {
        var targetComponents = Components;
        targetComponents.ForEach(c => c.FixedUpdate());
    }
    
    internal void PostFixedUpdate()
    {
        var targetComponents = Components;
        targetComponents.ForEach(c => c.PostFixedUpdate());
    }

    internal void Render()
    {
        var targetComponents = Components;
        targetComponents.ForEach(c => c.Render());
    }
    
    internal void Stop()
    {
        var targetComponents = Components;
        targetComponents.ForEach(c => c.Stop());
    }
}