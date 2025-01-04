using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Kebaberia
{

    /// <summary>
    /// Handles the generation, movement, drawing and collision of ingredients
    /// Fig Gamache
    /// </summary>
    internal class IngredientManager
    {
        //Fields
        private double ingredientSpawnRate;
        private int fallSpeed;
        private List<Ingredient> ingredients;
        private List<Ingredient> ingredientsOnStick;
        private Random rng;
        private double timer;
        private double spawnRateTimer;
        protected Dictionary<FoodType, Texture2D> ingredientTextures;
        private bool freezeOn;
        private SpriteBatch _spriteBatch;

        //for sound
        private SoundEffect stickNoise;

        //Properties
        /// <summary>
        /// is true when ingredients are frozen, false otherwise
        /// can be changed in debug mode 
        /// </summary>
        public bool FreezeOn
        {
            get { return freezeOn; } 
            set { freezeOn = value; } 
        }

        /// <summary>
        /// allows the fall speed of the ingredients to be set to a number
        /// this is used in debug to speed up the ingredients
        /// </summary>
        public int FallSpeed
        {
            get { return fallSpeed; }
            set { fallSpeed = value; }
        }

        /// <summary>
        /// allows the spawn speed of the ingredients to be set to a number
        /// this is used in debug to speed up the ingredients
        /// </summary>
        public double IngredientSpawnRate
        {
            get { return ingredientSpawnRate; }
            set { ingredientSpawnRate = value; }
        }


        /// <summary>
        /// returns the FoodType of all ingredients on the stick
        /// </summary>
        public List<FoodType> IngredientsOnStick
        {
            get
            {
                List<FoodType> ingredientTypes = new List<FoodType>();
                for (int i = 0; i < ingredientsOnStick.Count; i++)
                {
                    ingredientTypes.Add(ingredientsOnStick[i].Type);
                }
                return ingredientTypes;
            }
        }

        /// <summary>
        /// only allows ingredients to be added to the stick if adding that ingredient
        /// won't make the stick surpass 5 ingredients
        /// </summary>
        public Ingredient AddIngredientToStick
        {
            set 
            { 
                if(ingredientsOnStick.Count < 5)
                {
                    ingredientsOnStick.Add(value);
                } 
            }
        }

        //Contructors
        /// <summary>
        /// a default constructor that 
        ///  initializes necessary fields
        /// </summary>
        public IngredientManager(Dictionary<FoodType, Texture2D> ingredientTextures, SpriteBatch spriteBatch, SoundEffect stickNoise) 
        { 
            //for sound
            this.stickNoise = stickNoise;

            this.ingredientTextures = ingredientTextures;
            rng = new Random();
            fallSpeed = 0;
            timer = 0;
            spawnRateTimer = 0;
            ingredientSpawnRate = .5;
            ingredients = new List<Ingredient>();
            ingredientsOnStick = new List<Ingredient>();

            _spriteBatch = spriteBatch;

            //spawn offscreen so that an IndexOutOfRange exception isn't thrown when checking
            //ingredients against the last 5 spawned ingredients in Update
            ingredients.Add(new Ingredient(new Rectangle(-200, 500, 1,1), FoodType.AppleWorm, _spriteBatch, 3));
            ingredients.Add(new Ingredient(new Rectangle(-200, 500, 1, 1), FoodType.AppleWorm, _spriteBatch, 3));
            ingredients.Add(new Ingredient(new Rectangle(-200, 500, 1, 1), FoodType.AppleWorm, _spriteBatch, 3));
            ingredients.Add(new Ingredient(new Rectangle(-200, 500, 1, 1), FoodType.AppleWorm, _spriteBatch, 3));
            ingredients.Add(new Ingredient(new Rectangle(-200, 500, 1, 1), FoodType.AppleWorm, _spriteBatch, 3));


            freezeOn = false;
        }

        //Methods
        /// <summary>
        /// adds ingredients to the top of the screen and makes them fall
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime, Stick stick)
        {
            if (ingredientsOnStick.Count == 5)
            {
                ingredientsOnStick.Clear();
            }

            if(!freezeOn)
            {
                timer += gameTime.ElapsedGameTime.TotalSeconds; 
                spawnRateTimer += gameTime.ElapsedGameTime.TotalSeconds;

                FreezeIngredients();
                StickCollision(stick);
            }

            Rectangle placeholder;


            //moves each of the ingredients caught on the stick with the stick
            for(int i = 0; i < ingredientsOnStick.Count; ++i)
            {

                /* If the ingredient Y position is less than
                * the stick position increase it until it 
                * reaches the place holder position. */
                placeholder = ingredientsOnStick[i].Position;

                // Set the place holder x position.
                placeholder.X = stick.Position.X +
                    (stick.Position.Width / 2) - (placeholder.Width/2) - 2;

                /* If the y position is less than the stick 
                 * y position. */
                if (placeholder.Y < 913 - (55 * i))
                {
                    /* Increase ingredient slide speed by
                     * ingredient fall speed. */
                    placeholder.Y += 4;
                }

                /* This calculation should change once I know
                 * the dimensions of the stick. */
                // Stacks the ingredients on top of each other.
                /* Set the ingredient last position on the
                 * stick to the new place holder position. */
                ingredientsOnStick[i].Position = placeholder;
            }
        }

        /// <summary>
        /// draws each randoly falling randomly generated ingredient
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < ingredients.Count; i++)
            {
                spriteBatch.Draw(
                    ingredientTextures[ingredients[i].Type],
                    ingredients[i].Position,
                    Color.White);
            }
            for (int i = 0; i < ingredientsOnStick.Count; i++)
            {
                spriteBatch.Draw(
                    ingredientTextures[ingredientsOnStick[i].Type],
                    ingredientsOnStick[i].Position,
                    Color.White);
            }
        }

        /// <summary>
        /// Updates all debug boxes for all ingredients
        /// </summary>
        public void UpdateDebug()
        {
            for (int i = 0; i < ingredients.Count; i++)
            {
                ingredients[i].UpdateDebug();
            }
            for (int i = 0; i < ingredientsOnStick.Count; i++)
            {
                ingredientsOnStick[i].UpdateDebug();
            }
        }

        /// <summary>
        /// Draws all debug boxes for all ingredients
        /// </summary>
        public void DrawDebug()
        {
            for (int i = 0; i < ingredients.Count; i++)
            {
                ingredients[i].DrawDebug();
            }
            for (int i = 0; i < ingredientsOnStick.Count; i++)
            {
                ingredientsOnStick[i].DrawDebug();
            }
        }

        /// <summary>
        /// Generates a randome foodtype for spawning ingredients
        /// </summary>
        /// <returns></returns>
        
        public FoodType GenerateFoodType()
        {

            int food = rng.Next(0, 8);

            //generates a random food type to be assigned to an ingredient
            switch (food)
            {
                case 1:
                    return FoodType.Fish;
                case 2:
                    return FoodType.Grub;
                case 3:
                    return FoodType.MelonWater;
                case 4:
                    return FoodType.Pickle;
                case 5:
                    return FoodType.Pineapple;
                case 6:
                    return FoodType.Sashimi;
                case 7:
                    return FoodType.Shrimp;
                default:
                    return FoodType.AppleWorm;
            } //end of switch

        }

 

        /// <summary>
        /// checks each ingredient to see if it is colliding with the stick]
        /// stick must hit the bottom of the ingredient
        /// </summary>
        /// <param name="stick">the stick to check against</param>
        public void StickCollision(Stick stick)
        {
            for(int i = 0; i < ingredients.Count; ++i)
            {
                if (ingredients[i].Position.Y > 600 && ingredients[i].Position.Y < 700 &&
                    stick.Position.X + (stick.Position.Width/2) > ingredients[i].Position.X + 20 && 
                    stick.Position.X + (stick.Position.Width / 2) < ingredients[i].Position.X + 80)
                {
                    AddIngredientToStick = ingredients[i];

                    stickNoise.Play();


                    ingredients.RemoveAt(i);
                    i--;
                }
            }
        }

        /// <summary>
        /// This is all update code for ingredients 
        /// that shouldn't happen when the ingredients are frozen
        /// </summary>
        private void FreezeIngredients()
        {

            //increases the spawn rate by until ingredients are spawning
            //at the normal rate(at 35 seconds into the game)
            if (ingredientSpawnRate != 0.18)
            {
                if (ingredientSpawnRate == 0.5 && spawnRateTimer > 10)
                {
                    ingredientSpawnRate = 0.3;
                }
                else if (ingredientSpawnRate == 0.3 && spawnRateTimer > 20)
                {
                    ingredientSpawnRate = 0.25;
                }
                else if (ingredientSpawnRate == 0.25 && spawnRateTimer > 30)
                {
                    ingredientSpawnRate = 0.2;
                }
                else if (ingredientSpawnRate == 0.2 && spawnRateTimer > 35)
                {
                    ingredientSpawnRate = 0.18;
                }
            }

            //iterates through each ingredient to make them fall
            Rectangle placeholder;
            for (int i = 0; i < ingredients.Count; i++)
            {
                //moves the ingredients down
                placeholder = ingredients[i].Position;
                placeholder.Y += ingredients[i].FallSpeed + fallSpeed;
                ingredients[i].Position = placeholder;

                //removes ingredients that are off screen
                if (ingredients[i].Position.Y > 1150)
                {
                    ingredients.RemoveAt(i);
                    i--;
                }
            }


            //creates an ingredient every ingredientsSpawnRate seconds
            if (timer > ingredientSpawnRate)
            {
                //creates a new Ingredient at a random spot along the top
                //the do while loop makes sure the ingredients don't overlap
                int xPosition;
                Vector2 xPositionPoint;
                    do
                    {
                        xPosition = rng.Next(138, 1700);
                        xPositionPoint = new Vector2(xPosition, -50);

                        //checks the xPosition against the last 5 created Ingredients
                    } while (ingredients[ingredients.Count - 1].Position.Contains(xPositionPoint) ||
                    ingredients[ingredients.Count - 2].Position.Contains(xPositionPoint) ||
                    ingredients[ingredients.Count - 3].Position.Contains(xPositionPoint) ||
                    ingredients[ingredients.Count - 4].Position.Contains(xPositionPoint) ||
                    ingredients[ingredients.Count - 5].Position.Contains(xPositionPoint));

                    //creates the new ingredient
                    ingredients.Add(new Ingredient(new Rectangle(xPosition, -50, 80, 80), GenerateFoodType(), _spriteBatch, rng.Next(2, 5)));

                    timer -= ingredientSpawnRate;
            }

        }

        /// <summary>
        /// resets the ingredientManager so that the game can be played again
        /// </summary>
        public void Reset()
        {
            //reseting fields to what they are when the game begins
            fallSpeed = 0;
            timer = 0;
            ingredientSpawnRate = .5;
            ingredients.Clear();
            ingredientsOnStick.Clear();
            freezeOn = false;

            //spawn offscreen so that an IndexOutOfRange exception isn't thrown when checking
            //ingredients against the last 5 spawned ingredients in Update
            ingredients.Add(new Ingredient(new Rectangle(-200, 500, 1, 1), FoodType.AppleWorm, _spriteBatch, 3));
            ingredients.Add(new Ingredient(new Rectangle(-200, 500, 1, 1), FoodType.AppleWorm, _spriteBatch, 3));
            ingredients.Add(new Ingredient(new Rectangle(-200, 500, 1, 1), FoodType.AppleWorm, _spriteBatch, 3));
            ingredients.Add(new Ingredient(new Rectangle(-200, 500, 1, 1), FoodType.AppleWorm, _spriteBatch, 3));
            ingredients.Add(new Ingredient(new Rectangle(-200, 500, 1, 1), FoodType.AppleWorm, _spriteBatch, 3));
        }


        //removes all ingredients on the stick
        public void ClearStick()
        {
            ingredientsOnStick.Clear();
        }

    }


}
