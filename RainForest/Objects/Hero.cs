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
        private Animation _animationWalk, _animationIdle, _animationFall;
        private PhysicsBody _physicsBody;
        private bool _useController;

        public Vector2 Velocity { get => _physicsBody.Velocity; set => _physicsBody.Velocity = value; }
        public float HorizontalInput
        {
            get
            {
                if (_useController)
                    return GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X;

                if (Keyboard.GetState().IsKeyDown(Keys.A))
                    return -1f;

                if (Keyboard.GetState().IsKeyDown(Keys.D))
                    return 1f;

                return 0f;
            }
        }

        public bool JumpInput
        {
            get
            {
                if (_useController)
                    return GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed;

                if (Keyboard.GetState().IsKeyDown(Keys.W))
                    return true;

                return false;
            }
        }

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
            _animationFall = new Animations.HeroFall(_sprite);

            _sprite = GetComponent("sprite") as Sprite;
            _sprite.Animation = _animationIdle;

            _physicsBody = GetComponent("physics-1") as PhysicsBody;

            _useController = GamePad.GetState(PlayerIndex.One).IsConnected;

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            _physicsBody.HorizontalForce = HorizontalInput;

            if (_physicsBody.HorizontalForce != 0f)
                _sprite.FlipHorizontally = !(_physicsBody.HorizontalForce > 0f);

            if (_physicsBody.IsGrounded && JumpInput)
            {
                _physicsBody.Velocity += new Vector2(0f, 3f);
            }

            if (!_physicsBody.IsGrounded)
            {
                _sprite.Animation = _animationFall;
            }
            else if (_physicsBody.Velocity.X != 0f)
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
