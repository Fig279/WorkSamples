 using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
using System.Transactions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace Kebaberia
{
    /// <summary>
    /// 
    /// </summary>
    public class OrderManager
    {
        // Fields
        private List<Order> orders;
        private Random random;
        private Dictionary<FoodType, Texture2D> ingredientTextures;
        private Texture2D image;
        private int score;
        private List<FoodType> currentIngredientOnStick;
        private Color shade;
        private Color[] checkOrder;

        //Properties
        /// <summary>
        /// Returns the current order to compare against collected ingredients
        /// </summary>
        public Order CurrentOrder
        {
            get { return orders[orders.Count - 1]; }
        }

        public Texture2D Image
        {
            get { return image; }
            set { image = value; }
        }

        /// <summary>
        /// returns the player's score
        /// </summary>
        public int Score
        {
            get { return score; }
        }

        /// <summary>
        /// Initializes fields
        /// </summary>
        /// <param name="ingredientTextures">dictionary of loaded Texture2Ds</param>
        public OrderManager(Dictionary<FoodType, Texture2D> ingredientTextures)
        {
            orders = new List<Order>();
            random = new Random();
            this.ingredientTextures = ingredientTextures;
            score = 0;
            currentIngredientOnStick = null;
            checkOrder = new Color[5];
        }

        /// <summary>
        /// Generates 20 random orders with 20 random ingredients
        /// </summary>
        public void GenerateOrders()
        {
            // Create array of ingredients for each order
            FoodType[] orderIngredients;

            //for (int i = 0; i < 20; i++)
            //{
                // Init ingredients
                orderIngredients = new FoodType[5];

                // Create 5 random ingredients
                for (int j = 0; j < 5; j++)
                {
                    orderIngredients[j] = (FoodType)random.Next(0, 8);
                }

                // Add them to the orders list
                orders.Add(new Order(orderIngredients, ingredientTextures));
            //}
        }
        
        /// <summary>
        /// This method draws the order and the 
        /// check boxes.
        /// </summary>
        /// <param name="_spriteBatch"></param>
        public void Draw(SpriteBatch _spriteBatch)
        {
            // Fields:
            int orderBoxWidth = 90;
            int orderBoxHeight = 90;

            for (int i = 0; i < CurrentOrder.Count; i++)
            {
                // Draw a half black box in front of the black box.
                _spriteBatch.Draw(
                    Image,
                    new Rectangle(25, (25 + (105 * i)),
                    orderBoxHeight, orderBoxHeight),
                    Color.Black);

                // Draw a white background outline behind the order.
                _spriteBatch.Draw(
                    Image,
                    new Rectangle(20, 20 + (105 * i), 100, 100),
                    Color.WhiteSmoke);
            }

            // Draw a black check mark box using a for loop.
            for (int i = 0; i < CurrentOrder.Count; i++)
            {

                //if (orders[i].RightOrder)
                if (CurrentOrder.GetIngredient(i).RightIngredient)
                {
                    // Draw a lime green box in front of the black box.
                    _spriteBatch.Draw(
                        Image,
                        new Rectangle(25, (445 - (105 * i)),
                        orderBoxWidth ,
                        orderBoxHeight),
                        Color.LimeGreen);
                }
                else
                {
                    // Draw a half colour after white
                    // Draw a red infront of the black box.
                    _spriteBatch.Draw(
                        Image,
                        new Rectangle(25, (445 - (105 * i)),
                        orderBoxWidth ,
                        orderBoxHeight),
                        Color.Red);
                }

            }

            // Draw the current order.
            CurrentOrder.Draw(_spriteBatch);                
        }

        /// <summary>
        /// This method checks if and ingredient on the
        /// stick match the order ingredients.
        /// </summary>
        public void CheckItems(List<FoodType> ingredients)
        {
            List<FoodType> orderFoodTypes = CurrentOrder.IngredientTypes;
            currentIngredientOnStick = ingredients;

            if(currentIngredientOnStick.Count > 0)
            {
                //compares each item from each list
                for (int i = 0; i < currentIngredientOnStick.Count; i++) //int i = currentIngredientOnStick.Count - 1; i >= 0; i--
                {
                    // Set the right order to true.
                    if (orderFoodTypes[i] == ingredients[i])
                    {
                        //orders[0].RightOrder = true;
                        CurrentOrder.GetIngredient(i).RightIngredient = true;
                    }
                    else
                    {
                        //orders[0].RightOrder = false;
                        CurrentOrder.GetIngredient(i).RightIngredient = false;
                    }
                }
            }
            else
            {
                // Reset all right order to false.
                for (int loop = 0; loop < CurrentOrder.Count; loop++)
                {
                    //orders[loop].RightOrder = false;
                    orders[0].GetIngredient(loop).RightIngredient = false;
                }
            }
        }

        
        /// <summary>
        /// checks each ingredient on the stick against the ingredients in the current
        /// order to calculate the earned score for the order
        /// </summary>
        /// <param name="ingredients">ingredients on the stick being checked</param>
        public int CheckOrder(List<FoodType> ingredients)
        {
            List <FoodType> orderFoodTypes= CurrentOrder.IngredientTypes;
            currentIngredientOnStick = ingredients;
            int correctItems = 0;
            int scoreAddition = 0;

            //compares each item from each list
            for (int i = 0; i < 5; i++)
            {
                if (orderFoodTypes[i] == ingredients[i])
                {
                    correctItems++;
                }
            }

            //adds to the existing score 
            //10 points per correct item but a perfect order gets an extra 20 points
            if(correctItems == 5)
            {
                scoreAddition = 70;
            }
            else
            {
                //gives +10 per correect item but minus 5 per wrong item
                scoreAddition = (correctItems * 10) - ((5 - correctItems) * 5);
            }

            score += scoreAddition;

            GenerateOrders();

            return scoreAddition;
        }

        /// <summary>
        /// resets the orderManager so that the game can be played again
        /// </summary>
        public void Reset()
        {
            //sets fields to what they were when orderManager was created
            score = 0;
            currentIngredientOnStick = null;
        }
    }
}