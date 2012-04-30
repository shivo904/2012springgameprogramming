using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SideScrollShooter
{
    class Blood:AutomatedSprite
    {
        public Blood(Texture2D textureImage, Vector2 position, Vector2 speed)
            : base(textureImage, position, new Point(5, 5), Vector2.Zero, Point.Zero, new Point(1, 1), speed)
        {
        }
    }
}
