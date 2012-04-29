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
        float scrollSpeed;
        public Enemy(Texture2D textureImage, Vector2 position, Point frameSize,float scrollSpeed)
            : base(textureImage, position, frameSize, 0, Point.Zero, new Point(1, 1),Vector2.Zero)
        {
            this.scrollSpeed = scrollSpeed;
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            this.position.X -= scrollSpeed;
            base.Update(gameTime, clientBounds);
        }
    }
}
