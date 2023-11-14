using CopperEngine.Components;
using CopperEngine.Info;
using CopperEngine.Logs;
using CopperEngine.Rendering;
using CopperEngine.Scenes;
using CopperEngine.Utils;
using ImGuiNET;

namespace CopperEngine.Editor;

internal static class InfoWindow
{
    private struct InfoWindowTab
    {
        public readonly string Title;
        public readonly Action WindowAction;
        public bool TabOpen;

        public InfoWindowTab(string title, Action windowAction, bool tabOpen)
        {
            Title = title;
            WindowAction = windowAction;
            TabOpen = tabOpen;
        }
    }
    
    internal static bool IsOpen = true;
    private static bool initialized;
    private static readonly List<InfoWindowTab> Tabs = new();
    private static List<(Guid, Scene)> scenes = new();
    private static DateTime lastTime;
    private static int framesRendered;
    private static int fps;
    private static int peakFps;

    internal static void Initialize()
    {
        if (initialized)
            return;
        initialized = true;
        
        Log.Info("Initializing Info Window");

        Tabs.Add(new InfoWindowTab("FPS / Time", TimeTab, true));
        Tabs.Add(new InfoWindowTab("Camera", CameraTab, true));
        Tabs.Add(new InfoWindowTab("Scene", SceneTab, true));
        Tabs.Add(new InfoWindowTab("Input", InputTab, false));
        Tabs.Add(new InfoWindowTab("Models", ModelsTab, true));
        // very complex compared to everything else so it got moved out
        Tabs.Add(new InfoWindowTab("Object Browser", ObjectBrowserTab.Render, true));
        Tabs.Add(new InfoWindowTab("Logs", LogsTab, false));
        Tabs.Add(new InfoWindowTab("System", SystemInfoTab, false));
        Tabs.Add(new InfoWindowTab("Rendering", RenderingTab.Render, true));
        
        Log.Info("Initialized Info Window");
    }

    internal static void Update()
    {
        framesRendered++;        
        
        if ((DateTime.Now - lastTime).TotalSeconds >= 1)
        {
            fps = framesRendered;
            framesRendered = 0;
            lastTime = DateTime.Now;
        }

        if (fps > peakFps)
            peakFps = fps;
    }

    internal static void Render()
    {
        if (!IsOpen)
            return;

        if (ImGui.Begin("Info", ref IsOpen, ImGuiWindowFlags.None))
        {
            scenes = SceneManager.GetScenes();

            const ImGuiTabBarFlags flags = ImGuiTabBarFlags.Reorderable;

            if (ImGui.BeginTabBar("info_window_tab_bar", flags))
            {
                for (var index = 0; index < Tabs.Count; index++)
                {
                    var tab = Tabs[index];
                    
                    if (tab.TabOpen)
                    {
                        if (ImGui.BeginTabItem(tab.Title, ref tab.TabOpen, ImGuiTabItemFlags.None))
                        {
                            tab.WindowAction.Invoke();
                            ImGui.EndTabItem();
                        }
                    }

                    Tabs[index] = tab;
                }
            }
            
            if (ImGui.TabItemButton("+", ImGuiTabItemFlags.Trailing | ImGuiTabItemFlags.NoTooltip))
                ImGui.OpenPopup("info_window_selection_popup");
            if (ImGui.BeginPopup("info_window_selection_popup"))
            {
                for (var index = 0; index < Tabs.Count; index++)
                {
                    var tab = Tabs[index];
                    ImGui.Selectable(tab.Title, ref tab.TabOpen);
                    Tabs[index] = tab;
                }
                
                ImGui.EndPopup();
            }
        }
    }

    private static void TimeTab()
    {
        ImGui.LabelText("Total Time", $"{Time.TotalTime}");
        ImGui.LabelText("Delta Time", $"{Time.DeltaTime}");
        ImGui.LabelText("FPS", $"{fps}");
        ImGui.LabelText("Peak FPS", $"{peakFps}");  
    }

    private static void CameraTab()
    {
        var camera = EngineRenderer.Camera;
            
        ImGui.SeparatorText("Info");
        
        ImGui.BeginDisabled();
        
        ImGui.DragFloat3("Position", ref camera.Position);
        ImGui.DragFloat3("Front", ref camera.Front);
        ImGui.DragFloat3("Up", ref camera.Up);
        ImGui.DragFloat3("Direction", ref camera.Direction);
        ImGui.DragFloat("Yaw", ref camera.Yaw, 1);
        ImGui.DragFloat("Pitch", ref camera.Pitch, 1, -89, 89);
        
        var moveFast = CameraController.MoveFast;
        ImGui.Checkbox("Move Fast", ref moveFast);
        
        var targetSpeed = CameraController.TargetMoveSpeed;
        ImGui.DragFloat("Target Speed", ref targetSpeed);
        
        var canMove = CameraController.CanMove;
        ImGui.Checkbox("Can Move", ref canMove);

        ImGui.EndDisabled();
        
        EditorUtil.DragMatrix4X4("Camera Projection Matrix", EngineRenderer.Camera.ProjectionMatrix, false);
        EditorUtil.DragMatrix4X4("Camera View Matrix", EngineRenderer.Camera.ViewMatrix, false);
        
        ImGui.SeparatorText("Settings");
        
        ImGui.DragFloat("Zoom", ref camera.Zoom, 1, 1, 45);
        ImGui.DragFloat("Normal Move Speed", ref CameraController.NormalMoveSpeed, 0.25f, 0, 50);
        ImGui.DragFloat("Fast Move Speed", ref CameraController.FastMoveSpeed, 0.25f, 0, 50);
        ImGui.DragFloat2("Clipping Plane", ref camera.ClippingPlane, 1, 0.001f, 1000);
    }

