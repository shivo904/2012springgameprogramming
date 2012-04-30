using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SideScrollShooter
{
    class Enemy:AutomatedSprite
    {
        int aggroOffset=300;
        public Enemy(Texture2D textureImage, Vector2 position, Point frameSize)
            : this(textureImage, position, frameSize, Vector2.Zero, Point.Zero, new Point(1, 1),Vector2.Zero)
        {
            
        }
        public Enemy(Texture2D textureImage, Vector2 position, Point frameSize,Vector2 collisionOffset,Point sheetSize,Point currentFrame,Vector2 speed)
            : base(textureImage,position,frameSize,collisionOffset,currentFrame,sheetSize,speed)
        {
        }



        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            this.position.X -= GameController.game.spriteManager.scrollSpeed;
            base.Update(gameTime, clientBounds);
        }

        public Rectangle aggroRect
        {
            get
            {
                return new Rectangle((int)position.X - aggroOffset, (int)position.Y - aggroOffset, 2*aggroOffset + frameSize.X, 2*aggroOffset + frameSize.Y);
            }
        }

    }
}
