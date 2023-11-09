using System.Numerics;
using System.Reflection;
using CopperEngine.Components;
using CopperEngine.Data;
using CopperEngine.Logs;
using CopperEngine.Scenes;
using CopperEngine.Utils;
using ImGuiNET;

namespace CopperEngine.Editor;

internal static class ObjectBrowserTab
{
    private static GameObject? currentObjectBrowserTarget;
    
    internal static void Render()
    {
        ObjectBrowserSection();

        if (currentObjectBrowserTarget is null) 
            return;
        
        ImGui.SameLine();
        
        ImGui.BeginGroup();
        ImGui.BeginChild("object_browser_inspector_window", new Vector2(0, -ImGui.GetFrameHeightWithSpacing()));

        ObjectInspectorSection();
        
        ImGui.EndChild();
        ImGui.EndGroup();
    }

    private static void ObjectBrowserSection()
    {
        if (ImGui.BeginChild("object_browser_objects_window", new Vector2(ImGui.GetWindowWidth()*0.25f, 0), true))
        {
            var list = SceneManager.CurrentScene().GameObjects;
            for (var i = 0; i < list.Count; i++)
            {
                var gameObject = list[i];

                if (ImGui.Selectable($"GameObject #{i}"))
                    currentObjectBrowserTarget = gameObject;
            }
            ImGui.EndChild();
        }
    }

    private static void ObjectInspectorSection()
    {
        ObjectInspectorTransform();
        ObjectInspectorComponents();
    }

    private static void ObjectInspectorTransform()
    {
        if (currentObjectBrowserTarget is null) 
            return;
        
        if (ImGui.CollapsingHeader("Transform"))
        {
            ImGui.Indent();

            var position = currentObjectBrowserTarget.Transform.Position;
            if(ImGui.DragFloat3("Position", ref position, 0.1f))
                currentObjectBrowserTarget.Transform.Position = position;
                    
            var scale = currentObjectBrowserTarget.Transform.Scale;
            if(ImGui.DragFloat3("Scale", ref scale, 0.1f))
                currentObjectBrowserTarget.Transform.Scale = scale;
                    
            var rotation = currentObjectBrowserTarget.Transform.Rotation.ToEulerAngles();
            if(ImGui.DragFloat3("Rotation", ref rotation, 0.1f))
                currentObjectBrowserTarget.Transform.Rotation = rotation.FromEulerAngles();
            
            ImGui.Unindent();
        }
    }

    private static void ObjectInspectorComponents()
    {
        if (currentObjectBrowserTarget is null) 
            return;
        
        for (var index = 0; index < currentObjectBrowserTarget.Components.Count; index++)
        {
            var component = currentObjectBrowserTarget.Components[index];

            if (ImGui.CollapsingHeader($"{component.GetType().Name}##{index}"))
            {
                ImGui.Indent();
                ObjectInspectorComponent(component);
                ImGui.Unindent();
            }
        }
    }

