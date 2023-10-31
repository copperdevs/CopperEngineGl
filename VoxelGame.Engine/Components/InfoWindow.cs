using ImGuiNET;
using VoxelGame.Engine.Info;
using VoxelGame.Engine.Scenes;

namespace VoxelGame.Engine.Components;

internal static class InfoWindow
{
    internal static bool IsOpen = true;
    internal static void Render()
    {
        if(!IsOpen)
            return;
        
        if (ImGui.Begin("Info", ref IsOpen, ImGuiWindowFlags.None))
        {
            if (ImGui.CollapsingHeader("FPS / Time"))
            {
                ImGui.Text($"Total Time - {Time.TotalTime}");
                ImGui.Text($"Delta Time - {Time.DeltaTime}");
                
                ImGui.LabelText("Current Scene", SceneManager.CurrentScene().Name);
                ImGui.LabelText("Current Scene", SceneManager.CurrentScene().Name);
            }

            if (ImGui.CollapsingHeader("Camera"))
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

            var scenes = SceneManager.GetScenes();
            
            if (ImGui.CollapsingHeader("Scene"))
            {
                ImGui.LabelText("Current Scene", SceneManager.CurrentScene().Name);
                foreach (var scene in scenes)
                {
                    if(ImGui.Button(scene.Item2.Name))
                        SceneManager.LoadScene(scene.Item1);
                }
            }

            if (ImGui.CollapsingHeader("Components"))
            {
                foreach (var scene in scenes)
                {
                    ImGui.Indent();
                    if (ImGui.CollapsingHeader($"{scene.Item2.Name}"))
                    {
                        foreach (var component in scene.Item2.Components)
                        {
                            ImGui.Indent();
                            if (ImGui.Button(component.GetType().Name))
                            {
                                Console.WriteLine(component.GetType().Name);
                            }
                            ImGui.Unindent();
                        }
                    }
                    ImGui.Unindent();
                }
            }

            if (ImGui.CollapsingHeader("Input"))
            {
                var mousePosition = Input.MousePosition;
                ImGui.DragFloat2($"Mouse Position", ref mousePosition);
            }
        }
    }
}