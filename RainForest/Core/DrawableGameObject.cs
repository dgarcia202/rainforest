﻿using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace RainForest.Core
{
    public abstract class DrawableGameObject : GameObject
    {
        protected readonly ContentManager Content;

        protected virtual IEnumerable<PrimitiveRect> Shapes { get => Array.Empty<PrimitiveRect>(); }

        protected DrawableGameObject(ContentManager content)
        {
            Content = content;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsVisible)
            {
                InternalDraw(spriteBatch);
            }
        }

        public void DrawUI(SpriteBatch spriteBatch)
        {
            if (IsVisible)
            {
                InternalDrawUI(spriteBatch);
            }
        }

        public void DrawPrimitives(PrimitivesBatch primitivesBatch)
        {
            if (IsVisible)
            {
                foreach (var rect in Shapes)
                {
                    primitivesBatch.DrawRectangle(rect.X, rect.Y, rect.Width, rect.Height, rect.Color);
                }
            }

            foreach (var child in Children.Values)
            {
                if (child is DrawableGameObject drawable)
                    drawable.DrawPrimitives(primitivesBatch);
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

        protected virtual void InternalDrawUI(SpriteBatch spriteBatch)
        {
            foreach (var child in Children.Values)
            {
                if (child is DrawableGameObject drawable)
                    drawable.InternalDrawUI(spriteBatch);
            }
        }
    }
}
