using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Color;
using RayRectangle = Raylib_cs.Rectangle;

namespace RayWork.Objects.Primitives;

public class Rectangle
{
    private RayRectangle RectangleData = new(0, 0, 0, 0);
    private Vector2 RectanglePosition = Vector2.Zero;
    private Vector2 RectangleSize = Vector2.Zero;

    public float X
    {
        get => RectangleData.X;
        set
        {
            RectangleData.X = value;
            RectanglePosition.X = value;
        }
    }

    public float Y
    {
        get => RectangleData.Y;
        set
        {
            RectangleData.Y = value;
            RectanglePosition.Y = value;
        }
    }

    public float Width
    {
        get => RectangleData.Width;
        set
        {
            RectangleData.Width = value;
            RectangleSize.X = value;
        }
    }

    public float Height
    {
        get => RectangleData.Height;
        set
        {
            RectangleData.Height = value;
            RectangleSize.Y = value;
        }
    }

    public Vector2 Position
    {
        get => RectanglePosition;
        set
        {
            X = value.X;
            Y = value.Y;
        }
    }

    public Vector2 Size
    {
        get => RectangleSize;
        set
        {
            Width = value.X;
            Height = value.Y;
        }
    }

    public RayRectangle RayLibRectangle
    {
        get => RectangleData;
        set
        {
            X = value.X;
            Y = value.Y;
            Width = value.Width;
            Height = value.Height;
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

    public Rectangle GrowThis(float amount)
    {
        X -= amount;
        Y -= amount;
        Width += amount * 2;
        Height += amount * 2;
        return this;
    }

    public bool IsVector2In(Vector2 vector2) => Raylib.CheckCollisionPointRec(vector2, RayLibRectangle);
    public bool IsMouseIn() => IsVector2In(Raylib.GetMousePosition());
    public void Draw(Color? color = null) => Raylib.DrawRectangleRec(RayLibRectangle, color ?? WHITE);

    public void DrawLines(float lineThickness = 3, Color? color = null)
        => Raylib.DrawRectangleLinesEx(RayLibRectangle, lineThickness, color ?? WHITE);

    public void DrawPro(Vector2? origin = null, float rotation = 0, Color? color = null)
        => Raylib.DrawRectanglePro(RayLibRectangle, origin ?? Vector2.Zero, rotation, color ?? WHITE);

    public void DrawRounded(float roundness = .1f, int segments = 10, Color? color = null)
        => Raylib.DrawRectangleRounded(RayLibRectangle, roundness, segments, color ?? WHITE);

    public void DrawRoundedLines(float roundness = .1f, int segments = 10, float lineThickness = 3, Color? color = null)
        => Raylib.DrawRectangleRoundedLines(RayLibRectangle, roundness, segments, lineThickness, color ?? WHITE);

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