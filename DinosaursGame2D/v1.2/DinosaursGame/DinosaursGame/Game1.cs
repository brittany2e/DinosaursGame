using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace DinosaursGame
{
    /// <summary>
    /// Each game state has an associated screen
    /// </summary>
    enum State
    {
        Welcome,
        Main,
        Bonus,
        PlayAgain
    };
    
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private State currentState;
        private Screen currentScreen;
        private GameData data;
        private int level;
        private Boolean loading;
        private Texture2D loadingScreen;
        private int numFrames;
        private int maxLoadingFrames;

        private bool prevIsKeyDown; // References the space bar

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            data = new GameData(this);
            currentState = State.Welcome;
            level = 0;
            loading = false;
            numFrames = 0;
            maxLoadingFrames = 50;

            prevIsKeyDown = false;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //graphics.IsFullScreen = true;
            graphics.IsFullScreen = false;
            
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            data.screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            data.screenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            
            this.IsMouseVisible = true;
            graphics.ApplyChanges();
            Window.Title = "Dinosaurs Game";

            currentScreen = new WelcomeScreen(data);
            currentScreen.Initialize();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            loadingScreen =
                Content.Load<Texture2D>("textures/loading");
            currentScreen.LoadContent(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // If the game is paused, don't update anything.
            if (Paused())
            {
                return;
            }
            if (loading)
            {
                if (numFrames < maxLoadingFrames)
                {
                    numFrames++;
                }
                else
                {
                    loading = false;
                    numFrames = 0;
                }
                return;
            }
            
            /* State Machine */
            if (currentState == State.Welcome && data.welcome2main)
            {
                data.welcome2main = false;
                ChangeState(State.Main);
            }

            if (currentState == State.Main && data.main2bonus)
            {
                data.main2bonus = false;
                ChangeState(State.Bonus);
            }

            if (currentState == State.Main && data.main2playagain)
            {
                data.main2playagain = false;
                ChangeState(State.PlayAgain);
            }

            if (currentState == State.Bonus && data.bonus2main)
            {
                data.bonus2main = false;
                ChangeState(State.Main);
            }

            if (currentState == State.Bonus && data.bonus2playagain)
            {
                data.bonus2playagain = false;
                ChangeState(State.PlayAgain);
            }

            if (currentState == State.PlayAgain && data.playagain2welcome)
            {
                data.playagain2welcome = false;
                ChangeState(State.Welcome);
            }

            if (currentState == State.PlayAgain
                && data.playagain2exit)
            {
                this.Exit();
            }

            // In case we want to exit from anywhere in the game,
            // Emergency exit is E+<Esc>
            // Check keyboard state
            KeyboardState kb = Keyboard.GetState();
            if (kb.IsKeyDown(Keys.E) && kb.IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }


            currentScreen.Update(gameTime);
            
            base.Update(gameTime);
        }

        private void ChangeState(State newState)
        {
            if (newState == State.Welcome)
            {
                currentScreen = new WelcomeScreen(data);
                this.IsMouseVisible = true;
                if (level != 0)
                {
                    loading = true;
                    level = 0;
                }
            }
            else if (newState == State.Main)
            {
                level++;
                if (level == 4)
                {
                    ChangeState(State.PlayAgain);
                    return;
                }
                else
                {
                    currentScreen = new MainScreen(data, level);
                    this.IsMouseVisible = false;
                }
                loading = true;
            }
            else if (newState == State.Bonus)
            {
                currentScreen = new BonusScreen(data);
                this.IsMouseVisible = true;
            }
            else if (newState == State.PlayAgain)
            {
                if(level == 4)
				{
					currentScreen = new PlayAgainScreen(data, true);
				}
				else
				{
					currentScreen = new PlayAgainScreen(data);
				}
                this.IsMouseVisible = true;
            }
            else
            {
                currentScreen = new WelcomeScreen(data);
                System.Console.WriteLine("ERROR: New State not implemented. (ChangeState in Game1.cs)");
            }

            currentState = newState;
            currentScreen.Initialize();
            currentScreen.LoadContent(Content);
        }

        /// <summary>
        /// Checks to see if the game is currently paused or not. The space bar
        /// toggles if the game is paused or not.
        /// </summary>
        /// <returns>Whether the game is currently paused or not.</returns>
        private bool Paused()
        {
            KeyboardState kb = Keyboard.GetState();

            // The space bar controls pause state.
            if (kb.IsKeyDown(Keys.Space))
            {
                // We only want to change state when the space bar is initially
                // pressed. (i.e. previously not down)
                if (!prevIsKeyDown)
                {
                    data.paused = !data.paused; // toggle
                }
                prevIsKeyDown = true;
            }
            else
            {
                prevIsKeyDown = false;
            }

            return data.paused;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            if (loading)
            {
                spriteBatch.Draw(
                    loadingScreen,
                    new Rectangle(0, 0, data.screenWidth, data.screenHeight),
                    Color.White);
            }
            else
            {
                currentScreen.Draw(spriteBatch);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
