namespace VoxelGame.Engine.Scenes;

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
        scenes.Remove((VoxelEngine.EngineAssets, VoxelEngine.EngineAssets));
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

    public static void LoadScene(Guid targetScene)
    {
        currentScene = Scenes[targetScene];
    }

    public static Scene CurrentScene()
    {
        return Scenes[currentScene];
    }
    
    public static Scene CreateScene(string name, out Guid sceneId)
    {
        return Scene.CreateScene(name, out sceneId);
    }

    public static Scene CreateScene(string name)
    {
        return Scene.CreateScene(name);
    }

    #endregion
}