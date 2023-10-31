using ImGuiNET;
using VoxelGame.Engine.Info;
using VoxelGame.Engine.Scenes;
using VoxelGame.Engine.Utils;

namespace VoxelGame.Engine.Components;

internal static class InfoWindow
{
    internal static bool IsOpen = true;

    internal static void Render()
    {
        if (!IsOpen)
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
                    if (ImGui.Button(scene.Item2.Name))
                        SceneManager.LoadScene(scene.Item1);
                }
            }

            // TODO: Re-add components section

            if (ImGui.CollapsingHeader("Input"))
            {
                var mousePosition = Input.MousePosition;
                ImGui.DragFloat2($"Mouse Position", ref mousePosition);
            }

            if (ImGui.CollapsingHeader("Rendering"))
            {
                ImGui.LabelText("Model Count", $"{VoxelRenderer.Models.Count}");

                ImGui.Indent();
                for (var index1 = 0; index1 < VoxelRenderer.Models.Count; index1++)
                {
                    var model = VoxelRenderer.Models[index1];
                    if (ImGui.CollapsingHeader($"Model #{index1}"))
                    {
                        ImGui.Indent();
                        
                        if (ImGui.CollapsingHeader("Transform"))
                        {
                            ImGui.Indent();
                            
                            var position = model.Transform.Position;
                            ImGui.DragFloat3("Position", ref position);
                            var scale = model.Transform.Scale;
                            ImGui.DragFloat("Scale", ref scale);
                            var rotation = model.Transform.Rotation.ToVector();
                            ImGui.DragFloat4("Position", ref rotation);
                            
                            ImGui.Unindent();
                        }
                        
                        if (ImGui.CollapsingHeader("Meshes"))
                        {
                            for (var i = 0; i < model.Model.Meshes.Count; i++)
                            {
                                var mesh = model.Model.Meshes[i];

                                ImGui.Indent();
                                if (ImGui.CollapsingHeader($"Mesh #{i}"))
                                {
                                    ImGui.Indent();
                                    if (ImGui.CollapsingHeader("Vertices"))
                                    {
                                        ImGui.Indent();
                                        for (var ii = 0; ii < mesh.Vertices.Length; ii++)
                                        {
                                            var vertex = mesh.Vertices[ii];
                                            ImGui.InputFloat($"Vertex {ii}", ref vertex);
                                        }

                                        ImGui.Unindent();
                                    }

                                    ImGui.Unindent();

                                    ImGui.Indent();
                                    if (ImGui.CollapsingHeader("Indices"))
                                    {
                                        ImGui.Indent();
                                        for (var ii = 0; ii < mesh.Indices.Length; ii++)
                                        {
                                            var index = (int)mesh.Indices[ii];
                                            ImGui.InputInt($"Index {ii}", ref index);
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

                ImGui.Unindent();
            }
        }
    }
}