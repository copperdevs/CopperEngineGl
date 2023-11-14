namespace CopperEngine.Rendering;

public abstract class RenderFeature
{
    public abstract RenderFeatureEvent Event { get; set; }
    
    public abstract void Start();
    public abstract void Render();
    public abstract void Stop();
}