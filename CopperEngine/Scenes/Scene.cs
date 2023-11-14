using CopperEngine.Components;
using Jitter2;

namespace CopperEngine.Scenes;

public class Scene
{
    public string Name { get; }
    public Guid SceneId { get; private init; }
    internal List<GameObject> GameObjects { get; }
    internal World PhysicsWorld;

    /// <summary>
    /// Creates a new scene
    /// </summary>
    /// <param name="name">Name of the scene</param>
    /// <param name="sceneId">Scene Id</param>
    /// <returns>Created scene</returns>
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

    /// <summary>
    /// Creates a new scene
    /// </summary>
    /// <param name="name">Name of the scene</param>
    /// <returns>Created Scene</returns>
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

    /// <summary>
    /// Adds a new <see cref="GameComponent"/> to a new <see cref="GameObject"/> and adds that to this scene
    /// </summary>
    /// <typeparam name="T">Target <see cref="GameComponent"/></typeparam>
    public void AddComponent<T>() where T : GameComponent, new()
    {
        var gameObject = new GameObject(this);
        gameObject.AddComponent<T>();
        GameObjects.Add(gameObject);
    }
    
    public static implicit operator Guid(Scene scene) => scene.SceneId;
    
    /// <summary>
    /// Loads this scene
    /// </summary>
    public void Load() => SceneManager.LoadScene(this);

    /// <summary>
    /// Creates a new <see cref="GameObject"/> registered to this scene
    /// </summary>
    /// <returns>Created <see cref="GameObject"/></returns>
    public GameObject CreateGameObject()
    {
        var gameObject = new GameObject(this);
        GameObjects.Add(gameObject);
        return gameObject;
    }
}