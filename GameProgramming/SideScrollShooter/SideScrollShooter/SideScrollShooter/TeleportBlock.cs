using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SideScrollShooter
{
    class TeleportBlock:Block
    {
        public TeleportBlock(Texture2D textureImage, Vector2 position,float scrollSpeed)
            :base(textureImage,position,new Point(4,1),scrollSpeed)
        {

        }

        public override void bulletCollision(AutomatedSprite bullet)
        {
            GameController.game.spriteManager.player.position.X = position.X;
            GameController.game.spriteManager.player.position.Y = position.Y - 25;
            GameController.game.spriteManager.soundBank.PlayCue("warp");
            base.bulletCollision(bullet);
        }

        public override void playerCollision(Player player)
        {
            //base.playerCollision(player);
        }
    }
}
