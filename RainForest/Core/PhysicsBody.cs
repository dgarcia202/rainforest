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
        private Vector2 _force;
        private bool _hasGravity;
        private bool _isGrounded;
        private float _maxHorizontalSpeed;
        private float _horizontalAccel;
        private float _horizontalDeccel;
        private float _verticalDeccel;
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
        public float HorizontalForce { get => _force.X; set => _force = new Vector2(value, _force.Y); }
        public float VerticalForce { get => _force.Y; set => _force = new Vector2(_force.X, value); }
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

        public Vector2 Force { get => _force; set => _force = value; }

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
            _force = Vector2.Zero;
        }

        public PhysicsBody(ContentManager content, float x, float y, float width, float height) : this(content, width, height)
        {
            X = x;
            Y = y;
        }

        public void AddForce(Vector2 additionalForce) => _force += additionalForce;

        public override void Update(GameTime gameTime)
        {
            // Movement.
            float timeFactor = Convert.ToSingle(gameTime.ElapsedGameTime.TotalMilliseconds) / 1000f;

            if (IsGrounded)
            {
                if (_force.X != 0f)    // Movement input exists. We update our velocity if not at MAX.
                {
                    _velocity.X += (_horizontalAccel * timeFactor);
                    if (_velocity.X > (_maxHorizontalSpeed * _force.X))
                    {
                        _velocity.X = (_maxHorizontalSpeed * _force.X);
                    }
                    else if (_velocity.X < -(_maxHorizontalSpeed * _force.X))
                    {
                        _velocity.X = -(_maxHorizontalSpeed * _force.X);
                    }
                }
                else if (_velocity.X != 0.0)    // No movement input. Horizontal deceleration happens.
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
                    Parent.Position += new Vector2((col.AbsoluteX + col.Width) - AbsoluteX, 0f);
                }

                else if ((_velocity.X > 0f && geometryRect.Intersects(RightDetectionRect)))
                {
                    _velocity.X = 0f;
                    Parent.Position += new Vector2(-(AbsoluteX + _width - col.AbsoluteX), 0f);
                }
            }

            // Gravity.
            if (HasGravity)
            {
                CheckGrounded(geometry);
                if (!_isGrounded)
                {
                    _velocity.Y -= (_fallAccel * timeFactor);
                    if (_velocity.Y < -_maxFallSpeed)
                    {
                        _velocity.Y = -_maxFallSpeed;
                    }
                }
                else if (_velocity.Y < 0f)
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
                    Parent.Position += new Vector2(0f, -(AbsoluteY - (col.AbsoluteY + col.Height)));
                    isGroundedTentative = true;
                    break;
                }
            }

            _isGrounded = isGroundedTentative;
        }
    }
}
