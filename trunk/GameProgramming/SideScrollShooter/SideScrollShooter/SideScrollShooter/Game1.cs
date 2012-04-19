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
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D mouseImage;
        Vector2 mousePos;
        public SpriteManager spriteManager;
        public PauseMenuManager pauseMenuManager;
        public LevelTransition levelTransition;
        public enum GameState { gamePlay, menu, startScreen };
        public Random rnd { get; private set; }
        public Game1()
        {
            rnd = new Random();
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            //graphics.IsFullScreen = true;

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            spriteManager = new SpriteManager(this);
            pauseMenuManager = new PauseMenuManager(this);
            levelTransition = new LevelTransition(this);
            IsMouseVisible = false;
            pauseMenuManager.Visible = false;
            pauseMenuManager.Enabled = false;
            levelTransition.Visible = false;
            levelTransition.Enabled = false;
            Components.Add(spriteManager);
            Components.Add(pauseMenuManager);
            Components.Add(levelTransition);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            mouseImage = Content.Load<Texture2D>(@"Images/crossair");

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            mousePos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.P)&& spriteManager.Enabled==true)
            {
                spriteManager.Enabled = false;
                //spriteManager.Visible = false;
                pauseMenuManager.Enabled = true;
                pauseMenuManager.Visible = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.B) && spriteManager.Enabled==false)
            {
                Play();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                Restart();
            }
            // TODO: Add your update logic here

            base.Update(gameTime);
        }
        public void Play()
        {
            spriteManager.Enabled = true;
            spriteManager.Visible = true;
            pauseMenuManager.Enabled = false;
            pauseMenuManager.Visible = false;
        }
        public void Restart()
        {
            pauseMenuManager.Visible = false;
            pauseMenuManager.Enabled = false;
            Components.Remove(spriteManager);
            spriteManager = new SpriteManager(this,spriteManager.getLevelNumber());
            //spriteManager = new SpriteManager(this,spriteManager.getLevelNumber());
            Components.Add(spriteManager);
        }
        public void RestartGame()
        {
            pauseMenuManager.Visible = false;
            pauseMenuManager.Enabled = false;
            Components.Remove(spriteManager);
            spriteManager = new SpriteManager(this);
            //spriteManager = new SpriteManager(this,spriteManager.getLevelNumber());
            Components.Add(spriteManager);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.Draw(mouseImage, mousePos, Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
