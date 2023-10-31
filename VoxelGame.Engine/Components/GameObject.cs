using System.Numerics;
using VoxelGame.Engine.Data;

namespace VoxelGame.Engine.Components;

public class GameObject
{
    public Transform Transform { get; private set; } = new()
    {
        Position = Vector3.Zero,
        Scale = 1,
        Rotation = Quaternion.Identity
    };

    private readonly List<GameComponent> components = new();

    public void AddComponent<T>() where T : GameComponent, new()
    {
        var gameComponent = new T() as GameComponent;
        components.Add(gameComponent);

        gameComponent.Transform = Transform;
        
        gameComponent.Start();
    }

    public T[] GetComponents<T>() where T : GameComponent, new()
    {
        return components.Where(c => c.GetType() == typeof(T)).ToArray() as T[] ?? Array.Empty<T>();
    }

    public void RemoveComponents<T>() where T : GameComponent, new()
    {
        components.RemoveAll(gm => gm.GetType() == typeof(T));
    }

    internal void Update()
    {
        var targetComponents = components;
        targetComponents.ForEach(c => c.Update());
    }

    internal void Render()
    {
        var targetComponents = components;
        targetComponents.ForEach(c => c.Render());
    }

    internal void RenderEditor()
    {
        var targetComponents = components;
        targetComponents.ForEach(c => c.RenderEditor());
    }

    internal void Stop()
    {
        var targetComponents = components;
        targetComponents.ForEach(c => c.Stop());
    }
}