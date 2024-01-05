using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace RainForest.Core
{
    public class Sprite : DrawableGameObject
    {
        private Texture2D _sheet;
        private readonly string _textureName;
        private int _width, _height;
        private bool _flipHorizontally;

        public Sprite(ContentManager content, string textureName, int width, int height) : base(content)
        {
            _textureName = textureName;
            _width = width;
            _height = height;
        }

        public Animation Animation
        {
            get
            {
                return GetComponent("animation") as Animation;
            }

            set
            {
                AddComponent("animation", value);
            }
        }

        public bool FlipHorizontally { get => _flipHorizontally; set => _flipHorizontally = value; }

        public override void LoadContent()
        {
            _sheet = Content.Load<Texture2D>(_textureName);
        }

        protected override void InternalDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_sheet,
                new Rectangle(Convert.ToInt32(AbsoluteX), Convert.ToInt32(AbsoluteY), _width, _height),    // Destination.
                new Rectangle(Animation.FrameX * _width, Animation.FrameY * _height, _width, _height),    // Origin.
                Color.White,
                0f,
                Vector2.Zero,
                SpriteEffects.FlipVertically | (_flipHorizontally ? SpriteEffects.FlipHorizontally : SpriteEffects.None),
                0f);

            base.InternalDraw(spriteBatch);
        }
    }
}
