using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kebaberia
{
    internal class Stick
    {

        // Fields:
        private Texture2D stickImage;
        private Rectangle stickRect;
        private Rectangle collisionRectangle;
        private MouseState currentMState;
        private DebugBox debugBox;

        /// <summary>
        /// This property gets the stick rectangle
        /// to gain access to its position.
        /// </summary>
        public Rectangle Position
        {
            get { return stickRect; }
        }

        /// <summary>
        /// This create an instanace of the stick object.
        /// </summary>
        /// <param name="image">The stick texture image.</param>
        /// <param name="rect">The stick rectangle.</param>
        public Stick (
            Texture2D image,
            Rectangle rect,
            SpriteBatch _spriteBatch)
        {
            // Initialize the fields.
            stickImage = image;
            stickRect = rect;
            debugBox = new(_spriteBatch, stickRect);
            collisionRectangle = new Rectangle(stickRect.X + collisionRectangle.Width/2, 
                stickRect.Y + collisionRectangle.Height + 30,stickRect.Width/10,50);
        }

        /// <summary>
        /// This method updates the stick object position.
        /// </summary>
        /// <param name="gametime">The game time.</param>
        public void Update(GameTime gametime)
        {
            // Get the current mouse state.
            currentMState = Mouse.GetState();

            /* Only move the stick object based on the
             * mouse X position by setting . */
            stickRect.X = currentMState.Position.X - (Position.Width/2);
            collisionRectangle.X = currentMState.Position.X - (collisionRectangle.Width/2) -2;
            if(currentMState.Position.X > 1760)
            {
                stickRect.X = 1760 - (stickRect.Width / 2);
            }
            else if(currentMState.Position.X < 160)
            {
                stickRect.X = 160 - (stickRect.Width/2);
            }
        }

        /// <summary>
        /// This method draws the current stick on
        /// the window.
        /// </summary>
        /// <param name="_sb">The current sprite.</param>
        public void Draw(SpriteBatch _sb)
        {
            // Draw the stick.
            _sb.Draw(
                stickImage,
                stickRect,
                Color.White);

        }

        /// <summary>
        /// Calls update on this object's debug box, called in an if statement in main
        /// </summary>
        public void UpdateDebug()
        {
            debugBox.Update(collisionRectangle);
        }

        /// <summary>
        /// Calls draw on this object's debug box, called in an if statement in main
        /// </summary>
        public void DrawDebug()
        {
            debugBox.Draw();
        }
        
    }
}
