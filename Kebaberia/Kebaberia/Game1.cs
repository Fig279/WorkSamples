using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using MonoGame.Extended;
using System;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace Kebaberia
{
    //Credits Link: https://docs.google.com/document/d/1U3fb6RbiTuvKIzJvEwrbCw50m4vBPVDmDvKdizp80XA/edit?usp=sharing


    /// <summary>
    /// used to track what kind of food an Ingredient is
    /// </summary>
    public enum FoodType
    {
        AppleWorm,
        Fish,
        Grub,
        MelonWater,
        Pickle,
        Pineapple,
        Sashimi,
        Shrimp
    }

    /// <summary>
    /// state of the game
    /// </summary>
    public enum GameState
    {
        MainMenu,
        Gameplay,
        Pause,
        Credits,
        Leaderboard,
        GameOver,
        About
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Objects
        private IngredientManager ingredientManager;
        private OrderManager orderManager;
        private Stick playerStick;
        private KeyboardState previousKBState;
        private KeyboardState currentKBState;

        private double timer;

        // Collections
        private Dictionary<FoodType, Texture2D> ingredientTextures;

        // Textures
        private Texture2D appleWormTex;
        private Texture2D fishTex;
        private Texture2D grubTex;
        private Texture2D melonWaterTex;
        private Texture2D pickleTex;
        private Texture2D pineappleTex;
        private Texture2D sashimiTex;
        private Texture2D shrimpTex;

        private Texture2D verticalWood;
        private Texture2D woodBackground;

        private Texture2D horizontalPaper;
        private Texture2D verticalRibbon;

        private Texture2D stickTex;

        private Texture2D kebaberiaLogo;

        private SpriteFont cat_comic_36;
        private SpriteFont cat_comic_70;

        // Buttons
        private Button playButton;
        private Texture2D playButtonIdleTex;
        private Texture2D playButtonHoverTex;

        private Button infoButton;
        private Texture2D infoButtonIdleTex;
        private Texture2D infoButtonHoverTex;

        private Button leaderButton;
        private Texture2D leaderButtonIdleTex;
        private Texture2D leaderButtonHoverTex;

        private Button aboutButton;
        private Texture2D aboutButtonIdleTex;
        private Texture2D aboutButtonHoverTex;

        private Button backButton;
        private Texture2D backButtonIdleTex;
        private Texture2D backButtonHoverTex;

        private Button trashBin;
        private Texture2D openBinTex;
        private Texture2D closedBinTex;

        // GameState
        private GameState gameState;
        private GameState previousGameState;
        private bool debugMode;

        // Score popup numbers
        private List<ScorePopup> scorePopups;

        //border placeholder
        private List<Rectangle> border;
        private Texture2D borderImage;

        private int numberOfCompletedOrder;
        private int perfectOrdersInARow;

        //HighScore Fields
        private int iterates;
        private List<int> topScores;

        //Audio Fields
        private Song upbeatJazz;
        private List<SoundEffect> soundEffects;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);

            // 1080p window size
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;


            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            //Sound Effects
            soundEffects = new List<SoundEffect>();

        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();


            //for high score
            iterates = 0;
            topScores = new List<int>();

            // Initialize debug mode to true for now
            debugMode = false;
            
            gameState = GameState.MainMenu;
            timer = 120;

            border = new List<Rectangle>();
            border.Add(new Rectangle(0, -5, _graphics.PreferredBackBufferWidth, 85));
            border.Add(new Rectangle(0, 0, 130, _graphics.PreferredBackBufferHeight));
            border.Add(new Rectangle(_graphics.PreferredBackBufferWidth - 130, 0, 130, _graphics.PreferredBackBufferHeight));

            scorePopups = new List<ScorePopup>();
            numberOfCompletedOrder = 0;
            perfectOrdersInARow = 0;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            // Load stick texture
            stickTex = Content.Load<Texture2D>("Assets/Kebab Stick");

            // Load button textures
            playButtonIdleTex = Content.Load<Texture2D>("Assets/Buttons/play/342 px/play01");
            playButtonHoverTex = Content.Load<Texture2D>("Assets/Buttons/play/342 px/play02");

            infoButtonIdleTex = Content.Load<Texture2D>("Assets/Buttons/information/342 px/information01");
            infoButtonHoverTex = Content.Load<Texture2D>("Assets/Buttons/information/342 px/information02");

            leaderButtonIdleTex = Content.Load<Texture2D>("Assets/Buttons/leaderboard/72 px/leaderboard01");
            leaderButtonHoverTex = Content.Load<Texture2D>("Assets/Buttons/leaderboard/72 px/leaderboard02");

            aboutButtonIdleTex = Content.Load<Texture2D>("Assets/Buttons/about/72 px/about01");
            aboutButtonHoverTex = Content.Load<Texture2D>("Assets/Buttons/about/72 px/about02");

            backButtonIdleTex = Content.Load<Texture2D>("Assets/Buttons/back/72 px/back01");
            backButtonHoverTex = Content.Load<Texture2D>("Assets/Buttons/back/72 px/back02");

            closedBinTex = Content.Load<Texture2D>("trashBin");
            openBinTex = Content.Load<Texture2D>("openTrashBin");


            // Load ingredient textures
            appleWormTex = Content.Load<Texture2D>("Assets/Food We Picked/AppleWorm");
            fishTex = Content.Load<Texture2D>("Assets/Food We Picked/Fish");
            grubTex = Content.Load<Texture2D>("Assets/Food We Picked/Grub");
            melonWaterTex = Content.Load<Texture2D>("Assets/Food We Picked/MelonWater");
            pickleTex = Content.Load<Texture2D>("Assets/Food We Picked/Pickle");
            pineappleTex = Content.Load<Texture2D>("Assets/Food We Picked/Pineapple");
            sashimiTex = Content.Load<Texture2D>("Assets/Food We Picked/Sashimi");
            shrimpTex = Content.Load<Texture2D>("Assets/Food We Picked/Shrimp");
            kebaberiaLogo = Content.Load<Texture2D>("kebaberiaLogoNew");

            // Load fonts
            cat_comic_70 = Content.Load<SpriteFont>("Cat-Comic-70");
            cat_comic_36 = Content.Load<SpriteFont>("Cat-Comic-36");

            // Load background textures
            woodBackground = Content.Load<Texture2D>("woodBackground");
            verticalWood = Content.Load<Texture2D>("verticalWood");

            horizontalPaper = Content.Load<Texture2D>("horizontalPaper");
            verticalRibbon = Content.Load<Texture2D>("verticalRibbon");

            //borderImage = Content.Load<Texture2D>("solid_blue");

            borderImage = verticalWood;
            // Add food textures to enum-linked dictionary
            ingredientTextures = new()
            {
                { FoodType.AppleWorm, appleWormTex },
                { FoodType.Fish, fishTex },
                { FoodType.Grub, grubTex },
                { FoodType.MelonWater, melonWaterTex },
                { FoodType.Pickle, pickleTex },
                { FoodType.Pineapple, pineappleTex },
                { FoodType.Sashimi, sashimiTex },
                { FoodType.Shrimp, shrimpTex },
            };

            // Create player stick
            playerStick = new Stick(
                stickTex, 
                new Rectangle(
                    0, 650, 
                    334, 
                    478),
                _spriteBatch);

            // Create buttons
            playButton = new Button(
                new Rectangle(
                    (_graphics.PreferredBackBufferWidth / 2) - (playButtonHoverTex.Width /2), 
                    (_graphics.PreferredBackBufferHeight / 2) - (playButtonHoverTex.Height / 2), 
                    playButtonHoverTex.Width, 
                    playButtonHoverTex.Height), 
                playButtonHoverTex, 
                playButtonIdleTex);

            infoButton = new Button(
                new Rectangle(
                    (_graphics.PreferredBackBufferWidth / 2) - (infoButtonHoverTex.Width / 2),
                    (_graphics.PreferredBackBufferHeight / 2) - (infoButtonHoverTex.Height / 2) + 330,
                    infoButtonHoverTex.Width,
                    infoButtonHoverTex.Height),
                infoButtonHoverTex,
                infoButtonIdleTex);

            leaderButton = new Button(
                new Rectangle(
                    (_graphics.PreferredBackBufferWidth / 2) - (leaderButtonHoverTex.Width / 2) + 820,
                    (_graphics.PreferredBackBufferHeight / 2) - (leaderButtonHoverTex.Height / 2) + 380,
                    leaderButtonHoverTex.Width,
                    leaderButtonHoverTex.Height),
                leaderButtonHoverTex,
                leaderButtonIdleTex);

            
            aboutButton = new Button(
                new Rectangle(
                    _graphics.PreferredBackBufferWidth - (aboutButtonHoverTex.Width * 2),
                    (_graphics.PreferredBackBufferHeight / 2) + (aboutButtonHoverTex.Height * 2),
                    aboutButtonHoverTex.Width,
                    aboutButtonHoverTex.Height),
                aboutButtonHoverTex,
                aboutButtonIdleTex);

            backButton = new Button(
                new Rectangle(
                    (_graphics.PreferredBackBufferWidth / 2) - (backButtonHoverTex.Width / 2) + 820,
                    (_graphics.PreferredBackBufferHeight / 2) - (backButtonHoverTex.Height / 2) + 450,
                    backButtonHoverTex.Width,
                    backButtonHoverTex.Height),
                backButtonHoverTex,
                backButtonIdleTex);

            trashBin = new Button(
                new Rectangle(_graphics.PreferredBackBufferWidth - 155,
                _graphics.PreferredBackBufferHeight - 210, 180, 180),
                openBinTex,
                closedBinTex);

            //  Sound effects
            soundEffects.Add(Content.Load<SoundEffect>("Assets/Sounds/clickSound"));
            soundEffects.Add(Content.Load<SoundEffect>("Assets/Sounds/correctSound"));
            soundEffects.Add(Content.Load<SoundEffect>("Assets/Sounds/errorSound"));
            soundEffects.Add(Content.Load<SoundEffect>("Assets/Sounds/stickSound"));
            soundEffects.Add(Content.Load<SoundEffect>("Assets/Sounds/gameNotification"));

            // Create ingredient manager
            ingredientManager = new IngredientManager(ingredientTextures, _spriteBatch, soundEffects[3]);

            // Create order manager
            orderManager = new OrderManager(ingredientTextures);
            orderManager.Image = borderImage;

            // Generate random orders (temporary)
            orderManager.GenerateOrders();

            //  AUDIO from tutorial
            //  https://gamefromscratch.com/monogame-tutorial-audio/

            // Background music
            upbeatJazz = Content.Load<Song>("Assets/Sounds/upbeatJazz");
            MediaPlayer.Play(upbeatJazz);
            
            //  Uncomment the following line will also loop the song
            MediaPlayer.IsRepeating = true;
            MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;


        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            currentKBState = Keyboard.GetState();


            switch (gameState)
            {
                case GameState.MainMenu:

                    //for the highscore
                    iterates = 0;


                    // If the play button is pressed, go to gameplay
                    if (playButton.Update(gameTime) == true)
                    {
                        //click sound
                        soundEffects[0].Play();

                        Reset();

                        previousGameState = GameState.MainMenu;
                        gameState = GameState.Gameplay;
                        break;
                    }

                    // Info button update
                    if (infoButton.Update(gameTime) == true)
                    {
                        //click sound
                        soundEffects[0].Play();

                        previousGameState = GameState.MainMenu;
                        gameState = GameState.Pause;
                        break;
                    }

                    // About button update
                    if (aboutButton.Update(gameTime) == true)
                    {
                        //click sound
                        soundEffects[0].Play();

                        previousGameState = GameState.MainMenu;
                        gameState = GameState.About;
                        break;
                    }


                    break;
                case GameState.Gameplay:
                    
                    // check current manager.
                    orderManager.CheckItems(ingredientManager.IngredientsOnStick);

                    if (ingredientManager.IngredientsOnStick.Count >= 5)
                    {
                        int scoreAdd = orderManager.CheckOrder(ingredientManager.IngredientsOnStick);
                        numberOfCompletedOrder++;

                        //adds to streak of perfect orders 
                        if(scoreAdd == 70)
                        {
                            //perfect order sound
                            soundEffects[4].Play();

                            perfectOrdersInARow++;
                        }
                        else
                        {
                            perfectOrdersInARow = 0;
                        }

                        scorePopups.Add(new ScorePopup(
                            scoreAdd,
                            cat_comic_36,
                            playerStick.Position,
                            _spriteBatch));
                    }

                    //Turns Debug Mode on and off
                    else if (currentKBState.IsKeyDown(Keys.D) && previousKBState.IsKeyUp(Keys.D))
                    {
                        debugMode = !debugMode;
                        if (ingredientManager.FreezeOn)
                        {
                            //click sound
                            soundEffects[0].Play();

                            ingredientManager.FreezeOn = false;
                        }
                    }

                    //clicking on the trash bin or pressing T throws away all ingredients on the stick
                    if (trashBin.Update(gameTime) || (previousKBState.IsKeyDown(Keys.T) && currentKBState.IsKeyUp(Keys.T)))
                    {
                        ingredientManager.ClearStick();
                    }

                    //if debug mode is on the SpaceBar freezes all ingredients
                    if (debugMode && currentKBState.IsKeyDown(Keys.Space) && previousKBState.IsKeyUp(Keys.Space))
                    {
                       ingredientManager.FreezeOn = !ingredientManager.FreezeOn;
                    }

                    //if debug mode is on S speeds up all ingredients
                    if(debugMode && !ingredientManager.FreezeOn && currentKBState.IsKeyDown(Keys.S))
                    {
                        ingredientManager.FallSpeed = 2;
                        ingredientManager.IngredientSpawnRate = .1;
                    }
                    else if(debugMode)
                    {
                        ingredientManager.FallSpeed = 0;
                        ingredientManager.IngredientSpawnRate = .18;
                    }


                    playerStick.Update(gameTime);
                    ingredientManager.Update(gameTime, playerStick);
                    

                    if (debugMode)
                    {
                        playerStick.UpdateDebug();
                        ingredientManager.UpdateDebug();

                        // Hit K to go to game over state
                        if (Keyboard.GetState().IsKeyDown(Keys.K))
                        {
                            gameState = GameState.GameOver;
                            break;
                        }
                    }
                    else
                    {
                        timer -= gameTime.ElapsedGameTime.TotalSeconds;
                    }


                    if(timer <= 0)
                    {
                        gameState = GameState.GameOver;
                    }

                    if(currentKBState.IsKeyUp(Keys.P) && previousKBState.IsKeyDown(Keys.P))
                    {
                        previousGameState = GameState.Gameplay;
                        gameState = GameState.Pause;
                    }
                    break;
                case GameState.Pause:

                    // Unpause
                    if (backButton.Update(gameTime))
                    {
                        //click sound
                        soundEffects[0].Play();

                        gameState = previousGameState;
                    }
                    break;
                case GameState.GameOver:

                    //HIGHSCORE STUFF
                    HighScore thisScore = new HighScore(orderManager.Score);

                    if(iterates == 0)
                    {
                        topScores = thisScore.RunHighScore();
                        iterates++;
                    }

                    // Back to menu
                    if (backButton.Update(gameTime))
                    {
                        //click sound
                        soundEffects[0].Play();

                        gameState = GameState.MainMenu;
                    }

                    break;

                case GameState.About:

                    // Unpause
                    if (backButton.Update(gameTime))
                    {
                        //click sound
                        soundEffects[0].Play();

                        gameState = previousGameState;
                    }


                    break;
            }

            
            previousKBState = currentKBState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SandyBrown);

            // TODO: Add your drawing code here
            
            //QUIQUE1222 on r/Monogame
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);


            //WOOD BACKGROUND
            _spriteBatch.Draw(
                        woodBackground,
                        new Rectangle(0, 0, 1920, 1080),
                        Color.White);


            switch (gameState)
            {
                case GameState.MainMenu:

                    playButton.Draw(_spriteBatch);
                    infoButton.Draw(_spriteBatch);

                    aboutButton.Draw(_spriteBatch);

                    //add game title

                    _spriteBatch.Draw(
                        kebaberiaLogo,
                        new Rectangle(_graphics.PreferredBackBufferWidth/2 - (kebaberiaLogo.Width / 2) - 160,
                        100, 974, 216),
                        Color.White);
                    

                    break;
                case GameState.Gameplay:

                    if (scorePopups.Count > 0)
                    {
                        for (int i = 0; i < scorePopups.Count; i ++)
                        {
                            scorePopups[i].Draw();

                            if (scorePopups[i].Life <= 0)
                            {
                                scorePopups.Remove(scorePopups[i]);
                            }
                        }
                    }

                    playerStick.Draw(_spriteBatch);
                    ingredientManager.Draw(_spriteBatch);

                    foreach (Rectangle border in border)
                    {
                        _spriteBatch.Draw(
                            borderImage,
                            border,
                            Color.RosyBrown);
                    }

                    orderManager.Draw(_spriteBatch);

                    //displays the score as black if there is no perfect order streak
                    if(perfectOrdersInARow < 2)
                    {
                        _spriteBatch.DrawString(
                        cat_comic_36,
                        $"Score: {orderManager.Score}",
                        new Vector2(1660, 10),
                        Color.White);
                    }

                    //displays the score as Gold if multiple perfect orders occur in a row
                    else
                    {
                        _spriteBatch.DrawString(
                        cat_comic_36,
                        $"Score: {orderManager.Score}",
                        new Vector2(1660, 10),
                        Color.Goldenrod);
                    }

                    _spriteBatch.DrawString(
                        cat_comic_36,
                        $"Timer: " + String.Format("{0:0}", timer),
                        new Vector2(1410, 10),
                        Color.White);

                    if (debugMode)
                    {
                        playerStick.DrawDebug();
                        ingredientManager.DrawDebug();

                        _spriteBatch.DrawString(
                            cat_comic_70,
                            "D",
                            new Vector2(1650, 900),
                            Color.LightGray);
                    }

                    trashBin.Draw(_spriteBatch);

                    break;
                case GameState.Pause:

                    backButton.Draw(_spriteBatch);

                    //pause screen

                    _spriteBatch.DrawString(
                        cat_comic_36,
                        "CONTROLS:",
                        new Vector2(645, 340),
                        Color.LightGray);

                    _spriteBatch.DrawString(
                        cat_comic_36,
                        "Mouse: Move Stick to catch ingredients\n" +
                        "P: Pause\n" +
                        "T: Discard ingredients on stick " +
                        "or click recycle bin\n" +
                        "D: Enter Debug Mode\n" +
                        "    While in Debug Mode:\n" +
                        "    S: Ingredients fall faster\n" +
                        "    K: Force Game Over\n" +
                        "    Space: Freeze Ingredients",
                        new Vector2(670, 430),
                        Color.White);


                    break;
                case GameState.GameOver:

                    backButton.Draw(_spriteBatch);

                    _spriteBatch.DrawString(
                        cat_comic_70,
                        "GAME OVER!",
                        new Vector2(650, 150),
                        Color.White);

                    _spriteBatch.DrawString(
                        cat_comic_36,
                        $"You completed {numberOfCompletedOrder} orders." +
                        $"\nScore: {orderManager.Score}",
                        new Vector2(60, 120),
                        Color.White);

                    int scoreX = 700;
                    int scoreY = 400;

                    //  Yes this looks insane to draw each one individually but it was the only way to get it to cooperate
                    if(topScores.Count >= 10)
                    {
                        _spriteBatch.DrawString(
                        cat_comic_36, topScores[0].ToString(),
                        new Vector2(scoreX, scoreY),
                        Color.White);

                        _spriteBatch.DrawString(
                        cat_comic_36, topScores[1].ToString(),
                        new Vector2(scoreX, scoreY += 50),
                        Color.White);

                        _spriteBatch.DrawString(
                        cat_comic_36, topScores[2].ToString(),
                        new Vector2(scoreX, scoreY += 50),
                        Color.White);

                        _spriteBatch.DrawString(
                        cat_comic_36, topScores[3].ToString(),
                        new Vector2(scoreX, scoreY += 50),
                        Color.White);

                        _spriteBatch.DrawString(
                        cat_comic_36, topScores[4].ToString(),
                        new Vector2(scoreX, scoreY += 50),
                        Color.White);

                        _spriteBatch.DrawString(
                        cat_comic_36, topScores[5].ToString(),
                        new Vector2(scoreX, scoreY += 50),
                        Color.White);

                        _spriteBatch.DrawString(
                        cat_comic_36, topScores[6].ToString(),
                        new Vector2(scoreX, scoreY += 50),
                        Color.White);

                        _spriteBatch.DrawString(
                        cat_comic_36, topScores[7].ToString(),
                        new Vector2(scoreX, scoreY += 50),
                        Color.White);

                        _spriteBatch.DrawString(
                        cat_comic_36, topScores[8].ToString(),
                        new Vector2(scoreX, scoreY += 50),
                        Color.White);

                        _spriteBatch.DrawString(
                        cat_comic_36, topScores[9].ToString(),
                        new Vector2(scoreX, scoreY += 50),
                        Color.White);
                    }
                    
                    break;

                case GameState.About:
                    
                    //back button
                    backButton.Draw(_spriteBatch);

                    _spriteBatch.DrawString(
                        cat_comic_36,
                        "Developers: \nHenry Fehlner\nFig Gamache\nMayowa Famade\nEmma Duprey",
                        new Vector2(60, _graphics.PreferredBackBufferHeight / 3),
                        Color.White);

                    _spriteBatch.DrawString(
                        cat_comic_36,
                        "Images: \nHenry Software\nHiorespace\nIvy-marimonte\nSr.Toasty\nCrusenho\nFig Gamache\nEmma Duprey",
                        new Vector2(450, _graphics.PreferredBackBufferHeight / 3),
                        Color.White);

                    _spriteBatch.DrawString(
                        cat_comic_36,
                        "Sounds:\nMusic_For_Videos\nPixabay\nUNIVERSFIELD",
                        new Vector2(850, _graphics.PreferredBackBufferHeight / 3),
                        Color.White);

                    _spriteBatch.DrawString(
                        cat_comic_36,
                        "Fonts:\nSouldofkiran\ndafont.com",
                        new Vector2(1250, _graphics.PreferredBackBufferHeight / 3),
                        Color.White);

                    _spriteBatch.DrawString(
                        cat_comic_36,
                        "Tutorials:\nGameFrom\nScratch.com\nQuique1222",
                        new Vector2(1600, _graphics.PreferredBackBufferHeight / 3),
                        Color.White);

                    break;
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Creates a fortnite number popup
        /// </summary>
        /// <param name="score">the score to be displayed</param>
        public void CreateScorePopups(int score)
        {
            scorePopups.Add(new ScorePopup(
                score,
                cat_comic_36, 
                playerStick.Position, 
                _spriteBatch));
        }

        //SOUND METHODS


        /// <summary>
        /// used for audio from tutorial
        /// https://gamefromscratch.com/monogame-tutorial-audio/
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MediaPlayer_MediaStateChanged(object sender, System.
                                   EventArgs e)
        {
            // 0.0f is silent, 1.0f is full volume
            MediaPlayer.Volume -= 0.1f;
            MediaPlayer.Play(upbeatJazz);
        }

        public void Reset()
        {
            numberOfCompletedOrder = 0;
            perfectOrdersInARow = 0;
            timer = 120;
            debugMode = false;
            
            ingredientManager.Reset();
            orderManager.Reset();
        }
        

    }
}