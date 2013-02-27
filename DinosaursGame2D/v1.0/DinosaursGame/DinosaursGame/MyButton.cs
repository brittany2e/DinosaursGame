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
    public class MyButton
    {
        protected Vector2 position;
        protected Texture2D texture;
        
        /// <summary>
        /// Creates a custom button with the given position and texture.
        /// </summary>
        /// <param name="theData"></param> The global game data 
        /// <param name="thePosition"></param> The desired position of the center 
        /// of the button relative to the screen width and screen hight
        /// <param name="theTexture"></param>
        public MyButton(GameData theData, Vector2 thePosition)
        {
            position = new Vector2();
            position.X = thePosition.X * theData.screenWidth;
            position.Y = thePosition.Y * theData.screenHeight;
        }

        /// <summary>
        /// Load the texture is the given filename as the texture for 
        /// this button
        /// </summary>
        /// <param name="content"></param>
        /// <param name="filename"></param>
        public void LoadContent(ContentManager content, String filename)
        {
            texture = content.Load<Texture2D>(filename);

            // Now that we know the size of the texture, we can adjust the
            // positon of the target.
            position.X -= texture.Width / 2;
            position.Y -= texture.Height / 2;
        }

        /// <summary>
        /// Returns if the user clicked on the button texture or not
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public bool validLeftClick(MouseState ms)
        {
            return (ms.LeftButton == ButtonState.Pressed &&
                ms.X > position.X &&
                ms.X < (position.X + texture.Width) &&
                ms.Y > position.Y && 
                ms.Y < (position.Y + texture.Height));
        }

        /// <summary>
        /// Returns the upper-left position of the button. Useful for drawing.
        /// </summary>
        /// <returns></returns>
        public Vector2 getPosition()
        {
            return position;
        }

        /// <summary>
        /// Set the button to the specified position in screen coords
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void setPosition(float x, float y)
        {
            position = new Vector2(x, y);
        }

        /// <summary>
        /// Draws the button with a default color of White.
        /// </summary>
        /// <param name="sb"></param>
        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw( texture, position, Color.White );
        }

        public bool isOnButton(Vector2 pos)
        {
            return pos.X > position.X &&
                pos.X < (position.X + texture.Width) &&
                pos.Y > position.Y && 
                pos.Y < (position.Y + texture.Height);
        }
    }
}
