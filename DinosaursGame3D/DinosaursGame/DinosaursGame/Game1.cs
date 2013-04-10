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
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        KeyboardState prevState;

        Camera camera;
        Player player;
        DebugOut debug;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            this.IsMouseVisible = true;
            
            prevState = Keyboard.GetState();

            //camera = new Camera();

            LandscapeGenerator level = new LandscapeGenerator(this);
            level.Generate();

            // Add the player as a component, so the update and draw methods
            // are called.
            player = new Player(this, Vector3.Zero);//, new Vector3(0, -400, -1000));
            Components.Add(player);

            debug = new DebugOut(this);
            Components.Add(debug);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            // Analyze keyboard input
            keyboardInput();

            debug.setData("Player Position", string.Format("{0}", player.position));
            //debug.setData("Camera Position", string.Format("{0}", camera.position));

            base.Update(gameTime);
        }

        private void keyboardInput()
        {
            KeyboardState state = Keyboard.GetState();

            // Allows the game to exit
            if (state.IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }
            if (state.IsKeyDown(Keys.NumPad8))
            {
                player.goTo(Util.vec3sum(player.position, new Vector3(0, 0, 10)));
            }
            if (state.IsKeyDown(Keys.NumPad2))
            {
                player.goTo(Util.vec3sum(player.position, new Vector3(0, 0, -10)));
            }
            if (state.IsKeyDown(Keys.NumPad4))
            {
                player.goTo(Util.vec3sum(player.position, new Vector3(-10, 0, 0)));
            }
            if (state.IsKeyDown(Keys.NumPad6))
            {
                player.goTo(Util.vec3sum(player.position, new Vector3(10, 0, 0)));
            }

            prevState = state;

        }

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }

    }
}
