using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RainForest.Core;
using RainForest.Objects;
using System.Text;

namespace RainForest.Scenes
{
    internal class TestScene : DrawableGameObject
    {
        SpriteFont _font;
        Hero _hero;

        public TestScene(ContentManager content) : base(content)
        {
            AddObject("hero", new Hero(content));
        }

        public override void Initialize()
        {
            _hero = GetObject("hero") as Hero;
            base.Initialize();
        }

        public override void LoadContent()
        {
            _font = Content.Load<SpriteFont>("GameFont");
            base.LoadContent();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Visible)
            {
                var sb = new StringBuilder();
                sb.Append($"Velocity: {_hero.CurrentHorizontalSpeed}\r\n");
                sb.Append($"Joy: {GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X}");

                spriteBatch.DrawString(_font, sb.ToString(), new Vector2(700f, 100f), Color.White);
            }

            base.Draw(spriteBatch);
        }
    }
}
