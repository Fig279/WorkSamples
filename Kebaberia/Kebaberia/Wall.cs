using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Kebaberia
{
    /// <summary>
    /// Creates a wall used exclusively by the DebugBox class
    /// Created by Henry
    /// </summary>
    internal class Wall
    {
        private SpriteBatch _spriteBatch;
        private Vector2 pointOne;
        private Vector2 pointTwo;

        public Vector2 PointOne { get => pointOne; }
        public Vector2 PointTwo { get => pointTwo; }

        public Wall(Vector2 pointOne, Vector2 pointTwo, SpriteBatch spriteBatch)
        {
            this.pointOne = pointOne;
            this.pointTwo = pointTwo;
            _spriteBatch = spriteBatch;
        }

        public Wall(int X1, int Y1, int X2, int Y2, SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
            pointOne = new Vector2(X1, Y1);
            pointTwo = new Vector2(X2, Y2);
        }

        public Wall()
        {
            this.pointOne = new Vector2(10, 50);
            this.pointTwo = new Vector2(10, 100);
        }

        public void Display()
        {
            _spriteBatch.DrawLine(pointOne, pointTwo, Color.Red, 2);
        }

        public void UpdateDimensions(float x1, float y1, float x2, float y2)
        {
            pointOne.X = x1;
            pointOne.Y = y1;
            pointTwo.X = x2;
            pointTwo.Y = y2;
        }
    }
}
