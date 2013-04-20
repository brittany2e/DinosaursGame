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
    /// This creates a level.
    /// </summary>
    public class LandscapeGenerator
    {
        Game game;

        public LandscapeGenerator(Game theGame)
        {
            game = theGame;
        }

        public void Generate()
        {
            Vector3 position = Vector3.Zero; // new Vector3(400, 50, 1000));
            setTree(position);
            setShrubbery(position); //new Vector3(position.X - 100, position.Y - 200, 1000)); //, new Vector3(300, -150, 1000));
        }

        private void setTree(Vector3 position)
        {
            Tree tree = new Tree(game, position);
            game.Components.Add(tree);
        }

        private void setShrubbery(Vector3 position)
        {
            Shrubbery shrub = new Shrubbery(game, position);
            game.Components.Add(shrub);
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
