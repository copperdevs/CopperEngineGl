using CopperEngine;

namespace VoxelGame.Testing;

public static class Program
{
    public static void Main()
    {
        Engine.Initialize<PhysicsTestingApplication>();
        Engine.Run();
    }
}