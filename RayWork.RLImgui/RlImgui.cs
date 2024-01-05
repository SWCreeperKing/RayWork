/*******************************************************************************************
 *
 *   raylib-extras [ImGui] example - Simple Integration
 *
 *	This is a simple ImGui Integration
 *	It is done using C++ but with C style code
 *	It can be done in C as well if you use the C ImGui wrapper
 *	https://github.com/cimgui/cimgui
 *
 *   Copyright (c) 2021 Jeffery Myers
 *
 ********************************************************************************************/
/************************************
 *
 * Modified by SW_CreeperKing
 * for RayWork.RLImgui
 *
 ************************************/

using System.Numerics;
using System.Text;
using ImGuiNET;
using Raylib_cs;
using static System.Runtime.InteropServices.Marshal;
using static RayWork.RLImgui.FontAwesome6;
using static ImGuiNET.ImGui;

namespace RayWork.RLImgui;

public static class RlImgui
{
    public static List<KeyboardKey> Keys = [];
    
    private const int StackAllocationSizeLimit = 2048;
    private static IntPtr ImGuiContext = IntPtr.Zero;
    private static ImGuiMouseCursor CurrentMouseCursor = ImGuiMouseCursor.COUNT;
    private static Dictionary<ImGuiMouseCursor, MouseCursor> MouseCursorMap = new();
    private static Texture2D FontTexture;
    private static Dictionary<KeyboardKey, ImGuiKey> RaylibKeyMap = new();
    private static bool LastFrameFocused;
    private static bool LastControlPressed;
    private static bool LastShiftPressed;
    private static bool LastAltPressed;
    private static bool LastSuperPressed;

