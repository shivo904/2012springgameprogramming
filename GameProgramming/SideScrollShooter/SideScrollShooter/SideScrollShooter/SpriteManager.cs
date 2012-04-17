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

        Texture2D blank;
        private int levelNumber = 1;
        SpriteBatch spriteBatch;
        UserControlledSprite player;
        List<AutomatedSprite> bulletList;
        List<AutomatedSprite> groundTiles;
        List<AutomatedSprite> spikeTiles;
        List<AutomatedSprite> winTiles;
        MouseState prevMouseState;
        Texture2D bulletImage;
        public SpriteManager(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
            
        }
        public SpriteManager(Game game,int levelNumber)
            : base(game)
        {
            this.levelNumber = levelNumber;
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


            bulletList = new List<AutomatedSprite>();


            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            //player = new UserControlledSprite(Game.Content.Load<Texture2D>(@"Images/Player"), Vector2.Zero, new Point(80, 80), 0, new Point(0, 0), new Point(1, 1), new Vector2(10,10));
            player = new UserControlledSprite(Game.Content.Load<Texture2D>(@"Images/Player"), new Vector2(Game.Window.ClientBounds.Width/2,Game.Window.ClientBounds.Height-150), new Point(19, 30), 0, new Point(0, 0), new Point(1, 1), new Vector2(10,10));
            bulletImage = Game.Content.Load<Texture2D>(@"Images/bullet");
            SetLevel(levelNumber);

        blank = new Texture2D(Game.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
        blank.SetData(new[]{Color.White});
            base.LoadContent();
        }


        public void SetLevel(int levelNumber)
        {
            this.levelNumber=levelNumber;
            StreamReader streamReader = new StreamReader("Content/testmap" + levelNumber + ".txt");
            String line;
            int yTile = 0;
            groundTiles = new List<AutomatedSprite>();
            spikeTiles = new List<AutomatedSprite>();
            winTiles = new List<AutomatedSprite>();

            //converts the symbols into tiles for the level
            while ((line = streamReader.ReadLine()) != null)
            {
                for (int xTile = 0; xTile < line.Length; xTile++)
                {
                    if (line[xTile] == '#')
                    {
                        groundTiles.Add(new AutomatedSprite(Game.Content.Load<Texture2D>(@"Images/Ground"), new Vector2(xTile * 25, yTile * 25), new Point(25, 25), 0, new Point(0, 0), new Point(1, 1), Vector2.Zero));
                    }
                    if (line[xTile] == '^')
                    {
                        spikeTiles.Add(new AutomatedSprite(Game.Content.Load<Texture2D>(@"Images/Spike"), new Vector2(xTile * 25, yTile * 25), new Point(25, 25), 0, new Point(0, 0), new Point(1, 1), Vector2.Zero));
                    }
                    if (line[xTile] == '!')
                    {
                        winTiles.Add(new AutomatedSprite(Game.Content.Load<Texture2D>(@"Images/Win"), new Vector2(xTile * 25, yTile * 25), new Point(25, 25), 0, new Point(0, 0), new Point(1, 1), Vector2.Zero));
                    }

                }
                yTile++;
            }
        }

        public void NextLevel()
        {
            levelNumber++;
            StreamReader streamReader = new StreamReader("Content/testmap"+levelNumber+".txt");
            String line;
            int yTile = 0;
            groundTiles = new List<AutomatedSprite>();
            spikeTiles = new List<AutomatedSprite>();
            winTiles = new List<AutomatedSprite>();
            
            //converts the symbols into tiles for the level
            while ((line = streamReader.ReadLine()) != null)
            {
                for (int xTile = 0; xTile < line.Length; xTile++)
                {
                    if (line[xTile] == '#')
                    {
                        groundTiles.Add(new AutomatedSprite(Game.Content.Load<Texture2D>(@"Images/Ground"), new Vector2(xTile * 25, yTile * 25), new Point(25, 25), 0, new Point(0, 0), new Point(1, 1), Vector2.Zero));
                    }
                    if (line[xTile] == '^')
                    {
                        spikeTiles.Add(new AutomatedSprite(Game.Content.Load<Texture2D>(@"Images/Spike"), new Vector2(xTile * 25, yTile * 25), new Point(25, 25), 0, new Point(0, 0), new Point(1, 1), Vector2.Zero));
                    }
                    if (line[xTile] == '!')
                    {
                        winTiles.Add(new AutomatedSprite(Game.Content.Load<Texture2D>(@"Images/Win"), new Vector2(xTile * 25, yTile * 25), new Point(25, 25), 0, new Point(0, 0), new Point(1, 1), Vector2.Zero));
                    }

                }
                yTile++;
            }
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
            Shooting(gameTime);
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

        private void Shooting(GameTime gameTime)
        {
            foreach (AutomatedSprite bullet in bulletList)
                bullet.Update(gameTime, Game.Window.ClientBounds);
            MouseState curMouseState = Mouse.GetState();
            if (curMouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton!=curMouseState.LeftButton)
            {
                Vector2 bulletSpeed = calcBulletSpeed(curMouseState);
                bulletList.Add(new AutomatedSprite(bulletImage, player.position, new Point(5, 5), 0, Point.Zero, new Point(1, 1), bulletSpeed));
            }
            
            prevMouseState=curMouseState;
        }
        void DrawLine(SpriteBatch batch, Texture2D blank,
              float width, Color color, Vector2 point1, Vector2 point2)
        {
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);

            batch.Draw(blank, point1, null, color, 
                       angle, Vector2.Zero, new Vector2(length, width),
                       SpriteEffects.None, 0);
        }
        private Vector2 calcBulletSpeed(MouseState curMouseState)
        {
            Vector2 bulletSpeed;
            bulletSpeed = new Vector2((curMouseState.X - player.position.X) / curMouseState.X, (curMouseState.Y - player.position.Y) / curMouseState.X);
            
            return bulletSpeed*20;
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

            foreach (AutomatedSprite spike in spikeTiles)
            {
                spike.position.X -= 3F;
                spike.Update(gameTime, Game.Window.ClientBounds);

                //check if player touches a spike
                if (player.collisionRect.Intersects(spike.collisionRect))
                {
                    this.Enabled = false;
                    //this.Visible = false;
                }

            }

            foreach (AutomatedSprite winT in winTiles)
            {
                winT.position.X -= 3F;
                winT.Update(gameTime, Game.Window.ClientBounds);

                //check if player touches a spike
                if (player.collisionRect.Intersects(winT.collisionRect))
                {
                    //winTiles.Clear();
                    Visible = false;
                    Enabled = false;
                    ((Game1)Game).levelTransition.Visible = true;
                    ((Game1)Game).levelTransition.Enabled = true;
                    break;
                    //this.Visible = false;
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
            DrawLine(spriteBatch, blank, 2, Color.Black, player.position, new Vector2(Mouse.GetState().X, Mouse.GetState().Y));
            player.Draw(gameTime, spriteBatch);
            foreach (AutomatedSprite tile in groundTiles)
                tile.Draw(gameTime, spriteBatch);
            foreach (AutomatedSprite spike in spikeTiles)
                spike.Draw(gameTime, spriteBatch);
            foreach (AutomatedSprite winT in winTiles)
                winT.Draw(gameTime, spriteBatch);
            foreach (AutomatedSprite bullet in bulletList)
                bullet.Draw(gameTime, spriteBatch);


            base.Draw(gameTime);
            spriteBatch.End();
        }
        public int getLevelNumber()
        {
            return levelNumber;
        }
        public void setLevelNumber(int levelNumber)
        {
            this.levelNumber = levelNumber;
        }

    }
}
