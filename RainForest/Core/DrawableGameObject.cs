using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RainForest.Core
{
    internal abstract class DrawableGameObject : GameObject
    {
        protected DrawableGameObject(ContentManager content) : base(content)
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsVisible)
            {
                InternalDraw(spriteBatch);
            }
        }

        protected virtual void InternalDraw(SpriteBatch spriteBatch)
        {
            foreach (var child in Children.Values)
            {
                if (child is DrawableGameObject drawable)
                    drawable.InternalDraw(spriteBatch);
            }
        }
    }
}
