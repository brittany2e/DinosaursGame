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
        Player player;
        DebugOut debug;
        float fun = 8000;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            LandscapeGenerator level = new LandscapeGenerator(this);
            level.Generate();

            // Add the player as a component, so the update and draw methods
            // are called.
            player = new Player(this);//, new Vector3(0, -400, -1000));
            Components.Add(player);

            debug = new DebugOut(this);
            Components.Add(debug);

            debug.setData("Hello", "myFriend");
            //debug.addData("fun", 9001f);

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

            fun++;
            debug.setData("fun", string.Format("{0}", fun));

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
            if (state.IsKeyDown(Keys.Space))
            {
                player.goTo(new Vector3(400, 50, 1000));
            }
            if (state.IsKeyUp(Keys.Space))
            {
                player.goTo(Vector3.Zero);
            }

        }

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }

    }
}
