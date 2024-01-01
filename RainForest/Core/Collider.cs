using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace RainForest.Core
{
    public class Collider : DrawableGameObject
    {
        private float _width, _height;
        private Color _color = Color.White;

        protected override IEnumerable<PrimitiveRect> Shapes => new PrimitiveRect[] { new PrimitiveRect(AbsoluteX, AbsoluteY, Width, Height, _color) };
        public float Width { get => _width; set => _width = value; }
        public float Height { get => _height; set => _height = value; }
        public Color Color { get => _color; set => _color = value; }

        public Collider(ContentManager content) : base(content)
        {
        }

        public Collider(ContentManager content, float x, float y, float width, float height) : base(content)
        {
            X = x;
            Y = y;
            _width = width;
            _height = height;
        }

        public Collider(ContentManager content, float x, float y, float width, float height, Color color) : this(content, x, y, width, height)
        {
            Color = color;
        }
    }
}
