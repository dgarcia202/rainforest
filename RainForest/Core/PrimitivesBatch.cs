using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Framework.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainForest.Core
{
    public class PrimitivesBatch
    {
        private BasicEffect _effect;
        private GraphicsDevice _graphicsDevice;
        private const int BUFFER_SIZE = 1024;
        private VertexPositionColor[] _vertices;
        private int[] _indices;
        private int _vertexCount;
        private int _indexCount;
        private int _primitiveCount;
        private bool _hasBegun;

        public PrimitivesBatch(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
            _vertices = new VertexPositionColor[BUFFER_SIZE];
            _indices = new int[BUFFER_SIZE];
            _vertexCount = 0;
            _indexCount = 0;
            _primitiveCount = 0;
            _hasBegun = false;

            Matrix v = Matrix.CreateTranslation(-1f, -1f, .0f);
            Matrix s = Matrix.CreateScale(1f / ((float)Constants.SCREEN_WIDTH / 2f), 1f / ((float)Constants.SCREEN_HEIGHT / 2f), 0f);
            _effect = new BasicEffect(_graphicsDevice);
            _effect.VertexColorEnabled = true;
            _effect.World = s * v;
            _effect.View = Matrix.Identity;
            _effect.Projection = Matrix.Identity;
        }

        public void Begin()
        {
            if (_hasBegun)
                throw new Exception("Primitives batch has already started.");


            _hasBegun = true;
        }

        public void End()
        {
            if (!_hasBegun)
                throw new Exception("Primitives batch hasn't started.");

            Flush();
            _hasBegun = false;
        }

        public void DrawRectangle(float x, float y, float width, float height, Color color)
        {
            if (!_hasBegun)
                throw new Exception("Primitives batch hasn't started.");

            if (_indexCount > BUFFER_SIZE - 8)
                Flush();

            var firstVertexIndex = _vertexCount;
            _vertices[_vertexCount++] = new VertexPositionColor(new Vector3(x, y, 0f), color);
            _vertices[_vertexCount++] = new VertexPositionColor(new Vector3(x + width, y, 0f), color);
            _vertices[_vertexCount++] = new VertexPositionColor(new Vector3(x + width, y + height, 0f), color);
            _vertices[_vertexCount++] = new VertexPositionColor(new Vector3(x, y + height, 0f), color);
            _vertices[_vertexCount++] = new VertexPositionColor(new Vector3(x, y, 0f), color);
            _indices[_indexCount++] = firstVertexIndex;
            _indices[_indexCount++] = firstVertexIndex + 1;
            _indices[_indexCount++] = firstVertexIndex + 1;
            _indices[_indexCount++] = firstVertexIndex + 2;
            _indices[_indexCount++] = firstVertexIndex + 2;
            _indices[_indexCount++] = firstVertexIndex + 3;
            _indices[_indexCount++] = firstVertexIndex + 3;
            _indices[_indexCount++] = firstVertexIndex ;

            _primitiveCount += 4;
        }

        private void Flush()
        {
            if (!_hasBegun)
                throw new Exception("Primitives batch hasn't started.");

            if (_primitiveCount > 0)
            {
                foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    _graphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(
                        PrimitiveType.LineList,
                        _vertices,
                        0,
                        _vertices.Length,
                        _indices,
                        0,
                        _primitiveCount);
                }
            }
            _vertexCount = 0;
            _indexCount = 0;
            _primitiveCount = 0;
        }
    }
}
