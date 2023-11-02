// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using ImGuiNET;

namespace VoxelGame.Engine.Editor;

internal readonly struct ImGuiFontConfig
{
    internal ImGuiFontConfig(string fontPath, int fontSize, Func<ImGuiIOPtr, IntPtr> getGlyphRange = null)
    {
        if (fontSize <= 0) throw new ArgumentOutOfRangeException(nameof(fontSize));
        FontPath = fontPath ?? throw new ArgumentNullException(nameof(fontPath));
        FontSize = fontSize;
        GetGlyphRange = getGlyphRange;
    }

    internal string FontPath { get; }
    internal int FontSize { get; }
    internal Func<ImGuiIOPtr, IntPtr> GetGlyphRange { get; }
}