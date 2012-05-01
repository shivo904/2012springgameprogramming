using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SideScrollShooter
{
    public class Player : Sprite
    {
        public bool dead;
        public List<AutomatedSprite> bloodList;
        private KeyboardState previousKeyboardState;
        private GamePadState previousGamePadState;
        private int continuousMovementSpeed = 0;
        private float minorSpeed = .05F;
        private float gravity = .2F;
        public bool isJumping = true;
        public bool isDucking = false;
        Vector2 bloodDirection;
        Texture2D bloodImage;
        public Player(Texture2D textureImage,Texture2D bloodImage, Vector2 position,
                   Point frameSize, Vector2 collisionOffset, Point currentFrame, Point sheetSize,
                   Vector2 speed)
            :base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed,100)
        {
            bloodList = new List<AutomatedSprite>();
            this.bloodImage = bloodImage;
            dead = false;
        }


        public override Vector2 direction
        {
            get
            {
       
                    GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
                    Vector2 inputDirection = new Vector2(continuousMovementSpeed + minorSpeed, gravity);

                    KeyboardState keyboardState = Keyboard.GetState();
                    if (keyboardState.IsKeyDown(Keys.A))
                    {
                        inputDirection.X -= .4F;
                        millisecondsPerFrame = 200;
                    }
                    if (keyboardState.IsKeyDown(Keys.D))
                    {
                        inputDirection.X += .2F;
                        millisecondsPerFrame = 75;
                    }
                    if ((!keyboardState.IsKeyDown(Keys.D) && !keyboardState.IsKeyDown(Keys.A)))
                    {
                        millisecondsPerFrame = 100;
                    }
                    if ((keyboardState.IsKeyDown(Keys.Space) || gamepadState.IsButtonDown(Buttons.LeftStick)) && isJumping == false && !dead)
                    {
                        speed.Y = -50;
                        isJumping = true;
                    }

                    if (isJumping == true)
                    {
                        if (!dead && (keyboardState.IsKeyDown(Keys.Space) || gamepadState.IsButtonDown(Buttons.LeftStick))) //chagne rate of falling based on if space is held down
                            speed.Y += 2;
                        else
                        {
                            speed.Y += 5;
                            if (speed.Y > 50)
                                speed.Y = 50;

                        }
                    }


                    if (gamepadState.ThumbSticks.Left.X != 0)
                        inputDirection.X += gamepadState.ThumbSticks.Left.X / 5;




                    previousGamePadState = gamepadState;
                    previousKeyboardState = keyboardState;

                    if(!dead)
                        return inputDirection * speed;
                    else
                      return new Vector2(0,(inputDirection.Y*speed.Y));
                }

            

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (AutomatedSprite blood in bloodList)
            {
                blood.Draw(gameTime, spriteBatch);
            }
            base.Draw(gameTime, spriteBatch);
        }


        
        public Vector2 bloodSpeed()
        {
            int ySpeed = GameController.game.rnd.Next(-5,5);       
            int xSpeed = GameController.game.rnd.Next(-5,5);
            bloodDirection = new Vector2(xSpeed, ySpeed);
            return bloodDirection;
            
        }
        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            foreach (AutomatedSprite blood in bloodList)
            {
                blood.speed.Y += .1F;
                blood.Update(gameTime, clientBounds);
                
            }
            if (dead)
            {
                bloodList.Add(new Blood(bloodImage,new Vector2(position.X+13,position.Y+10),bloodSpeed()));
            }
            position += direction;

            if (position.Y < 0)
                position.Y = 0;
            if (position.X < clientBounds.Width / 2)
                minorSpeed = .05F;
            if (position.X > clientBounds.Width / 2)
                minorSpeed = -.05F;
            if (position.X == clientBounds.Width / 2)
                minorSpeed = 0F;
            if (position.X+frameSize.X > clientBounds.Width)
            {
                position.X = clientBounds.Width-frameSize.X;
            }
            
            if (position.Y > clientBounds.Height - frameSize.Y)
            {
              
            }
            if (!dead)
                base.Update(gameTime, clientBounds);
        }

        public void kill()
        {

            if (!dead)
            {
                GameController.game.soundBank.PlayCue("hit");
                GameController.game.spriteManager.scrollSpeed = 0;
                GameController.game.spriteManager.backgroundScrollSpeed = Vector2.Zero;
                currentFrame = new Point(0, 1);
                sheetSize = new Point(1, 1);
                frameSize = new Point(32, frameSize.Y);
                currentFrame = new Point(0, 1);
                GameController.game.pauseMenuManager.Enabled = true;
                GameController.game.pauseMenuManager.Visible = true;


            }

            dead = true;

            //position = new Vector2(position.X - GameController.game.spriteManager.scrollSpeed, position.Y);
           


        }
    }
}
