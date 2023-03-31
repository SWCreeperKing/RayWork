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
/*******************************************************************************************
 *
 * Modified for Raylib-cslo
 * 
********************************************************************************************/

using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using ImGuiNET;
using Raylib_CsLo;

namespace RayWork.RLImgui;

public static class RlImgui
{
    public static readonly Vector2 V2MaxValue = new(float.MaxValue, float.MaxValue);
    public static readonly Vector2 V2MinValue = new(float.MinValue, float.MinValue);
    public static readonly Vector3 V3MaxValue = new(float.MaxValue, float.MaxValue, float.MaxValue);
    public static readonly Vector3 V3MinValue = new(float.MinValue, float.MinValue, float.MinValue);
    public static readonly Vector4 V4MaxValue = new(float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue);
    public static readonly Vector4 V4MinValue = new(float.MinValue, float.MinValue, float.MinValue, float.MinValue);

    public static nint ImGuiContext = nint.Zero;

    private const int StackAllocationSizeLimit = 2048;
    private static ImGuiMouseCursor CurrentMouseCursor = ImGuiMouseCursor.COUNT;
    private static Dictionary<ImGuiMouseCursor, MouseCursor> MouseCursorMap;
    private static KeyboardKey[] KeyEnumMap;
    private static Texture FontTexture;
    private static Func<Vector2> WindowSize;

    public static void Setup(Func<Vector2> windowSize, bool darkTheme = true)
    {
        WindowSize = windowSize;
        MouseCursorMap = new();
        KeyEnumMap = Enum.GetValues(typeof(KeyboardKey)) as KeyboardKey[];

        FontTexture.id = 0;

        BeginInitImGui();

        if (darkTheme) ImGui.StyleColorsDark();
        else ImGui.StyleColorsLight();

        EndInitImGui();
    }

    public static void BeginInitImGui() => ImGuiContext = ImGui.CreateContext();

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

    public static unsafe void ReloadFonts()
    {
        ImGui.SetCurrentContext(ImGuiContext);
        var io = ImGui.GetIO();

        io.Fonts.GetTexDataAsRGBA32(out byte* pixels, out var width, out var height, out var bytesPerPixel);

        var image = new Image
        {
            data = pixels,
            width = width,
            height = height,
            mipmaps = 1,
            format = (int) PixelFormat.PIXELFORMAT_UNCOMPRESSED_R8G8B8A8,
        };

        FontTexture = Raylib.LoadTextureFromImage(image);

        io.Fonts.SetTexID(new(FontTexture.id));
    }

    public static void EndInitImGui()
    {
        SetupMouseCursors();

        ImGui.SetCurrentContext(ImGuiContext);
        var io = ImGui.GetIO();

        io.Fonts.AddFontDefault();
        io.KeyMap[(int) ImGuiKey.Tab] = (int) KeyboardKey.KEY_TAB;
        io.KeyMap[(int) ImGuiKey.LeftArrow] = (int) KeyboardKey.KEY_LEFT;
        io.KeyMap[(int) ImGuiKey.RightArrow] = (int) KeyboardKey.KEY_RIGHT;
        io.KeyMap[(int) ImGuiKey.UpArrow] = (int) KeyboardKey.KEY_UP;
        io.KeyMap[(int) ImGuiKey.DownArrow] = (int) KeyboardKey.KEY_DOWN;
        io.KeyMap[(int) ImGuiKey.PageUp] = (int) KeyboardKey.KEY_PAGE_UP;
        io.KeyMap[(int) ImGuiKey.PageDown] = (int) KeyboardKey.KEY_PAGE_DOWN;
        io.KeyMap[(int) ImGuiKey.Home] = (int) KeyboardKey.KEY_HOME;
        io.KeyMap[(int) ImGuiKey.End] = (int) KeyboardKey.KEY_END;
        io.KeyMap[(int) ImGuiKey.Delete] = (int) KeyboardKey.KEY_DELETE;
        io.KeyMap[(int) ImGuiKey.Backspace] = (int) KeyboardKey.KEY_BACKSPACE;
        io.KeyMap[(int) ImGuiKey.Enter] = (int) KeyboardKey.KEY_ENTER;
        io.KeyMap[(int) ImGuiKey.Escape] = (int) KeyboardKey.KEY_ESCAPE;
        io.KeyMap[(int) ImGuiKey.Space] = (int) KeyboardKey.KEY_SPACE;
        io.KeyMap[(int) ImGuiKey.A] = (int) KeyboardKey.KEY_A;
        io.KeyMap[(int) ImGuiKey.C] = (int) KeyboardKey.KEY_C;
        io.KeyMap[(int) ImGuiKey.V] = (int) KeyboardKey.KEY_V;
        io.KeyMap[(int) ImGuiKey.X] = (int) KeyboardKey.KEY_X;
        io.KeyMap[(int) ImGuiKey.Y] = (int) KeyboardKey.KEY_Y;
        io.KeyMap[(int) ImGuiKey.Z] = (int) KeyboardKey.KEY_Z;

        ReloadFonts();
    }

