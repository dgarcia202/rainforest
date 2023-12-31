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
        private readonly Rectangle _rectagle;

        public Collider(ContentManager content) : base(content)
        {
        }
    }
}
