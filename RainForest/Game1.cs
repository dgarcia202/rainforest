using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RainForest.Objects;
using RainForest.Scenes;
using System;

namespace RainForest
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private TestScene _scene;
        private BasicEffect _spritesEffect;
        private BasicEffect _shapesEffect;

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

            // sprites
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

            // shapes
            Matrix v = Matrix.CreateTranslation(-1f, -1f, .0f);
            Matrix s = Matrix.CreateScale(1f / ((float)Constants.SCREEN_WIDTH / 2f), 1f / ((float)Constants.SCREEN_HEIGHT / 2f), 0f);
            _shapesEffect = new BasicEffect(GraphicsDevice);
            _shapesEffect.VertexColorEnabled = true;
            _shapesEffect.World = s * v;
            _shapesEffect.View = Matrix.Identity;
            _shapesEffect.Projection = Matrix.Identity;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _scene.LoadContent();

            Texture2D rect = new Texture2D(_graphics.GraphicsDevice, 80, 30);
            Color[] data = new Color[80 * 30];
            for (int i = 0; i < data.Length; ++i)
                data[i] = Color.Chocolate;

            rect.SetData(data);
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

            // projection matrix
            var viewPort = _graphics.GraphicsDevice.Viewport;
            _spritesEffect.Projection = Matrix.CreateOrthographicOffCenter(0f, viewPort.Width, 0f, viewPort.Height, 0f, 1f);

            // TODO: Add your drawing code here
            _spriteBatch.Begin(
                blendState: BlendState.AlphaBlend,
                samplerState: SamplerState.PointClamp,
                rasterizerState: RasterizerState.CullNone,
                effect: _spritesEffect);

            _scene.Draw(_spriteBatch);

            _spriteBatch.End();

            // shape test

            VertexPositionColor[] vertex = new VertexPositionColor[4];
            vertex[0] = new VertexPositionColor(new Vector3(0f, 0f, 0f), Color.Yellow);
            vertex[1] = new VertexPositionColor(new Vector3(64f, 0f, 0f), Color.Red);
            vertex[2] = new VertexPositionColor(new Vector3(64f, 64f, 0f), Color.Blue);
            vertex[3] = new VertexPositionColor(new Vector3(0f, 64f, 0f), Color.Green);

            int[] indexes = new int[5];
            indexes[0] = 0;
            indexes[1] = 1;
            indexes[2] = 2;
            indexes[3] = 3;
            indexes[4] = 0;

            foreach (EffectPass pass in _shapesEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(
                    PrimitiveType.LineStrip,
                    vertex,
                    0,
                    vertex.Length,
                    indexes,
                    0,
                    4);
            }

            base.Draw(gameTime);
        }
    }
}