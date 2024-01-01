using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RainForest.Core;
using RainForest.Objects;
using RainForest.Scenes;
using System;

namespace RainForest
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private PrimitivesBatch _primitivesBatch;

        private TestScene _scene;
        private BasicEffect _spritesEffect;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;  
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = Constants.SCREEN_WIDTH;
            _graphics.PreferredBackBufferHeight = Constants.SCREEN_HEIGHT;
            _graphics.ApplyChanges();

            _spritesEffect = new BasicEffect(GraphicsDevice);
            _spritesEffect.VertexColorEnabled = true;
            _spritesEffect.TextureEnabled = true;
            _spritesEffect.FogEnabled = false;
            _spritesEffect.LightingEnabled = false;
            _spritesEffect.World = Matrix.Identity;
            _spritesEffect.View = Matrix.Identity;
            _spritesEffect.Projection = Matrix.Identity;

            // components
            _scene = new TestScene(Content);
            _scene.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _primitivesBatch = new PrimitivesBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _scene.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            _scene.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkGray);

            // Projection matrix.
            var viewPort = _graphics.GraphicsDevice.Viewport;
            _spritesEffect.Projection = Matrix.CreateOrthographicOffCenter(0f, viewPort.Width, 0f, viewPort.Height, 0f, 1f);

            // Sprites.
            _spriteBatch.Begin(
                blendState: BlendState.AlphaBlend,
                samplerState: SamplerState.PointClamp,
                rasterizerState: RasterizerState.CullNone,
                effect: _spritesEffect);

            _scene.Draw(_spriteBatch);

            _spriteBatch.End();

            // Primitives.
            _primitivesBatch.Begin();
            _scene.DrawPrimitives(_primitivesBatch);
            _primitivesBatch.End();

            base.Draw(gameTime);
        }
    }
}