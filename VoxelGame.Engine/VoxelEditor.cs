﻿using System.Numerics;
using ImGuiNET;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using VoxelGame.Engine.Components;
using VoxelGame.Engine.Scenes;

namespace VoxelGame.Engine;

internal static class VoxelEditor
{
    private static bool initialized;
    internal static ImGuiController? imGuiController;

    internal static bool imGuiInitialized = false;
    
    internal static void Initialize()
    {
        if (initialized)
            return;
        initialized = true;
        
        imGuiController = new ImGuiController
        (
            VoxelWindow.Gl,
            VoxelWindow.Window,
            VoxelWindow.InputContext
        );
        
        LoadConfig();
        LoadStyle();
        // LoadFont();

        imGuiInitialized = true;
    }
    private static void LoadConfig()
    {
        ImGui.GetIO().ConfigFlags |= ImGuiConfigFlags.DockingEnable;
        ImGui.GetIO().ConfigFlags |= ImGuiConfigFlags.ViewportsEnable;
        ImGui.GetIO().ConfigFlags |= ImGuiConfigFlags.NavEnableKeyboard;
        ImGui.GetIO().ConfigFlags |= ImGuiConfigFlags.NavEnableGamepad;
        // ImGui.GetIO().ConfigWindowsMoveFromTitleBarOnly = true;
        
        ImGui.GetStyle().WindowRounding = 5;
        ImGui.GetStyle().ChildRounding = 5;
        ImGui.GetStyle().FrameRounding = 5;
        ImGui.GetStyle().PopupRounding = 5;
        ImGui.GetStyle().ScrollbarRounding = 5;
        ImGui.GetStyle().GrabRounding = 5;
        ImGui.GetStyle().TabRounding = 5;

        ImGui.GetStyle().TabBorderSize = 1;

        ImGui.GetStyle().WindowTitleAlign = new Vector2(0.5f);
        // ImGui.GetStyle().SeparatorTextAlign = new Vector2(0.5f);
        // ImGui.GetStyle().SeparatorTextPadding = new Vector2(20, 5);
        
    }
    private static void LoadStyle()
    {
        var colors = ImGui.GetStyle().Colors;
        colors[(int)ImGuiCol.WindowBg] = new Vector4(0.1f, 0.105f, 0.11f, 1.0f);

        // Headers
        colors[(int)ImGuiCol.Header] = new Vector4( 0.2f, 0.205f, 0.21f, 1.0f );
        colors[(int)ImGuiCol.HeaderHovered] = new Vector4( 0.3f, 0.305f, 0.31f, 1.0f );
        colors[(int)ImGuiCol.HeaderActive] = new Vector4( 0.15f, 0.1505f, 0.151f, 1.0f );

        // Buttons
        colors[(int)ImGuiCol.Button] = new Vector4( 0.2f, 0.205f, 0.21f, 1.0f );
        colors[(int)ImGuiCol.ButtonHovered] = new Vector4( 0.3f, 0.305f, 0.31f, 1.0f );
        colors[(int)ImGuiCol.ButtonActive] = new Vector4( 0.15f, 0.1505f, 0.151f, 1.0f );

        // Frame BG
        colors[(int)ImGuiCol.FrameBg] = new Vector4( 0.2f, 0.205f, 0.21f, 1.0f );
        colors[(int)ImGuiCol.FrameBgHovered] = new Vector4( 0.3f, 0.305f, 0.31f, 1.0f );
        colors[(int)ImGuiCol.FrameBgActive] = new Vector4( 0.15f, 0.1505f, 0.151f, 1.0f );

        // Tabs
        colors[(int)ImGuiCol.Tab] = new Vector4( 0.15f, 0.1505f, 0.151f, 1.0f );
        colors[(int)ImGuiCol.TabHovered] = new Vector4( 0.38f, 0.3805f, 0.381f, 1.0f );
        colors[(int)ImGuiCol.TabActive] = new Vector4( 0.28f, 0.2805f, 0.281f, 1.0f );
        colors[(int)ImGuiCol.TabUnfocused] = new Vector4( 0.15f, 0.1505f, 0.151f, 1.0f );
        colors[(int)ImGuiCol.TabUnfocusedActive] = new Vector4( 0.2f, 0.205f, 0.21f, 1.0f );

        // Title
        colors[(int)ImGuiCol.TitleBg] = new Vector4( 0.15f, 0.1505f, 0.151f, 1.0f );
        colors[(int)ImGuiCol.TitleBgActive] = new Vector4( 0.15f, 0.1505f, 0.151f, 1.0f );
        colors[(int)ImGuiCol.TitleBgCollapsed] = new Vector4( 0.15f, 0.1505f, 0.151f, 1.0f );
    }
    private static void LoadFont()
    {
        var fonts = ImGui.GetIO().Fonts;

        
        // fonts.AddFontFromFileTTF("Resources/Fonts/Inter/static/Inter-Regular.ttf", 
            // 15,  null, fonts.GetGlyphRangesCyrillic());

        fonts.AddFontFromFileTTF("Resources/Fonts/Inter/static/Inter-Regular.ttf", 15);
    }

    internal static void Update(double delta)
    {
        imGuiController?.Update((float) delta);
    }

    internal static void Render()
    {
        RenderMenuBar();
        
        if(showDemoWindow)
            ImGui.ShowDemoWindow(ref showDemoWindow);
        
        InfoWindow.Render();
        
        imGuiController?.Render();
        SceneManager.CurrentSceneGameComponentsRenderEditor();
        SceneManager.GameComponentsRenderEditor(VoxelEngine.EngineAssets);
    }

    private static bool showDemoWindow = false;

    internal static void RenderMenuBar()
    {
        if (ImGui.BeginMainMenuBar())
        {
            if (ImGui.BeginMenu("Windows"))
            {
                ImGui.MenuItem("ImGui Demo", null, ref showDemoWindow);
                ImGui.MenuItem("Info", InfoWindow.IsOpen);
                ImGui.EndMenu();
            }
            
            ImGui.EndMainMenuBar();
        }
    }
}