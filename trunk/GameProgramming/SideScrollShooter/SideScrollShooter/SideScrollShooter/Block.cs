using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SideScrollShooter
{
    abstract class Block:AutomatedSprite
    {

        public Block(Texture2D textureImage,Vector2 position)
            :this(textureImage,position,new Point(1,1))
        {
        }

        public Block(Texture2D textureImage, Vector2 position, Point sheetSize)
            : this(textureImage, position, new Point(30, 30), Vector2.Zero, new Point(1, 1), sheetSize, Vector2.Zero)
        {
        }

        public Block(Texture2D textureImage, Vector2 position, Point frameSize, Vector2 collisionOffset,
             Point currentFrame, Point sheetSize, Vector2 speed)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed)
        {
        }


        public Block(Texture2D textureImage, Vector2 position, Point frameSize, Vector2 collisionOffset,
            Point currentFrame, Point sheetSize, Vector2 speed, int millisecondsPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed, millisecondsPerFrame)
        {
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            position.X -= GameController.game.spriteManager.scrollSpeed;
            base.Update(gameTime, clientBounds);
        }
        virtual public void bulletCollision(AutomatedSprite bullet)
        {
        }
        virtual public void playerCollision(Player player)
        {
            //check if player is on top of tile
            if (player.collisionRect.Bottom > collisionRect.Top && player.collisionRect.Bottom < collisionRect.Bottom)
            {
                player.isJumping = false;
                player.position.Y = position.Y - player.frameSize.Y;

            }
            else if (player.collisionRect.Top < collisionRect.Top && player.collisionRect.Top > collisionRect.Bottom)
            {
                //player hits bottom
                player.speed.Y = 0;
                player.position.Y = position.Y + frameSize.Y;
                player.isJumping = true; 
            }
            else if (player.collisionRect.Right > collisionRect.Left && player.collisionRect.Right < collisionRect.Right)
            {
                //player is on left of tile
               //ameController.game.Exit();
                if(!player.dead)
                player.currentFrame.X = 3;
                player.position.X = position.X - player.frameSize.X;
                player.isJumping = true; 
            }
            else if(player.collisionRect.Left< collisionRect.Right && player.collisionRect.Left > collisionRect.Left)
            {
                //player is to the right
                if(!player.dead)
                player.currentFrame.X = 3;
                player.position.X =position.X+frameSize.X;
                player.isJumping = true; 
            }
            else
            {
                player.isJumping = true; 
            }
            //check if player is next to (on left of tile)
            
      

        }
    }
}
