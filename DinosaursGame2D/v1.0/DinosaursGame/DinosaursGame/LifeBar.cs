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
    public class LifeBar : Actor
    {
        int maximum;
        int current;
        Actor player; // Actor which this health bar is attached to
        Texture2D background; // Secondary texture for the background of the bar
        
        /// <summary>
        /// Constructs the healthbar and attaced it to the player
        /// </summary>
        /// <param name="thePlayer"></param>
        /// <param name="max"></param>
        public LifeBar(Actor thePlayer, int max = 75)
        {
            player = thePlayer;
            current = player.Score;
            maximum = max;

            Position = player.Position;
            OffsetPos = new Vector2(-50, -30);

            Scale = new Vector2(1.0f, 0.5f);
        }

        /// <summary>
        /// Loads the textures required for the life bar
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            Texture = content.Load<Texture2D>("textures/mainHealthBar1");
            background = content.Load<Texture2D>("textures/mainHealthBar2");
        }

        /// <summary>
        /// Updates the size of the life bar
        /// </summary>
        public void Update()
        {
            Position = player.Position;

            if (player.Score > maximum)
            {
                player.Score = maximum;
            }
            current = player.Score;
            Scale.X = (float)(current / (maximum * 1.0));
        }

        /// <summary>
        /// Disolays the life bar on the screen.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void DrawLife(SpriteBatch spriteBatch)
        {
            // Draw the background
            spriteBatch.Draw(
                background,
                Position + OffsetPos,
                null,
                Color,
                0,
                Vector2.Zero,
                new Vector2(1.0f, Scale.Y),
                SpriteEffects.None,
                0);

            // Draw the top layer
            spriteBatch.Draw(
                Texture,
                Position + OffsetPos,
                null,
                Color,
                0,
                Vector2.Zero,
                Scale,
                SpriteEffects.None,
                0);
        }

        /// <summary>
        /// Increases the maximum amount in the life bar
        /// </summary>
        /// <param name="amount"></param>
        public void increaseMax(int amount)
        {
            // First increase current health, then 
            // add to the maximum health.
            current += amount;
            //if (current > maximum)
            //{
            //    current = maximum;
            //}
        }
    }
}
