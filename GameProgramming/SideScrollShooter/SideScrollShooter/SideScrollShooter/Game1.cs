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
        public Texture2D mouseImage;
        public Vector2 mousePos;
        public SpriteManager spriteManager;
        public PauseMenuManager pauseMenuManager;
        public LevelTransition levelTransition;
        public enum GameState { gamePlay, menu, startScreen,credits };
        public GameState currentState;
        Texture2D startImage;
        Texture2D winImage;
        public Random rnd { get; private set; }


        //Sounds
        AudioEngine audioEngine;
        WaveBank waveBank;
        public SoundBank soundBank;
        public Cue backgroundCue;
        public Game1()
        {
            GameController gameController = new GameController(this);
            rnd = new Random();
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1300;
            graphics.PreferredBackBufferHeight = 720;
            graphics.IsFullScreen = true;

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

            currentState = GameState.startScreen;
            

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
            winImage = Content.Load<Texture2D>(@"Images/credits");
            startImage = Content.Load<Texture2D>(@"Images/StartScreen"); audioEngine = new AudioEngine(@"Content\Audio\Sounds.xgs");
            waveBank = new WaveBank(audioEngine, @"Content\Audio\Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, @"Content\Audio\Sound Bank.xsb");
            backgroundCue = soundBank.GetCue("backgroundMusic");
            backgroundCue.Play();
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

            if (currentState != GameState.startScreen && currentState != GameState.credits)
            {
                if ((Keyboard.GetState().IsKeyDown(Keys.P) || Keyboard.GetState().IsKeyDown(Keys.Escape)) && spriteManager.Enabled == true)
                {

                    spriteManager.Enabled = false;
                    //spriteManager.Visible = false;
                    pauseMenuManager.Enabled = true;
                    pauseMenuManager.Visible = true;

                }
                if (Keyboard.GetState().IsKeyDown(Keys.B) && spriteManager.Enabled == false)
                {
                    Play();
                }
                if (Keyboard.GetState().IsKeyDown(Keys.R) && spriteManager.Visible == true)
                {
                    Restart();
                }
                // TODO: Add your update logic here
            }
            else if (currentState == GameState.startScreen)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    currentState = GameState.gamePlay;
                    Components.Add(spriteManager);
                    Components.Add(pauseMenuManager);
                    Components.Add(levelTransition);
                }
            }
            else
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {

                    Exit();
                }
            }
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
            Components.Remove(pauseMenuManager);
            spriteManager = new SpriteManager(this,spriteManager.getLevelNumber());
            //spriteManager = new SpriteManager(this,spriteManager.getLevelNumber());
            Components.Add(spriteManager);
            Components.Add(pauseMenuManager);
        }
        public void RestartGame()
        {
            pauseMenuManager.Visible = false;
            pauseMenuManager.Enabled = false;
            Components.Remove(spriteManager);
            spriteManager = new SpriteManager(this);
            //spriteManager = new SpriteManager(this,spriteManager.getLevelNumber());
            Components.Remove(pauseMenuManager);
            Components.Add(spriteManager);
            Components.Add(pauseMenuManager);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            if (currentState == GameState.startScreen)
                spriteBatch.Draw(startImage,new Rectangle(0,0,Window.ClientBounds.Width,Window.ClientBounds.Height),Color.White);
            if (currentState == GameState.credits)
                spriteBatch.Draw(winImage, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), Color.White); ;

            spriteBatch.Draw(mouseImage, mousePos, Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
