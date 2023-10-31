using VoxelGame.Engine.Components;

namespace VoxelGame.Engine.Scenes;

public class Scene
{
    public string Name { get; internal set; }
    public Guid SceneId { get; internal set; }
    public List<GameComponent> Components { get; internal set; }

    public static Scene CreateScene(string name, out Guid sceneId)
    {
        var scene = new Scene(name)
        {
            SceneId = Guid.NewGuid()
        };

        sceneId = scene.SceneId;
        
        SceneManager.AddScene(scene);
        
        return scene;
    }

    public static Scene CreateScene(string name)
    {
        return CreateScene(name, out var id);
    }

    private Scene(string name)
    {
        Name = name;
        Components = new List<GameComponent>();
    }

    public static implicit operator Guid(Scene scene) => scene.SceneId;


    public void AddComponent(GameComponent component)
    {
        component.Start();
        Components.Add(component);
    }
    
    public void Load() => SceneManager.LoadScene(this);
}