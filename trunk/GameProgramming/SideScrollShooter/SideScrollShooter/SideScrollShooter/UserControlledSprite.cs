﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SideScrollShooter
{
    class UserControlledSprite : Sprite
    {
        private KeyboardState previousKeyboardState;
        private GamePadState previousGamePadState;
        private int continuousMovementSpeed = 0;
        private float gravity = .2F;
        public bool isJumping = true;
        public bool isDucking = false;
        public UserControlledSprite(Texture2D textureImage, Vector2 position,
                   Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize,
                   Vector2 speed)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed)
        {
        }

        public UserControlledSprite(Texture2D textureImage, Vector2 position,
            Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize,
            Vector2 speed, int millisecondsPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, millisecondsPerFrame)
        {
        }

        public override Vector2 direction
        {
            get
            {
                Vector2 inputDirection = new Vector2(continuousMovementSpeed+.05F,gravity);
                
                KeyboardState keyboardState = Keyboard.GetState();
//                if (keyboardState.IsKeyDown(Keys.Left))
  //                  inputDirection.X -= 1;
    //            if (keyboardState.IsKeyDown(Keys.Right))
      //              inputDirection.X += 1;
                if (keyboardState.IsKeyDown(Keys.Space)  && isJumping == false)
                {
                    speed.Y = -50;
                    isJumping = true;
                }
                /*
                if (!isJumping && isDucking==false&& keyboardState.IsKeyDown(Keys.LeftShift))
                {
                    isDucking = true;
                    frameSize.Y /= 2;
                }
                if (isDucking && keyboardState.IsKeyUp(Keys.LeftShift))
                {
                    frameSize.Y *= 2;
                    isDucking = false;
                }
                 */
               if(isJumping==true )
               {
                   if(keyboardState.IsKeyDown(Keys.Space)) //chagne rate of falling based on if space is held down
                       speed.Y += 2;
                   else
                       speed.Y += 5;
               }

                GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
                if (gamepadState.ThumbSticks.Left.X != 0)
                    inputDirection.X += gamepadState.ThumbSticks.Left.X;
                 if (gamepadState.IsButtonDown(Buttons.A) && !previousGamePadState.IsButtonDown(Buttons.A))
                {
                    inputDirection.Y += -2;
                    isJumping = true;
                }
                previousGamePadState = gamepadState;
                previousKeyboardState = keyboardState;
                
                
                return inputDirection * speed;
            }

        }


        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {

            position += direction;

            if (position.Y < 0)
                position.Y = 0;
            if (position.X > clientBounds.Width/2)
                position.X = clientBounds.Width /2;
            if (position.Y > clientBounds.Height - frameSize.Y)
            {
              
            }

            base.Update(gameTime, clientBounds);
        }
    }
}
