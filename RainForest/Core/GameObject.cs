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
        private readonly IDictionary<string, GameObject> _children = new Dictionary<string, GameObject>(DEFAULT_CHILDREN_CAPACITY);
        private GameObject _parent;
        protected readonly ContentManager Content;

        protected IDictionary<string, GameObject> Children => _children;
        public double X { get; set; }
        public double Y { get; set; }
        public bool IsVisible { get; set; } = true;
        public GameObject Parent { get => _parent; set => _parent = value; }
        public double AbsoluteX => X + _parent.X;
        public double AbsoluteY => Y + _parent.Y;

        protected GameObject(ContentManager content)
        {
            Content = content;
        }

        protected void AddComponent(string name, GameObject obj)
        {
            obj.Parent = this;
            _children[name] = obj;
        }

        public GameObject GetComponent(string name)
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