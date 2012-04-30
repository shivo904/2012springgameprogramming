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
    public class
        SpriteManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch spriteBatch;


        public Player player;

        //Lists for updating stuff
        List<AutomatedSprite> bulletList;
        List<AutomatedSprite> bloodList;
        List<Block> blocks;
        List<Enemy> enemies;
        MouseState prevMouseState;
        GamePadState prevGamePadState;

        //Images
        Texture2D blank;
        Texture2D bulletImage;
        Texture2D bloodImage;
        Texture2D blockImage;
        Texture2D spikeImage;
        Texture2D winImage;
        Texture2D destructableImage;
        Texture2D teleportImage;
        Texture2D flyerImage;


        //Settings
        public float scrollSpeed = 3F;
        private int levelNumber = 1;


        public SpriteManager(Game game)
            : base(game)
        {
            // TODO: Construct any child components here

        }
        public SpriteManager(Game game, int levelNumber)
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
            blocks = new List<Block>();
            enemies = new List<Enemy>();
            bulletList = new List<AutomatedSprite>();
            bloodList = new List<AutomatedSprite>();

            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            //player = new UserControlledSprite(Game.Content.Load<Texture2D>(@"Images/Player"), Vector2.Zero, new Point(80, 80), 0, new Point(0, 0), new Point(1, 1), new Vector2(10,10));

            flyerImage = Game.Content.Load<Texture2D>(@"Images/Monster");
            bulletImage = Game.Content.Load<Texture2D>(@"Images/bullet");
            bloodImage = Game.Content.Load<Texture2D>(@"Images/Blood");
            blockImage = Game.Content.Load<Texture2D>(@"Images/Ground");
            spikeImage = Game.Content.Load<Texture2D>(@"Images/Spike");
            winImage = Game.Content.Load<Texture2D>(@"Images/Win");
            destructableImage = Game.Content.Load<Texture2D>(@"Images/Destructable");
            teleportImage = Game.Content.Load<Texture2D>(@"Images/Teleport");

            player = new Player(Game.Content.Load<Texture2D>(@"Images/Player"), bloodImage, new Vector2(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height - 150), new Point(19, 30), Vector2.Zero, new Point(0, 0), new Point(1, 1), new Vector2(10, 10));
            blank = new Texture2D(Game.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { Color.White });
            SetLevel(levelNumber);
            base.LoadContent();
        }

        public void NextLevel()
        {
            levelNumber++;
            SetLevel(levelNumber);
        }
        public void SetLevel(int levelNumber)
        {
            WinBlock.hit = false;
            player.dead = false;
            blocks = new List<Block>();
            enemies = new List<Enemy>();
            this.levelNumber = levelNumber;

            StreamReader streamReader = new StreamReader("Content/testmap" + levelNumber + ".txt");
            String line;
            int yTile = 0;


            //converts the symbols into tiles for the level
            while ((line = streamReader.ReadLine()) != null)
            {
                for (int xTile = 0; xTile < line.Length; xTile++)
                {
                    
                    if (line[xTile] == '#')
                        blocks.Add(new GroundBlock(blockImage, new Vector2(xTile * 25, yTile * 25), scrollSpeed));
                    else if (line[xTile] == '^')
                        blocks.Add(new Spike(spikeImage, new Vector2(xTile * 25, yTile * 25), scrollSpeed));
                    else if (line[xTile] == '!')
                        blocks.Add(new WinBlock(winImage, new Vector2(xTile * 25, yTile * 25), scrollSpeed));
                    else if (line[xTile] == 'x')
                        blocks.Add(new DestructableBlock(destructableImage, new Vector2(xTile * 25, yTile * 25), scrollSpeed));
                    else if (line[xTile] == '?')
                        blocks.Add(new TeleportBlock(teleportImage, new Vector2(xTile * 25, yTile * 25), scrollSpeed));
                    else if (line[xTile] == 'Q')
                        enemies.Add(new FlyingEnemy(flyerImage, new Vector2(xTile * 25, yTile * 25), scrollSpeed));
                        

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
            for (int i = 0; i < enemies.Count;i++)
            {
                enemies[i].Update(gameTime, Game.Window.ClientBounds);
                if (enemies[i].collisionRect.Intersects(player.collisionRect))
                {
                    player.dead = true;
                }

                if (bulletList.Count != 0)
                {
                    for (int j = 0; j < bulletList.Count; j++)
                    {

                        if (bulletList[j].collisionRect.Intersects(enemies[i].collisionRect))
                        {
                            bulletList.RemoveAt(j);
                            enemies.RemoveAt(i);
                        }
                    }
                }

            }
            foreach (Block block in blocks)
            {
                block.Update(gameTime, Game.Window.ClientBounds);
                if (block.collisionRect.Intersects(player.collisionRect))
                {
                    block.playerCollision(player);
                }
                for (int i=0;i<bulletList.Count;i++)
                {
                    if(block.collisionRect.Intersects(bulletList[i].collisionRect))
                    {
                        
                        block.bulletCollision(bulletList[i]);
                        bulletList.RemoveAt(i);
                    }
                }
            }
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
            for(int i=0;i<bulletList.Count;i++)
            {
                bulletList[i].Update(gameTime, Game.Window.ClientBounds);
                if (bulletList[i].IsOutOfBounds(Game.Window.ClientBounds))
                {
                    bulletList.RemoveAt(i);
                }
            }
            MouseState curMouseState = Mouse.GetState();
            if (curMouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton != curMouseState.LeftButton)
            {
                Vector2 bulletSpeed = calcBulletSpeed(curMouseState);
                bulletList.Add(new AutomatedSprite(bulletImage, new Vector2(player.position.X+19,player.position.Y+15), new Point(5, 5), Vector2.Zero, Point.Zero, new Point(1, 1), bulletSpeed));
            }

            GamePadState curGamePadState = GamePad.GetState(PlayerIndex.One);
            if (curGamePadState.IsButtonDown(Buttons.RightStick) && prevGamePadState.IsButtonDown(Buttons.RightStick) != curGamePadState.IsButtonDown(Buttons.RightStick))
            {
                
                Vector2 bulletSpeed = calcBulletSpeedController(curGamePadState);
                bulletList.Add(new AutomatedSprite(bulletImage, player.position, new Point(5, 5), Vector2.Zero, Point.Zero, new Point(1, 1), bulletSpeed));
            }



            prevGamePadState = curGamePadState;
            prevMouseState = curMouseState;
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

        private Vector2 calcBulletSpeedController(GamePadState curGamePadState)
        {


            double speed = 10F;
            //Vector2 gamePadPosition = new Vector2((curMouseState.X - player.position.X) / (curMouseState.X - player.position.X), (curMouseState.Y - player.position.Y) / (curMouseState.X - player.position.X));
            Vector2 gamePadPosition = curGamePadState.ThumbSticks.Right;
            double angle = Math.Atan(-gamePadPosition.Y / gamePadPosition.X);


            Vector2 finalSpeed = new Vector2((float)(speed * Math.Cos(angle)), (float)(speed * Math.Sin(angle)));
            if (curGamePadState.ThumbSticks.Right.X < 0)
            {
                finalSpeed*=-1;
            }
            return finalSpeed;
        }
        private Vector2 calcBulletSpeed(MouseState curMouseState)
        {


            double speed = 10F;
            Vector2 mousePosition = new Vector2((curMouseState.X - player.position.X) / (curMouseState.X - player.position.X), (curMouseState.Y - player.position.Y) / (curMouseState.X - player.position.X));
            double angle = Math.Atan(mousePosition.Y / mousePosition.X);


            Vector2 finalSpeed = new Vector2((float)(speed*Math.Cos(angle)), (float)(speed*Math.Sin(angle)));
            if (curMouseState.X < player.position.X)
                finalSpeed *= -1;
            return finalSpeed;
        }

        
        
        private void deathBlood(GameTime gameTime)
        {
            foreach (AutomatedSprite blood in bloodList)
                blood.Update(gameTime, Game.Window.ClientBounds);
            
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
            foreach (Block block in blocks)
                block.Draw(gameTime, spriteBatch);
            foreach (AutomatedSprite bullet in bulletList)
                bullet.Draw(gameTime, spriteBatch);
            foreach (AutomatedSprite blood in bloodList)
                blood.Draw(gameTime, spriteBatch);
            foreach (Enemy enemy in enemies)
                enemy.Draw(gameTime, spriteBatch);


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
