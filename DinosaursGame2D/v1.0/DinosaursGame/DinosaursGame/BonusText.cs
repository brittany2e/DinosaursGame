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
    class BonusText
    {
        // The text is split into three different parts.
        // The first and last parts stay the same, but the 
        // middle part changes depending on the current 
        // objective.
        private Texture2D textBeginning;
        private List<Texture2D> textObjective;
        private Texture2D textEnd;

        private int screenWidth;
        private int screenHeight;

        private int index;

        /// <summary>
        /// Constructs the Bonus Text
        /// </summary>
        /// <param name="data"></param>
        /// <param name="theIndex"></param>
        public BonusText(GameData data, int theIndex)
        {
            screenWidth = data.screenWidth;
            screenHeight = data.screenHeight;
            index = theIndex;
        }

        /// <summary>
        /// Loads the beginning and ending textures and all posible 
        /// textures for the objective 
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            textBeginning = Load(content, "Beginning");

            textObjective = new List<Texture2D>();
            textObjective.Add(Load(content, "Red"));
            textObjective.Add(Load(content, "Orange"));
            textObjective.Add(Load(content, "Yellow"));
            textObjective.Add(Load(content, "Green"));
            textObjective.Add(Load(content, "Blue"));
            textObjective.Add(Load(content, "Purple"));

            textEnd = Load(content, "End");
        }

        /// <summary>
        /// This method was made as a wrapper for loading textures. Now
        /// textures can be loaded on one line. :)
        /// </summary>
        /// <param name="content">Usual Content required to load textures</param>
        /// <param name="filename">Specifies which texture to load</param>
        /// <returns></returns>
        private Texture2D Load(ContentManager content, string name)
        {
            return content.Load<Texture2D>(("textures/bonusText" + name));
        }

        /// <summary>
        /// Displays the textures on the screen in the correct positions.
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {
            Texture2D begin = textBeginning;
            Texture2D middle = textObjective.GetRange(index, 1)[0];
            Texture2D end = textEnd;

            int lengthOfSentence = begin.Width + middle.Width + end.Width;
            int height = screenHeight * 8 / 9;

            // Draw the first part of the sentence
            int position = (screenWidth - lengthOfSentence) / 2;
            sb.Draw(begin, new Vector2(position, height), Color.White);

            // Draw the middle section (objective)
            position += begin.Width;
            sb.Draw(middle, new Vector2(position, height), Color.White);

            // Draw the end part of the sentence
            position += middle.Width;
            sb.Draw(end, new Vector2(position, height), Color.White);
        }


    }
}