    private static void NewFrame()
    {
        var io = ImGui.GetIO();

        if (Raylib.IsWindowFullscreen())
        {
            var monitor = Raylib.GetCurrentMonitor();
            io.DisplaySize = new(Raylib.GetMonitorWidth(monitor), Raylib.GetMonitorHeight(monitor));
        }
        else io.DisplaySize = WindowSize();

        io.DisplayFramebufferScale = new(1, 1);
        io.DeltaTime = Raylib.GetFrameTime();

        io.KeyCtrl = Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT_CONTROL) ||
                     Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_CONTROL);
        io.KeyShift = Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT_SHIFT) || Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_SHIFT);
        io.KeyAlt = Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT_ALT) || Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_ALT);
        io.KeySuper = Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT_SUPER) || Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_SUPER);

        if (io.WantSetMousePos) Raylib.SetMousePosition((int) io.MousePos.X, (int) io.MousePos.Y);
        else io.MousePos = Raylib.GetMousePosition();

        io.MouseDown[0] = Raylib.IsMouseButtonDown(MouseButton.MOUSE_BUTTON_LEFT);
        io.MouseDown[1] = Raylib.IsMouseButtonDown(MouseButton.MOUSE_BUTTON_RIGHT);
        io.MouseDown[2] = Raylib.IsMouseButtonDown(MouseButton.MOUSE_BUTTON_MIDDLE);

        if (Raylib.GetMouseWheelMove() > 0) io.MouseWheel += 1;
        else if (Raylib.GetMouseWheelMove() < 0) io.MouseWheel -= 1;

        if ((io.ConfigFlags & ImGuiConfigFlags.NoMouseCursorChange) != 0) return;

        var imguiCursor = ImGui.GetMouseCursor();
        if (imguiCursor == CurrentMouseCursor && !io.MouseDrawCursor) return;

        CurrentMouseCursor = imguiCursor;
        if (io.MouseDrawCursor || imguiCursor == ImGuiMouseCursor.None) Raylib.HideCursor();
        else
        {
            Raylib.ShowCursor();

            if ((io.ConfigFlags & ImGuiConfigFlags.NoMouseCursorChange) != 0) return;

            Raylib.SetMouseCursor(!MouseCursorMap.ContainsKey(imguiCursor)
                ? MouseCursor.MOUSE_CURSOR_DEFAULT
                : MouseCursorMap[imguiCursor]);
        }
    }

    private static void FrameEvents()
    {
        var io = ImGui.GetIO();

        foreach (var key in KeyEnumMap) io.KeysDown[(int) key] = Raylib.IsKeyDown(key);

        var pressed = (uint) Raylib.GetCharPressed();
        while (pressed != 0)
        {
            io.AddInputCharacter(pressed);
            pressed = (uint) Raylib.GetCharPressed();
        }
    }

    public static void Begin()
    {
        ImGui.SetCurrentContext(ImGuiContext);

        NewFrame();
        FrameEvents();
        ImGui.NewFrame();
    }

    private static void EnableScissor(float x, float y, float width, float height)
    {
        RlGl.rlEnableScissorTest();
        RlGl.rlScissor((int) x, Raylib.GetScreenHeight() - (int) (y + height), (int) width, (int) height);
    }

    private static void TriangleVert(ImDrawVertPtr idxVert)
    {
        var c = ImGui.ColorConvertU32ToFloat4(idxVert.col);

        RlGl.rlColor4f(c.X, c.Y, c.Z, c.W);
        RlGl.rlTexCoord2f(idxVert.uv.X, idxVert.uv.Y);
        RlGl.rlVertex2f(idxVert.pos.X, idxVert.pos.Y);
    }

    private static void RenderTriangles(uint count, uint indexStart, ImVector<ushort> indexBuffer,
        ImPtrVector<ImDrawVertPtr> vertBuffer, nint texturePtr)
    {
        if (count < 3) return;

        uint textureId = 0;
        if (texturePtr != nint.Zero) textureId = (uint) texturePtr.ToInt32();

        RlGl.rlBegin(RlGl.RL_TRIANGLES);
        RlGl.rlSetTexture(textureId);

        for (var i = 0; i <= count - 3; i += 3)
        {
            if (RlGl.rlCheckRenderBatchLimit(3))
            {
                RlGl.rlBegin(RlGl.RL_TRIANGLES);
                RlGl.rlSetTexture(textureId);
            }

            TriangleVert(vertBuffer[indexBuffer[(int) indexStart + i]]);
            TriangleVert(vertBuffer[indexBuffer[(int) indexStart + i + 1]]);
            TriangleVert(vertBuffer[indexBuffer[(int) indexStart + i + 2]]);
        }

        RlGl.rlEnd();
    }

    private delegate void Callback(ImDrawListPtr list, ImDrawCmdPtr cmd);

    private static void RenderData()
    {
        RlGl.rlDrawRenderBatchActive();
        RlGl.rlDisableBackfaceCulling();

        var data = ImGui.GetDrawData();

        for (var l = 0; l < data.CmdListsCount; l++)
        {
            var commandList = data.CmdListsRange[l];

            for (var cmdIndex = 0; cmdIndex < commandList.CmdBuffer.Size; cmdIndex++)
            {
                var cmd = commandList.CmdBuffer[cmdIndex];

                EnableScissor(cmd.ClipRect.X - data.DisplayPos.X, cmd.ClipRect.Y - data.DisplayPos.Y,
                    cmd.ClipRect.Z - (cmd.ClipRect.X - data.DisplayPos.X),
                    cmd.ClipRect.W - (cmd.ClipRect.Y - data.DisplayPos.Y));
                if (cmd.UserCallback != nint.Zero)
                {
                    var cb = Marshal.GetDelegateForFunctionPointer<Callback>(cmd.UserCallback);
                    cb(commandList, cmd);
                    continue;
                }

                RenderTriangles(cmd.ElemCount, cmd.IdxOffset, commandList.IdxBuffer, commandList.VtxBuffer,
                    cmd.TextureId);

                RlGl.rlDrawRenderBatchActive();
            }
        }

        RlGl.rlSetTexture(0);
        RlGl.rlDisableScissorTest();
        RlGl.rlEnableBackfaceCulling();
    }

    public static void End()
    {
        ImGui.SetCurrentContext(ImGuiContext);
        ImGui.Render();
        RenderData();
    }

    public static void Shutdown() => Raylib.UnloadTexture(FontTexture);

    public static void Image(Texture image)
    {
        ImGui.Image(new(image.id), new(image.width, image.height));
    }

    public static void ImageSize(Texture image, int width, int height)
    {
        ImGui.Image(new(image.id), new(width, height));
    }

    public static void ImageSize(Texture image, Vector2 size) => ImGui.Image(new(image.id), size);

    public static void ImageRect(Texture image, int destWidth, int destHeight, Rectangle sourceRect)
    {
        var uv0 = new Vector2();
        var uv1 = new Vector2();

        if (sourceRect.width < 0)
        {
            uv0.X = -(sourceRect.x / image.width);
            uv1.X = uv0.X - Math.Abs(sourceRect.width) / image.width;
        }
        else
        {
            uv0.X = sourceRect.x / image.width;
            uv1.X = uv0.X + sourceRect.width / image.width;
        }

        if (sourceRect.height < 0)
        {
            uv0.Y = -(sourceRect.y / image.height);
            uv1.Y = uv0.Y - Math.Abs(sourceRect.height) / image.height;
        }
        else
        {
            uv0.Y = sourceRect.y / image.height;
            uv1.Y = uv0.Y + sourceRect.height / image.height;
        }

        ImGui.Image(new(image.id), new(destWidth, destHeight), uv0, uv1);
    }

    public static Vector4 ToV4(this Color color) => new Vector4(color.r, color.g, color.b, color.a) / 255f;
    public static Vector3 ToV3(this Color color) => new Vector3(color.r, color.g, color.b) / 255f;
    public static uint ToUint(this Vector4 color) => ImGui.ColorConvertFloat4ToU32(color);
    public static uint ToUint(this Color color) => color.ToV4().ToUint();
    public static Color ToColor(this Vector3 color) => new((short) color.X, (short) color.Y, (short) color.Z, 255);

    public static Color ToColor(this Vector4 color)
        => new((byte) (color.X * 255), (byte) (color.Y * 255), (byte) (color.Z * 255), (byte) (color.W * 255));

    public static Vector2 MeasureText(this string text) => ImGui.CalcTextSize(text);

    public static Vector2 MeasureText(this string text, bool hideAfterDoubleHash)
        => ImGui.CalcTextSize(text, hideAfterDoubleHash);

    public static Vector2 MeasureText(this string text, bool hideAfterDoubleHash, float wrapWidth)
        => ImGui.CalcTextSize(text, hideAfterDoubleHash, wrapWidth);

    public static Vector2 MeasureText(this string text, float wrapWidth) => ImGui.CalcTextSize(text, wrapWidth);
    public static Vector2 MeasureText(this string text, int start) => ImGui.CalcTextSize(text, start);

    public static Vector2 MeasureText(this string text, int start, bool hideAfterDoubleHash)
        => ImGui.CalcTextSize(text, start, hideAfterDoubleHash);

    public static Vector2 MeasureText(this string text, int start, float wrapWidth)
        => ImGui.CalcTextSize(text, start, wrapWidth);

    public static Vector2 MeasureText(this string text, int start, int length)
        => ImGui.CalcTextSize(text, start, length);

    public static Vector2 MeasureText(this string text, int start, int length, bool hideAfterDoubleHash)
        => ImGui.CalcTextSize(text, start, length, hideAfterDoubleHash);

    public static Vector2 MeasureText(this string text, int start, int length, bool hideAfterDoubleHash,
        float wrapWidth)
        => ImGui.CalcTextSize(text, start, length, hideAfterDoubleHash, wrapWidth);

    public static Vector2 MeasureText(this string text, int start, int length, float wrapWidth)
        => ImGui.CalcTextSize(text, start, length, wrapWidth);

    public static void SetScale(float scale = 1) => ImGui.GetIO().FontGlobalScale = scale;

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
                var textToCopyLen = length.HasValue ? length.Value : text.Length;
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
        => ImGui.Combo(label, ref currentItem, items, items.Length);

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

    private static unsafe byte* Allocate(int byteCount) => (byte*) Marshal.AllocHGlobal(byteCount);
    private static unsafe void Free(byte* ptr) => Marshal.FreeHGlobal((nint) ptr);
}