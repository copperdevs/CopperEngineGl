using System.Numerics;
using Silk.NET.OpenGL;

namespace CopperEngine.Rendering.Internal;

internal class Shader : IDisposable
{
    //Our handle and the GL instance this class will use, these are private because they have no reason to be public.
    //Most of the time you would want to abstract items to make things like this invisible.
    private readonly uint handle;
    private readonly GL gl;

    public Shader(GL gl, string vertexData, string fragmentData)
    {
        this.gl = gl;

        //Load the individual shaders.
        var vertex = LoadShader(ShaderType.VertexShader, vertexData);
        var fragment = LoadShader(ShaderType.FragmentShader, fragmentData);
        //Create the shader program.
        handle = this.gl.CreateProgram();
        //Attach the individual shaders.
        this.gl.AttachShader(handle, vertex);
        this.gl.AttachShader(handle, fragment);
        this.gl.LinkProgram(handle);
        //Check for linking errors.
        this.gl.GetProgram(handle, GLEnum.LinkStatus, out var status);
        if (status == 0)
        {
            throw new Exception($"Program failed to link with error: {this.gl.GetProgramInfoLog(handle)}");
        }
        //Detach and delete the shaders
        this.gl.DetachShader(handle, vertex);
        this.gl.DetachShader(handle, fragment);
        this.gl.DeleteShader(vertex);
        this.gl.DeleteShader(fragment);
    }

    public void Use()
    {
        //Using the program
        gl.UseProgram(handle);
    }

    //Uniforms are properties that applies to the entire geometry
    public void SetUniform(string name, int value)
    {
        //Setting a uniform on a shader using a name.
        int location = gl.GetUniformLocation(handle, name);
        if (location == -1) //If GetUniformLocation returns -1 the uniform is not found.
        {
            throw new Exception($"{name} uniform not found on shader.");
        }
        gl.Uniform1(location, value);
    }

    public unsafe void SetUniform(string name, Matrix4x4 value)
    {
        //A new overload has been created for setting a uniform so we can use the transform in our shader.
        var location = gl.GetUniformLocation(handle, name);
        if (location == -1)
        {
            throw new Exception($"{name} uniform not found on shader.");
        }
        gl.UniformMatrix4(location, 1, false, (float*) &value);
    }

    public void SetUniform(string name, float value)
    {
        int location = gl.GetUniformLocation(handle, name);
        if (location == -1)
        {
            throw new Exception($"{name} uniform not found on shader.");
        }
        gl.Uniform1(location, value);
    }

    public void Dispose()
    {
        //Remember to delete the program when we are done.
        gl.DeleteProgram(handle);
    }

    private uint LoadShader(ShaderType type, string data)
    {
        var _handle = gl.CreateShader(type);
        gl.ShaderSource(_handle, data);
        gl.CompileShader(_handle);
        var infoLog = gl.GetShaderInfoLog(_handle);
        if (!string.IsNullOrWhiteSpace(infoLog))
        {
            throw new Exception($"Error compiling shader of type {type}, failed with error {infoLog}");
        }

        return _handle;
    }

    public static Shader LoadFromFile(GL gl, string vertexPath, string fragmentPath)
    {
        return new Shader(gl, File.ReadAllText(vertexPath), File.ReadAllText(fragmentPath));
    }
}