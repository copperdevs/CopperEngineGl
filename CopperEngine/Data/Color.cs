using System.Numerics;

namespace CopperEngine.Data;

public struct Color
{
    public float R;
    public float G;
    public float B;
    public float A;

    public Color(float r, float g, float b, float a)
    {
        R = r;
        G = g;
        B = b;
        A = a;
    }

    public Color(float r, float g, float b)
    {
        R = r;
        G = g;
        B = b;
        A = 255;
    }

    public Color(float value)
    {
        R = value;
        G = value;
        B = value;
        A = value;
    }

    public Color() : this(255)
    {
        
    }

    public Color(Vector4 vector) : this(vector.X, vector.Y, vector.Z, vector.W)
    
    {
        
    }

    public Color(Vector3 vector) : this(vector.X, vector.Y, vector.Z)
    {
        
    }

    public static implicit operator Vector3(Color color)
    {
        return new Vector3(color.R, color.G, color.B);
    }

    public static implicit operator Vector4(Color color)
    {
        return new Vector4(color.R, color.G, color.B, color.A);
    }

    public static Color operator /(Color color, float value)
    {
        return new Color(color.R / value, color.G / value, color.B / value, color.A / value);
    }

    public static Color operator *(Color color, float value)
    {
        return new Color(color.R * value, color.G * value, color.B * value, color.A * value);
    }
}