using System.Numerics;
using CopperEngine.Components;
using CopperEngine.Rendering.Internal;
using CopperEngine.Resources;
using CopperEngine.Utils;
using Color = CopperEngine.Data.Color;

namespace CopperEngine.Rendering;

public class Light : GameComponent
{
    private Shader shader;
    public Color LightColor = new(255);
    public Color ObjectColor = new(255);

    public Light()
    {
        EngineRenderer.Lights.Add(this);
    }

    public override void Start()
    {
        var vertShader = ResourcesLoader.LoadTextResourceDirect("CopperEngine.Resources.Shaders.shader.vert");
        var fragShader = ResourcesLoader.LoadTextResourceDirect("CopperEngine.Resources.Shaders.lighting.frag");
        shader = new Shader(vertShader, fragShader);
    }

    public override void Render()
    {
        shader.Use();

        shader.SetUniform("uModel", Transform!.Matrix);
        shader.SetUniform("uView", EngineRenderer.Camera.ViewMatrix);
        shader.SetUniform("uProjection", EngineRenderer.Camera.ProjectionMatrix);
        shader.SetUniform("objectColor", new Vector3(1.0f, 0.5f, 0.31f));
        shader.SetUniform("lightColor", ObjectColor/255);
        shader.SetUniform("lightPos", LightColor/255);
        shader.SetUniform("viewPos", EngineRenderer.Camera.Position);
    }
}