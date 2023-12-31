using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainForest.Core
{
    internal class Collider : DrawableGameObject
    {
        private double _width, _height;

        public Collider(ContentManager content) : base(content)
        {
        }

        public double Width { get => _width; set => _width = value; }
        public double Height { get => _height; set => _height = value; }
    }
}
