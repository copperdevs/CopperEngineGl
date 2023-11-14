using System.Reflection;
using System.Text;

namespace CopperEngine.Resources;

internal static class ResourcesLoader
{
    private static readonly Assembly? Assembly = Assembly.GetAssembly(typeof(ResourcesLoader));
    private static readonly string[] Resources = Assembly?.GetManifestResourceNames()!;

    public static string[] GetAllResources() => Resources;
    
    public static object? LoadResource(string name)
    {
        if (Resources.Any(x => x.EndsWith(name)))
        {
            var enumerable = Resources.Where(x => x.EndsWith(name));
            return LoadResourceDirect(enumerable.ToList()[0]);
        }

        return null;
    }
    
    public static object LoadResourceDirect(string path)
    {
        using var stream = Assembly?.GetManifestResourceStream(path);
        using var reader = new StreamReader(stream!);
        return reader.ReadToEnd();
    }

    public static string LoadTextResource(string name) => LoadResource(name) as string ?? string.Empty;
    public static string LoadTextResourceDirect(string path) => LoadResourceDirect(path) as string ?? string.Empty;

    public static byte[] LoadImageResource(string name)
    {
        return Encoding.ASCII.GetBytes(LoadTextResource(name));
        return Array.Empty<byte>();
    }

    public static byte[] LoadImageResourceDirect(string path) => LoadResourceDirect(path) as byte[] ?? Array.Empty<byte>();
}