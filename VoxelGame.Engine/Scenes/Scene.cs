using VoxelGame.Engine.Components;

namespace VoxelGame.Engine.Scenes;

public class Scene
{
    public string Name { get; internal set; }
    public Guid SceneId { get; internal set; }
    public List<GameObject> GameObjects { get; internal set; }

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
    }

    public void AddComponent<T>() where T : GameComponent, new()
    {
        var gameObject = new GameObject();
        gameObject.AddComponent<T>();
        AddGameObject(gameObject);
    }

    public static implicit operator Guid(Scene scene) => scene.SceneId;
    
    public void AddGameObject(GameObject gameObject) => GameObjects.Add(gameObject);
    public void Load() => SceneManager.LoadScene(this);
}