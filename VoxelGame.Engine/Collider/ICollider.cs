using VoxelGame.Engine.Rendering;

namespace VoxelGame.Engine.Collider;

public interface ICollider
{
    public CopperModel ColliderModel { get; set; }
}