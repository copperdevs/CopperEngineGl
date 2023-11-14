namespace CopperEngine.Scenes;

public static class SceneManager
{
    internal static readonly Dictionary<Guid, Scene> Scenes = new();
    private static Scene emptyScene = Scene.CreateScene("Empty Scene - Don't Use");
    private static Guid currentScene = emptyScene;
    
    #region internal api

    internal static List<(Guid, Scene)> GetScenes()
    {
        List<(Guid, Scene)> scenes = new();
        scenes.AddRange(Scenes.Select(scene => (scene.Key, scene.Value)));
        scenes.Remove((emptyScene, emptyScene));
        scenes.Remove((CopperEngine.Engine.EngineAssets, CopperEngine.Engine.EngineAssets));
        return scenes;
    }

    internal static void AddScene(Scene scene)
    {
        Scenes.Add(scene, scene);
    }

    internal static void CurrentSceneGameObjectsPreUpdate() => GameObjectsPreUpdate(currentScene);

    internal static void GameObjectsPreUpdate(Guid targetScene) =>
        Scenes[targetScene].GameObjects.ForEach(c => c.PreUpdate());
    
    internal static void CurrentSceneGameObjectsUpdate() => GameObjectsUpdate(currentScene);

    internal static void GameObjectsUpdate(Guid targetScene) =>
        Scenes[targetScene].GameObjects.ForEach(c => c.Update());
    
    internal static void CurrentSceneGameObjectsPostUpdate() => GameObjectsPostUpdate(currentScene);

    internal static void GameObjectsPostUpdate(Guid targetScene) =>
        Scenes[targetScene].GameObjects.ForEach(c => c.PostUpdate());
    
    internal static void CurrentSceneGameObjectsFixedUpdate() => GameObjectsFixedUpdate(currentScene);

    
    internal static void CurrentSceneGameObjectsPreFixedUpdate() => GameObjectsPreFixedUpdate(currentScene);

    internal static void GameObjectsPreFixedUpdate(Guid targetScene) =>
        Scenes[targetScene].GameObjects.ForEach(c => c.PreFixedUpdate());
    
    internal static void GameObjectsFixedUpdate(Guid targetScene) =>
        Scenes[targetScene].GameObjects.ForEach(c => c.FixedUpdate());
    
    
    internal static void CurrentSceneGameObjectsPostFixedUpdate() => GameObjectsPostFixedUpdate(currentScene);

    internal static void GameObjectsPostFixedUpdate(Guid targetScene) =>
        Scenes[targetScene].GameObjects.ForEach(c => c.PostFixedUpdate());

    internal static void CurrentSceneGameObjectsRender() => GameObjectsRender(currentScene);

    internal static void GameObjectsRender(Guid targetScene) =>
        Scenes[targetScene].GameObjects.ForEach(c => c.Render());
    
    internal static void CurrentSceneGameObjectsStop() => GameObjectsStop(currentScene);
    internal static void GameObjectsStop(Guid targetScene) => Scenes[targetScene].GameObjects.ForEach(c => c.Stop());

    #endregion

    #region public api

    /// <summary>
    /// Loads a target scene
    /// </summary>
    /// <param name="targetScene">Scene to load</param>
    public static void LoadScene(Guid targetScene)
    {
        currentScene = Scenes[targetScene];
    }

    /// <summary>
    /// Gets the current scene
    /// </summary>
    /// <returns>Current scene</returns>
    public static Scene CurrentScene()
    {
        return Scenes[currentScene];
    }
    
    /// <summary>
    /// Creates a new scene
    /// </summary>
    /// <param name="name">Name of the scene</param>
    /// <param name="sceneId">Id of the scene</param>
    /// <returns>Created Scene</returns>
    public static Scene CreateScene(string name, out Guid sceneId)
    {
        return Scene.CreateScene(name, out sceneId);
    }
    
    /// <summary>
    /// Creates a new scene
    /// </summary>
    /// <param name="name">Name of the scene</param>
    /// <returns>Created Scene</returns>
    public static Scene CreateScene(string name)
    {
        return Scene.CreateScene(name);
    }

    #endregion
}