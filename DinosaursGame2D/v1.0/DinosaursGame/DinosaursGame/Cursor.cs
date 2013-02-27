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
    class Cursor
    {
        private Texture2D texture;
        private Vector2 position; // coord of the middle toe of texture
        private float scale;

        /// <summary>
        /// Contructs a scaled down cursor
        /// </summary>
        public Cursor(float theScale = 0.35f)
        {
            scale = theScale;
        }

        /// <summary>
        /// Load the texture for the cursor.
        /// </summary>
        /// <param name="content"></param>
        public void Load(ContentManager content)
        {
            texture = content.Load<Texture2D>("textures/mainFootprint1");
        }

        /// <summary>
        /// The cursor will follow the mouse, but for this particular texture,
        /// we want to offset it a little bit.
        /// </summary>
        /// <param name="mousePos">Position of the mouse in screen coordinates
        /// </param>
        public void Update(int x, int y)
        {
            // We want the cursor to follow the mouse
            position = new Vector2(x, y);

            // Except we want the "point" of the cursor to be the middle toe
            // on the texture
            position.X -= texture.Width * scale / 2;
        }

        /// <summary>
        /// Draws the cursor at the current position
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {
            Rectangle rect =
                new Rectangle(
                    (int)position.X, 
                    (int)position.Y,
                    (int)(texture.Width * scale), 
                    (int)(texture.Height * scale));
            //sb.Draw(texture, position, Color.White);
            sb.Draw(texture, position, null, Color.White, 0, new Vector2(), scale, SpriteEffects.None, 0);
        }
    }
}
