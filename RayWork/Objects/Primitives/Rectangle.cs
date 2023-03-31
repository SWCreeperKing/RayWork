using System.Numerics;
using Raylib_CsLo;
using RayRectangle = Raylib_CsLo.Rectangle;

namespace RayWork.Objects;

public class Rectangle
{
    private RayRectangle _rectangleData = new(0, 0, 0, 0);
    private Vector2 _rectanglePosition = Vector2.Zero;
    private Vector2 _rectangleSize = Vector2.Zero;

    public float X
    {
        get => _rectangleData.x;
        set
        {
            _rectangleData.x = value;
            _rectanglePosition.X = value;
        }
    }

    public float Y
    {
        get => _rectangleData.y;
        set
        {
            _rectangleData.y = value;
            _rectanglePosition.Y = value;
        }
    }

    public float Width
    {
        get => _rectangleData.width;
        set
        {
            _rectangleData.width = value;
            _rectangleSize.X = value;
        }
    }

    public float Height
    {
        get => _rectangleData.height;
        set
        {
            _rectangleData.height = value;
            _rectangleSize.Y = value;
        }
    }

    public Vector2 Position
    {
        get => _rectanglePosition;
        set
        {
            X = value.X;
            Y = value.Y;
        }
    }

    public Vector2 Size
    {
        get => _rectangleSize;
        set
        {
            Width = value.X;
            Height = value.Y;
        }
    }

    public RayRectangle RayLibRectangle
    {
        get => _rectangleData;
        set
        {
            X = value.x;
            Y = value.y;
            Width = value.width;
            Height = value.height;
        }
    }

    public Rectangle(RayRectangle rectangle) => RayLibRectangle = rectangle;

    public Rectangle(Vector2 position, Vector2 size)
    {
        Position = position;
        Size = size;
    }

    public Rectangle(Vector2 positionSize)
    {
        Position = positionSize;
        Size = positionSize;
    }

    public Rectangle(float x = 0, float y = 0, float w = 0, float h = 0)
    {
        X = x;
        Y = y;
        Width = w;
        Height = h;
    }

    public Rectangle(float xy, float wh) : this(xy, xy, wh, wh)
    {
    }

    public bool IsVector2In(Vector2 vector2) => Raylib.CheckCollisionPointRec(vector2, RayLibRectangle);
    public bool IsMouseIn() => IsVector2In(Raylib.GetMousePosition());
    public void Draw(Color? color = null) => Raylib.DrawRectangleRec(RayLibRectangle, color ?? Raylib.WHITE);

    public void DrawLines(float lineThickness = 3, Color? color = null)
        => Raylib.DrawRectangleLinesEx(RayLibRectangle, lineThickness, color ?? Raylib.WHITE);

    public void DrawPro(Vector2? origin = null, float rotation = 0, Color? color = null)
        => Raylib.DrawRectanglePro(RayLibRectangle, origin ?? Vector2.Zero, rotation, color ?? Raylib.WHITE);

    public void DrawRounded(float roundness = .1f, int segments = 10, Color? color = null)
        => Raylib.DrawRectangleRounded(RayLibRectangle, roundness, segments, color ?? Raylib.WHITE);

    public void DrawRoundedLines(float roundness = .1f, int segments = 10, float lineThickness = 3, Color? color = null)
        => Raylib.DrawRectangleRoundedLines(RayLibRectangle, roundness, segments, lineThickness, color ?? Raylib.WHITE);

    public void DrawGradientH(Color color1, Color color2)
        => Raylib.DrawRectangleGradientH((int) X, (int) Y, (int) Width, (int) Height, color1, color2);

    public void DrawGradientV(Color color1, Color color2)
        => Raylib.DrawRectangleGradientV((int) X, (int) Y, (int) Width, (int) Height, color1, color2);

    public void DrawGradientEx(Color color1, Color color2, Color color3, Color color4)
        => Raylib.DrawRectangleGradientEx(RayLibRectangle, color1, color2, color3, color4);

    public void DrawMask(Vector2 size, Action draw) => Position.MaskDraw(size, draw);
    public void DrawMask(Action draw) => Position.MaskDraw(Size, draw);

    public static implicit operator RayRectangle(Rectangle rectangle) => rectangle.RayLibRectangle;
    public static implicit operator Rectangle(RayRectangle rayRectangle) => new(rayRectangle);
}