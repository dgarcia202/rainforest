using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
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
        private double _interval = 200f;
        private double _animationSpeedFactor = 1f;

        public Animation(ContentManager content, Sprite sprite) : base(content)
        {
            _sprite = sprite;
        }

        public Animation(ContentManager content, Sprite sprite, float interval) : this(content, sprite)
        {
            _interval = interval;
        }

        public int FrameX { get => _frameX; protected set => _frameX = value; }
        public int FrameY { get => _frameY; set => _frameY = value; }
        public double Interval { get => _interval; set => _interval = value; }
        public Sprite Sprite { get => _sprite; }
        public double AnimationSpeedFactor { get => _animationSpeedFactor; set => _animationSpeedFactor = value; }
    }
}
