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

        private bool prevIsKeyDown; // References the space bar

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            data = new GameData(this);
            currentState = State.Welcome;

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
            graphics.IsFullScreen = true;
            //graphics.IsFullScreen = false;
            
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
            
            /* State Machine */
            if (currentState == State.Welcome && 
                data.welcome2main)
            {
                currentState = State.Main;
                currentScreen = new MainScreen(data);
                currentScreen.Initialize();
                currentScreen.LoadContent(Content);
                
                data.welcome2main = false;

                this.IsMouseVisible = false;
            }

            if (currentState == State.Main
                && data.main2bonus)
            {
                currentState = State.Bonus;
                currentScreen = new BonusScreen(data);
                currentScreen.Initialize();
                currentScreen.LoadContent(Content);

                data.main2bonus = false;

                this.IsMouseVisible = true;
            }

            if (currentState == State.Main
                && data.main2playagain)
            {
                currentState = State.PlayAgain;
                currentScreen = new PlayAgainScreen(data);
                currentScreen.Initialize();
                currentScreen.LoadContent(Content);

                data.main2playagain = false;

                this.IsMouseVisible = true;
            }

            if (currentState == State.Bonus
                && data.bonus2main)
            {
                currentState = State.Main;
                currentScreen = new MainScreen(data, false);
                currentScreen.Initialize();
                currentScreen.LoadContent(Content);

                data.bonus2main = false;

                this.IsMouseVisible = false;
            }

            if (currentState == State.Bonus
                && data.bonus2playagain)
            {
                currentState = State.PlayAgain;
                currentScreen = new PlayAgainScreen(data);
                currentScreen.Initialize();
                currentScreen.LoadContent(Content);

                data.bonus2playagain = false;

                this.IsMouseVisible = true;
            }

            if (currentState == State.PlayAgain
                && data.playagain2welcome)
            {
                currentState = State.Welcome;
                currentScreen = new WelcomeScreen(data);
                currentScreen.Initialize();
                currentScreen.LoadContent(Content);
                
                data.playagain2welcome = false;

                this.IsMouseVisible = true;
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
            currentScreen.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
