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
    class Marker
    {
        private Texture2D texture;
        private Vector2 position;
        private float scale;
        private bool isActive; // determines if it is drawn or not

        /// <summary>
        /// Constructs a new marker with default scale 0.25.
        /// </summary>
        public Marker()
        {
            scale = 0.25f;
            isActive = false;
        }

        /// <summary>
        /// Load the texture for the cursor.
        /// </summary>
        /// <param name="content"></param>
        public void Load(ContentManager content)
        {
            texture = content.Load<Texture2D>("textures/mainFootprint2");
        }

        /// <summary>
        /// The marker should set at the given location in screen coordinates.
        /// </summary>
        /// <param name="mousePos">Position in screen coordinates
        /// </param>
        public void setPosition(int x, int y)
        {
            position = new Vector2(x, y);
            isActive = true;
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
            
            if (isActive)
            {
                sb.Draw(texture, position, null, Color.White, 0, new Vector2(), scale, SpriteEffects.None, 0);
            }
        }

        /// <summary>
        /// Disables the marker by not drawing it anymore.
        /// </summary>
        public void Disable()
        {
            isActive = false;
        }
    }
}
