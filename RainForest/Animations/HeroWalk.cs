using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using RainForest.Core;

namespace RainForest.Animations
{
    internal class HeroWalk : Animation
    {
        private const int LAST_FRAME = 5;

        private double _lastFrameTime = 0.0;

        public HeroWalk(ContentManager content, Sprite sprite) : base(content, sprite)
        {
            FrameX = 0;
            FrameY = 0;
            Interval = 150.0;
        }

        public override void Update(GameTime gameTime)
        {
            if (_lastFrameTime == 0.0)
                _lastFrameTime = gameTime.TotalGameTime.TotalMilliseconds;

            if (gameTime.TotalGameTime.TotalMilliseconds - _lastFrameTime >= (Interval * AnimationSpeedFactor))
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