    private static void ObjectInspectorComponent(GameComponent component)
    {
        ImGui.Text(component.GetType().Name);
        var fieldInfos = component.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
        // fieldInfos[0].GetType();
        foreach (var fieldInfo in fieldInfos)
        {
            try
            {
                ImGuiRenderers[fieldInfo.FieldType].Invoke(fieldInfo, component);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            // ImGui.Text($"{fieldInfo.FieldType.FullName}");
        }
        component.RenderEditor();
    }

    private static readonly Dictionary<Type, Action<FieldInfo, GameComponent>> ImGuiRenderers = new()
    {
        { typeof(float), FloatFieldRenderer },
        { typeof(int), IntFieldRenderer },
        { typeof(bool), BoolFieldRenderer },
        { typeof(Vector2), Vector2FieldRenderer },
        { typeof(Vector3), Vector3FieldRenderer },
        { typeof(Vector4), Vector4FieldRenderer },
        { typeof(Quaternion), QuaternionFieldRenderer },
        { typeof(Guid), GuidFieldRenderer },
        { typeof(Scene), SceneFieldRenderer },
        { typeof(Transform), TransformFieldRenderer },
    };

    private static void FloatFieldRenderer(FieldInfo fieldInfo, GameComponent component)
    {
        var value = (float)(fieldInfo.GetValue(component) ?? 0);
        if(ImGui.DragFloat($"{fieldInfo.Name}##{fieldInfo.Name}", ref value))
            fieldInfo.SetValue(component, value);
    }
    
    private static void IntFieldRenderer(FieldInfo fieldInfo, GameComponent component)
    {
        var value = (int)(fieldInfo.GetValue(component) ?? 0);
        if(ImGui.DragInt($"{fieldInfo.Name}##{fieldInfo.Name}", ref value))
            fieldInfo.SetValue(component, value);
    }
    
    private static void BoolFieldRenderer(FieldInfo fieldInfo, GameComponent component)
    {
        var value = (bool)(fieldInfo.GetValue(component) ?? false);
        if(ImGui.Checkbox($"{fieldInfo.Name}##{fieldInfo.Name}", ref value))
            fieldInfo.SetValue(component, value);
    }
    
    private static void Vector2FieldRenderer(FieldInfo fieldInfo, GameComponent component)
    {
        var value = (Vector2)(fieldInfo.GetValue(component) ?? Vector2.Zero);
        if(ImGui.DragFloat2($"{fieldInfo.Name}##{fieldInfo.Name}", ref value))
            fieldInfo.SetValue(component, value);
    }
    
    private static void Vector3FieldRenderer(FieldInfo fieldInfo, GameComponent component)
    {
        var value = (Vector3)(fieldInfo.GetValue(component) ?? Vector3.Zero);
        if(ImGui.DragFloat3($"{fieldInfo.Name}##{fieldInfo.Name}", ref value))
            fieldInfo.SetValue(component, value);
    }
    
    private static void Vector4FieldRenderer(FieldInfo fieldInfo, GameComponent component)
    {
        var value = (Vector4)(fieldInfo.GetValue(component) ?? Vector4.Zero);
        if(ImGui.DragFloat4($"{fieldInfo.Name}##{fieldInfo.Name}", ref value))
            fieldInfo.SetValue(component, value);
    }
    
    private static void QuaternionFieldRenderer(FieldInfo fieldInfo, GameComponent component)
    {
        var value = ((Quaternion)(fieldInfo.GetValue(component) ?? Quaternion.Identity)).ToVector();
        if(ImGui.DragFloat4($"{fieldInfo.Name}##{fieldInfo.Name}", ref value))
        {
            var result = value.ToQuaternion();
            fieldInfo.SetValue(component, result);
        }
    }
    
    private static void GuidFieldRenderer(FieldInfo fieldInfo, GameComponent component)
    {
        var value = (Guid)(fieldInfo.GetValue(component) ?? new Guid());
        ImGui.LabelText($"{fieldInfo.Name}##{fieldInfo.Name}", value.ToString());
    }
    
    private static void SceneFieldRenderer(FieldInfo fieldInfo, GameComponent component)
    {
        var value = (Scene)(fieldInfo.GetValue(component) ?? null)!;

        if (ImGui.CollapsingHeader($"{fieldInfo.Name}##{fieldInfo.Name}"))
        {
            ImGui.LabelText("Scene Name", value.Name);
            ImGui.LabelText("Scene Id", value.SceneId.ToString());
        }
    }
    
    private static void TransformFieldRenderer(FieldInfo fieldInfo, GameComponent component)
    {
        var value = (Transform)(fieldInfo.GetValue(component) ?? 0);
        
        if (ImGui.CollapsingHeader($"{fieldInfo.Name}##{fieldInfo.Name}"))
        {
            ImGui.Indent();

            var position = value.Position;
            if(ImGui.DragFloat3("Position", ref position, 0.1f))
            {
                value.Position = position;
                fieldInfo.SetValue(component, value);
            }

            var scale = value.Scale;
            if(ImGui.DragFloat3("Scale", ref scale, 0.1f))
            {
                value.Scale = scale;
                fieldInfo.SetValue(component, value);
            }

            var rotation = value.Rotation.ToEulerAngles();
            if(ImGui.DragFloat3("Rotation", ref rotation, 0.1f))
            {
                value.Rotation = rotation.FromEulerAngles();
                fieldInfo.SetValue(component, value);
            }

            ImGui.Unindent();
        }
    }
}