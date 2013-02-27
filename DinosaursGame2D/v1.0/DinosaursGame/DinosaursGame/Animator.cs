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
    public class Animator
    {
        List<Texture2D> textures;
        int count; // Total number of textures
        int index; // Current texture index
        bool shouldCycle;

        /// <summary>
        /// Constructs animatior with the list of textures.
        /// </summary>
        /// <param name="givenTextures"></param>
        /// <param name="wrap"></param>
        public Animator(List<Texture2D> givenTextures, bool wrap = true)
        {
            count = givenTextures.Count;
            index = 0;
            textures = givenTextures;

            // Some actors should not start back at the beginning.
            shouldCycle = wrap;
        }

        /// <summary>
        /// Appends the given texture to the end of the list of textures
        /// </summary>
        /// <param name="newTexture"></param>
        public void add(Texture2D newTexture)
        {
            textures.Add(newTexture);
            count++;
        }

        /// <summary>
        /// Cycles to the next texture
        /// </summary>
        /// <returns></returns>
        public Texture2D next()
        {
            if (count <= 0)
            {
                return null;
            }

            Texture2D toReturn = textures[index];

            // Increase the index without leaving the bounds
            // of the list.
            index = (index + 1) % count;
            
            return toReturn;
        }

        /// <summary>
        /// Skips the given number of textures
        /// </summary>
        /// <param name="steps"></param>
        public void advance(int steps)
        {
            index = (index + steps) % count;
        }

        /// <summary>
        /// Starts the animation back at the first texture in the list
        /// </summary>
        /// <returns></returns>
        public Texture2D reset()
        {
            index = 0;
            return textures[0];
        }
    }
}
