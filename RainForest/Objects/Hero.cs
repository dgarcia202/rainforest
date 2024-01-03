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

        private Sprite _sprite;
        private Animation _animationWalk, _animationIdle;
        private PhysicsBody _physicsBody;

        public Vector2 Velocity { get => _physicsBody.Velocity; set => _physicsBody.Velocity = value; }

        public Hero(ContentManager content) : base(content)
        {
            AddComponent("sprite", new Sprite(Content, "Sprite-0001-Sheet", 64, 64));
            AddComponent("physics-1", new PhysicsBody(Content, 25f, 0f, 14f, 64f)
            {
                MaxHorizontalSpeed = MAX_SPEED,
                HorizontalAccel = 3.0f,
                HorizontalDeccel = 6.0f,
                HasGravity = true,
            });
        }

        public override void Initialize()
        {
            _animationIdle = new Animations.HeroIdle(_sprite);
            _animationWalk = new Animations.HeroWalk(_sprite);

            _sprite = GetComponent("sprite") as Sprite;
            _sprite.Animation = _animationIdle;

            _physicsBody = GetComponent("physics-1") as PhysicsBody;

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            _physicsBody.HorizontalForce = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X;

            if (_physicsBody.HorizontalForce != 0f)
                _sprite.FlipHorizontally = !(_physicsBody.HorizontalForce > 0f);

            if (_physicsBody.Velocity.X != 0f)
            {
                _animationWalk.AnimationSpeedFactor = 2 - (Math.Abs(_physicsBody.Velocity.X) / MAX_SPEED);
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
