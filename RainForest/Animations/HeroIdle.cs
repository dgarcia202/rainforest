using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using RainForest.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainForest.Animations
{
    internal class HeroIdle : Animation
    {
        private const float INTERVAL = 250f;
        private const int LAST_FRAME = 1;

        private double _lastFrameTime = 0.0;

        public HeroIdle(ContentManager content, Sprite sprite) : base(content, sprite)
        {
            FrameX = 0;
            FrameY = 1;
        }

        public override void Update(GameTime gameTime)
        {
            if (_lastFrameTime == 0.0)
                _lastFrameTime = gameTime.TotalGameTime.TotalMilliseconds;

            if (gameTime.TotalGameTime.TotalMilliseconds - _lastFrameTime >= INTERVAL)
            {
                if (FrameX == LAST_FRAME)
                    FrameX = 0;
                else
                    FrameX++;

                _lastFrameTime = gameTime.TotalGameTime.TotalMilliseconds;
            }

            base.Update(gameTime);
        }
    }
}
