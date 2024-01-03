using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using RainForest.Extensions;
using System;
using System.Collections.Generic;

namespace RainForest.Core
{
    public class PhysicsBody : DrawableGameObject
    {
        private Vector2 _velocity;
        private bool _hasGravity;
        private bool _isGrounded;
        private float _maxHorizontalSpeed;
        private float _horizontalAccel;
        private float _horizontalDeccel;
        private float _fallAccel;
        private float _maxFallSpeed;
        private GameObject _geometrySource;     // Collisions will take into account all the colliders under this object.
        private float _width, _height;
        private Color _color;

        public Vector2 Velocity { get => _velocity; set => _velocity = value; }
        public bool HasGravity { get => _hasGravity; set => _hasGravity = value; }
        public float MaxHorizontalSpeed { get => _maxHorizontalSpeed; set => _maxHorizontalSpeed = value; }
        public float HorizontalAccel { get => _horizontalAccel; set => _horizontalAccel = value; }
        public float HorizontalDeccel { get => _horizontalDeccel; set => _horizontalDeccel = value; }
        public GameObject GeometrySource { get => _geometrySource; set => _geometrySource = value; }
        public bool IsGrounded { get => _isGrounded; }
        protected override IEnumerable<PrimitiveRect> Shapes =>
            new PrimitiveRect[] { 
                new PrimitiveRect(AbsoluteX, AbsoluteY, _width, _height, _color),
                RightDetectionRect.ToPrimitiveRect(Color.Green),
                LeftDetectionRect.ToPrimitiveRect(Color.Yellow),
                BottomDetectionRect.ToPrimitiveRect(Color.Blue)
            };

        public Color Color { get => _color; set => _color = value; }
        public Rectangle RightDetectionRect => new Rectangle(
                    Convert.ToInt32(AbsoluteX + _width),
                    Convert.ToInt32(AbsoluteY + 2f),
                    1,
                    Convert.ToInt32(_height));

        public Rectangle LeftDetectionRect => new Rectangle(
                    Convert.ToInt32(AbsoluteX - 1f),
                    Convert.ToInt32(AbsoluteY + 2f),
                    1,
                    Convert.ToInt32(_height));

        public Rectangle BottomDetectionRect => new Rectangle(
                Convert.ToInt32(AbsoluteX),
                Convert.ToInt32(AbsoluteY - 1),
                Convert.ToInt32(_width),
                1);

        public PhysicsBody(ContentManager content, float width, float height) : base(content)
        {
            _hasGravity = false;
            _maxHorizontalSpeed = 0f;
            _horizontalAccel = 0f;
            _horizontalDeccel = 0f;
            _fallAccel = 7f;
            _maxFallSpeed = 4f;
            _width = width;
            _height = height;
            _color = Color.White;
        }

        public PhysicsBody(ContentManager content, float x, float y, float width, float height) : this(content, width, height)
        {
            X = x;
            Y = y;
        }

        public override void Update(GameTime gameTime)
        {
            // Movement.
            float leftStick = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X;
            float timeFactor = Convert.ToSingle(gameTime.ElapsedGameTime.TotalMilliseconds) / 1000f;

            if (leftStick != 0f)    // Movement input exists. We update our velocity if not at MAX.
            {
                _velocity.X += (_horizontalAccel * timeFactor);
                if (_velocity.X > (_maxHorizontalSpeed * leftStick))
                {
                    _velocity.X = (_maxHorizontalSpeed * leftStick);
                }
                else if (_velocity.X < -(_maxHorizontalSpeed * leftStick))
                {
                    _velocity.X = -(_maxHorizontalSpeed * leftStick);
                }
            }
            else if (_velocity.X != 0.0)    // No movement input. deceleration happens.
            {
                float delta = (_horizontalDeccel * timeFactor);
                if (_velocity.X > 0f)
                {
                    _velocity.X = _velocity.X > delta ?
                        (_velocity.X - delta) :
                        0f;
                }
                else
                {
                    _velocity.X = _velocity.X + delta <= 0f ?
                        (_velocity.X + delta) :
                        0f;
                }
            }

            // Detect collisions.
            var geometry = _geometrySource.GetComponents<Collider>();
            foreach (Collider col in geometry)
            {
                var geometryRect = new Rectangle(Convert.ToInt32(col.AbsoluteX), Convert.ToInt32(col.AbsoluteY),
                    Convert.ToInt32(col.Width), Convert.ToInt32(col.Height));

                if (_velocity.X < 0f && geometryRect.Intersects(LeftDetectionRect))
                {
                    _velocity.X = 0f;
                }

                else if ((_velocity.X > 0f && geometryRect.Intersects(RightDetectionRect)))
                {
                    _velocity.X = 0f;
                }
            }

            // Gravity.
            if (HasGravity)
            {
                CheckGrounded(geometry);
                if (!_isGrounded)
                {
                    _velocity.Y -= (_fallAccel * timeFactor);
                    if (_velocity.Y > _maxFallSpeed)
                    {
                        _velocity.Y = -_maxFallSpeed;
                    }
                }
                else
                    _velocity.Y = 0f;
            }

            Parent.Position += _velocity;

            base.Update(gameTime);
        }

        private void CheckGrounded(IEnumerable<Collider> geometry)
        {
            bool isGroundedTentative = false;

            foreach (Collider col in geometry)
            {
                var geometryRect = new Rectangle(Convert.ToInt32(col.AbsoluteX), Convert.ToInt32(col.AbsoluteY),
                    Convert.ToInt32(col.Width), Convert.ToInt32(col.Height));

                if (geometryRect.Intersects(BottomDetectionRect))
                {
                    isGroundedTentative = true;
                    break;
                }
            }

            _isGrounded = isGroundedTentative;
        }
    }
}
