﻿using System.Numerics;
using ImGuiNET;
using Jitter2.LinearMath;

namespace CopperEngine.Utils;

public static class EditorUtil
{

    /// <summary>
    /// Creates an non interactable ImGui 4X4 matrix UI.
    /// </summary>
    /// <remarks>Creates a copy of the matrix first and uses that</remarks>
    /// <param name="name">Name of the dropdown</param>
    /// <param name="matrix4X4">Target matrix</param>
    /// <param name="enabled">Are the fields interactable</param>
    /// <returns>True if the ui was interacted with</returns>
    public static bool DragMatrix4X4(string name, Matrix4x4 matrix4X4, bool enabled = true)
    {
        var matrix = matrix4X4;
        return DragMatrix4X4(name, ref matrix, enabled);
    }
    
    /// <summary>
    /// Creates an interactable ImGui 4X4 matrix UI.
    /// </summary>
    /// <param name="name">Name of the dropdown</param>
    /// <param name="matrix">Target matrix</param>
    /// <param name="enabled">Are the fields interactable</param>
    /// <returns>True if the ui was interacted with</returns>
    public static bool DragMatrix4X4(string name, ref Matrix4x4 matrix, bool enabled = true)
    {
        var interacted = false;
        
        if (ImGui.CollapsingHeader(name))
        {
            ImGui.Indent();
            
            interacted =
                DragMatrix4X4Row($"Row One##{name}", ref matrix.M11, ref matrix.M12, ref matrix.M13, ref matrix.M14, enabled) || 
                DragMatrix4X4Row($"Row Two##{name}", ref matrix.M21, ref matrix.M22, ref matrix.M23, ref matrix.M24, enabled) || 
                DragMatrix4X4Row($"Row Three##{name}", ref matrix.M31, ref matrix.M32, ref matrix.M33, ref matrix.M34, enabled) || 
                DragMatrix4X4Row($"Row Four##{name}", ref matrix.M41, ref matrix.M42, ref matrix.M43, ref matrix.M44, enabled);
            
            ImGui.Unindent();
        }

        return interacted;
    }

    private static bool DragMatrix4X4Row(string rowName, ref float itemOne, ref float itemTwo, ref float itemThree, ref float itemFour, bool enabled = true)
    {
        var interacted = false;
        var row = new Vector4(itemOne, itemTwo, itemThree, itemFour);

        if(!enabled) ImGui.BeginDisabled();
        
        if (ImGui.DragFloat4(rowName, ref row))
        {
            interacted = true;
            itemOne = row.X;
            itemTwo = row.Y;
            itemThree = row.Z;
            itemFour = row.W;
        }
        
        if(!enabled) ImGui.EndDisabled();
        
        return interacted;
    }
    
    /// <summary>
    /// Creates an interactable ImGui 3X3 matrix UI.
    /// </summary>
    /// <param name="name">Name of the dropdown</param>
    /// <param name="matrix3X3">Target matrix</param>
    /// <param name="enabled">Are the fields interactable</param>
    /// <returns>True if interacted with</returns>
    /// <remarks>
    /// <para> Creates a copy of the matrix first, so it is not interactable. </para>
    /// Uses a JMatrix from JitterPhysics because there is no built in 3X3 matrix. 
    /// </remarks>
    public static bool DragMatrix3X3(string name, JMatrix matrix3X3, bool enabled = true)
    {
        var matrix = matrix3X3;
        return DragMatrix3X3(name, ref matrix, enabled);
    }
    
    /// <summary>
    /// Creates an interactable ImGui 3X3 matrix UI.
    /// </summary>
    /// <param name="name">Name of the dropdown</param>
    /// <param name="matrix">Target matrix</param>
    /// <param name="enabled">Are the fields interactable</param>
    /// <returns>True if interacted with</returns>
    /// <remarks>
    /// <para> Creates a copy of the matrix first, so it is not interactable. </para>
    /// Uses a JMatrix from JitterPhysics because there is no built in 3X3 matrix. 
    /// </remarks>
    public static bool DragMatrix3X3(string name, ref JMatrix matrix, bool enabled = true)
    {
        var interacted = false;
        
        if (ImGui.CollapsingHeader(name))
        {
            ImGui.Indent();
            
            interacted =
                DragMatrix3X3Row($"Row One##{name}", ref matrix.M11, ref matrix.M12, ref matrix.M13, enabled) || 
                DragMatrix3X3Row($"Row Two##{name}", ref matrix.M21, ref matrix.M22, ref matrix.M23, enabled) || 
                DragMatrix3X3Row($"Row Three##{name}", ref matrix.M31, ref matrix.M32, ref matrix.M33, enabled);
            
            ImGui.Unindent();
        }

        return interacted;
    }

    private static bool DragMatrix3X3Row(string rowName, ref float itemOne, ref float itemTwo, ref float itemThree, bool enabled = true)
    {
        var interacted = false;
        var row = new Vector3(itemOne, itemTwo, itemThree);

        if(!enabled) ImGui.BeginDisabled();
        
        if (ImGui.DragFloat3("Row One", ref row))
        {
            interacted = true;
            itemOne = row.X;
            itemTwo = row.Y;
            itemThree = row.Z;
        }
        
        
        if(!enabled) ImGui.EndDisabled();
        
        return interacted;
    }
}