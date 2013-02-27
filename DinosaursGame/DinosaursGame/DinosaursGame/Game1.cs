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
        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

        }

        protected override void Initialize()
        {
            Player p1 = new Player(this, new Vector3(0, -400, -1000));
            Tree t1 = new Tree(this, new Vector3(400, 50, 1000));
            Shrubbery s1 = new Shrubbery(this, new Vector3(300, -150, 1000));

            // Add the player as a component, so the update and draw methods
            // are called.
            Components.Add(p1);
            Components.Add(t1);
            Components.Add(s1);

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
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }

        //http://www.i-programmer.info/projects/119-graphics-and-games/1108-getting-started-with-3d-xna.html?start=2
        protected VertexPositionNormalTexture[] MakePlane()
        {
            VertexPositionNormalTexture[] vertexes =
                 new VertexPositionNormalTexture[6];

            Vector2 Texcoords = new Vector2(0f, 0f);

            Vector3[] face = new Vector3[6];

            //TopLeft
            face[0] = new Vector3(-1f, 1f, 0.0f);
            //BottomLeft
            face[1] = new Vector3(-1f, -1f, 0.0f);
            //TopRight
            face[2] = new Vector3(1f, 1f, 0.0f);
            //BottomLeft
            face[3] = new Vector3(-1f, -1f, 0.0f);
            //BottomRight
            face[4] = new Vector3(1f, -1f, 0.0f);
            //TopRight
            face[5] = new Vector3(1f, 1f, 0.0f);

            //Top face
            Matrix RotX90 = Matrix.CreateRotationX(
                                -(float)Math.PI / 2f);
            for (int i = 0; i <= 2; i++)
            {
                vertexes[i] =
                  new VertexPositionNormalTexture(
                   Vector3.Transform(face[i], RotX90)
                    + Vector3.UnitY,
                    Vector3.UnitY, Texcoords);
                vertexes[i + 3] =
                  new VertexPositionNormalTexture(
                   Vector3.Transform(face[i + 3], RotX90)
                    + Vector3.UnitY,
                    Vector3.UnitY, Texcoords);
            }

            return vertexes;
        }
    }
}
