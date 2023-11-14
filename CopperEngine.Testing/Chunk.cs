using Silk.NET.OpenGL;

namespace CopperEngine.Testing;

public class Chunk
{
    public const int ChunkSizeX = 8;
    public const int ChunkSizeY = 8;
    public const int ChunkSizeZ = 8;

    public readonly int[,,] Blocks = new int[ChunkSizeX, ChunkSizeY, ChunkSizeZ];

}