using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RainForest.Core
{
    internal abstract class Animation : GameObject
    {
        private Sprite _sprite;
        private int _frameX;
        private int _frameY;

        public Animation(ContentManager content, Sprite sprite) : base(content)
        {
            _sprite = sprite;
        }

        public int FrameX { get => _frameX; protected set => _frameX = value; }
        public int FrameY { get => _frameY; set => _frameY = value; }
        internal Sprite Sprite { get => _sprite; }
    }
}
