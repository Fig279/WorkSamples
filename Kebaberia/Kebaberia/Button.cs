using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Kebaberia
{
    internal class Button
    {
        // Fields variables:
        private Rectangle buttonBox;
        private Texture2D buttonImageHover;
        private Texture2D buttonImageIdle;
        private MouseState currentMstate;

        /// <summary>
        /// This property gets and set the rectangle
        /// of a button class object.
        /// </summary>
        public Rectangle ButtonBox
        {
            get { return buttonBox; }
            set { buttonBox = value; }
        }
        
        /// <summary>
        /// This property gets and sets the X position 
        /// of the button box rectangle object.
        /// </summary>
        public int XPosition
        {
            get { return buttonBox.Y; }
            set { buttonBox.X = value; }
        }

        /// <summary>
        /// This property gets and sets the X position 
        /// of the button box rectangle object.
        /// </summary>
        public int YPosition
        {
            get { return buttonBox.Y; }
            set { buttonBox.Y = value; }
        }

        /// <summary>
        /// This property gets and set the width of the
        /// image button.
        /// </summary>
        public int ImageWidth
        {
            get { return buttonBox.Width; }
            set { buttonBox.Width = value; }
        }

        /// <summary>
        /// This property gets and set the height of the
        /// image button.
        /// </summary>
        public int ImageHeight
        {
            get { return buttonBox.Height; }
            set { buttonBox.Height = value; }
        }

        /// <summary>
        /// This creates a constructor for the button
        /// class object.
        /// </summary>
        /// <param name="rect">The rectangle for image input.</param>
        /// <param name="mouseHoverImage">The hovered image.</param>
        /// <param name="idleButtonImage">The idle image.</param>
        public Button(
            Rectangle rect,
            Texture2D mouseHoverImage,
            Texture2D idleButtonImage)
        {
            // Initialize fields:
            buttonBox = rect;
            buttonImageHover = mouseHoverImage;
            buttonImageIdle = idleButtonImage;
        }


        /// <summary>
        /// This method draw the button to the window.
        /// </summary>
        /// <param name="_sb">The input spritebatch.</param>
        public void Draw(SpriteBatch _sb)
        {
            // If there is collusion with mouse.
            if (buttonBox.Contains(currentMstate.Position))
            {
                /* Draw button texture to the game 1.
                 * Use image hover texture for button. */
                _sb.Draw(
                    buttonImageHover,       // The image hover texture.
                    buttonBox,              // The button box rectangle.
                    Color.White);           // The drawn color display.
                
            }
            else
            {
                /* Draw button texture to the game 1.
                 * Use image idle texure for the button. */
                _sb.Draw(
                    buttonImageIdle,        // The image idle texture.
                    buttonBox,              // The button box rectangle.
                    Color.White);           // The drawn color display.
            }

        }

        /// <summary>
        /// This method updates the button state if the
        /// button class is pressed.
        /// </summary>
        /// <param name="gameTime">The current GameTime.</param>
        /// <returns>A true or false value.</returns>
        public bool Update(GameTime gameTime)
        {

            // Get the current mouse state.
            currentMstate = Mouse.GetState();

            /* If collision is true and left mouse 
             * button is pressed. */
            if (buttonBox.Contains(currentMstate.Position) &&
                currentMstate.LeftButton == ButtonState.Pressed)
            {
                // Return true.
                return true;
            }
            else
            {
                // Return false.
                return false;
            }
        }

        #region Button TO-DO List:
        // #1: Create a debug method for each button.
        // #2: Create a delegate method signature for each button.
        // #3: Create an event list using the above delegate signature.
        #endregion
    }
}
