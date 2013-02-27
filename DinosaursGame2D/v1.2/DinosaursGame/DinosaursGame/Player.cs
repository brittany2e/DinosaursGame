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
    /// This is a game component that extends Actor.
    /// </summary>
    public class Player : Actor
    {
        /// <summary>
        /// Constructs a player
        /// </summary>
        /// <param name="theData"></param>
        public Player(GameData theData)
            : base(true)
        {
            data = theData;
            OffsetPos = new Vector2(-57, -14);
            FacingLeft = true;
            Scale = new Vector2(1.0f, 1.0f);

            Score = 25;
        }

        /// <summary>
        /// Loads the textures for the player and its animations
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            Texture2D dinosaur1Texture =
                content.Load<Texture2D>("textures/mainDinoPurple");

            Texture2D dinosaur2Texture =
                content.Load<Texture2D>("textures/mainDinoPurple");

            // Special texture arrays for collision detection
            Color[,] colorArray =
                TextureTo2DArray(dinosaur1Texture);

            List<Texture2D> textures = new List<Texture2D>();
            textures.Add(content.Load<Texture2D>("textures/mainDinoPurple0"));
            textures.Add(content.Load<Texture2D>("textures/mainDinoPurple1"));
            textures.Add(content.Load<Texture2D>("textures/mainDinoPurple0"));
            textures.Add(content.Load<Texture2D>("textures/mainDinoPurple2"));
            
            // Texture and scale the dinosaur
            Animation = new Animator(textures);
            Texture = Animation.next();
            TextureArray = colorArray;

            CreateBoundingBox();

            life.LoadContent(content);
        }

    }
}
