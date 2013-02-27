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
    class BonusScreen : Screen
    {
        public GameData data;

        // Textures
        Texture2D background;
        BonusText text;
        
        // Target Stuff
        List<Target> targets;
        List<Target> winners; // subset of targets
        int numberOfTargets;
        int objectiveIndex;
        Target currentTarget;
        
        // Player Stuff
        BonusPlayer player;
        
        /// <summary>
        /// Constructs new bonus screen
        /// </summary>
        /// <param name="theData"></param>
        public BonusScreen(GameData theData)
        {
            data = theData;
            player = new BonusPlayer(theData);
            numberOfTargets = 12;
            targets = new List<Target>();
            winners = new List<Target>();
            player = new BonusPlayer(data);
            currentTarget = null;
        }
        
        /// <summary>
        /// Sets up the targets and decides the objective
        /// </summary>
        public void Initialize()
        {
            SetUpTargets();
            text = new BonusText(data, objectiveIndex);
        }

        /// <summary>
        /// Loads all the textures required for the bonus area
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            background =
                content.Load<Texture2D>("textures/bonusBackground");

            text.LoadContent(content);

            player.LoadContent(content);

            for (int i = 0; i < numberOfTargets; i++)
            {
                targets[i].LoadContent(content);
            }

        }

        /// <summary>
        /// Places the targets on the screen
        /// </summary>
        public void SetUpTargets()
        {
            // Set the positions
            // Values gotten from clicking at the desired postions
            // and printing the values
            // Values are interpreted as percentage of screen size.
            List<Vector2> positions = new List<Vector2>();
            positions.Add(new Vector2(0.247f, 0.441f));
            positions.Add(new Vector2(0.235f, 0.334f));
            positions.Add(new Vector2(0.392f, 0.294f));
            positions.Add(new Vector2(0.338f, 0.403f));
            positions.Add(new Vector2(0.649f, 0.186f));
            positions.Add(new Vector2(0.774f, 0.276f));
            positions.Add(new Vector2(0.790f, 0.399f));
            positions.Add(new Vector2(0.662f, 0.432f));
            positions.Add(new Vector2(0.502f, 0.206f));
            positions.Add(new Vector2(0.321f, 0.250f));
            positions.Add(new Vector2(0.589f, 0.250f));
            positions.Add(new Vector2(0.514f, 0.311f));

            List<Color> colors = new List<Color>();
            colors.Add(Color.Red);
            colors.Add(Color.Orange);
            colors.Add(Color.Yellow);
            colors.Add(Color.Green);
            colors.Add(Color.Blue);
            colors.Add(Color.Purple);

            // First let's specify the color which we are looking for.
            Random rand = new Random();
            objectiveIndex = rand.Next(colors.Count);
            Color objective = colors[objectiveIndex];

            for (int i = 0; i < numberOfTargets; i++)
            {
                Color color = colors[rand.Next(colors.Count)];
                targets.Add(new Target(data, positions[i], color, objective));

                // Keep a list of all the correct leaves to know when we are done.
                if (targets[i].isCorrect())
                {
                    winners.Add(targets[i]);
                }

                // Add some variety to the leaves
                if (i % 2 == 0)
                {
                    targets[i].Flip();
                }
            }

            // Make sure at least two of the targets are correct
            if (winners.Count < 2)
            {
                // Set the first one as a winner.
                targets[0].setAsWinner();
                winners.Add(targets[0]);

                // Set the last target as a winner.
                int last = targets.Count - 1;
                targets[last].setAsWinner();
                winners.Add(targets[last]);
            }

        }

        /// <summary>
        /// Allows the game screen to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {
            KeyboardState kb = Keyboard.GetState();

            if (kb.IsKeyDown(Keys.M) || 
                isFinished())
            {
                data.bonus2main = true;
            }

            if (kb.IsKeyDown(Keys.Escape))
            {
                data.bonus2playagain = true;
            }

            ProcessMouse(gameTime);

            player.Update(gameTime);
            
        }

        /// <summary>
        /// Describes whether the bonus area has been completed or not
        /// </summary>
        /// <returns></returns>
        public bool isFinished()
        {
            bool isFinished = true;

            foreach (Target t in winners)
            {
                if(!t.hasBeenChosen())
                {
                    isFinished = false;
                }
            }
            return isFinished;
        }

        /// <summary>
        /// Displays the components on the bonus area to the screen
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {
            // Draw Background
            sb.Draw(
                background,
                new Rectangle( 0, 0, data.screenWidth, data.screenHeight),
                Color.White);

            // Draw the targets
            for (int i = 0; i < numberOfTargets; i++)
            {
                targets[i].Draw(sb);
            }

            // Draw the player
            player.Draw(sb);

            // Draw the Text
            text.Draw(sb);
        }

        /// <summary>
        /// Handle mouse event driven aspects of the bonus area
        /// </summary>
        /// <param name="gameTime"></param>
        private void ProcessMouse(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            
            for (int i = 0; i < numberOfTargets; i++)
            {
                if (targets[i].validLeftClick(mouseState))
                {
                    targets[i].Animate(gameTime);

                    if (targets[i].isCorrect() && !targets[i].hasBeenChosen())
                    {
                        player.GoTo(mouseState.X, mouseState.Y);
                        currentTarget = targets[i];
                    }
                }
                if (targets[i].Equals(currentTarget) &&
                    //player.reachedDestination(currentTarget.getPosition()))
                    player.CheckPlayersCollision(targets[i]))
                {
                    data.currentPlayer.life.increaseMax(targets[i].select());
                }
            }

        }
    }

}
