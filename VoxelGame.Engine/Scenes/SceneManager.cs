using VoxelGame.Engine.Components;

namespace VoxelGame.Engine.Scenes;

public static class SceneManager
{
    private static readonly Dictionary<Guid, Scene> Scenes = new();
    private static Scene emptyScene = Scene.CreateScene("Empty Scene - Don't Use");
    private static Guid currentScene = emptyScene;
    
    #region internal api

    internal static List<(Guid, Scene)> GetScenes()
    {
        List<(Guid, Scene)> scenes = new();
        scenes.AddRange(Scenes.Select(scene => (scene.Key, scene.Value)));
        scenes.Remove((emptyScene, emptyScene));
        return scenes;
    }

    internal static void AddScene(Scene scene)
    {
        Scenes.Add(scene, scene);
    }

    internal static void CurrentSceneGameObjectsUpdate() => GameObjectsUpdate(currentScene);

    internal static void GameObjectsUpdate(Guid targetScene) =>
        Scenes[targetScene].GameObjects.ForEach(c => c.Update());

    internal static void CurrentSceneGameObjectsRender() => GameObjectsRender(currentScene);

    internal static void GameObjectsRender(Guid targetScene) =>
        Scenes[targetScene].GameObjects.ForEach(c => c.Render());

    internal static void CurrentSceneGameObjectsRenderEditor() => GameObjectsRenderEditor(currentScene);

    internal static void GameObjectsRenderEditor(Guid targetScene) =>
        Scenes[targetScene].GameObjects.ForEach(c => c.RenderEditor());

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

    #endregion
}