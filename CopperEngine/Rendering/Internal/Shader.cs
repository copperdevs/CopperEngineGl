using System.Numerics;
using Silk.NET.OpenGL;

namespace CopperEngine.Rendering.Internal;

internal class Shader : IDisposable
{
    //Our handle and the GL instance this class will use, these are private because they have no reason to be public.
    //Most of the time you would want to abstract items to make things like this invisible.
    private readonly uint handle;
    private static GL Gl => EngineWindow.Gl!;

    public Shader(string vertexData, string fragmentData)
    {
        //Load the individual shaders.
        var vertex = LoadShader(ShaderType.VertexShader, vertexData);
        var fragment = LoadShader(ShaderType.FragmentShader, fragmentData);
        //Create the shader program.
        handle = Gl.CreateProgram();
        //Attach the individual shaders.
        Gl.AttachShader(handle, vertex);
        Gl.AttachShader(handle, fragment);
        Gl.LinkProgram(handle);
        //Check for linking errors.
        Gl.GetProgram(handle, GLEnum.LinkStatus, out var status);
        if (status == 0)
        {
            throw new Exception($"Program failed to link with error: {Gl.GetProgramInfoLog(handle)}");
        }
        //Detach and delete the shaders
        Gl.DetachShader(handle, vertex);
        Gl.DetachShader(handle, fragment);
        Gl.DeleteShader(vertex);
        Gl.DeleteShader(fragment);
    }

    public void Use()
    {
        //Using the program
        Gl.UseProgram(handle);
    }

    //Uniforms are properties that applies to the entire geometry
    public void SetUniform(string name, int value)
    {
        //Setting a uniform on a shader using a name.
        int location = Gl.GetUniformLocation(handle, name);
        if (location == -1) //If GetUniformLocation returns -1 the uniform is not found.
        {
            throw new Exception($"{name} uniform not found on shader.");
        }
        Gl.Uniform1(location, value);
    }

    public unsafe void SetUniform(string name, Matrix4x4 value)
    {
        //A new overload has been created for setting a uniform so we can use the transform in our shader.
        var location = Gl.GetUniformLocation(handle, name);
        if (location == -1)
        {
            throw new Exception($"{name} uniform not found on shader.");
        }
        Gl.UniformMatrix4(location, 1, false, (float*) &value);
    }

    public void SetUniform(string name, float value)
    {
        int location = Gl.GetUniformLocation(handle, name);
        if (location == -1)
        {
            throw new Exception($"{name} uniform not found on shader.");
        }
        Gl.Uniform1(location, value);
    }

    public void SetUniform(string name, Vector3 value)
    {
        int location = Gl.GetUniformLocation(handle, name);
        if (location == -1)
        {
            throw new Exception($"{name} uniform not found on shader.");
        }
        Gl.Uniform3(location, value);
    }
    
    public void SetUniform(string name, int x, int y, int z)
    {
        int location = Gl.GetUniformLocation(handle, name);
        if (location == -1)
        {
            throw new Exception($"{name} uniform not found on shader.");
        }
        Gl.Uniform3(location, x, y, z);
    }
    
    public void SetUniform(string name, float x, float y, float z)
    {
        int location = Gl.GetUniformLocation(handle, name);
        if (location == -1)
        {
            throw new Exception($"{name} uniform not found on shader.");
        }
        Gl.Uniform3(location, x, y, z);
    }
    
    public void Dispose()
    {
        //Remember to delete the program when we are done.
        Gl.DeleteProgram(handle);
    }

    private uint LoadShader(ShaderType type, string data)
    {
        var _handle = Gl.CreateShader(type);
        Gl.ShaderSource(_handle, data);
        Gl.CompileShader(_handle);
        var infoLog = Gl.GetShaderInfoLog(_handle);
        if (!string.IsNullOrWhiteSpace(infoLog))
        {
            throw new Exception($"Error compiling shader of type {type}, failed with error {infoLog}");
        }

        return _handle;
    }

    public static Shader LoadFromFile(GL gl, string vertexPath, string fragmentPath)
    {
        return new Shader(File.ReadAllText(vertexPath), File.ReadAllText(fragmentPath));
    }
}