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

        private float _leftStickX;

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

        public override void Update(GameTime gameTime)
        {
            _leftStickX = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X;
            base.Update(gameTime);
        }

        protected override void InternalDraw(SpriteBatch spriteBatch)
        {
            var sb = new StringBuilder();
            sb.Append($"Velocity: {_hero.CurrentHorizontalSpeed}\r\n");
            sb.Append($"Joy: {_leftStickX}");

            spriteBatch.DrawString(_font, sb.ToString(), new Vector2(0f, 0f), Color.White);

            base.InternalDraw(spriteBatch);
        }
    }
}
