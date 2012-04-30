using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SideScrollShooter
{
    class DestructableBlock:Block
    {
        public DestructableBlock(Texture2D textureImage, Vector2 position)
            :base(textureImage,position)
        {
        }

        public override void bulletCollision(AutomatedSprite bullet)
        {
            frameSize = new Point(0, 0);
        }
    }
}
