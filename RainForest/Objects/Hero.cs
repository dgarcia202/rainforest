using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using RainForest.Core;
using System;

namespace RainForest.Objects
{
    internal class Hero : DrawableGameObject
    {
        private const float MAX_SPEED = 2.5f;
        private const float HORIZONTAL_ACCEL = 3.0f;
        private const float HORIZONTAL_DECCEL = 6.0f;

        private float _currentHorizontalSpeed = 0.0f;

        private Sprite _sprite;
        private Animation _animationWalk, _animationIdle;

        public float CurrentHorizontalSpeed { get => _currentHorizontalSpeed; }

        public Hero(ContentManager content) : base(content)
        {
            AddComponent("sprite", new Sprite(Content, "Sprite-0001-Sheet", 64, 64));
            AddComponent("collider-1", new Collider(Content, 29f, 3f, 10f, 50f, Color.Red));
        }

        public override void Initialize()
        {
            _animationIdle = new Animations.HeroIdle(Content, _sprite);
            _animationWalk = new Animations.HeroWalk(Content, _sprite);

            _sprite = GetComponent("sprite") as Sprite;
            _sprite.Animation = _animationIdle;

            X = 0f;
            Y = 0f;

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            // Movement
            float leftStick = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X;
            float ellapsedTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalMilliseconds);

            if (leftStick != 0f) // There´s horizontal move input.
            {
                _sprite.FlipHorizontally = !(leftStick > 0f);

                _currentHorizontalSpeed += (HORIZONTAL_ACCEL * (ellapsedTime / 1000f));
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
                float delta = (HORIZONTAL_DECCEL * (ellapsedTime / 1000f));
                if (_currentHorizontalSpeed > 0f)
                {
                    _currentHorizontalSpeed = _currentHorizontalSpeed > delta ?
                        (_currentHorizontalSpeed - delta) :
                        0f;
                }
                else
                {
                    _currentHorizontalSpeed = _currentHorizontalSpeed + delta <= 0f ?
                        (_currentHorizontalSpeed + delta) :
                        0f;
                }
            }

            X += _currentHorizontalSpeed;

            // Animation
            if (_currentHorizontalSpeed != 0f)
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
