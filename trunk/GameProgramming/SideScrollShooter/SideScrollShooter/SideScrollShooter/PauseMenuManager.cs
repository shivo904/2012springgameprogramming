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
        int menuPosition;
        SpriteBatch spriteBatch;
        AutomatedSprite play;
        AutomatedSprite quit;
        AutomatedSprite restart;
        AutomatedSprite restartGame;
        List<AutomatedSprite> menu;
        KeyboardState prevKeyboardState;
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
          //  Game.IsMouseVisible = true;
            base.Initialize();
        }


        protected override void LoadContent()
        {
            menuPosition = 0;
            menu = new List<AutomatedSprite>();
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            play = new AutomatedSprite(Game.Content.Load<Texture2D>(@"Images/Continue"),new Vector2(Game.Window.ClientBounds.Width/2,0),new Point(200,50),Vector2.Zero,Point.Zero,new Point(1,1),new Vector2(0,0));
            restart = new AutomatedSprite(Game.Content.Load<Texture2D>(@"Images/RestartLevel"), new Vector2(Game.Window.ClientBounds.Width / 2, 60), new Point(200, 50), Vector2.Zero, Point.Zero, new Point(1, 1), Vector2.Zero);
            restartGame = new AutomatedSprite(Game.Content.Load<Texture2D>(@"Images/Restart"), new Vector2(Game.Window.ClientBounds.Width / 2, 120), new Point(200, 50), Vector2.Zero, Point.Zero, new Point(1, 1), new Vector2(0, 0));
            quit = new AutomatedSprite(Game.Content.Load<Texture2D>(@"Images/Quit"), new Vector2(Game.Window.ClientBounds.Width / 2, 180), new Point(200, 50), Vector2.Zero, Point.Zero, new Point(1, 1), new Vector2(0, 0));
            menu.Add(play);
            menu.Add(restart);
            menu.Add(restartGame);
            menu.Add(quit);
            base.LoadContent();
        }
        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            KeyboardState keyboardState = Keyboard.GetState();
            foreach(AutomatedSprite item in menu)
            {
                item.Update(gameTime, Game.Window.ClientBounds);
                if (keyboardState.IsKeyDown(Keys.Down) && menuPosition<menu.Count-1 && keyboardState!=prevKeyboardState)
                {
                    menuPosition++;
                }
                if (keyboardState.IsKeyDown(Keys.Up) && menuPosition >0 && keyboardState!=prevKeyboardState)
                {
                    menuPosition--;
                }

                if (item.collisionRect.Intersects(new Rectangle(mouseState.X, mouseState.Y, 1, 1)))
                {
                    menuPosition = menu.IndexOf(item);
                }
                prevKeyboardState = keyboardState;
            }
            menu[menuPosition].currentFrame.X = 1;

            if (keyboardState.IsKeyDown(Keys.Enter) || mouseState.LeftButton == ButtonState.Pressed)
            {
                if(menu[menuPosition]==play)
                {
                    ((Game1)Game).Play();
                }
                if(menu[menuPosition]==restart) 
                {
                    ((Game1)Game).Restart();
                    menuPosition = 0;
                }
                if (menu[menuPosition] == restartGame)
                {
                    ((Game1)Game).RestartGame();
                }
                if(menu[menuPosition]==quit)
                {
                    Game.Exit();
                }
            }

            // TODO: Add your update code here
            /*
            play.Update(gameTime, Game.Window.ClientBounds);
            restart.Update(gameTime,Game.Window.ClientBounds);
            quit.Update(gameTime, Game.Window.ClientBounds);
  

            if(play.collisionRect.Intersects(new Rectangle(mouseState.X,mouseState.Y,1,1)))
            {
                play.currentFrame.X = 1;
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    //Game.IsMouseVisible = false;
                    Visible = false;
                    Enabled = false;
                    ((Game1)Game).spriteManager.Visible = true;
                    ((Game1)Game).spriteManager.Enabled = true;
                }

            }
            if (restart.collisionRect.Intersects(new Rectangle(mouseState.X, mouseState.Y, 1, 1)))
            {
                restart.currentFrame.X = 1;
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    ((Game1)Game).Restart();
                }
            }
            if (quit.collisionRect.Intersects(new Rectangle(mouseState.X, mouseState.Y, 1, 1)))
            {
                quit.currentFrame.X = 1;
                if(mouseState.LeftButton == ButtonState.Pressed)
                 Game.Exit();
            }
            */
            base.Update(gameTime);
        }

        protected override void OnEnabledChanged(object sender, EventArgs args)
        {
            //if (Enabled)
               // Game.IsMouseVisible = true;
            if (!Enabled)
                Game.IsMouseVisible = false;
            base.OnEnabledChanged(sender, args);
        }
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            play.Draw(gameTime,spriteBatch);
            restart.Draw(gameTime, spriteBatch);
            restartGame.Draw(gameTime, spriteBatch);
            quit.Draw(gameTime,spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
