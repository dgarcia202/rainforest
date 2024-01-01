using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using RainForest.Core;

namespace RainForest.Objects
{
    public class Block : DrawableGameObject
    {
        public Block(ContentManager content, float x, float y, float width, float height) : base(content)
        {
            X = x;
            Y = y;
            AddComponent("collider-1", new Collider(Content, 0f, 0f, width, height, Color.Green));
        }

        public override void Initialize()
        {
            base.Initialize();
        }
    }
}
