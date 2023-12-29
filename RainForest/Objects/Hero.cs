using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RainForest.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainForest.Objects
{
    internal class Hero : DrawableGameObject
    {
        private const double MAX_SPEED = 2.5;
        private const double HORIZONTAL_ACCEL = 3.0;

        private double _currentHorizontalSpeed = 0.0;

        private Sprite _sprite;

        public double CurrentHorizontalSpeed { get => _currentHorizontalSpeed; }

        public Hero(ContentManager content) : base(content)
        {
            var sprite = new Sprite(Content, "Sprite-0001-Sheet", 64, 64);
            sprite.Animation = new Animations.HeroWalk(content, sprite);
            sprite.SetPosition(400, 600);
            AddObject("sprite", sprite);
        }

        public override void Initialize()
        {
            _sprite = GetObject("sprite") as Sprite;
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            // Movement
            var leftStick = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X;
           
            _sprite.FlipHorizontally = !(leftStick > 0);

            _currentHorizontalSpeed += (HORIZONTAL_ACCEL * (gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0));
            if (_currentHorizontalSpeed > (MAX_SPEED * leftStick))
            {
                _currentHorizontalSpeed = (MAX_SPEED * leftStick);
            }
            else if (_currentHorizontalSpeed < -(MAX_SPEED * leftStick))
            {
                _currentHorizontalSpeed = -(MAX_SPEED * leftStick);
            }

            _sprite.X += _currentHorizontalSpeed;

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
