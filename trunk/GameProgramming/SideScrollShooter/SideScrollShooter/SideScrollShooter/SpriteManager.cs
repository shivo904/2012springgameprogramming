using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;
using Microsoft.Xna.Framework.Storage;


namespace SideScrollShooter
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class SpriteManager : Microsoft.Xna.Framework.DrawableGameComponent
    {

        SpriteBatch spriteBatch;
        UserControlledSprite player;
        List<AutomatedSprite> groundTiles;
        public SpriteManager(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
            
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            //Game.Content.Load<
            
            StreamReader streamReader = new StreamReader("Content/testmap.txt");
            groundTiles = new List<AutomatedSprite>();
            String line;
            int yTile = 0;
            while((line =streamReader.ReadLine()) != null)
            {
                for (int xTile = 0; xTile < line.Length; xTile++)
                {
                    if (line[xTile]=='#')
                    {
                        groundTiles.Add(new AutomatedSprite(Game.Content.Load<Texture2D>(@"Images/Ground"), new Vector2(xTile * 25, yTile*25), new Point(25, 25), 0, new Point(0, 0), new Point(1, 1), Vector2.Zero));
                    }
                }
                yTile++;
            }
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            //player = new UserControlledSprite(Game.Content.Load<Texture2D>(@"Images/Player"), Vector2.Zero, new Point(80, 80), 0, new Point(0, 0), new Point(1, 1), new Vector2(10,10));
            player = new UserControlledSprite(Game.Content.Load<Texture2D>(@"Images/Player"), new Vector2(Game.Window.ClientBounds.Width/2,Game.Window.ClientBounds.Height-150), new Point(40, 72), 0, new Point(0, 0), new Point(1, 1), new Vector2(10,10));

            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            player.Update(gameTime, Game.Window.ClientBounds);
            collisionCheck(gameTime);


            if (checkFallBelow())
            {
                this.Enabled = false;
                //this.Visible = false;
            }
            if (checkPushLeft())
            {
                this.Enabled = false;
                //this.Visible = false; 
            }
            base.Update(gameTime);
        }

        private void collisionCheck(GameTime gameTime)
        {
            foreach (AutomatedSprite tile in groundTiles)
            {
                tile.position.X -= 3F;
                tile.Update(gameTime, Game.Window.ClientBounds);

                //check if player is on top of tile
                if (player.collisionRect.Intersects(tile.collisionRect) && player.collisionRect.Bottom >= tile.collisionRect.Top && player.collisionRect.Bottom <= tile.collisionRect.Bottom)
                {
                    player.isJumping = false;
                    player.position.Y = tile.position.Y - player.frameSize.Y;

                }

                //check if player is next to (on left of tile)
                if (player.collisionRect.Intersects(tile.collisionRect) && player.collisionRect.Right >= tile.collisionRect.Left && player.collisionRect.Right <= tile.collisionRect.Left + 5)
                {
                    player.position.X = tile.position.X - player.frameSize.X;
                }

                if (player.collisionRect.Intersects(tile.collisionRect) && player.collisionRect.Top <= tile.collisionRect.Bottom && player.collisionRect.Top > tile.collisionRect.Top)
                {
                    player.speed.Y = 0;
                    player.position.Y = tile.position.Y + tile.frameSize.Y;
                }

            }
        }
        private bool checkPushLeft()
        {
            if (player.position.X <= 0)
            {
                return true;
 
            }
            return false;
        }
        private bool checkFallBelow()
        {
            if (player.position.Y >= Game.Window.ClientBounds.Height)
            {
                return true;
            }
            return false;
        }
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            player.Draw(gameTime, spriteBatch);
            foreach (AutomatedSprite tile in groundTiles)
                tile.Draw(gameTime, spriteBatch);
            base.Draw(gameTime);
            spriteBatch.End();
        }
    }
}
