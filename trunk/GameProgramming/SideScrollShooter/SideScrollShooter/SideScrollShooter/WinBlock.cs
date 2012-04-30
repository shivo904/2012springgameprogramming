using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SideScrollShooter
{
    class WinBlock:Block
    {
        public static bool hit = false;
        public WinBlock(Texture2D textureImage, Vector2 position,float scrollSpeed)
            : base(textureImage, position,scrollSpeed) 
        { 
        }


        public override void playerCollision(Player player)
        {
            if (!hit)
            {
                hit = true;
                GameController.game.spriteManager.Visible = false;
                GameController.game.spriteManager.Enabled = false;
                GameController.game.levelTransition.Visible = true;
                GameController.game.levelTransition.Enabled = true;
            }


            //winTiles.Clear();

            //break;
            //this.Visible = false;
            //base.playerCollision(player);
        }
    }
}
