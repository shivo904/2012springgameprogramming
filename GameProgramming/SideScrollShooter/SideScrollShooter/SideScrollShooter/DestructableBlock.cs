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
        int hp = 3;
        int maxhp = 3;
        public DestructableBlock(Texture2D textureImage, Vector2 position)
            :base(textureImage,position)
        {
        }

        public override void bulletCollision(AutomatedSprite bullet)
        {
            hp--;

           

        }
        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            
            position.X -= GameController.game.spriteManager.scrollSpeed;
            if (hp != 0)
                currentFrame = new Point(maxhp- hp, 0);
            else
            {
                frameSize = new Point(0, 0);
            }
        }
    }
}
