using CopperEngine.Components;
using Jitter2;

namespace CopperEngine.Scenes;

public class Scene
{
    public string Name { get; internal set; }
    public Guid SceneId { get; internal set; }
    public List<GameObject> GameObjects { get; internal set; }
    internal World PhysicsWorld;

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
        GameObjects = new List<GameObject>();
        PhysicsWorld = new World();
    }

    public void AddComponent<T>() where T : GameComponent, new()
    {
        var gameObject = new GameObject(this);
        gameObject.AddComponent<T>();
        GameObjects.Add(gameObject);
    }

    public static implicit operator Guid(Scene scene) => scene.SceneId;
    
    public void Load() => SceneManager.LoadScene(this);

    public GameObject CreateGameObject()
    {
        var gameObject = new GameObject(this);
        GameObjects.Add(gameObject);
        return gameObject;
    }
}