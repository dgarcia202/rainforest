using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace RainForest.Core
{
    public class Camera : GameObject
    {
        public const float MIN_Z = 180f, MAX_Z = 500f;
        public const float ZOOM_SPEED = 6f;

        private Vector2 _position;
        private float _z;
        private float _zoomOrigin;
        private float _aspectRatio;
        private float _fieldOfView;
        private Matrix _viewMatrix;
        private Matrix _projectionMatrix;
        private GameObject _toFollow;

        public Camera()
        {
            _position = Vector2.Zero;
            _aspectRatio = Constants.SCREEN_WIDTH / Constants.SCREEN_HEIGHT;
            _fieldOfView = MathHelper.PiOver2;

            _zoomOrigin = CalculateZoomOrigin();
            _z = _zoomOrigin;

            UpdateMatrices();
        }

        public Vector2 Position { get => _position; }
        public float Z { get => _z; }
        public Matrix ViewMatrix { get => _viewMatrix; }
        public Matrix ProjectionMatrix { get => _projectionMatrix; }
        public GameObject ToFollow { get => _toFollow; set => _toFollow = value; }

        public void UpdateMatrices()
        {
            Vector2 lookAt = _toFollow != null ? _toFollow.AbsolutePosition : Vector2.Zero;
            _viewMatrix = Matrix.CreateLookAt(new Vector3(lookAt, _z), new Vector3(lookAt, 0f), Vector3.Up);
            _projectionMatrix = Matrix.CreatePerspectiveFieldOfView(_fieldOfView, _aspectRatio, MIN_Z, MAX_Z);
        }

        public override void Update(GameTime gameTime)
        {
            float rightStickY = GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Y; 
            if (rightStickY != 0f)
            {
                float delta = ZOOM_SPEED * rightStickY;
                MoveZ(delta);
            }
        }

        public void MoveZ(float newZ)
        {
            _z += newZ;
            if (_z < MIN_Z) {
                _z = MIN_Z;
            }
            else if (_z > MAX_Z)
            {
                _z = MAX_Z;
            }
        }

        private float CalculateZoomOrigin() => (Constants.SCREEN_HEIGHT * .5f) / MathF.Tan(.5f * _fieldOfView);
    }
}
