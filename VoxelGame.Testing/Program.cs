using VoxelGame.Engine;

namespace VoxelGame.Testing;

public static class Program
{
    public static void Main()
    {
        VoxelEngine.Initialize<TestingApplication>();
        VoxelEngine.Run();
    }
}