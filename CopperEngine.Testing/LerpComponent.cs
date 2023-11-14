using CopperEngine.Components;
using CopperEngine.Info;
using CopperEngine.Utils;

namespace CopperEngine.Testing;

public class LerpComponent : GameComponent
{
    private GameObject targetGameObject;
    public float MoveSpeed = 0.5f;
    
    public LerpComponent(GameObject targetGameObject)
    {
        this.targetGameObject = targetGameObject;
    }

    public override void Update()
    {
        Transform.Position = MathUtil.Lerp(Transform.Position, targetGameObject.Transform.Position, MoveSpeed * Time.DeltaTime);
    }
}