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
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            //player = new UserControlledSprite(Game.Content.Load<Texture2D>(@"Images/Player"), Vector2.Zero, new Point(80, 80), 0, new Point(0, 0), new Point(1, 1), new Vector2(10,10));
            player = new UserControlledSprite(Game.Content.Load<Texture2D>(@"Images/Player"), new Vector2(0,Game.Window.ClientBounds.Height-72), new Point(40, 72), 0, new Point(0, 0), new Point(1, 1), new Vector2(10,10));
            groundTiles = new List<AutomatedSprite>();
            for (int i = 0; i < Game.Window.ClientBounds.Width/2; i += 25)
            {
                groundTiles.Add(new AutomatedSprite(Game.Content.Load<Texture2D>(@"Images/Ground"), new Vector2(i, Game.Window.ClientBounds.Height - 25), new Point(25, 25), 0, new Point(0, 0), new Point(1, 1), Vector2.Zero));
            }
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
            foreach (AutomatedSprite tile in groundTiles)
            {
                tile.position.X -= 1F;
                tile.Update(gameTime, Game.Window.ClientBounds);
                
                if (player.collisionRect.Intersects(tile.collisionRect))
                {
                    player.position.Y = tile.position.Y - player.frameSize.Y;
                    player.isJumping = false;
                }

                    
            }
            base.Update(gameTime);
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
