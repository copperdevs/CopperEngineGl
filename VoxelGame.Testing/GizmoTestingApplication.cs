using System.Numerics;
using CopperEngine;
using CopperEngine.Components;
using CopperEngine.Rendering;
using CopperEngine.Scenes;
using CopperEngine.Utils;
using ImGuiNET;
using ImGuizmoNET;

namespace VoxelGame.Testing;

public class GizmoTestingApplication : GameApplication
{
    private GameObject gizmoGameObject;

    public override void Load()
    {
        var testingScene = SceneManager.CreateScene("Testing Scene");
        testingScene.Load();

        gizmoGameObject = testingScene.CreateGameObject();
        gizmoGameObject.AddComponent(new Model("Resources/Images/copper.png", "Resources/Models/cube.obj"));
    }

    public override void EditorRender()
    {
        if (ImGui.Begin("window"))
        {
            var io = ImGui.GetIO();
            ImGuizmo.SetRect(0, 0, io.DisplaySize.X, io.DisplaySize.Y);
            
            var cameraViewMatrix = EngineRenderer.Camera.ViewMatrix;
            var cameraProjectionMatrix = EngineRenderer.Camera.ProjectionMatrix;
            var transformMatrix = gizmoGameObject.Transform.Matrix;
            // var transformMatrix = Matrix4x4.Identity * Matrix4x4.CreateTranslation(gizmoGameObject.Transform.Position);
            
            EditorUtil.DragMatrix4X4("Pre Manipulate", transformMatrix);
            
            
            ImGuizmo.Manipulate(ref cameraViewMatrix.M44, ref cameraProjectionMatrix.M44, OPERATION.TRANSLATE, MODE.WORLD, ref transformMatrix.M44);
            ImGuizmo.DrawCubes(ref cameraViewMatrix.M44, ref cameraProjectionMatrix.M44, ref transformMatrix.M44, 1);
            
            
            
            EditorUtil.DragMatrix4X4("Post Manipulate", transformMatrix);
            gizmoGameObject.Transform.Matrix = transformMatrix;
            
            ImGui.End();
        }
    }
}