using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RainForest.Core
{
    public abstract class GameObject
    {
        private const int DEFAULT_CHILDREN_CAPACITY = 20;
        private readonly IDictionary<string, GameObject> _children = new Dictionary<string, GameObject>(DEFAULT_CHILDREN_CAPACITY);
        private GameObject _parent;

        protected IDictionary<string, GameObject> Children => _children;
        public float X { get; set; }
        public float Y { get; set; }
        public bool IsVisible { get; set; } = true;
        public GameObject Parent { get => _parent; set => _parent = value; }
        public float AbsoluteX => X + _parent?.AbsoluteX ?? 0f;
        public float AbsoluteY => Y + _parent?.AbsoluteY ?? 0f;

        public Vector2 AbsolutePosition { get => new Vector2(AbsoluteX, AbsoluteY); }

        public Vector2 Position { 
            get => new Vector2(X, Y); 
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        public void AddComponent(string name, GameObject obj)
        {
            obj.Parent = this;
            _children[name] = obj;
        }

        public GameObject GetComponent(string name)
        {
            return _children.TryGetValue(name, out GameObject obj) ? obj : null;
        }

        public IEnumerable<T> GetComponents<T>() where T : GameObject
        {
            var objects = new List<T>();
            foreach(var child in _children.Values)
            {
                if (child is T obj)
                {
                    objects.Add(obj);
                }
                else
                {
                    objects.AddRange(child.GetComponents<T>());
                }
            }

            return objects;
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