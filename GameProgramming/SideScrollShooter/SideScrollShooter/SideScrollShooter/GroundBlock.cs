using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SideScrollShooter
{
    class GroundBlock:Block
    {
        public GroundBlock(Texture2D textureImage, Vector2 position)
            : base(textureImage, position)
        {
        }
    }
}
