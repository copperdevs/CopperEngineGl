using CopperEngine.Components;
using ImGuiNET;
using ImGuizmoNET;
using Silk.NET.GLFW;

namespace CopperEngine.Editor;

internal static class Gizmo
{
    public static OPERATION Operation = OPERATION.TRANSLATE;
    private static bool usingGizmo = false;

    public static void Draw(ref GameObject gameObject)
    {
        var cameraView = EngineRenderer.Camera.ViewMatrix;
        var cameraProj = EngineRenderer.Camera.ProjectionMatrix;

        ImGuizmo.SetDrawlist();
        ImGuizmo.SetOrthographic(true);
        if (ImGui.IsWindowHovered())
        {
            if (ImGui.IsKeyDown(ImGuiKey.Q)) Operation = OPERATION.TRANSLATE;
            if (ImGui.IsKeyDown(ImGuiKey.E)) Operation = OPERATION.ROTATE;
            if (ImGui.IsKeyDown(ImGuiKey.R)) Operation = OPERATION.SCALE;
        }

        ImGuizmo.SetGizmoSizeClipSpace(0.3f);
        ImGuizmo.SetRect(ImGui.GetWindowPos().X, ImGui.GetWindowPos().Y, ImGui.GetItemRectSize().X,
            ImGui.GetItemRectSize().Y);

        var transform = gameObject.Transform;

        var transformMatrix = transform.Matrix;
        ImGuizmo.Manipulate(ref cameraView.M11, ref cameraProj.M11, Operation, MODE.LOCAL, ref transformMatrix.M11);
        transform.Matrix = transformMatrix;

        if (ImGuizmo.IsUsing())
        {
            gameObject.Transform = transform;
            usingGizmo = true;
        }
        else
        {
            usingGizmo = false;
        }


        ImGuizmo.DrawGrid(ref cameraView.M11, ref cameraProj.M11, ref transformMatrix.M11, 50);
    }
}