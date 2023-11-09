// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CopperEngine.Utils;
using Silk.NET.OpenGL;

namespace CopperEngine.Editor.Dear_ImGui;

internal enum TextureCoordinate
{
    S = TextureParameterName.TextureWrapS,
    T = TextureParameterName.TextureWrapT,
    R = TextureParameterName.TextureWrapR
}

internal class ImGuiTexture : IDisposable
{
    internal const SizedInternalFormat Srgb8Alpha8 = (SizedInternalFormat)GLEnum.Srgb8Alpha8;
    internal const SizedInternalFormat Rgb32F = (SizedInternalFormat)GLEnum.Rgb32f;

    internal const GLEnum MaxTextureMaxAnisotropy = (GLEnum)0x84FF;

    internal static float? MaxAniso;
    private readonly GL _gl;
    internal readonly string Name;
    internal readonly uint GlTexture;
    internal readonly uint Width, Height;
    internal readonly uint MipmapLevels;
    internal readonly SizedInternalFormat InternalFormat;

    internal unsafe ImGuiTexture(GL gl, int width, int height, IntPtr data, bool generateMipmaps = false,
        bool srgb = false)
    {
        _gl = gl;
        MaxAniso ??= gl.GetFloat(MaxTextureMaxAnisotropy);
        Width = (uint)width;
        Height = (uint)height;
        InternalFormat = srgb ? Srgb8Alpha8 : SizedInternalFormat.Rgba8;
        MipmapLevels = (uint)(generateMipmaps == false ? 1 : (int)Math.Floor(Math.Log(Math.Max(Width, Height), 2)));

        GlTexture = _gl.GenTexture();
        Bind();

        PixelFormat pxFormat = PixelFormat.Bgra;

        _gl.TexStorage2D(GLEnum.Texture2D, MipmapLevels, InternalFormat, Width, Height);
        _gl.TexSubImage2D(GLEnum.Texture2D, 0, 0, 0, Width, Height, pxFormat, PixelType.UnsignedByte, (void*)data);

        if (generateMipmaps)
            _gl.GenerateTextureMipmap(GlTexture);
        SetWrap(TextureCoordinate.S, TextureWrapMode.Repeat);
        SetWrap(TextureCoordinate.T, TextureWrapMode.Repeat);

        _gl.TexParameterI(GLEnum.Texture2D, TextureParameterName.TextureMaxLevel, MipmapLevels - 1);
    }

    internal void Bind()
    {
        _gl.BindTexture(GLEnum.Texture2D, GlTexture);
    }

    internal void SetMinFilter(TextureMinFilter filter)
    {
        _gl.TexParameterI(GLEnum.Texture2D, TextureParameterName.TextureMinFilter, (int)filter);
    }

    internal void SetMagFilter(TextureMagFilter filter)
    {
        _gl.TexParameterI(GLEnum.Texture2D, TextureParameterName.TextureMagFilter, (int)filter);
    }

    internal void SetAnisotropy(float level)
    {
        const TextureParameterName textureMaxAnisotropy = (TextureParameterName)0x84FE;
        _gl.TexParameter(GLEnum.Texture2D, (GLEnum)textureMaxAnisotropy,
            MathUtil.Clamp(level, 1, MaxAniso.GetValueOrDefault()));
    }

    internal void SetLod(int @base, int min, int max)
    {
        _gl.TexParameterI(GLEnum.Texture2D, TextureParameterName.TextureLodBias, @base);
        _gl.TexParameterI(GLEnum.Texture2D, TextureParameterName.TextureMinLod, min);
        _gl.TexParameterI(GLEnum.Texture2D, TextureParameterName.TextureMaxLod, max);
    }

    internal void SetWrap(TextureCoordinate coord, TextureWrapMode mode)
    {
        _gl.TexParameterI(GLEnum.Texture2D, (TextureParameterName)coord, (int)mode);
    }

    public void Dispose()
    {
        _gl.DeleteTexture(GlTexture);
    }
}