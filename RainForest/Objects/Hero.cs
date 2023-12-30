using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using RainForest.Core;
using System;

namespace RainForest.Objects
{
    internal class Hero : DrawableGameObject
    {
        private const double MAX_SPEED = 2.5;
        private const double HORIZONTAL_ACCEL = 3.0;
        private const double HORIZONTAL_DECCEL = 6.0;

        private double _currentHorizontalSpeed = 0.0;

        private Sprite _sprite;
        private Animation _animationWalk, _animationIdle;

        public double CurrentHorizontalSpeed { get => _currentHorizontalSpeed; }

        public Hero(ContentManager content) : base(content)
        {
            AddObject("sprite", new Sprite(Content, "Sprite-0001-Sheet", 64, 64));
            _animationIdle = new Animations.HeroIdle(Content, _sprite);
            _animationWalk = new Animations.HeroWalk(Content, _sprite);
        }

        public override void Initialize()
        {
            _sprite = GetObject("sprite") as Sprite;
            _sprite.Animation = _animationIdle;
            _sprite.SetPosition(400, 600);

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            // Movement
            var leftStick = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X;

            if (leftStick != 0f) // There´s horizontal move input.
            {
                _sprite.FlipHorizontally = !(leftStick > 0f);

                _currentHorizontalSpeed += (HORIZONTAL_ACCEL * (gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0));
                if (_currentHorizontalSpeed > (MAX_SPEED * leftStick))
                {
                    _currentHorizontalSpeed = (MAX_SPEED * leftStick);
                }
                else if (_currentHorizontalSpeed < -(MAX_SPEED * leftStick))
                {
                    _currentHorizontalSpeed = -(MAX_SPEED * leftStick);
                }
            }
            else if (_currentHorizontalSpeed != 0.0)    // There´s no input for horizontal move decceleration quicks in.
            {
                var delta = (HORIZONTAL_DECCEL * (gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0));
                if (_currentHorizontalSpeed > 0.0)
                {
                    _currentHorizontalSpeed = _currentHorizontalSpeed > delta ?
                        (_currentHorizontalSpeed - delta) :
                        0.0;
                }
                else
                {
                    _currentHorizontalSpeed = _currentHorizontalSpeed + delta <= 0.0 ?
                        (_currentHorizontalSpeed + delta) :
                        0.0;
                }
                
            }

            _sprite.X += _currentHorizontalSpeed;

            // Animation
            if (_currentHorizontalSpeed != 0.0)
            {
                _animationWalk.AnimationSpeedFactor = 2 - (Math.Abs(_currentHorizontalSpeed) / MAX_SPEED);

                _sprite.Animation = _animationWalk;
            }
            else
            {
                _sprite.Animation = _animationIdle;
            }

            base.Update(gameTime);
        }
    }
}
