using System.Numerics;
using ImGuiNET;
using ImGuizmoNET;
using VoxelGame.Engine.Editor;
using VoxelGame.Engine.Info;
using VoxelGame.Engine.Logs;
using VoxelGame.Engine.Rendering;
using VoxelGame.Engine.Scenes;
using VoxelGame.Engine.Utils;

namespace VoxelGame.Engine.Components;

internal static class InfoWindow
{
    internal static bool IsOpen = true;
    private static bool initialized;
    private static readonly List<(string, Action)> Tabs = new();
    private static List<(Guid, Scene)> scenes = new();
    private static GameObject? currentObjectBrowserTarget;
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

        Tabs.Add(("FPS / Time", TimeTab));
        Tabs.Add(("Camera", CameraTab));
        Tabs.Add(("Scene", SceneTab));
        Tabs.Add(("Input", InputTab));
        Tabs.Add(("Rendering", RenderingTab));
        // very complex compared to everything else so it got moved out
        Tabs.Add(("Object Browser", ObjectBrowserTab.Render));
        Tabs.Add(("Logs", LogsTab));
        Tabs.Add(("System", SystemInfoTab));
        
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

            const ImGuiTabBarFlags flags = ImGuiTabBarFlags.Reorderable | ImGuiTabBarFlags.NoCloseWithMiddleMouseButton;

            if (ImGui.BeginTabBar("info_window_tab_bar", flags))
            {
                foreach (var tab in Tabs)
                {
                    if (ImGui.BeginTabItem(tab.Item1))
                    {
                        tab.Item2.Invoke();
                        ImGui.EndTabItem();
                    }
                }
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
        var camera = VoxelRenderer.Camera;
        ImGui.DragFloat3("Position", ref camera.Position);
        ImGui.DragFloat3("Front", ref camera.Front);
        ImGui.DragFloat3("Up", ref camera.Up);
        ImGui.DragFloat3("Direction", ref camera.Direction);
        ImGui.DragFloat("Yaw", ref camera.Yaw, 1);
        ImGui.DragFloat("Pitch", ref camera.Pitch, 1, -89, 89);
        ImGui.DragFloat("Zoom", ref camera.Zoom, 1, 1, 45);
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

    private static void RenderingTab()
    {
        var models = new List<CopperModel>();
        SceneManager.CurrentScene().GameObjects.ForEach(gm => gm.Components.ForEach(c => { if (c.GetType() == typeof(CopperModel)) models.Add((CopperModel)c); }));
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
                    for (var i = 0; i < model.Model.Meshes.Count; i++)
                    {
                        var mesh = model.Model.Meshes[i];

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

    private static void LogsTab()
    {
        ImGui.Checkbox("Deep Info Logs Enabled", ref CopperLogger.DeepInfoLogsEnabled);
        ImGui.Checkbox("Info Logs Enabled", ref CopperLogger.InfoLogsEnabled);
        ImGui.Checkbox("Warning Logs Enabled", ref CopperLogger.WarningLogsEnabled);
        ImGui.Checkbox("Error Logs Enabled", ref CopperLogger.ErrorLogsEnabled);

        if (ImGui.CollapsingHeader("Logs"))
        {
            ImGui.Indent();

            foreach (var logs in CopperLogger.Logs)
            {
                // ImGui.Text(logs.Item1);
                ImGui.TextColored(logs.Item2.ToImGuiColor(), logs.Item1);
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