using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace RainForest.Core
{
    internal abstract class DrawableGameObject : GameObject
    {
        protected DrawableGameObject(ContentManager content) : base(content)
        {
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (Visible)
            {
                foreach (var child in Children.Select(x => x.Value as DrawableGameObject))
                {
                    child.Draw(spriteBatch);
                }
            }
        }
    }
}
