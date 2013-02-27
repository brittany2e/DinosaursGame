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
    class WelcomeScreen : Screen
    {
        Texture2D background;
        MyButton playButton;
		MyButton optionsButton;
		MyButton creditsButton;
        GameData data;

        public WelcomeScreen(GameData theData)
        {
            data = theData;

            Vector2 buttonPos = new Vector2(0.5f, 0.6f);
            playButton = new MyButton(data, buttonPos);
			
			buttonPos = new Vector2(0.5f, 0.75f);
            optionsButton = new MyButton(data, buttonPos);
			
			buttonPos = new Vector2(0.5f, 0.9f);
            creditsButton = new MyButton(data, buttonPos);
        }

        public void Initialize()
        {

        }

        public void LoadContent(ContentManager content)
        {
            // Background texture
            background =
                content.Load<Texture2D>("textures/welcomeBackground_OLD");
            
            // Button texture and button placement
            playButton.LoadContent(content, "textures/welcomePlay");
			optionsButton.LoadContent(content, "textures/welcomeOptions");
			creditsButton.LoadContent(content, "textures/welcomeCredits");
            
            // Background music
            if (data.backgroundMusic == null)
            {
                data.backgroundMusic =
                    content.Load<SoundEffect>("sounds/mainMusic").CreateInstance();
                data.backgroundMusic.Volume = 0.2f;
                data.backgroundMusic.IsLooped = true;
            }
            data.changeMusic(data.backgroundMusic);
        }

        public void Update(GameTime gameTime)
        {
            // Check mouse state
            MouseState ms = Mouse.GetState();
            if (playButton.validLeftClick(ms))
            {
                data.welcome2main = true;
            }
			if (optionsButton.validLeftClick(ms))
            {
                // Display options
            }
			if (creditsButton.validLeftClick(ms))
            {
                // Display credits
            }
        }

        public void Draw(SpriteBatch sb)
        {
            // Draw the background first
            Rectangle screenSize = 
                new Rectangle(0, 0, data.screenWidth, data.screenHeight);
            sb.Draw( background, screenSize, Color.White);

            // Draw the buttons
            playButton.Draw(sb);
			optionsButton.Draw(sb);
			creditsButton.Draw(sb);
        }
    }
}