    private static bool RlImGuiIsControlDown()
        => Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT_CONTROL) || Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_CONTROL);

    private static bool RlImGuiIsShiftDown()
        => Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT_SHIFT) || Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_SHIFT);

    private static bool RlImGuiIsAltDown()
        => Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT_ALT) || Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_ALT);

    private static bool RlImGuiIsSuperDown()
        => Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT_SUPER) || Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_SUPER);

    private delegate void SetupUserFontsCallback(ImGuiIOPtr imGuiIo);

    /// <summary>
    /// Callback for cases where the user wants to install additional fonts.
    /// </summary>
    private static SetupUserFontsCallback? SetupUserFonts = null;

    /// <summary>
    /// Sets up ImGui, loads fonts and themes
    /// </summary>
    /// <param name="darkTheme">when true(default) the dark theme is used, when false the light theme is used</param>
    /// <param name="enableDocking">when true(not default) docking support will be enabled/param>
    public static void Setup(bool darkTheme = true, bool enableDocking = false)
    {
        MouseCursorMap = new Dictionary<ImGuiMouseCursor, MouseCursor>();
        MouseCursorMap = new Dictionary<ImGuiMouseCursor, MouseCursor>();

        LastFrameFocused = Raylib.IsWindowFocused();
        LastControlPressed = false;
        LastShiftPressed = false;
        LastAltPressed = false;
        LastSuperPressed = false;

        FontTexture.Id = 0;

        BeginInitImGui();

        if (darkTheme)
        {
            StyleColorsDark();
        }
        else
        {
            StyleColorsLight();
        }

        if (enableDocking)
        {
            GetIO().ConfigFlags |= ImGuiConfigFlags.DockingEnable;
        }

        EndInitImGui();
    }

    /// <summary>
    /// Custom initialization. Not needed if you call Setup. Only needed if you want to add custom setup code.
    /// must be followed by EndInitImGui
    /// </summary>
    private static void BeginInitImGui()
    {
        SetupKeymap();
        ImGuiContext = CreateContext();
    }

    private static void SetupKeymap()
    {
        if (RaylibKeyMap.Count > 0) return;

        // build up a map of raylib keys to ImGuiKeys
        RaylibKeyMap[KeyboardKey.KEY_APOSTROPHE] = ImGuiKey.Apostrophe;
        RaylibKeyMap[KeyboardKey.KEY_COMMA] = ImGuiKey.Comma;
        RaylibKeyMap[KeyboardKey.KEY_MINUS] = ImGuiKey.Minus;
        RaylibKeyMap[KeyboardKey.KEY_PERIOD] = ImGuiKey.Period;
        RaylibKeyMap[KeyboardKey.KEY_SLASH] = ImGuiKey.Slash;
        RaylibKeyMap[KeyboardKey.KEY_ZERO] = ImGuiKey._0;
        RaylibKeyMap[KeyboardKey.KEY_ONE] = ImGuiKey._1;
        RaylibKeyMap[KeyboardKey.KEY_TWO] = ImGuiKey._2;
        RaylibKeyMap[KeyboardKey.KEY_THREE] = ImGuiKey._3;
        RaylibKeyMap[KeyboardKey.KEY_FOUR] = ImGuiKey._4;
        RaylibKeyMap[KeyboardKey.KEY_FIVE] = ImGuiKey._5;
        RaylibKeyMap[KeyboardKey.KEY_SIX] = ImGuiKey._6;
        RaylibKeyMap[KeyboardKey.KEY_SEVEN] = ImGuiKey._7;
        RaylibKeyMap[KeyboardKey.KEY_EIGHT] = ImGuiKey._8;
        RaylibKeyMap[KeyboardKey.KEY_NINE] = ImGuiKey._9;
        RaylibKeyMap[KeyboardKey.KEY_SEMICOLON] = ImGuiKey.Semicolon;
        RaylibKeyMap[KeyboardKey.KEY_EQUAL] = ImGuiKey.Equal;
        RaylibKeyMap[KeyboardKey.KEY_A] = ImGuiKey.A;
        RaylibKeyMap[KeyboardKey.KEY_B] = ImGuiKey.B;
        RaylibKeyMap[KeyboardKey.KEY_C] = ImGuiKey.C;
        RaylibKeyMap[KeyboardKey.KEY_D] = ImGuiKey.D;
        RaylibKeyMap[KeyboardKey.KEY_E] = ImGuiKey.E;
        RaylibKeyMap[KeyboardKey.KEY_F] = ImGuiKey.F;
        RaylibKeyMap[KeyboardKey.KEY_G] = ImGuiKey.G;
        RaylibKeyMap[KeyboardKey.KEY_H] = ImGuiKey.H;
        RaylibKeyMap[KeyboardKey.KEY_I] = ImGuiKey.I;
        RaylibKeyMap[KeyboardKey.KEY_J] = ImGuiKey.J;
        RaylibKeyMap[KeyboardKey.KEY_K] = ImGuiKey.K;
        RaylibKeyMap[KeyboardKey.KEY_L] = ImGuiKey.L;
        RaylibKeyMap[KeyboardKey.KEY_M] = ImGuiKey.M;
        RaylibKeyMap[KeyboardKey.KEY_N] = ImGuiKey.N;
        RaylibKeyMap[KeyboardKey.KEY_O] = ImGuiKey.O;
        RaylibKeyMap[KeyboardKey.KEY_P] = ImGuiKey.P;
        RaylibKeyMap[KeyboardKey.KEY_Q] = ImGuiKey.Q;
        RaylibKeyMap[KeyboardKey.KEY_R] = ImGuiKey.R;
        RaylibKeyMap[KeyboardKey.KEY_S] = ImGuiKey.S;
        RaylibKeyMap[KeyboardKey.KEY_T] = ImGuiKey.T;
        RaylibKeyMap[KeyboardKey.KEY_U] = ImGuiKey.U;
        RaylibKeyMap[KeyboardKey.KEY_V] = ImGuiKey.V;
        RaylibKeyMap[KeyboardKey.KEY_W] = ImGuiKey.W;
        RaylibKeyMap[KeyboardKey.KEY_X] = ImGuiKey.X;
        RaylibKeyMap[KeyboardKey.KEY_Y] = ImGuiKey.Y;
        RaylibKeyMap[KeyboardKey.KEY_Z] = ImGuiKey.Z;
        RaylibKeyMap[KeyboardKey.KEY_SPACE] = ImGuiKey.Space;
        RaylibKeyMap[KeyboardKey.KEY_ESCAPE] = ImGuiKey.Escape;
        RaylibKeyMap[KeyboardKey.KEY_ENTER] = ImGuiKey.Enter;
        RaylibKeyMap[KeyboardKey.KEY_TAB] = ImGuiKey.Tab;
        RaylibKeyMap[KeyboardKey.KEY_BACKSPACE] = ImGuiKey.Backspace;
        RaylibKeyMap[KeyboardKey.KEY_INSERT] = ImGuiKey.Insert;
        RaylibKeyMap[KeyboardKey.KEY_DELETE] = ImGuiKey.Delete;
        RaylibKeyMap[KeyboardKey.KEY_RIGHT] = ImGuiKey.RightArrow;
        RaylibKeyMap[KeyboardKey.KEY_LEFT] = ImGuiKey.LeftArrow;
        RaylibKeyMap[KeyboardKey.KEY_DOWN] = ImGuiKey.DownArrow;
        RaylibKeyMap[KeyboardKey.KEY_UP] = ImGuiKey.UpArrow;
        RaylibKeyMap[KeyboardKey.KEY_PAGE_UP] = ImGuiKey.PageUp;
        RaylibKeyMap[KeyboardKey.KEY_PAGE_DOWN] = ImGuiKey.PageDown;
        RaylibKeyMap[KeyboardKey.KEY_HOME] = ImGuiKey.Home;
        RaylibKeyMap[KeyboardKey.KEY_END] = ImGuiKey.End;
        RaylibKeyMap[KeyboardKey.KEY_CAPS_LOCK] = ImGuiKey.CapsLock;
        RaylibKeyMap[KeyboardKey.KEY_SCROLL_LOCK] = ImGuiKey.ScrollLock;
        RaylibKeyMap[KeyboardKey.KEY_NUM_LOCK] = ImGuiKey.NumLock;
        RaylibKeyMap[KeyboardKey.KEY_PRINT_SCREEN] = ImGuiKey.PrintScreen;
        RaylibKeyMap[KeyboardKey.KEY_PAUSE] = ImGuiKey.Pause;
        RaylibKeyMap[KeyboardKey.KEY_F1] = ImGuiKey.F1;
        RaylibKeyMap[KeyboardKey.KEY_F2] = ImGuiKey.F2;
        RaylibKeyMap[KeyboardKey.KEY_F3] = ImGuiKey.F3;
        RaylibKeyMap[KeyboardKey.KEY_F4] = ImGuiKey.F4;
        RaylibKeyMap[KeyboardKey.KEY_F5] = ImGuiKey.F5;
        RaylibKeyMap[KeyboardKey.KEY_F6] = ImGuiKey.F6;
        RaylibKeyMap[KeyboardKey.KEY_F7] = ImGuiKey.F7;
        RaylibKeyMap[KeyboardKey.KEY_F8] = ImGuiKey.F8;
        RaylibKeyMap[KeyboardKey.KEY_F9] = ImGuiKey.F9;
        RaylibKeyMap[KeyboardKey.KEY_F10] = ImGuiKey.F10;
        RaylibKeyMap[KeyboardKey.KEY_F11] = ImGuiKey.F11;
        RaylibKeyMap[KeyboardKey.KEY_F12] = ImGuiKey.F12;
        RaylibKeyMap[KeyboardKey.KEY_LEFT_SHIFT] = ImGuiKey.LeftShift;
        RaylibKeyMap[KeyboardKey.KEY_LEFT_CONTROL] = ImGuiKey.LeftCtrl;
        RaylibKeyMap[KeyboardKey.KEY_LEFT_ALT] = ImGuiKey.LeftAlt;
        RaylibKeyMap[KeyboardKey.KEY_LEFT_SUPER] = ImGuiKey.LeftSuper;
        RaylibKeyMap[KeyboardKey.KEY_RIGHT_SHIFT] = ImGuiKey.RightShift;
        RaylibKeyMap[KeyboardKey.KEY_RIGHT_CONTROL] = ImGuiKey.RightCtrl;
        RaylibKeyMap[KeyboardKey.KEY_RIGHT_ALT] = ImGuiKey.RightAlt;
        RaylibKeyMap[KeyboardKey.KEY_RIGHT_SUPER] = ImGuiKey.RightSuper;
        RaylibKeyMap[KeyboardKey.KEY_KB_MENU] = ImGuiKey.Menu;
        RaylibKeyMap[KeyboardKey.KEY_LEFT_BRACKET] = ImGuiKey.LeftBracket;
        RaylibKeyMap[KeyboardKey.KEY_BACKSLASH] = ImGuiKey.Backslash;
        RaylibKeyMap[KeyboardKey.KEY_RIGHT_BRACKET] = ImGuiKey.RightBracket;
        RaylibKeyMap[KeyboardKey.KEY_GRAVE] = ImGuiKey.GraveAccent;
        RaylibKeyMap[KeyboardKey.KEY_KP_0] = ImGuiKey.Keypad0;
        RaylibKeyMap[KeyboardKey.KEY_KP_1] = ImGuiKey.Keypad1;
        RaylibKeyMap[KeyboardKey.KEY_KP_2] = ImGuiKey.Keypad2;
        RaylibKeyMap[KeyboardKey.KEY_KP_3] = ImGuiKey.Keypad3;
        RaylibKeyMap[KeyboardKey.KEY_KP_4] = ImGuiKey.Keypad4;
        RaylibKeyMap[KeyboardKey.KEY_KP_5] = ImGuiKey.Keypad5;
        RaylibKeyMap[KeyboardKey.KEY_KP_6] = ImGuiKey.Keypad6;
        RaylibKeyMap[KeyboardKey.KEY_KP_7] = ImGuiKey.Keypad7;
        RaylibKeyMap[KeyboardKey.KEY_KP_8] = ImGuiKey.Keypad8;
        RaylibKeyMap[KeyboardKey.KEY_KP_9] = ImGuiKey.Keypad9;
        RaylibKeyMap[KeyboardKey.KEY_KP_DECIMAL] = ImGuiKey.KeypadDecimal;
        RaylibKeyMap[KeyboardKey.KEY_KP_DIVIDE] = ImGuiKey.KeypadDivide;
        RaylibKeyMap[KeyboardKey.KEY_KP_MULTIPLY] = ImGuiKey.KeypadMultiply;
        RaylibKeyMap[KeyboardKey.KEY_KP_SUBTRACT] = ImGuiKey.KeypadSubtract;
        RaylibKeyMap[KeyboardKey.KEY_KP_ADD] = ImGuiKey.KeypadAdd;
        RaylibKeyMap[KeyboardKey.KEY_KP_ENTER] = ImGuiKey.KeypadEnter;
        RaylibKeyMap[KeyboardKey.KEY_KP_EQUAL] = ImGuiKey.KeypadEqual;
    }

    private static void SetupMouseCursors()
    {
        MouseCursorMap.Clear();
        MouseCursorMap[ImGuiMouseCursor.Arrow] = MouseCursor.MOUSE_CURSOR_ARROW;
        MouseCursorMap[ImGuiMouseCursor.TextInput] = MouseCursor.MOUSE_CURSOR_IBEAM;
        MouseCursorMap[ImGuiMouseCursor.Hand] = MouseCursor.MOUSE_CURSOR_POINTING_HAND;
        MouseCursorMap[ImGuiMouseCursor.ResizeAll] = MouseCursor.MOUSE_CURSOR_RESIZE_ALL;
        MouseCursorMap[ImGuiMouseCursor.ResizeEW] = MouseCursor.MOUSE_CURSOR_RESIZE_EW;
        MouseCursorMap[ImGuiMouseCursor.ResizeNESW] = MouseCursor.MOUSE_CURSOR_RESIZE_NESW;
        MouseCursorMap[ImGuiMouseCursor.ResizeNS] = MouseCursor.MOUSE_CURSOR_RESIZE_NS;
        MouseCursorMap[ImGuiMouseCursor.ResizeNWSE] = MouseCursor.MOUSE_CURSOR_RESIZE_NWSE;
        MouseCursorMap[ImGuiMouseCursor.NotAllowed] = MouseCursor.MOUSE_CURSOR_NOT_ALLOWED;
    }

    /// <summary>
    /// Forces the font texture atlas to be recomputed and re-cached
    /// </summary>
    private static unsafe void ReloadFonts()
    {
        SetCurrentContext(ImGuiContext);
        var io = GetIO();

        io.Fonts.GetTexDataAsRGBA32(out byte* pixels, out var width, out var height, out _);

        var image = new Image
        {
            Data = pixels,
            Width = width,
            Height = height,
            Mipmaps = 1,
            Format = PixelFormat.PIXELFORMAT_UNCOMPRESSED_R8G8B8A8,
        };

        if (Raylib.IsTextureReady(FontTexture))
        {
            Raylib.UnloadTexture(FontTexture);
        }

        FontTexture = Raylib.LoadTextureFromImage(image);

        io.Fonts.SetTexID(new IntPtr(FontTexture.Id));
    }

    private static unsafe sbyte* RImGuiGetClipText(IntPtr userData) => Raylib.GetClipboardText();
    private static unsafe void RlImGuiSetClipText(IntPtr userData, sbyte* text) => Raylib.SetClipboardText(text);

    private unsafe delegate sbyte* GetClipTextCallback(IntPtr userData);

    private unsafe delegate void SetClipTextCallback(IntPtr userData, sbyte* text);

    /// <summary>
    /// End Custom initialization. Not needed if you call Setup. Only needed if you want to add custom setup code.
    /// must be proceeded by BeginInitImGui
    /// </summary>
    private static void EndInitImGui()
    {
        SetupMouseCursors();

        SetCurrentContext(ImGuiContext);
        GetIO().Fonts.AddFontDefault();

        unsafe
        {
            var iconsConfig = new ImFontConfig
            {
                MergeMode = 1, // merge the glyph ranges into the default font
                PixelSnapH = 1, // don't try to render on partial pixels
                FontDataOwnedByAtlas = 0, // the font atlas does not own this font data
                GlyphMaxAdvanceX = float.MaxValue,
                RasterizerMultiply = 1.0f,
                OversampleH = 2,
                OversampleV = 1
            };

            ushort[] iconRanges = [IconMin, IconMax, 0];

            fixed (ushort* range = &iconRanges[0])
            {
                // this unmanaged memory must remain allocated for the entire run of rlImgui
                IconFontRanges = AllocHGlobal(6);
                Buffer.MemoryCopy(range, IconFontRanges.ToPointer(), 6, 6);
                iconsConfig.GlyphRanges = (ushort*) IconFontRanges.ToPointer();

                var fontDataBuffer = Convert.FromBase64String(IconFontData);

                fixed (byte* buffer = fontDataBuffer)
                {
                    GetIO().Fonts
                        .AddFontFromMemoryTTF(new IntPtr(buffer), fontDataBuffer.Length, 11, &iconsConfig);
                }
            }
        }

        var io = GetIO();
        SetupUserFonts?.Invoke(io);
        io.BackendFlags |= ImGuiBackendFlags.HasMouseCursors;
        io.MousePos.X = 0;
        io.MousePos.Y = 0;

        // copy/paste callbacks
        unsafe
        {
            var getClip = new GetClipTextCallback(RImGuiGetClipText);
            var setClip = new SetClipTextCallback(RlImGuiSetClipText);

            io.SetClipboardTextFn = GetFunctionPointerForDelegate(setClip);
            io.GetClipboardTextFn = GetFunctionPointerForDelegate(getClip);
        }

        io.ClipboardUserData = IntPtr.Zero;
        ReloadFonts();
    }

    private static void SetMouseEvent(ImGuiIOPtr io, MouseButton rayMouse, ImGuiMouseButton imGuiMouse)
    {
        if (Raylib.IsMouseButtonPressed(rayMouse))
        {
            io.AddMouseButtonEvent((int) imGuiMouse, true);
        }
        else if (Raylib.IsMouseButtonReleased(rayMouse))
        {
            io.AddMouseButtonEvent((int) imGuiMouse, false);
        }
    }

    private static void NewFrame(float dt = -1)
    {
        var io = GetIO();

        if (Raylib.IsWindowFullscreen() || Raylib.IsWindowMaximized())
        {
            var monitor = Raylib.GetCurrentMonitor();
            io.DisplaySize = new Vector2(Raylib.GetMonitorWidth(monitor), Raylib.GetMonitorHeight(monitor));
        }
        else
        {
            io.DisplaySize = new Vector2(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
        }

        io.DisplayFramebufferScale = Raylib.GetWindowScaleDPI();
        io.DeltaTime = dt >= 0 ? dt : Raylib.GetFrameTime();

        if (io.WantSetMousePos)
        {
            Raylib.SetMousePosition((int) io.MousePos.X, (int) io.MousePos.Y);
        }
        else
        {
            io.AddMousePosEvent(Raylib.GetMouseX(), Raylib.GetMouseY());
        }

        SetMouseEvent(io, MouseButton.MOUSE_BUTTON_LEFT, ImGuiMouseButton.Left);
        SetMouseEvent(io, MouseButton.MOUSE_BUTTON_RIGHT, ImGuiMouseButton.Right);
        SetMouseEvent(io, MouseButton.MOUSE_BUTTON_MIDDLE, ImGuiMouseButton.Middle);
        SetMouseEvent(io, MouseButton.MOUSE_BUTTON_FORWARD, ImGuiMouseButton.Middle + 1);
        SetMouseEvent(io, MouseButton.MOUSE_BUTTON_BACK, ImGuiMouseButton.Middle + 2);

        var wheelMove = Raylib.GetMouseWheelMoveV();
        io.AddMouseWheelEvent(wheelMove.X, wheelMove.Y);

        if ((io.ConfigFlags & ImGuiConfigFlags.NoMouseCursorChange) != 0) return;

        var imguiCursor = GetMouseCursor();
        if (imguiCursor == CurrentMouseCursor && !io.MouseDrawCursor) return;

        CurrentMouseCursor = imguiCursor;
        if (io.MouseDrawCursor || imguiCursor == ImGuiMouseCursor.None)
        {
            Raylib.HideCursor();
        }
        else
        {
            Raylib.ShowCursor();

            if ((io.ConfigFlags & ImGuiConfigFlags.NoMouseCursorChange) != 0) return;
            Raylib.SetMouseCursor(MouseCursorMap.GetValueOrDefault(imguiCursor, MouseCursor.MOUSE_CURSOR_DEFAULT));
        }
    }

    private static void FrameEvents()
    {
        var io = GetIO();

        bool focused = Raylib.IsWindowFocused();
        if (focused != LastFrameFocused)
        {
            io.AddFocusEvent(focused);
        }

        LastFrameFocused = focused;


        // handle the modifyer key events so that shortcuts work
        var ctrlDown = RlImGuiIsControlDown();
        if (ctrlDown != LastControlPressed)
        {
            io.AddKeyEvent(ImGuiKey.ModCtrl, ctrlDown);
        }

        LastControlPressed = ctrlDown;

        var shiftDown = RlImGuiIsShiftDown();
        if (shiftDown != LastShiftPressed)
        {
            io.AddKeyEvent(ImGuiKey.ModShift, shiftDown);
        }

        LastShiftPressed = shiftDown;

        var altDown = RlImGuiIsAltDown();
        if (altDown != LastAltPressed)
        {
            io.AddKeyEvent(ImGuiKey.ModAlt, altDown);
        }

        LastAltPressed = altDown;

        var superDown = RlImGuiIsSuperDown();
        if (superDown != LastSuperPressed)
        {
            io.AddKeyEvent(ImGuiKey.ModSuper, superDown);
        }

        LastSuperPressed = superDown;

        // get the pressed keys, they are in event order
        Keys.Clear();
        var keyId = Raylib.GetKeyPressed();
        while (keyId != 0)
        {
            var key = (KeyboardKey) keyId;
            Keys.Add(key);
            if (RaylibKeyMap.TryGetValue(key, out var value))
            {
                io.AddKeyEvent(value, true);
            }

            keyId = Raylib.GetKeyPressed();
        }

        // look for any keys that were down last frame and see if they were down and are released
        foreach (var keyItr in RaylibKeyMap.Where(keyItr => Raylib.IsKeyReleased(keyItr.Key)))
        {
            io.AddKeyEvent(keyItr.Value, false);
        }

        // add the text input in order
        var pressed = Raylib.GetCharPressed();
        while (pressed != 0)
        {
            io.AddInputCharacter((uint) pressed);
            pressed = Raylib.GetCharPressed();
        }
    }

    /// <summary>
    /// Starts a new ImGui Frame
    /// </summary>
    /// <param name="dt">optional delta time, any value < 0 will use raylib GetFrameTime</param>
    public static void Begin(float dt = -1)
    {
        SetCurrentContext(ImGuiContext);

        NewFrame(dt);
        FrameEvents();
        ImGui.NewFrame();
    }

    private static void EnableScissor(float x, float y, float width, float height)
    {
        Rlgl.EnableScissorTest();
        var io = GetIO();

        Rlgl.Scissor((int) (x * io.DisplayFramebufferScale.X),
            (int) ((io.DisplaySize.Y - (int) (y + height)) * io.DisplayFramebufferScale.Y),
            (int) (width * io.DisplayFramebufferScale.X),
            (int) (height * io.DisplayFramebufferScale.Y));
    }

    private static void TriangleVert(ImDrawVertPtr idxVert)
    {
        var color = ColorConvertU32ToFloat4(idxVert.col);

        Rlgl.Color4f(color.X, color.Y, color.Z, color.W);
        Rlgl.TexCoord2f(idxVert.uv.X, idxVert.uv.Y);
        Rlgl.Vertex2f(idxVert.pos.X, idxVert.pos.Y);
    }

    private static void RenderTriangles(uint count, uint indexStart, ImVector<ushort> indexBuffer,
        ImPtrVector<ImDrawVertPtr> vertBuffer, IntPtr texturePtr)
    {
        if (count < 3) return;

        uint textureId = 0;
        if (texturePtr != IntPtr.Zero)
        {
            textureId = (uint) texturePtr.ToInt32();
        }

        Rlgl.Begin(DrawMode.TRIANGLES);
        Rlgl.SetTexture(textureId);

        for (var i = 0; i <= count - 3; i += 3)
        {
            if (Rlgl.CheckRenderBatchLimit(3))
            {
                Rlgl.Begin(DrawMode.TRIANGLES);
                Rlgl.SetTexture(textureId);
            }

            var indexA = indexBuffer[(int) indexStart + i];
            var indexB = indexBuffer[(int) indexStart + i + 1];
            var indexC = indexBuffer[(int) indexStart + i + 2];

            var vertexA = vertBuffer[indexA];
            var vertexB = vertBuffer[indexB];
            var vertexC = vertBuffer[indexC];

            TriangleVert(vertexA);
            TriangleVert(vertexB);
            TriangleVert(vertexC);
        }

        Rlgl.End();
    }

    private delegate void Callback(ImDrawListPtr list, ImDrawCmdPtr cmd);

    private static void RenderData()
    {
        Rlgl.DrawRenderBatchActive();
        Rlgl.DisableBackfaceCulling();

        var data = GetDrawData();

        for (var l = 0; l < data.CmdListsCount; l++)
        {
            var commandList = data.CmdLists[l];

            for (var cmdIndex = 0; cmdIndex < commandList.CmdBuffer.Size; cmdIndex++)
            {
                var cmd = commandList.CmdBuffer[cmdIndex];

                EnableScissor(cmd.ClipRect.X - data.DisplayPos.X, cmd.ClipRect.Y - data.DisplayPos.Y,
                    cmd.ClipRect.Z - (cmd.ClipRect.X - data.DisplayPos.X),
                    cmd.ClipRect.W - (cmd.ClipRect.Y - data.DisplayPos.Y));
                if (cmd.UserCallback != IntPtr.Zero)
                {
                    var cb = GetDelegateForFunctionPointer<Callback>(cmd.UserCallback);
                    cb(commandList, cmd);
                    continue;
                }

                RenderTriangles(cmd.ElemCount, cmd.IdxOffset, commandList.IdxBuffer, commandList.VtxBuffer,
                    cmd.TextureId);

                Rlgl.DrawRenderBatchActive();
            }
        }

        Rlgl.SetTexture(0);
        Rlgl.DisableScissorTest();
        Rlgl.EnableBackfaceCulling();
    }

    /// <summary>
    /// Ends an ImGui frame and submits all ImGui drawing to raylib for processing.
    /// </summary>
    public static void End()
    {
        SetCurrentContext(ImGuiContext);
        Render();
        RenderData();
    }

    /// <summary>
    /// Cleanup ImGui and unload font atlas
    /// </summary>
    public static void Shutdown()
    {
        Raylib.UnloadTexture(FontTexture);
        DestroyContext();

        if (IconFontRanges != IntPtr.Zero)
        {
            FreeHGlobal(IconFontRanges);
        }

        IconFontRanges = IntPtr.Zero;
    }

    /// <summary>
    /// Draw a texture as an image in an ImGui Context
    /// Uses the current ImGui Cursor position and the full texture size.
    /// </summary>
    /// <param name="image">The raylib texture to draw</param>
    public static void Image(Texture2D image)
        => ImGui.Image(new IntPtr(image.Id), new Vector2(image.Width, image.Height));

    /// <summary>
    /// Draw a texture as an image in an ImGui Context at a specific size
    /// Uses the current ImGui Cursor position and the specified width and height
    /// The image will be scaled up or down to fit as needed
    /// </summary>
    /// <param name="image">The raylib texture to draw</param>
    /// <param name="width">The width of the drawn image</param>
    /// <param name="height">The height of the drawn image</param>
    public static void ImageSize(Texture2D image, int width, int height)
        => ImGui.Image(new IntPtr(image.Id), new Vector2(width, height));

    /// <summary>
    /// Draw a texture as an image in an ImGui Context at a specific size
    /// Uses the current ImGui Cursor position and the specified size
    /// The image will be scaled up or down to fit as needed
    /// </summary>
    /// <param name="image">The raylib texture to draw</param>
    /// <param name="size">The size of drawn image</param>
    public static void ImageSize(Texture2D image, Vector2 size) => ImGui.Image(new IntPtr(image.Id), size);

    /// <summary>
    /// Draw a portion texture as an image in an ImGui Context at a defined size
    /// Uses the current ImGui Cursor position and the specified size
    /// The image will be scaled up or down to fit as needed
    /// </summary>
    /// <param name="image">The raylib texture to draw</param>
    /// <param name="destWidth">The width of the drawn image</param>
    /// <param name="destHeight">The height of the drawn image</param>
    /// <param name="sourceRect">The portion of the texture to draw as an image. Negative values for the width and height will flip the image</param>
    public static void ImageRect(Texture2D image, int destWidth, int destHeight, Rectangle sourceRect)
    {
        var uv0 = new Vector2();
        var uv1 = new Vector2();

        if (sourceRect.Width < 0)
        {
            uv0.X = -(sourceRect.X / image.Width);
            uv1.X = uv0.X - Math.Abs(sourceRect.Width) / image.Width;
        }
        else
        {
            uv0.X = sourceRect.X / image.Width;
            uv1.X = uv0.X + sourceRect.Width / image.Width;
        }

        if (sourceRect.Height < 0)
        {
            uv0.Y = -(sourceRect.Y / image.Height);
            uv1.Y = uv0.Y - Math.Abs(sourceRect.Height) / image.Height;
        }
        else
        {
            uv0.Y = sourceRect.Y / image.Height;
            uv1.Y = uv0.Y + sourceRect.Height / image.Height;
        }

        ImGui.Image(new IntPtr(image.Id), new Vector2(destWidth, destHeight), uv0, uv1);
    }

    /// <summary>
    /// Draws a render texture as an image an ImGui Context, automatically flipping the Y axis so it will show correctly on screen
    /// </summary>
    /// <param name="image">The render texture to draw</param>
    public static void ImageRenderTexture(RenderTexture2D image)
        => ImageRect(image.Texture, image.Texture.Width, image.Texture.Height,
            new Rectangle(0, 0, image.Texture.Width, -image.Texture.Height));

    /// <summary>
    /// Draws a render texture as an image to the current ImGui Context, flipping the Y axis so it will show correctly on the screen
    /// The texture will be scaled to fit the content are available, centered if desired
    /// </summary>
    /// <param name="image">The render texture to draw</param>
    /// <param name="center">When true the texture will be centered in the content area. When false the image will be left and top justified</param>
    public static void ImageRenderTextureFit(RenderTexture2D image, bool center = true)
    {
        var area = GetContentRegionAvail();

        var scale = area.X / image.Texture.Width;

        var y = image.Texture.Height * scale;
        if (y > area.Y)
        {
            scale = area.Y / image.Texture.Height;
        }

        var sizeX = (int) (image.Texture.Width * scale);
        var sizeY = (int) (image.Texture.Height * scale);

        if (center)
        {
            SetCursorPosX(0);
            SetCursorPosX(area.X / 2 - sizeX / 2f);
            SetCursorPosY(GetCursorPosY() + (area.Y / 2 - sizeY / 2f));
        }

        ImageRect(image.Texture, sizeX, sizeY, new Rectangle(0, 0, image.Texture.Width, -image.Texture.Height));
    }

    /// <summary>
    /// Draws a texture as an image button in an ImGui context. Uses the current ImGui cursor position and the full size of the texture
    /// </summary>
    /// <param name="name">The display name and ImGui ID for the button</param>
    /// <param name="image">The texture to draw</param>
    /// <returns>True if the button was clicked</returns>
    public static bool ImageButton(string name, Texture2D image)
        => ImageButtonSize(name, image, new Vector2(image.Width, image.Height));

    /// <summary>
    /// Draws a texture as an image button in an ImGui context. Uses the current ImGui cursor position and the specified size.
    /// </summary>
    /// <param name="name">The display name and ImGui ID for the button</param>
    /// <param name="image">The texture to draw</param>
    /// <param name="size">The size of the button/param>
    /// <returns>True if the button was clicked</returns>
    public static bool ImageButtonSize(string name, Texture2D image, Vector2 size)
        => ImGui.ImageButton(name, new IntPtr(image.Id), size);

    public static Vector4 ToV4(this Color color) => new Vector4(color.R, color.G, color.B, color.A) / 255f;
    public static Vector3 ToV3(this Color color) => new Vector3(color.R, color.G, color.B) / 255f;
    public static uint ToUint(this Vector4 color) => ColorConvertFloat4ToU32(color);
    public static uint ToUint(this Color color) => color.ToV4().ToUint();
    public static Color ToColor(this Vector3 color) => new((short) color.X, (short) color.Y, (short) color.Z, 255);

    public static Color ToColor(this Vector4 color)
        => new((byte) (color.X * 255), (byte) (color.Y * 255), (byte) (color.Z * 255), (byte) (color.W * 255));

    public static Vector2 MeasureText(this string text) => CalcTextSize(text);

    public static Vector2 MeasureText(this string text, bool hideAfterDoubleHash)
        => CalcTextSize(text, hideAfterDoubleHash);

    public static Vector2 MeasureText(this string text, bool hideAfterDoubleHash, float wrapWidth)
        => CalcTextSize(text, hideAfterDoubleHash, wrapWidth);

    public static Vector2 MeasureText(this string text, float wrapWidth) => CalcTextSize(text, wrapWidth);
    public static Vector2 MeasureText(this string text, int start) => CalcTextSize(text, start);

    public static Vector2 MeasureText(this string text, int start, bool hideAfterDoubleHash)
        => CalcTextSize(text, start, hideAfterDoubleHash);

    public static Vector2 MeasureText(this string text, int start, float wrapWidth)
        => CalcTextSize(text, start, wrapWidth);

    public static Vector2 MeasureText(this string text, int start, int length) => CalcTextSize(text, start, length);

    public static Vector2 MeasureText(this string text, int start, int length, bool hideAfterDoubleHash)
        => CalcTextSize(text, start, length, hideAfterDoubleHash);

    public static Vector2 MeasureText(this string text, int start, int length, bool hideAfterDoubleHash,
        float wrapWidth)
        => CalcTextSize(text, start, length, hideAfterDoubleHash, wrapWidth);

    public static Vector2 MeasureText(this string text, int start, int length, float wrapWidth)
        => CalcTextSize(text, start, length, wrapWidth);

    public static void SetScale(float scale = 1) => GetIO().FontGlobalScale = scale;

    public static void DrawBezierCurve(this ImDrawListPtr ptr, Vector2 pos1, Vector2 pos2, uint color, float thickness)
    {
        Vector2 p1 = new(pos1.X, pos2.Y);
        Vector2 p2 = new(pos2.X, pos1.Y);
        ptr.AddBezierCubic(pos1, p2, p1, pos2, color, thickness);
    }

    public static bool IsInRange(this Range range, int i) => range.Start.Value <= i && i < range.End.Value;

    public static bool Equals(this Range range1, Range range2)
        => range1.Start.Value == range2.Start.Value && range1.End.Value == range2.End.Value;

    public static bool Intercept(this Range range1, Range range2)
        => range1.IsInRange(range2.Start.Value) || range1.IsInRange(range2.End.Value) ||
           range2.IsInRange(range1.Start.Value) || range2.IsInRange(range1.End.Value);

    public static void AddText(this ImDrawListPtr drawPtr, Vector2 pos, uint col, Span<char> textBegin)
    {
        unsafe
        {
            var textBeginByteCount = Encoding.UTF8.GetByteCount(textBegin);
            var nativeTextBegin = stackalloc byte[textBeginByteCount + 1];
            fixed (char* textBeginPtr = textBegin)
            {
                var nativeTextBeginOffset = Encoding.UTF8.GetBytes(textBeginPtr, textBegin.Length,
                    nativeTextBegin, textBeginByteCount);
                nativeTextBegin[nativeTextBeginOffset] = 0;
            }

            byte* nativeTextEnd = null;
            ImGuiNative.ImDrawList_AddText_Vec2(drawPtr.NativePtr, pos, col, nativeTextBegin, nativeTextEnd);
        }
    }

    public static Vector2 CalcTextSpan(Span<char> text, int start = 0, int? length = null,
        bool hideTextAfterDoubleHash = false, float wrapWidth = -1.0f)
    {
        unsafe
        {
            Vector2 ret;
            byte* nativeTextStart = null;
            byte* nativeTextEnd = null;
            var textByteCount = 0;
            if (text != null)
            {
                var textToCopyLen = length ?? text.Length;
                textByteCount = CalcSizeInUtf8(text, start, textToCopyLen);
                if (textByteCount > StackAllocationSizeLimit) nativeTextStart = Allocate(textByteCount + 1);
                else
                {
                    var nativeTextStackBytes = stackalloc byte[textByteCount + 1];
                    nativeTextStart = nativeTextStackBytes;
                }

                var nativeTextOffset = GetUtf8(text, start, textToCopyLen, nativeTextStart, textByteCount);
                nativeTextStart[nativeTextOffset] = 0;
                nativeTextEnd = nativeTextStart + nativeTextOffset;
            }

            ImGuiNative.igCalcTextSize(&ret, nativeTextStart, nativeTextEnd, *(byte*) &hideTextAfterDoubleHash,
                wrapWidth);
            if (textByteCount > StackAllocationSizeLimit) Free(nativeTextStart);

            return ret;
        }
    }

    public static bool ImguiComobox(this Enum options, string label, ref int currentItem, params string[] items)
        => Combo(label, ref currentItem, items, items.Length);

    public static bool ImguiComobox(this Enum options, string label, ref int currentItem)
        => options.ImguiComobox(label, ref currentItem, options.GetType().GetEnumNames());

    private static int CalcSizeInUtf8(Span<char> s, int start, int length)
    {
        unsafe
        {
            if (start < 0 || length < 0 || start + length > s.Length) throw new ArgumentOutOfRangeException();
            fixed (char* utf16Ptr = s) return Encoding.UTF8.GetByteCount(utf16Ptr + start, length);
        }
    }

    private static unsafe int GetUtf8(Span<char> s, int start, int length, byte* utf8Bytes, int utf8ByteCount)
    {
        if (start < 0 || length < 0 || start + length > s.Length) throw new ArgumentOutOfRangeException();
        fixed (char* utf16Ptr = s) return Encoding.UTF8.GetBytes(utf16Ptr + start, length, utf8Bytes, utf8ByteCount);
    }

    private static unsafe byte* Allocate(int byteCount) => (byte*) AllocHGlobal(byteCount);
    private static unsafe void Free(byte* ptr) => FreeHGlobal((nint) ptr);
}