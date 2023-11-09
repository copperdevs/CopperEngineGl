using VoxelGame.Engine;
using VoxelGame.Engine.Logs;

namespace VoxelGame.Testing;

public static class Program
{
    public static void Main()
    {
        VoxelEngine.Initialize<PhysicsTestingApplication>();
        VoxelEngine.Run();
    }
}