﻿using System;
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
        List<AutomatedSprite> bloodList;
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
            sheetSize, speed)
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
                Vector2 inputDirection = new Vector2(continuousMovementSpeed+minorSpeed,gravity);
                
                KeyboardState keyboardState = Keyboard.GetState();
                if (keyboardState.IsKeyDown(Keys.A))
                   inputDirection.X -= .5F;
                if (keyboardState.IsKeyDown(Keys.D))
                    inputDirection.X += .2F;
                if ((keyboardState.IsKeyDown(Keys.Space) || gamepadState.IsButtonDown(Buttons.LeftStick))  && isJumping == false)
                {
                    speed.Y = -50;
                    isJumping = true;
                }

               if(isJumping==true )
               {
                   if (keyboardState.IsKeyDown(Keys.Space) || gamepadState.IsButtonDown(Buttons.LeftStick)) //chagne rate of falling based on if space is held down
                       speed.Y += 2;
                   else
                   {
                       speed.Y += 5;
                       if (speed.Y > 50)
                           speed.Y =  50;

                   }
               }


                if (gamepadState.ThumbSticks.Left.X != 0)
                    inputDirection.X += gamepadState.ThumbSticks.Left.X/5;
 


 
                previousGamePadState = gamepadState;
                previousKeyboardState = keyboardState;
                
                
                return inputDirection * speed;
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
                bloodList.Add(new Blood(bloodImage,position,bloodSpeed()));
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
            
            if (position.Y > clientBounds.Height - frameSize.Y)
            {
              
            }

            base.Update(gameTime, clientBounds);
        }
    }
}