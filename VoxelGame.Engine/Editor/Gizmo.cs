using System.Numerics;
using ImGuiNET;
using ImGuizmoNET;
using VoxelGame.Engine.Components;
using VoxelGame.Engine.Data;
using VoxelGame.Engine.Utils;

namespace VoxelGame.Engine.Editor;

public static class Gizmo
{
    public static void EditTransform(ref GameObject gameObject)
    {
        var transform = gameObject.Transform;
        
        // var position = transform.Position;
        // if(ImGui.DragFloat3("Position", ref position, 0.1f))
        //     transform.Position = position;
        //         
        // var scale = transform.Scale;
        // if(ImGui.DragFloat("Scale", ref scale, 0.1f))
        //     transform.Scale = scale;
        //         
        // var rotation = transform.Rotation.ToEulerAngles();
        // if(ImGui.DragFloat3("Rotation", ref rotation, 0.1f))
        //     transform.Rotation = rotation.FromEulerAngles();

        var camera = VoxelRenderer.Camera;
        var transformMatrix = transform.ViewMatrix;

        var matrixTranslation = new float();
        var matrixRotation = new float();
        var matrixScale = new float();
        
        ImGuizmo.DecomposeMatrixToComponents(ref transformMatrix.M44, ref matrixTranslation, ref matrixRotation, ref matrixScale);
        
        var position = transform.Position;
        if(ImGui.DragFloat3("Position", ref position, 0.1f))
            transform.Position = position;
                
        var scale = transform.Scale;
        if(ImGui.DragFloat("Scale", ref scale, 0.1f))
            transform.Scale = scale;
                
        var rotation = transform.Rotation.ToEulerAngles();
        if(ImGui.DragFloat3("Rotation", ref rotation, 0.1f))
            transform.Rotation = rotation.FromEulerAngles();

        ImGuizmo.Manipulate(ref camera.ViewMatrix.M44, ref camera.ProjectionMatrix.M44, OPERATION.TRANSLATE, MODE.WORLD, ref transformMatrix.M44);
    }
}