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
    /// <summary>
    /// This is a game component that extends Actor
    /// </summary>
    public class Enemy : Actor
    {
        public SoundEffectInstance roarSound;

        /// <summary>
        /// Contructs an enemy, aka hostile dinosaur
        /// </summary>
        /// <param name="theData"></param>
        public Enemy(GameData theData)
            : base()
        {
            data = theData;
            IsAlive = true;
            Color = Color.White;
            Score = 15;

            // Initialize Direction/Velocity
            Direction = randomDirection();

            FacingLeft = false;
        }

        /// <summary>
        /// Loads the textures and animations for the enemy
        /// </summary>
        /// <param name="content"></param>
        /// <param name="step"></param>
        public void LoadContent(ContentManager content, int step)
        {
            Texture2D trexTexture =
                content.Load<Texture2D>("textures/mainTrexGray");

            Color[,] colorArray = TextureTo2DArray(trexTexture);


            List<Texture2D> textures = new List<Texture2D>();
            if(step == 0)
            {
                textures.Add(content.Load<Texture2D>("textures/mainTrexGray1"));
                textures.Add(content.Load<Texture2D>("textures/mainTrexGray1"));
            }
            textures.Add(content.Load<Texture2D>("textures/mainTrexGray2"));
            textures.Add(content.Load<Texture2D>("textures/mainTrexGray0"));
            textures.Add(content.Load<Texture2D>("textures/mainTrexGray3"));
            textures.Add(content.Load<Texture2D>("textures/mainTrexGray0"));
            textures.Add(content.Load<Texture2D>("textures/mainTrexGray2"));
            textures.Add(content.Load<Texture2D>("textures/mainTrexGray0"));
            textures.Add(content.Load<Texture2D>("textures/mainTrexGray3"));

            // Texture and scale the dinosaur
            Animation = new Animator(textures);
            Animation.advance(step * 3);
            Texture = Animation.next();
            TextureArray = colorArray;

            CreateBoundingBox();

            roarSound =
                content.Load<SoundEffect>("sounds/mainTrexRoar").CreateInstance();

            roarSound.Volume = 1.0f;
            roarSound.IsLooped = false;
                
        }

        /// <summary>
        /// Plays the roar sound file
        /// </summary>
        public void Roar()
        {
            if (roarSound != null &&
                roarSound.State != SoundState.Playing)
            {
                roarSound.Play();
                
            }
        }
        
    }
}
