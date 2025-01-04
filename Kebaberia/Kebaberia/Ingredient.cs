using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Kebaberia
{
    /// <summary>
    /// Holds the position and type of a specific ingredient
    /// container class to be manipulated by Order and IngredientManager
    /// Fig Gamache
    /// </summary>
    public class Ingredient
    {
        //Fields
        private Rectangle position;
        private FoodType type;
        private Dictionary<FoodType, Texture2D> ingredientTextures;
        private DebugBox debugBox;
        private int fallSpeed;
        private bool rightIngredient;

        //Properties
        /// <summary>
        /// returns an ingredients type
        /// </summary>
        public FoodType Type 
        { 
            get { return type; }
        } 

        /// <summary>
        /// returns the position of an ingredient
        /// allows an ingredient's position to be set to a value
        /// </summary>
        public Rectangle Position
        {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// allows each ingedients fallspeed to be returned or changed
        /// </summary>
        public int FallSpeed
        {
            get { return fallSpeed; }
            set { fallSpeed = value; }
        }


        public bool RightIngredient
        {
            get { return rightIngredient; }
            set { rightIngredient = value; }
        }

        //Contructors
        /// <summary>
        /// creates an ingredient with specified fields
        /// </summary>
        /// <param name="position">where it is</param>
        /// <param name="type">kind of food ingredient</param>
        /// <param name="spriteBatch"></param>
        /// <param name="fallSpeed">random fall speed determines how fast they move</param>
        public Ingredient(Rectangle position, FoodType type, SpriteBatch spriteBatch, int fallSpeed)
        {
            this.position = position;
            this.type = type;
            debugBox = new(spriteBatch, position);
            this.fallSpeed = fallSpeed;
        }

        /// <summary>
        /// doesn't require a position to be passed in
        /// creates an ingredient with a specified type
        /// this is used specifically for displaying orders
        /// </summary>
        /// <param name="type"></param>
        public Ingredient(FoodType type, Dictionary<FoodType, Texture2D> ingredientTextures)
        {
            this.type = type;
            this.ingredientTextures = ingredientTextures;
        }


        /// <summary>
        /// draws the ingredient for order display
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch, Vector2 orderPosition)
        {
            spriteBatch.Draw(
                ingredientTextures[Type],
                new Rectangle((int)orderPosition.X, (int)orderPosition.Y, 100, 100),
                Color.White);
        }

        /// <summary>
        /// Updates the debug box
        /// </summary>
        public void UpdateDebug()
        {
            debugBox.Update(position);
        }

        /// <summary>
        /// Draws the debug box
        /// </summary>
        public void DrawDebug()
        {
            debugBox.Draw();
        }
    }
}
