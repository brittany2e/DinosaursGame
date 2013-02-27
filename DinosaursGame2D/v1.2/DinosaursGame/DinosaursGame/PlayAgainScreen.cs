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
    class PlayAgainScreen : Screen
    {
        GameData data;
		Boolean isWinner;

        // Textures
        Texture2D background;

        // Buttons
        MyButton yesButton;
        MyButton noButton;
		MyButton winner;

        public PlayAgainScreen(GameData theData, Boolean isTheWinner = false)
        {
            data = theData;
			isWinner = isTheWinner;
            System.Console.Out.WriteLine("isWinner", isWinner);

            Vector2 yesPos = new Vector2(0.5f, 5 / 8f);
            yesButton = new MyButton(data, yesPos);

            Vector2 noPos = new Vector2(0.5f, 6 / 8f);
            noButton = new MyButton(data, noPos);
			
			Vector2 winnerPos = new Vector2(0.5f, 0.2f);
            winner = new MyButton(data, winnerPos);
        }
        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
            
        }

        /// <summary>
        /// Load the textures associated with the play again screen
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            // Background texture
            background =
                content.Load<Texture2D>("textures/playAgainBackground");
			
            // Button textures
            yesButton.LoadContent(content, "textures/playAgainYes");
            noButton.LoadContent(content, "textures/playAgainNo");
            winner.LoadContent(content, "textures/youWin");
			
            // Background music
            if (data.playAgainMusic == null)
            {
                data.playAgainMusic =
                    content.Load<SoundEffect>("sounds/playAgainMusic").CreateInstance();
                data.playAgainMusic.Volume = 0.1f;
                data.playAgainMusic.IsLooped = true;
            }
            data.changeMusic(data.playAgainMusic);
        }

        /// <summary>
        /// Update the state of the play again screen
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            MouseState ms = Mouse.GetState();

            if (yesButton.validLeftClick(ms))
            {
                data.playagain2welcome = true;
            }

            if (noButton.validLeftClick(ms))
            {
                data.playagain2exit = true;
            }
        }

        /// <summary>
        /// Draw everything on the screen
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {
            // Draw the background first
            sb.Draw(
                background,
                new Rectangle(0, 0, data.screenWidth, data.screenHeight),
                Color.White);
			
			if( isWinner )
			{
				winner.Draw(sb);
			}	
				
            // Draw the buttons
            yesButton.Draw(sb);
            noButton.Draw(sb);
        }
    }
}
