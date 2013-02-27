using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

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
    /// This is a game component and is pretty much my version of a 
    /// drawable game component, except the Update and Draw methods 
    /// are called manually from the Game1.cs file.
    /// </summary>
    public class Actor 
    {
        protected GameData data; // Shared game data

        public Vector2 Position; // Position in screen coords
        protected Vector2 OffsetPos; // Added to position
        protected bool IsAlive; // Determines if you should draw it or not
        protected Color Color;
        public int Score;
        public Vector2 Direction;
        protected bool FacingLeft; // Helps when drawing the texture
        public Texture2D Texture;
        protected Color[,] TextureArray; // Used for better collision detection
        public Animator Animation; // Controls texture swapping for animations
        protected Vector2 Scale;
        protected Rectangle Bound; // Bounding box 

        protected bool hasLife; // Denotes whether to make health bar
        public LifeBar life;

        protected const int playerSpeed = 3;
        protected Vector2 destination; // In screen coords
        private Vector2 accuracy; // Used to avoid floating point comparisons

        Random numGenerator;
        
        /// <summary>
        /// Constructs actor without a healthbar, unless otherwise specified.
        /// </summary>
        /// <param name="hasLifeBar">Optional paramter to display a health bar 
        /// above the actor.</param>
        public Actor(bool hasLifeBar = false)
        {
            Position = Vector2.Zero;
            OffsetPos = Vector2.Zero;
            IsAlive = true;
            Color = Color.White;
            Score = 0;
            Direction = Position;
            FacingLeft = false;
            Scale = new Vector2(1.0f, 1.0f);
            destination = Position;
            accuracy = new Vector2(5, 5);

            numGenerator = new Random();

            hasLife = hasLifeBar;
            if (hasLifeBar)
            {
                life = new LifeBar(this);
            }
        }

        /// <summary>
        /// Returns destination of the actor in screen coords
        /// </summary>
        /// <returns></returns>
        public Vector2 getDestination()
        {
            return destination;
        }

        /// <summary>
        /// Sets the players position in screen coords
        /// </summary>
        /// <param name="x">The horizontal component</param>
        /// <param name="y">The verticle component</param>
        public void SetPosition(int x, int y)
        {
            Position = new Vector2(x, y);
        }

        public Vector2 GetPosition()
        {
            return Position;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public void Initialize()
        {
            // TODO: Add your initialization code here
        }

        /// <summary>
        /// Uses the position, offset and texture size to generate a bounding box
        /// </summary>
        protected void CreateBoundingBox()
        {
            // Now that we set the position, create the bounding box
            Bound = new Rectangle(
                (int)(Position.X + OffsetPos.X),
                (int)(Position.Y + OffsetPos.Y),
                Texture.Width,
                Texture.Height);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public virtual void Update(GameTime gameTime)
        {
            // Move toward destination
            if (isCloseEnough(Position, destination))
            {
                data.currentPlayer.Direction = Vector2.Zero;
            }
            else
            {
                Position += Direction;
            }
          
            Bound.X = (int)(Position.X + OffsetPos.X);
            Bound.Y = (int)(Position.Y + OffsetPos.Y);

            if (hasLife)
            {
                life.Update();
            }
        }

        /// <summary>
        /// Sometimes, we don't need perfect collision, we just want to be 
        /// close enough.
        /// </summary>
        /// <param name="Position">First point</param>
        /// <param name="destination">Second point</param>
        /// <returns></returns>
        protected bool isCloseEnough(Vector2 Position, Vector2 destination)
        {
            return Math.Abs(Position.X - destination.X) < accuracy.X &&
                   Math.Abs(Position.Y - destination.Y) < accuracy.Y;
        }

        /// <summary>
        /// Draws this actor, if they are currently alive
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (IsAlive)
            {
                SpriteEffects isLeft = SpriteEffects.None;
                if (Direction.X < 0)
                    isLeft = SpriteEffects.FlipHorizontally;

                spriteBatch.Draw(
                    Texture,
                    Position + OffsetPos,
                    null,
                    Color,
                    0,
                    Vector2.Zero,
                    Scale,
                    isLeft,
                    0);

                if (hasLife)
                {
                    life.DrawLife(spriteBatch);
                }
            }

        }

        /// <summary>
        /// The main collisions we care about are with the player. This method
        /// returns if they have collided with the given actor or not
        /// </summary>
        /// <param name="p2"></param>
        /// <returns></returns>
        public bool CheckPlayersCollision(Actor p2)
        {
            bool trueCollisionDetected = false;

            // First check if the texture bounding boxes intersect
            Rectangle overlap = Rectangle.Intersect(Bound, p2.Bound);

            // If collision exists, check pixels for real collision
            if (!overlap.IsEmpty)
            {
                trueCollisionDetected =
                    TexturesCollide(TextureArray, Position,
                    p2.TextureArray, p2.Position);
            }

            return !overlap.IsEmpty;

        }

        /// <summary>
        /// Checks to see if the player has collided with the given target.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool CheckPlayersCollision(Target target)
        {
            Rectangle overlap = Rectangle.Intersect(Bound, target.GetBound());
            return !overlap.IsEmpty;
        }

        /// <summary>
        /// Looks through the textures pixel by pixel to determine if there is a 
        /// true collision of the textures.
        /// </summary>
        /// <param name="tex1">First actor's texture</param>
        /// <param name="pos1">First actor's position</param>
        /// <param name="tex2">Second actor's texture</param>
        /// <param name="pos2">Second actor's position</param>
        /// <returns></returns>
        private bool TexturesCollide(Color[,] tex1, Vector2 pos1,
            Color[,] tex2, Vector2 pos2)
        {
            bool isTrueCollision = false;
            int width1 = tex1.GetLength(0);
            int height1 = tex1.GetLength(1);
            int width2 = tex2.GetLength(0);
            int height2 = tex2.GetLength(1);
            int offsetX = (int)(pos2.X - pos1.X);
            int offsetY = (int)(pos2.Y - pos1.Y);

            for (int x1 = 0; x1 < width1; x1++)
            {
                for (int y1 = 0; y1 < height1; y1++)
                {
                    int x2 = x1 - offsetX;
                    int y2 = y1 - offsetY;

                    if ((x2 >= 0) && (x2 < width2) &&
                        (y2 >= 0) && (y2 < height2))
                    {
                        if (tex1[x1, y1].A > 0 &&
                            tex2[x2, y2].A > 0)
                        {
                            isTrueCollision = true;
                        }
                    }
                }
            }
            return isTrueCollision;
        }

        /// <summary>
        /// Based on actor's speed, moves character's position slightly to the 
        /// left
        /// </summary>
        public void MoveLeft()
        {
            Direction.X = -playerSpeed;
            FacingLeft = true;
        }

        /// <summary>
        /// Based on actor's speed, moves character's position slightly to the 
        /// right
        /// </summary>
        public void MoveRight()
        {
            Direction.X = playerSpeed;
            FacingLeft = false;
        }

        /// <summary>
        /// Based on actor's speed, moves character's position slightly 
        /// upward
        /// </summary>
        public void MoveUp()
        {
            Direction.Y = playerSpeed;
        }

        /// <summary>
        /// Based on actor's speed, moves character's position slightly 
        /// downward
        /// </summary>
        public void MoveDown()
        {
            Direction.Y = -playerSpeed;
        }

        /// <summary>
        /// Send the player towards the given destination.
        /// </summary>
        /// <param name="x">Horizontal destination in screen coordinates</param> 
        /// <param name="y">Verticle destination in screen coordinates</param> 
        /// <param name="speed"></param>
        public void MoveToward(int x, int y, int speed = playerSpeed)
        {
            // Normalize the distance vector between the
            // current position and the destination position.
            float angle = (float)Math.Atan((y - Position.Y) / (x - Position.X));

            if (x < Position.X)
            {
                angle += (float)Math.PI;
            }

            Direction.X = (float)(speed * Math.Cos(angle));
            Direction.Y = (float)(speed * Math.Sin(angle));
        }
    
        /// <summary>
        /// Send the player towards the given destination. The player will stop when
        /// within a certain accuracy of the destination.
        /// </summary>
        /// <param name="x"></param> Horizontal destination in screen coordinates
        /// <param name="y"></param> Verticle destination in screen coordinates
        /// <param name="speed"></param>
        public void GoTo(int x, int y, int speed = playerSpeed)
        {
            MoveToward(x, y, speed);
            destination = new Vector2(x, y);
        }

        public void KeepInBounds(int screenWidth,
            int screenHeight,
            Boolean bounce = false)
        {
            int xMin = (int)Position.X;
            int xMax = (int)Position.X + Texture.Width;
            int yMin = (int)Position.Y;
            int yMax = (int)Position.Y + Texture.Width;

            if (bounce) // Enemies bounce
            {
                if (xMax > screenWidth) // hit right wall
                {
                    Direction = randomDirection(3.14 / 2, 3 * 3.14 / 2);
                }
                if (xMin < 0) // hit left wall
                {
                    Direction = randomDirection(-3.14 / 2, 3.14 / 2);
                }
                if (yMax > screenHeight) // hit bottom
                {
                    Direction = randomDirection(3.14, 2 * 3.14);
                }
                if (yMin < 0) // hit top
                {
                    Direction = randomDirection(0, 3.14);
                }

            }
            else // Player Case
            {
                if (xMax > screenWidth || xMin < 0)
                {
                    Direction.X = -Direction.X;
                }
                if (yMax > screenHeight || yMin < 0)
                {
                    Direction.Y = -Direction.Y;
                }
            }
        }

        /// <summary>
        /// Change the actor to go in a random direction within the given range.
        /// The default is 0 to 6.28 radians.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        protected Vector2 randomDirection(double min = 0, double max = 6.28)
        {
            double angle = numGenerator.NextDouble() * (max - min) + min;
            float x = (float)Math.Cos(angle);
            float y = (float)Math.Sin(angle);

            return new Vector2(x, y);
        }

        /// <summary>
        /// Given a texture returns a 2D array of colors
        /// </summary>
        /// <param name="texture"></param>
        /// <returns></returns>
        protected Color[,] TextureTo2DArray(Texture2D texture)
        {
            Color[] colors1D = new Color[texture.Width * texture.Height];
            texture.GetData(colors1D);

            Color[,] colors2D = new Color[texture.Width, texture.Height];
            for (int x = 0; x < texture.Width; x++)
            {
                for (int y = 0; y < texture.Height; y++)
                {
                    colors2D[x, y] = colors1D[x + y * texture.Width];
                }
            }
            return colors2D;
        }
    }


}
