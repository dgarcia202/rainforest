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
        private SpriteFont _font;
        private Hero _hero;
        private Camera _camera;

        private float _leftStickX;
        private float _rightStickY;
        private double _fps;

        public TestScene(ContentManager content) : base(content)
        {
            _camera = new Camera();
            AddComponent("camera", _camera);
            AddComponent("hero", new Hero(content));
            AddComponent("block-1", new Block(Content, 0f, -64f, 64f, 64f));
            AddComponent("block-2", new Block(Content, 64f, -80f, 120f, 35f));
        }

        public override void Initialize()
        {
            _hero = GetComponent("hero") as Hero;
            _camera.ToFollow = _hero;

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
            _rightStickY = GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Y;
            _fps = 1000d / gameTime.ElapsedGameTime.TotalMilliseconds;
            base.Update(gameTime);
        }

        protected override void InternalDraw(SpriteBatch spriteBatch)
        {
            var sb = new StringBuilder();
            sb.Append($"FPS: {_fps:0}\r\n");
            sb.Append($"Controller Connected: {GamePad.GetState(PlayerIndex.One).IsConnected.ToString()}\r\n");
            sb.Append($"Velocity: X:{_hero.Velocity.X:0.000},Y:{_hero.Velocity.Y:0.000}\r\n");
            sb.Append($"Joy LX: {_leftStickX:0.000}\r\n");
            sb.Append($"Joy RY: {_rightStickY:0.000}\r\n");
            sb.Append($"Hero: X:{_hero.X:0.000},Y:{_hero.Y:0.000}\r\n");
            sb.Append($"Look At: X:{_camera.ToFollow.AbsolutePosition.X:0.000},Y:{_camera.ToFollow.AbsolutePosition.Y:0.000}\r\n");

            Camera cam = GetComponent("camara") as Camera;
            if (cam is not null)
                sb.Append($"CamZ: {cam.Z:0.000}\r\n");

            spriteBatch.DrawString(_font, sb.ToString(), new Vector2(0f, 0f), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.FlipVertically, 0f);

            base.InternalDraw(spriteBatch);
        }
    }
}
