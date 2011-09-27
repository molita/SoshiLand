using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SoshiLandSilverlight
{
    public class Button
    {
        private string buttonName;

        private Texture2D buttonPressed;
        private Texture2D buttonUnPressed;
        private Texture2D currentTexture;

        public Rectangle buttonRectangle;
        public bool buttonTriggered;

        public Button(string name, Texture2D pressed, Texture2D notPressed, Rectangle rectangle)
        {
            // Set constructor variables
            buttonName = name;
            buttonPressed = pressed;
            buttonUnPressed = notPressed;
            buttonRectangle = rectangle;

            // Set the button to be unpressed initially
            currentTexture = buttonUnPressed;
        }

        public void PressButton()
        {
            currentTexture = buttonPressed;
            buttonTriggered = true;
        }

        public void UnPressButton()
        {
            currentTexture = buttonUnPressed;
        }

        // Draw the button
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(currentTexture, buttonRectangle, Color.White);
            spriteBatch.End();
        }

        public void ButtonClickUpdate(MouseState ms)
        {
            // Check if mouse is within button bounds
            if (ms.X > buttonRectangle.X &&
                ms.X < buttonRectangle.X + buttonRectangle.Width &&
                ms.Y > buttonRectangle.Y &&
                ms.Y < buttonRectangle.Y + buttonRectangle.Height)
            {
                if (ms.LeftButton == ButtonState.Pressed && !buttonTriggered && SoshilandGame.turnPhase == 0)
                    PressButton();
                else
                    UnPressButton();
            }
            else
                UnPressButton();
        }
    }
}
