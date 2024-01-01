using Microsoft.Xna.Framework;
using System;

namespace RainForest.Core
{
    public readonly struct PrimitiveRect
    {
        public PrimitiveRect(float x, float y, float width, float height, Color color)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Color = color;
        }

        public float X { get; init; }
        public float Y { get; init; }
        public float Width { get; init; }
        public float Height { get; init; }
        public Color Color { get; init; }

        public override bool Equals(object obj)
        {
            return obj is PrimitiveRect rect &&
                   X == rect.X &&
                   Y == rect.Y &&
                   Width == rect.Width &&
                   Height == rect.Height &&
                   Color.Equals(rect.Color);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Width, Height, Color);
        }

        public override string ToString() => $"Rect({X:0.00},{Y:0.00},{Width:0.00},{Height:0.00},{Color})";
    }
}
