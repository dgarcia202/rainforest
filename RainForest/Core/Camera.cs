using Microsoft.Xna.Framework;
using System;

namespace RainForest.Core
{
    public class Camera
    {
        public const float MIN_Z = 1f, MAX_Z = 2048f;

        private Vector2 _position;
        private float _z;
        private float _zoomOrigin;
        private float _aspectRatio;
        private float _fieldOfView;
        private Matrix _viewMatrix;
        private Matrix _projectionMatrix;

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

        public void UpdateMatrices()
        {
            _viewMatrix = Matrix.CreateLookAt(new Vector3(0f, 0f, _z), Vector3.Zero, Vector3.Up);
            _projectionMatrix = Matrix.CreatePerspectiveFieldOfView(_fieldOfView, _aspectRatio, MIN_Z, MAX_Z);
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
