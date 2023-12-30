using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace RainForest.Core
{
    internal class Sprite : DrawableGameObject
    {
        private Texture2D _sheet;
        private double _x, _y;
        private readonly string _textureName;
        private int _width, _height;
        private bool _flipHorizontally;
        private Animation _animation;

        public Sprite(ContentManager content, string textureName, int width, int height) : base(content)
        {
            _textureName = textureName;
            _width = width;
            _height = height;
        }

        public double X { get => _x; set => _x = value; }
        public double Y { get => _y; set => _y = value; }
        public Animation Animation
        {
            get
            {
                return GetObject("animation") as Animation;
            }

            set
            {
                AddObject("animation", value);
            }
        }
        public bool FlipHorizontally { get => _flipHorizontally; set => _flipHorizontally = value; }

        public void SetPosition(double x, double y)
        {
            this._x = x;
            this._y = y;
        }

        public void SetPosition(int x, int y)
        {
            this._x = x;
            this._y = y;
        }


        public override void Update(GameTime gameTime)
        {
            if (Animation is not null)
            {
                Animation.Update(gameTime);
            }
            base.Update(gameTime);
        }

        public override void LoadContent()
        {
            _sheet = Content.Load<Texture2D>(_textureName);
        }

        protected override void InternalDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_sheet,
                new Rectangle(Convert.ToInt32(_x), Convert.ToInt32(_y), _width, _height),                   // Destination.
                new Rectangle(Animation.FrameX * _width, Animation.FrameY * _height, _width, _height),    // Origin.
                Color.White,
                0f,
                Vector2.Zero,
                _flipHorizontally ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                0f);

            base.InternalDraw(spriteBatch);
        }
    }
}
