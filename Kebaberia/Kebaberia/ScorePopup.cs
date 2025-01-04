using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Kebaberia
{
    /// <summary>
    /// Displays the score on screen when an ingredient is collected
    /// Created by Henry
    /// </summary>
    internal class ScorePopup
    {
        private string scoreString;
        private SpriteFont font;
        private SpriteBatch _spriteBatch;
        private Rectangle position;
        private int lifeFrames;

        /// <summary>
        /// This number is decremented each frame, when 0, the object will be destroyed in main
        /// </summary>
        public int Life
        {
            get { return lifeFrames; }
        }

        /// <summary>
        /// Create a new score popup
        /// </summary>
        /// <param name="score">the score to display</param>
        /// <param name="font">font</param>
        /// <param name="position">put stick position here</param>
        /// <param name="spriteBatch">sprite batch</param>
        public ScorePopup(int score, SpriteFont font, Rectangle position, SpriteBatch spriteBatch)
        {
            string sign = "";

            if (score > 0)
            {
                sign = "+";
            }

            scoreString = sign + score;
            this.font = font;
            this.position = position;
            _spriteBatch = spriteBatch;
            lifeFrames = 60;
        }

        /// <summary>
        /// Draw the string, move the position, and slowly kill it
        /// </summary>
        public void Draw()
        {
            _spriteBatch.DrawString(
                font,
                scoreString,
                new Vector2(position.X, position.Y), 
                Color.White);

            // Move number diagonally
            position.X++;
            position.Y--;

            // Move towards end of lifespan
            lifeFrames--;
        }
    }
}
