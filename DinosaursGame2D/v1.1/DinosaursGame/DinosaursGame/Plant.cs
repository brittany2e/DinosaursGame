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
    class Plant : Actor
    {
        public bool hasFlower;
        public Texture2D flowerTexture;
        private SoundEffectInstance eatSound;

        public Plant(GameData theData, bool flower = false)
        {
            data = theData;
            Color = Color.SpringGreen ;
            //Color = Color.PaleGreen;
            Score = 7;
            hasFlower = flower;
        }
        
        /// <summary>
        /// The given plant will dimminish, and the amount of points 
        /// earned will be returned. The sound of eating also plays.
        /// </summary>
        /// <returns></returns>
        public int eat()
        {
            Texture = Animation.next();

            if (IsAlive)
            {
                eatSound.Play();
            }

            Score--;
            if (Score <= 0)
            {
                Score = 0;
                IsAlive = false;
            }
            return Score;
        }

        /// <summary>
        /// Load the texture associated with the plant and its animation.
        /// If the plant has a flower, load that texture too.
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            // Texture and scale the plant
            Texture2D plantTexture =
                content.Load<Texture2D>("textures/mainPlant");

            Color[,] colorArray =
                base.TextureTo2DArray(plantTexture);

            List<Texture2D> textures = new List<Texture2D>();
            //textures.Add(content.Load<Texture2D>("textures/mainPlant0"));
            textures.Add(content.Load<Texture2D>("textures/mainPlant0"));
            textures.Add(content.Load<Texture2D>("textures/mainPlant1"));
            textures.Add(content.Load<Texture2D>("textures/mainPlant2"));
            textures.Add(content.Load<Texture2D>("textures/mainPlant3"));
            textures.Add(content.Load<Texture2D>("textures/mainPlant4"));
            textures.Add(content.Load<Texture2D>("textures/mainPlant5"));
            textures.Add(content.Load<Texture2D>("textures/mainPlant6"));
            textures.Add(content.Load<Texture2D>("textures/mainPlant6"));
           
            Animation = new Animator(textures);
            Texture = Animation.next();
            TextureArray = colorArray;

            CreateBoundingBox();

            flowerTexture = 
                content.Load<Texture2D>("textures/mainFlowerYellow");

            eatSound =
                content.Load<SoundEffect>("sounds/mainEating").CreateInstance();
            eatSound.Volume = 1.0f;
            eatSound.IsLooped = false;
                
        }

        /// <summary>
        /// Diplay the plant on the screen
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw the plant
            base.Draw(spriteBatch);

            if (hasFlower)
            {
                // Draw the flower
                spriteBatch.Draw(
                    flowerTexture,
                    new Rectangle(
                        (int)(Position.X + 13),
                        (int)(Position.Y + 25),
                        flowerTexture.Width,
                        flowerTexture.Height),
                    Color.White);
            }

        }

    }
}