    private static void SceneTab()
    {
        ImGui.LabelText("Current Scene", SceneManager.CurrentScene().Name);

        ImGui.Separator();

        foreach (var scene in scenes)
        {
            if (ImGui.Button(scene.Item2.Name))
                SceneManager.LoadScene(scene.Item1);
        }
    }

    private static void InputTab()
    {
        var mousePosition = Input.MousePosition;
        ImGui.DragFloat2($"Mouse Position", ref mousePosition);
    }

    private static void ModelsTab()
    {
        var models = new List<Model>();
        SceneManager.CurrentScene().GameObjects.ForEach(gm => gm.Components.ForEach(c => { if (c.GetType() == typeof(Model)) models.Add((Model)c); }));
        // var models = VoxelRenderer.Models.Distinct().ToList();
        
        ImGui.LabelText("Model Count", $"{models.Count}");

        ImGui.Separator();
        
        for (var index1 = 0; index1 < models.Count; index1++)
        {
            var model = models[index1];
            if (ImGui.CollapsingHeader($"Model #{index1}##{index1}"))
            {
                ImGui.Indent();

                if (ImGui.CollapsingHeader($"Meshes##{index1}"))
                {
                    for (var i = 0; i < model.LoadedModel!.Meshes.Count; i++)
                    {
                        var mesh = model.LoadedModel.Meshes[i];

                        ImGui.Indent();
                        if (ImGui.CollapsingHeader($"Mesh #{i}##{index1}"))
                        {
                            ImGui.Indent();
                            if (ImGui.CollapsingHeader($"{mesh.Vertices.Length} Vertices##{index1}"))
                            {
                                ImGui.Indent();
                                for (var ii = 0; ii < mesh.Vertices.Length; ii++)
                                {
                                    var vertex = mesh.Vertices[ii];
                                    ImGui.InputFloat($"Vertex {ii}##{index1}", ref vertex);
                                }

                                ImGui.Unindent();
                            }

                            ImGui.Unindent();

                            ImGui.Indent();
                            if (ImGui.CollapsingHeader($"{mesh.Indices.Length} Indices##{index1}"))
                            {
                                ImGui.Indent();
                                for (var ii = 0; ii < mesh.Indices.Length; ii++)
                                {
                                    var index = (int)mesh.Indices[ii];
                                    ImGui.InputInt($"Index {ii}##{index1}", ref index);
                                }

                                ImGui.Unindent();
                            }

                            ImGui.Unindent();
                        }

                        ImGui.Unindent();
                    }
                }

                ImGui.Unindent();
            }
        }
    }

    private static bool showDisabledLogs;
    
    private static void LogsTab()
    {
        ImGui.Checkbox("Show Disabled Logs", ref showDisabledLogs);
        ImGui.Checkbox("Deep Info Logs Enabled", ref CopperLogger.DeepInfoLogsEnabled);
        ImGui.Checkbox("Info Logs Enabled", ref CopperLogger.InfoLogsEnabled);
        ImGui.Checkbox("Warning Logs Enabled", ref CopperLogger.WarningLogsEnabled);
        ImGui.Checkbox("Error Logs Enabled", ref CopperLogger.ErrorLogsEnabled);

        if (ImGui.CollapsingHeader("Logs"))
        {
            ImGui.Indent();
            
            try
            {
                foreach (var logs in CopperLogger.Logs.ToList())
                {
                    ImGui.TextColored(logs.Item2.ToImGuiColor(), logs.Item1);
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            
            ImGui.Unindent();
        }
    }

    private static void SystemInfoTab()
    {
        ImGui.LabelText("User Name", SystemInfo.UserName);
        ImGui.LabelText("Machine Name", SystemInfo.MachineName);
        ImGui.LabelText("CPU", SystemInfo.Cpu);
        ImGui.LabelText("Memory Size", SystemInfo.MemorySize.ToString());
        ImGui.LabelText("Threads", SystemInfo.Threads.ToString());
        ImGui.LabelText("OS", SystemInfo.Os);
    }
}