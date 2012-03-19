using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SideScrollShooter
{
    abstract class Sprite
    {
        private Texture2D textureImage;
        private Point frameSize;
        private Point currentFrame;
        private Point sheetSize;
        private int collisionOffset;
        private int timeSinceLastFrame = 0;
        private int millisecondsPerFrame;
        private const int defaultMillisecondsPerFrame = 16;
        private Vector2 speed;
        private Vector2 position;
       
        public Sprite(Texture2D textureImage,Vector2 position, Point frameSize,int collisionOffset,
            Point currentFrame, Point sheetSize,Vector2 speed)
            :this(textureImage, position,frameSize,collisionOffset,currentFrame,sheetSize,speed,defaultMillisecondsPerFrame)
        {
        }
        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset,
            Point currentFrame, Point sheetSize, Vector2 speed,int millisecondsPerFrame)
        {
            this.textureImage = textureImage;
            this.position = position;
            this.frameSize = frameSize;
            this.collisionOffset = collisionOffset;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.speed = speed;
            this.millisecondsPerFrame = millisecondsPerFrame;
        }

        public virtual void Update(GameTime gameTime, Rectangle clientBounds)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > millisecondsPerFrame)
            {
                timeSinceLastFrame = 0;
                ++currentFrame.X;
                if (currentFrame.X >= sheetSize.Y)
                {
                    currentFrame.X = 0;
                    ++currentFrame.Y;
                    if (currentFrame.Y >= frameSize.Y)
                    {
                        currentFrame.Y = 0;
                    }
                }
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureImage, position,
                new Rectangle(currentFrame.X * frameSize.X,
                                currentFrame.Y * frameSize.Y,
                                frameSize.X,frameSize.Y),
                                Color.White, 0, Vector2.Zero, 1f, 
                                SpriteEffects.None, 0);

        }

        public abstract Vector2 direction
        {
            get;
        }

        public Rectangle collisionRect
        {
            get
            {
                return new Rectangle(
                    (int)position.X + collisionOffset,
                    (int)position.Y + collisionOffset,
                    frameSize.X - (collisionOffset * 2),
                    frameSize.Y - (collisionOffset * 2));
            }
        }

        public bool IsOutOfBounds(Rectangle clientRect)
        {
            if (position.X < -frameSize.X ||
                position.X > clientRect.Width ||
                position.Y < -frameSize.Y ||
                position.Y > clientRect.Height)
            {
                return true;
            }
            else
                return false;
        }
    }
}
