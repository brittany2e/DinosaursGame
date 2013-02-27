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
        GameData data;

        public WelcomeScreen(GameData theData)
        {
            data = theData;

            Vector2 buttonPos = new Vector2(0.5f, 0.9f);
            playButton = new MyButton(data, buttonPos);
        }

        public void Initialize()
        {

        }

        public void LoadContent(ContentManager content)
        {
            // Background texture
            background =
                content.Load<Texture2D>("textures/welcomeBackground");
            
            // Button texture and button placement
            playButton.LoadContent(content, "textures/welcomePlay");
            
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
        }

        public void Draw(SpriteBatch sb)
        {
            // Draw the background first
            Rectangle screenSize = 
                new Rectangle(0, 0, data.screenWidth, data.screenHeight);
            sb.Draw( background, screenSize, Color.White);

            // Draw the buttons
            playButton.Draw(sb);
        }
    }
}
