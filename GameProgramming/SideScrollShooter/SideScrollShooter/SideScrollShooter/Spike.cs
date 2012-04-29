using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SideScrollShooter
{
    class Spike:Block
    {
        public Spike(Texture2D textureImage, Vector2 position,float scrollSpeed)
            :base(textureImage,position,scrollSpeed)
        {
        }


        public override void playerCollision(Player player)
        {
            GameController.game.spriteManager.player.dead = true;
            //Game1.Restart();
            base.playerCollision(player);
        }

    }
}
