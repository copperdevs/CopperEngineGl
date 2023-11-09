using System.Numerics;
using System.Xml;
using CopperEngine.Components;
using CopperEngine.Info;
using CopperEngine.Scenes;
using Silk.NET.Input;

namespace CopperEngine;

public static class Engine
{
    private static bool initialized;
    internal static readonly Scene EngineAssets = Scene.CreateScene("Engine Assets");
    private static GameApplication? application;

    /// <summary>
    /// Initializes the base engine with nothing loaded 
    /// </summary>
    public static void Initialize()
    {
        Initialize(() => {});
    }

    /// <summary>
    /// Initializes the engine with a custom game application
    /// </summary>
    /// <typeparam name="T"> Target <see cref="GameApplication">GameApplication</see> to initialize</typeparam>
    public static void Initialize<T>() where T : GameApplication, new()
    {
        application = new T();
        
        Initialize((() =>
        {
            // app stuff
            // EngineWindow.Window.Load += Application.Load;
            application.Load();
            EngineWindow.Window!.Update += delta => application.Update((float)delta);
            EngineWindow.Window.Render += delta => application.Render((float)delta);
            EngineEditor.RenderEditor += application.EditorRender;
            EngineWindow.Window.Closing += application.Close;
            EngineWindow.Window.Move += position => application.WindowMove(new Vector2(position.X, position.Y));
            EngineWindow.Window.Resize += size => application.WindowResize(new Vector2(size.X, size.Y));
            EngineWindow.Window.StateChanged += application.WindowStateChange;
            EngineWindow.Window.FramebufferResize += size => application.WindowFrameBufferResize(new Vector2(size.X, size.Y));
        }));
    }
    
    /// <summary>
    /// Initializes the engine and runs a load event
    /// </summary>
    /// <param name="loadEvent">Load event when the window loads</param>
    public static void Initialize(Action loadEvent)
    {
        if (initialized)
            return;
        initialized = true;
        
        EngineWindow.Initialize(() =>
        {
            // engine initialize
            EngineRenderer.Initialize();
            EngineEditor.Initialize();
            Input.Initialize();
            
            #if DEBUG
            
            // engine assets
            EngineAssets.AddComponent<CameraController>();
            
            // testing stuff
            Input.RegisterInput(Key.Escape, EngineWindow.Window!.Close, Input.RegisterType.Pressed);
            
            #endif
            
            // physics
            Task.Run(async () =>
            {
                const float fixedUpdate = 0.02f;
                while (EngineWindow.WindowLoaded || initialized)
                {
                    SceneManager.GameObjectsPreFixedUpdate(EngineAssets);
                    SceneManager.CurrentSceneGameObjectsPreFixedUpdate();
                    
                    SceneManager.CurrentScene().PhysicsWorld.Step(fixedUpdate);
                    
                    SceneManager.GameObjectsFixedUpdate(EngineAssets);
                    SceneManager.CurrentSceneGameObjectsFixedUpdate();
            
                    SceneManager.GameObjectsPostFixedUpdate(EngineAssets);
                    SceneManager.CurrentSceneGameObjectsPostFixedUpdate();
                    
                    await Task.Delay(TimeSpan.FromSeconds(fixedUpdate));
                }
            });
            
            loadEvent.Invoke();
        });

        EngineWindow.Window!.Closing += () =>
        {
            SceneManager.CurrentSceneGameObjectsStop();
            SceneManager.GameObjectsStop(EngineAssets);
            EngineRenderer.Close();
        };

        EngineWindow.Window.Update += delta =>
        {
            Input.CheckInput();

            #if DEBUG
            EngineWindow.SetTitle($"CopperEngine | Delta Time - {delta} | Size - <{EngineWindow.Window.Size.X},{EngineWindow.Window.Size.Y}>");
            #endif
            
            SceneManager.GameObjectsPreUpdate(EngineAssets);
            SceneManager.CurrentSceneGameObjectsPreUpdate();
            
            SceneManager.GameObjectsUpdate(EngineAssets);
            SceneManager.CurrentSceneGameObjectsUpdate();
            
            SceneManager.GameObjectsPostUpdate(EngineAssets);
            SceneManager.CurrentSceneGameObjectsPostUpdate();
        };
    }

    /// <summary>
    /// Runs the window
    /// </summary>
    public static void Run()
    {
        EngineWindow.Run();
        
        EngineWindow.Window?.Dispose();
    }
}