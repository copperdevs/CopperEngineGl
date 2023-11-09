namespace CopperEngine.Utils;

public static class FileUtil
{
    /// <summary>
    /// Creates a new temporary file in the local directory
    /// </summary>
    /// <param name="name">Name of the file</param>
    /// <returns>Path of the file</returns>
    public static string CreateTempFile(string name)
    {
        try
        {
            if (!Directory.Exists("Resources/Temporary"))
                Directory.CreateDirectory("Resources/Temporary");
            
            var fileName = $"Resources/Temporary/{name}";

            File.Create(fileName).Dispose();

            Console.WriteLine("Creating temp file at " + fileName);
            return fileName;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Unable to create a temp file: " + ex.Message);
            return Path.GetTempFileName();
        }
    }
}