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

    internal readonly List<GameComponent> Components = new();

    public void AddComponent(GameComponent component)
    {
        Components.Add(component);
        component.Transform = Transform;
        component.Start();
    }
    
    public void AddComponent<T>() where T : GameComponent, new()
    {
        AddComponent(new T());
    }

    public GameComponent[] GetComponents<T>() where T : GameComponent, new()
    {
        return Components.Where(c => c.GetType() == typeof(T)).ToArray();
    }

    public void RemoveComponents<T>() where T : GameComponent, new()
    {
        Components.RemoveAll(gm => gm.GetType() == typeof(T));
    }

    internal void Update()
    {
        var targetComponents = Components;
        targetComponents.ForEach(c => c.Update());
    }

    internal void Render()
    {
        var targetComponents = Components;
        targetComponents.ForEach(c => c.Render());
    }

    internal void RenderEditor()
    {
        var targetComponents = Components;
        targetComponents.ForEach(c => c.RenderEditor());
    }

    internal void Stop()
    {
        var targetComponents = Components;
        targetComponents.ForEach(c => c.Stop());
    }
}