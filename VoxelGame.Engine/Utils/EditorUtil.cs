using System.Numerics;
using ImGuiNET;
using Jitter2.LinearMath;

namespace VoxelGame.Engine.Utils;

public static class EditorUtil
{

    public static bool DragMatrix4X4(string name, Matrix4x4 matrix4X4)
    {
        var matrix = matrix4X4;
        return DragMatrix4X4(name, ref matrix);
    }
    
    public static bool DragMatrix4X4(string name, ref Matrix4x4 matrix)
    {
        var interacted = false;
        
        if (ImGui.CollapsingHeader(name))
        {
            ImGui.Indent();
            
            interacted =
                DragMatrix4X4Row(ref matrix.M11, ref matrix.M12, ref matrix.M13, ref matrix.M14) || 
                DragMatrix4X4Row(ref matrix.M21, ref matrix.M22, ref matrix.M23, ref matrix.M24) || 
                DragMatrix4X4Row(ref matrix.M31, ref matrix.M32, ref matrix.M33, ref matrix.M34) || 
                DragMatrix4X4Row(ref matrix.M41, ref matrix.M42, ref matrix.M43, ref matrix.M44);
            
            ImGui.Unindent();
        }

        return interacted;
    }

    private static bool DragMatrix4X4Row(ref float itemOne, ref float itemTwo, ref float itemThree, ref float itemFour)
    {
        var interacted = false;
        var row = new Vector4(itemOne, itemTwo, itemThree, itemFour);

        if (ImGui.DragFloat4("Row One", ref row))
        {
            interacted = true;
            itemOne = row.X;
            itemTwo = row.Y;
            itemThree = row.Z;
            itemFour = row.W;
        }
        
        return interacted;
    }
    
    public static bool DragMatrix3X3(string name, JMatrix matrix3X3)
    {
        var matrix = matrix3X3;
        return DragMatrix3X3(name, ref matrix);
    }
    
    public static bool DragMatrix3X3(string name, ref JMatrix matrix)
    {
        var interacted = false;
        
        if (ImGui.CollapsingHeader(name))
        {
            ImGui.Indent();
            
            interacted =
                DragMatrix3X3Row(ref matrix.M11, ref matrix.M12, ref matrix.M13) || 
                DragMatrix3X3Row(ref matrix.M21, ref matrix.M22, ref matrix.M23) || 
                DragMatrix3X3Row(ref matrix.M31, ref matrix.M32, ref matrix.M33);
            
            ImGui.Unindent();
        }

        return interacted;
    }

    private static bool DragMatrix3X3Row(ref float itemOne, ref float itemTwo, ref float itemThree)
    {
        var interacted = false;
        var row = new Vector3(itemOne, itemTwo, itemThree);

        if (ImGui.DragFloat3("Row One", ref row))
        {
            interacted = true;
            itemOne = row.X;
            itemTwo = row.Y;
            itemThree = row.Z;
        }
        
        return interacted;
    }
}