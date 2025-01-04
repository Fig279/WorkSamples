using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace Kebaberia
{
    /// <summary>
    /// Creates a debug box around objects
    /// Created by Henry
    /// </summary>
    internal class DebugBox
    {
        private SpriteBatch _spriteBatch;
        private Vector2 startPosition;
        private Vector2 endPosition;
        private List<Wall> walls;

        public List<Wall> Walls { get { return walls; } }

        public DebugBox(SpriteBatch spriteBatch, Rectangle textureRect)
        {
            _spriteBatch = spriteBatch;
            startPosition = Vector2.Zero;
            endPosition = Vector2.One;
            walls = new List<Wall>()
            {
                new Wall(startPosition, endPosition, spriteBatch),
                new Wall(startPosition, endPosition, spriteBatch),
                new Wall(startPosition, endPosition, spriteBatch),
                new Wall(startPosition, endPosition, spriteBatch)
            };
        }

        public void Update(Rectangle textureRect)
        {
            // Top wall
            walls[0].UpdateDimensions(textureRect.X, textureRect.Y, textureRect.X + textureRect.Width, textureRect.Y);

            // Left wall
            walls[1].UpdateDimensions(textureRect.X, textureRect.Y, textureRect.X, textureRect.Y + textureRect.Height);

            // Bottom wall
            walls[2].UpdateDimensions(textureRect.X, textureRect.Y + textureRect.Height, textureRect.X + textureRect.Width, textureRect.Y + textureRect.Height);

            // Right wall
            walls[3].UpdateDimensions(textureRect.X + textureRect.Width, textureRect.Y, textureRect.X + textureRect.Width, textureRect.Y + textureRect.Height);
        }

        public void Draw()
        {
            walls[0].Display();
            walls[1].Display();
            walls[2].Display();
            walls[3].Display();
        }
    }
}
