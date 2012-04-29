using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SideScrollShooter
{
    class FlyingEnemy:Enemy
    {
        int aggroDistance = 300;
        public FlyingEnemy(Texture2D textureImage, Vector2 position, float scrollSpeed)
            : base(textureImage, position, new Point(30,30),scrollSpeed)
        {
        }


        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {

            if (position.X - aggroDistance < GameController.game.spriteManager.player.position.X)
            {
                speed.X -= .1F;
            }
            if (position.Y - aggroDistance < GameController.game.spriteManager.player.position.Y)
            {
                speed.Y += .1F;
            }
            base.Update(gameTime, clientBounds);
        }
    }
}
