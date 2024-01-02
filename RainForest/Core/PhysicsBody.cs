using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using RainForest.Extensions;
using System;

namespace RainForest.Core
{
    public class PhysicsBody : GameObject
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

        public Vector2 Velocity { get => _velocity; set => _velocity = value; }
        public bool HasGravity { get => _hasGravity; set => _hasGravity = value; }
        public float MaxHorizontalSpeed { get => _maxHorizontalSpeed; set => _maxHorizontalSpeed = value; }
        public float HorizontalAccel { get => _horizontalAccel; set => _horizontalAccel = value; }
        public float HorizontalDeccel { get => _horizontalDeccel; set => _horizontalDeccel = value; }
        public GameObject GeometrySource { get => _geometrySource; set => _geometrySource = value; }
        public bool IsGrounded { get => _isGrounded; }

        public PhysicsBody()
        {
            _hasGravity = false;
            _maxHorizontalSpeed = 0f;
            _horizontalAccel = 0f;
            _horizontalDeccel = 0f;
            _fallAccel = 7f;
            _maxFallSpeed = 4f;
        }

        public override void Update(GameTime gameTime)
        {
            // Movement
            float leftStick = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X;
            float timeFactor = Convert.ToSingle(gameTime.ElapsedGameTime.TotalMilliseconds) / 1000f;

            if (leftStick != 0f)
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
            else if (_velocity.X != 0.0)
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

            // Gravity
            if (HasGravity)
            {
                CheckGrounded();
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

        private void CheckGrounded()
        {
            var self = GetComponents<Collider>();
            var geometry = _geometrySource.GetComponents<Collider>();
            Collider lowestSelfCollider = null;
            bool isGroundedTentative = false;

            foreach (var col in self)
            {
                if (lowestSelfCollider is null)
                    lowestSelfCollider = col;
                else
                {
                    if (col.Y < lowestSelfCollider.AbsoluteY)
                        lowestSelfCollider = col;
                }
            }

            foreach(Collider col in geometry)
            {
                if (Object.ReferenceEquals(col, lowestSelfCollider))
                    continue;

                var rect = new Rectangle((int)col.AbsoluteX, (int)col.AbsoluteY, (int)col.Width, (int)col.Height);
                if (rect.Contains(lowestSelfCollider.AbsoluteX + (lowestSelfCollider.Width * 0.5f), lowestSelfCollider.AbsoluteY - 1))
                {
                    isGroundedTentative = true;
                    break;
                }
            }

            _isGrounded = isGroundedTentative;
        }
    }
}
