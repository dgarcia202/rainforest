using Microsoft.Xna.Framework;
using RainForest.Core;

namespace RainForest.Extensions
{
    public static class RectangleExtensions
    {
        public static PrimitiveRect ToPrimitiveRect(this Rectangle rect, Color color) => 
            new PrimitiveRect(rect.X, rect.Y, rect.Width, rect.Height, color);
    }
}
