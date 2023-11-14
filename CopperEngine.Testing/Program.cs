using CopperEngine;

namespace CopperEngine.Testing;

public static class Program
{
    public static void Main()
    {
        Engine.Initialize<TestingApplication>();
        // Engine.Initialize();
        Engine.Run();
    }
}