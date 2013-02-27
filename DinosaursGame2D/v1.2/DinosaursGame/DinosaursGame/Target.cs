using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Custom button class
    /// </summary>
    public class Target : MyButton
    {
        private bool isAlive; // Whether it should be drawn or not
        private bool isLeft; // Toggle for flipping left
        private bool isDropped; // Toggle for dropping
        private int score;
        private Color color; // Color tint while drawing
        private float angle;

        private Color objective; // correct color of bonus area
        
        private int prevTime; // used for animaitions
        
        // Sounds
        private SoundEffectInstance winSound;
        private SoundEffectInstance failSound;

        /// <summary>
        /// Constructs a target
        /// </summary>
        /// <param name="theData">Global game data</param>
        /// <param name="thePosition"></param>
        /// <param name="theColor">Specified color</param>
        /// <param name="theObjective">Correct objective color</param>
        public Target(GameData theData, Vector2 thePosition, Color theColor, Color theObjective)
            : base(theData, thePosition)
        {
            isAlive = true;
            isLeft = false;
            isDropped = false;
            score = 24;
            color = theColor;
            objective = theObjective;
            angle = 0;
            prevTime = -1;
        }

        /// <summary>
        /// Randomize the angle of the target
        /// </summary>
        public void setAngle()
        {
            Random rand = new Random();
            int degrees = rand.Next(40) - 20;
            angle = (float) (degrees*Math.PI/180.0);
        }

        /// <summary>
        /// Loads the texture and sounds for the target
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            base.LoadContent(content, "textures/bonusLeaf");

            winSound =
                content.Load<SoundEffect>("sounds/bonusCorrect").CreateInstance();
            winSound.Volume = 1.0f;
            winSound.IsLooped = false;

            failSound =
                content.Load<SoundEffect>("sounds/bonusIncorrect").CreateInstance();
            failSound.Volume = 1.0f;
            failSound.IsLooped = false;
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            // Sound and/or animation if correct leaf.
        }

        /// <summary>
        /// Displays the target to the screen at the correct positon
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isAlive)
            {
                // Logic for left or right facing
                SpriteEffects direction = SpriteEffects.None;
                if (isLeft)
                {
                    direction = SpriteEffects.FlipHorizontally;
                }

                // Logic for dropped or not
                Vector2 offset = Vector2.Zero;
                if (isDropped)
                {
                    offset = new Vector2(0, 4);
                }

                // Draw the target
                spriteBatch.Draw(
                    texture,
                    GetBound(),
                    null,
                    color,
                    angle,
                    Vector2.Zero,
                    direction,
                    0);
            }
        }

        /// <summary>
        /// Returns the bounding rectangle for this target
        /// </summary>
        /// <returns></returns>
        public Rectangle GetBound()
        {
            return new Rectangle(
                (int)(position.X),
                (int)(position.Y),
                texture.Width,
                texture.Height);
        }

        /// <summary>
        /// The given plant will disappear, and the amount of points
        /// earned will be returned.
        /// </summary>
        /// <returns></returns>
        public int select()
        {
            int returnScore = 0;
            if (isCorrect())
            {
                returnScore = score;
                isAlive = false;
            }
            return returnScore;
        }

        /// <summary>
        /// Animate and play the sound for the current target
        /// </summary>
        /// <param name="gameTime"></param>
        public void Animate(GameTime gameTime)
        {
            int curTime = gameTime.TotalGameTime.Milliseconds;
            if (prevTime == -1) prevTime = curTime;

            if ((curTime - prevTime) > 200 ||
                (curTime - prevTime) == 0)
            {
                prevTime = curTime;
                if (isCorrect())
                {
                    isLeft = !isLeft; // toggle
                }
                else
                {
                    isDropped = !isDropped;
                }
            }
            if (isAlive)
            {
                playSound();
            }
        }

        /// <summary>
        /// Starts the sound file
        /// </summary>
        public void playSound()
        {
            if (isAlive)
            {
                if (isCorrect())
                {
                    winSound.Play();
                }
                else
                {
                    failSound.Play();
                }
            }
        }

        /// <summary>
        /// Returns if this target matches the objective 
        /// </summary>
        /// <returns></returns>
        public bool isCorrect()
        {
            return color.Equals(objective);
        }

        /// <summary>
        /// Sets this target to match the objective
        /// </summary>
        public void setAsWinner()
        {
            color = objective;
        }

        /// <summary>
        /// Flips the target over the y-axis
        /// </summary>
        public void Flip()
        {
            isLeft = true;
        }

        /// <summary>
        /// Returns whether the target has been chosen yet, meaning it has
        /// disappeared from the bonus level. Targets can only disappear
        /// if the target matches the objective.
        /// </summary>
        /// <returns>True if the target is correct and invisible.</returns>
        public bool hasBeenChosen()
        {
            return isCorrect() && !isAlive;
        }
    }
}
