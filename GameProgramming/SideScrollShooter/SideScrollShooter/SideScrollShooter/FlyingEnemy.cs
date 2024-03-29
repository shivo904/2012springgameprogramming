﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SideScrollShooter
{
    class FlyingEnemy:Enemy
    {
        public FlyingEnemy(Texture2D textureImage, Vector2 position)
            : base(textureImage, position, new Point(90,32),new Vector2(15,0),new Point(1,4),Point.Zero,Vector2.Zero)
        {

        }


        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {

            if(!IsOutOfBounds(clientBounds))
            {
                if (position.X+collisionOffset.X >= GameController.game.spriteManager.player.position.X)
                {
                    position.X -= 3F;
                    speed.X -= .1F;
                }
                if (position.X+collisionOffset.X < GameController.game.spriteManager.player.position.X)
                {
                    position.X += 3F;
                    speed.X += .1F;
                }
               
                if (position.Y >= GameController.game.spriteManager.player.position.Y)
                {
                    position.Y -= 2F;
                    speed.Y -= .1F;
                }
                if (position.Y < GameController.game.spriteManager.player.position.Y)
                {
                    position.Y += 2F;
                    //speed.Y += .1F;
                }
            }
                





            base.Update(gameTime, clientBounds);
        }
    }
}
