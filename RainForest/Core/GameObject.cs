using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace RainForest.Core
{
    internal abstract class GameObject
    {
        private const int DEFAULT_CHILDREN_CAPACITY = 20;

        protected readonly ContentManager Content;
        private readonly IDictionary<string, GameObject> _children = new Dictionary<string, GameObject>(DEFAULT_CHILDREN_CAPACITY);

        protected GameObject(ContentManager content)
        {
            Content = content;
            Visible = true;
        }

        protected void AddObject(string name, GameObject obj)
        {
            _children[name] = obj;
        }

        public bool Visible { get; set; }

        protected IDictionary<string, GameObject> Children => _children;

        public GameObject GetObject(string name)
        {
            return _children[name];
        }

        public virtual void Initialize()
        {
            foreach (var child in _children)
            {
                child.Value.Initialize();
            }
        }

        public virtual void LoadContent()
        {
            foreach (var child in _children)
            {
                child.Value.LoadContent();
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach(var child in _children)
            {
                child.Value.Update(gameTime);
            }
        }
    }
}