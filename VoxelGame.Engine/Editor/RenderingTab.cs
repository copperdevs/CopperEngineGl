using System.Numerics;
using System.Reflection;
using CopperEngine.Components;
using CopperEngine.Data;
using CopperEngine.Logs;
using CopperEngine.Rendering;
using CopperEngine.Scenes;
using CopperEngine.Utils;
using ImGuiNET;
using Color = CopperEngine.Data.Color;

namespace CopperEngine.Editor;

internal static class RenderingTab
{
    internal static void Render()
    {
        RenderFeatureInspector();
    }
    
    private static void RenderFeatureInspector()
    {
        var renderFeatures = EngineRenderer.renderFeatures;

        for (var index = 0; index < renderFeatures.Count; index++)
        {
            var renderFeature = renderFeatures[index];
            
            if (ImGui.CollapsingHeader(renderFeature.GetType().FullName))
            {
                ImGui.Indent();
                var fieldInfos = renderFeature.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
                foreach (var fieldInfo in fieldInfos)
                {
                    try
                    {
                        ImGuiRenderers[fieldInfo.FieldType].Invoke(fieldInfo, renderFeature);
                    }
                    catch (Exception e)
                    {
                        Log.Error(e);
                    }
                    // ImGui.Text($"{fieldInfo.FieldType.FullName}");
                }
                ImGui.Unindent();
            }

            renderFeatures[index] = renderFeature;
        }
        
        EngineRenderer.renderFeatures = renderFeatures;
    }

    private static readonly Dictionary<Type, Action<FieldInfo, RenderFeature>> ImGuiRenderers = new()
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
        { typeof(Color), ColorFieldRenderer }
    };

    private static void FloatFieldRenderer(FieldInfo fieldInfo, RenderFeature component)
    {
        var value = (float)(fieldInfo.GetValue(component) ?? 0);
        if(ImGui.DragFloat($"{fieldInfo.Name}##{fieldInfo.Name}", ref value))
            fieldInfo.SetValue(component, value);
    }
    
    private static void IntFieldRenderer(FieldInfo fieldInfo, RenderFeature component)
    {
        var value = (int)(fieldInfo.GetValue(component) ?? 0);
        if(ImGui.DragInt($"{fieldInfo.Name}##{fieldInfo.Name}", ref value))
            fieldInfo.SetValue(component, value);
    }
    
    private static void BoolFieldRenderer(FieldInfo fieldInfo, RenderFeature component)
    {
        var value = (bool)(fieldInfo.GetValue(component) ?? false);
        if(ImGui.Checkbox($"{fieldInfo.Name}##{fieldInfo.Name}", ref value))
            fieldInfo.SetValue(component, value);
    }
    
    private static void Vector2FieldRenderer(FieldInfo fieldInfo, RenderFeature component)
    {
        var value = (Vector2)(fieldInfo.GetValue(component) ?? Vector2.Zero);
        if(ImGui.DragFloat2($"{fieldInfo.Name}##{fieldInfo.Name}", ref value))
            fieldInfo.SetValue(component, value);
    }
    
    private static void Vector3FieldRenderer(FieldInfo fieldInfo, RenderFeature component)
    {
        var value = (Vector3)(fieldInfo.GetValue(component) ?? Vector3.Zero);
        if(ImGui.DragFloat3($"{fieldInfo.Name}##{fieldInfo.Name}", ref value))
            fieldInfo.SetValue(component, value);
    }
    
    private static void Vector4FieldRenderer(FieldInfo fieldInfo, RenderFeature component)
    {
        var value = (Vector4)(fieldInfo.GetValue(component) ?? Vector4.Zero);
        if(ImGui.DragFloat4($"{fieldInfo.Name}##{fieldInfo.Name}", ref value))
            fieldInfo.SetValue(component, value);
    }
    
    private static void QuaternionFieldRenderer(FieldInfo fieldInfo, RenderFeature component)
    {
        var value = ((Quaternion)(fieldInfo.GetValue(component) ?? Quaternion.Identity)).ToVector();
        if(ImGui.DragFloat4($"{fieldInfo.Name}##{fieldInfo.Name}", ref value))
        {
            var result = value.ToQuaternion();
            fieldInfo.SetValue(component, result);
        }
    }
    
    private static void GuidFieldRenderer(FieldInfo fieldInfo, RenderFeature component)
    {
        var value = (Guid)(fieldInfo.GetValue(component) ?? new Guid());
        ImGui.LabelText($"{fieldInfo.Name}##{fieldInfo.Name}", value.ToString());
    }
    
    private static void SceneFieldRenderer(FieldInfo fieldInfo, RenderFeature component)
    {
        var value = (Scene)(fieldInfo.GetValue(component) ?? null)!;

        if (ImGui.CollapsingHeader($"{fieldInfo.Name}##{fieldInfo.Name}"))
        {
            ImGui.LabelText("Scene Name", value.Name);
            ImGui.LabelText("Scene Id", value.SceneId.ToString());
        }
    }
    
    private static void TransformFieldRenderer(FieldInfo fieldInfo, RenderFeature component)
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

    private static void ColorFieldRenderer(FieldInfo fieldInfo, RenderFeature component)
    {
        var value = (Color)(fieldInfo.GetValue(component) ?? new Color(0));
        var color = value / 255;
        Vector4 vecColor = color;
        if (ImGui.ColorEdit4($"{fieldInfo.Name}##{fieldInfo.Name}", ref vecColor))
        {
            fieldInfo.SetValue(component, new Color(vecColor * 255));
        }
    }
}