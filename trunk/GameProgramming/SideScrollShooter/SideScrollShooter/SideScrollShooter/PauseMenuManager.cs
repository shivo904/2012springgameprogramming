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
    public class PauseMenuManager: Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        AutomatedSprite play;
        AutomatedSprite quit;
        public PauseMenuManager(Game game)
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
            Game.IsMouseVisible = true;
            base.Initialize();
        }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            play = new AutomatedSprite(Game.Content.Load<Texture2D>(@"Images/Continue"),new Vector2(Game.Window.ClientBounds.Width/2,0),new Point(200,50),0,Point.Zero,new Point(1,1),new Vector2(0,0));
            quit = new AutomatedSprite(Game.Content.Load<Texture2D>(@"Images/Quit"), new Vector2(Game.Window.ClientBounds.Width / 2, 50), new Point(200, 50), 0, Point.Zero, new Point(1, 1), new Vector2(0, 0));
            base.LoadContent();
        }
        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            play.Update(gameTime, Game.Window.ClientBounds);
            quit.Update(gameTime, Game.Window.ClientBounds);
            MouseState mouseState = Mouse.GetState();

            if(play.collisionRect.Intersects(new Rectangle(mouseState.X,mouseState.Y,1,1)) && mouseState.LeftButton == ButtonState.Pressed)
            {
                //Game.IsMouseVisible = false;
                Visible = false;
                Enabled = false;
                ((Game1)Game).spriteManager.Visible = true;
                ((Game1)Game).spriteManager.Enabled = true;

            }
            if (quit.collisionRect.Intersects(new Rectangle(mouseState.X, mouseState.Y, 1, 1)) && mouseState.LeftButton == ButtonState.Pressed)
            {
                Game.Exit();
            }
            base.Update(gameTime);
        }
        protected override void OnEnabledChanged(object sender, EventArgs args)
        {
            if (Enabled)
                Game.IsMouseVisible = true;
            if (!Enabled)
                Game.IsMouseVisible = false;
            base.OnEnabledChanged(sender, args);
        }
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            play.Draw(gameTime,spriteBatch);
            quit.Draw(gameTime,spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
