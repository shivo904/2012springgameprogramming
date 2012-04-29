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
        protected float scrollSpeed;
        public Block(Texture2D textureImage,Vector2 position, float scrollSpeed)
            :this(textureImage,position,new Point(1,1),scrollSpeed)
        {
        }

        public Block(Texture2D textureImage, Vector2 position, Point sheetSize, float scrollSpeed)
            :this(textureImage,position,new Point(30,30),0,new Point(1,1),sheetSize,Vector2.Zero,scrollSpeed)
        {
        }

        public Block(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset,
             Point currentFrame, Point sheetSize, Vector2 speed,float scrollSpeed)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed)
        {
            this.scrollSpeed = scrollSpeed;
        }


        public Block(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset,
            Point currentFrame, Point sheetSize, Vector2 speed, int millisecondsPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed, millisecondsPerFrame)
        {
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            position.X -= scrollSpeed;
            base.Update(gameTime, clientBounds);
        }
        virtual public void bulletCollision(AutomatedSprite bullet)
        {
        }

        virtual public void playerCollision(Player player)
        {
            //check if player is on top of tile
            if (player.collisionRect.Bottom >= collisionRect.Top && player.collisionRect.Bottom <= collisionRect.Bottom)
            {
                player.isJumping = false;
                player.position.Y = position.Y - player.frameSize.Y;

            }

            //check if player is next to (on left of tile)
            if (player.collisionRect.Right >= collisionRect.Left && player.collisionRect.Right <= collisionRect.Left + 5)
            {
                player.position.X = position.X - player.frameSize.X;
            }

            if (player.collisionRect.Top <= collisionRect.Bottom && player.collisionRect.Top > collisionRect.Top)
            {
                player.speed.Y = 0;
                player.position.Y = position.Y + frameSize.Y;
            }
        }
    }
}
