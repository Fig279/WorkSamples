using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Kebaberia
{
    /// <summary>
    /// Creates a list of ingredients that are drawn to the 
    /// screen, tells the player which ingredients to collect
    /// </summary>
    public class Order
    {
        // Fields
        private List<Ingredient> ingredients;
        private Dictionary<FoodType, Texture2D> ingredientTextures;

        //Properties
        public List<FoodType> IngredientTypes
        {
            get { List<FoodType> ingredientTypes = new List<FoodType>();
                for(int i = 4; i >= 0; i--)
                {
                    ingredientTypes.Add(ingredients[i].Type);
                }
                return ingredientTypes; 
            }
        }

        /// <summary>
        /// Gets the current number of ingredients
        /// in the order.
        /// </summary>
        public int Count
        {
            get { return ingredients.Count; }
        }

        /// <summary>
        /// Parameterized constructor, takes an array of food 
        /// types and creates ingredients with those types
        /// </summary>
        /// <param name="ingredientNames">array of FoodTypes to create ingredients</param>
        /// <param name="ingredientTextures">loaded texture dictionary</param>
        public Order(FoodType[] ingredientNames, Dictionary<FoodType, Texture2D> ingredientTextures)
        {
            ingredients = new List<Ingredient>();
            this.ingredientTextures = ingredientTextures;

            for (int i = 0; i < ingredientNames.Length; i++)
            {
                ingredients.Add(new Ingredient(ingredientNames[i], ingredientTextures));
            }
        }

        /// <summary>
        /// Draws the order in a column with the ingredients' draw method
        /// </summary>
        /// <param name="_spriteBatch">game1 spriteBatch</param>
        public void Draw(SpriteBatch _spriteBatch)
        {
            for (int i = ingredients.Count - 1; i >= 0; i--)
            {
                ingredients[i].Draw(_spriteBatch,
                    new Vector2(25, 25 + (105 * i)));
            }
        }

        /// <summary>
        /// Gets the ingredient in an order with index
        /// </summary>
        /// <param name="index">index of ingredient</param>
        /// <returns>ingredient at index</returns>
        public Ingredient GetIngredient(int index)
        {
            if (index >= 0 && index < ingredients.Count)
            {
                return ingredients[index];
            }

            throw new Exception("Error! Invalid ingredient index");
        }
    }
}
