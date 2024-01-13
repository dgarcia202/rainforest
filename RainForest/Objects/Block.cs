using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using RainForest.Core;

namespace RainForest.Objects
{
    public class Block : DrawableGameObject
    {
        public Block(ContentManager content, float x, float y, float width, float height) : this(content, x, y, width, height, Color.Green)
        {
        }

        public Block(ContentManager content, float x, float y, float width, float height, Color color) : base(content)
        {
            X = x;
            Y = y;
            AddComponent("collider-1", new Collider(Content, 0f, 0f, width, height, color));
        }

        public override void Initialize()
        {
            base.Initialize();
        }
    }
}
