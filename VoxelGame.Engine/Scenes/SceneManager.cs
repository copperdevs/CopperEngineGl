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

    internal static void CurrentSceneGameComponentsUpdate() => GameComponentsUpdate(currentScene);

    internal static void GameComponentsUpdate(Guid targetScene) =>
        Scenes[targetScene].Components.ForEach(c => c.Update());

    internal static void CurrentSceneGameComponentsRender() => GameComponentsRender(currentScene);

    internal static void GameComponentsRender(Guid targetScene) =>
        Scenes[targetScene].Components.ForEach(c => c.Render());

    internal static void CurrentSceneGameComponentsRenderEditor() => GameComponentsRenderEditor(currentScene);

    internal static void GameComponentsRenderEditor(Guid targetScene) =>
        Scenes[targetScene].Components.ForEach(c => c.RenderEditor());

    internal static void CurrentSceneGameComponentsStop() => GameComponentsStop(currentScene);
    internal static void GameComponentsStop(Guid targetScene) => Scenes[targetScene].Components.ForEach(c => c.Stop());

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