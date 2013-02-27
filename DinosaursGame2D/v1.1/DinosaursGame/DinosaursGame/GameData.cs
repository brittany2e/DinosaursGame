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
    
    public class GameData
    {
        public int screenWidth;
        public int screenHeight;

        // Flags to set if it is ok to move to the 
        // next state (current2next)
        public bool welcome2main;
        public bool main2bonus;
        public bool main2playagain;
        public bool bonus2main;
        public bool bonus2playagain;
        public bool playagain2welcome;
        public bool playagain2exit;

        public bool paused;

        public SoundEffectInstance backgroundMusic;
        public SoundEffectInstance bonusMusic;
        public SoundEffectInstance playAgainMusic;

        public Game game;

        // needed to reward bonus points
        public Player currentPlayer; 

        public GameData(Game thisGame)
        {
            welcome2main = false;
            main2bonus = false;
            main2playagain = false;
            bonus2main = false;
            bonus2playagain = false;
            playagain2welcome = false;
            playagain2exit = false;

            screenWidth = 1024;
            screenHeight = 640;

            paused = false;

            game = thisGame;
        }

        /// <summary>
        /// Changes the background music of the game
        /// </summary>
        /// <param name="sound"></param>
        /// <param name="volume"></param>
        public void changeMusic(SoundEffectInstance sound, float volume=0.2f)
        {
            if (sound != bonusMusic &&
                bonusMusic != null &&
                bonusMusic.State == SoundState.Playing)
            {
                bonusMusic.Stop();
            }

            if (sound != backgroundMusic &&
                backgroundMusic != null &&
                backgroundMusic.State == SoundState.Playing)
            {
                backgroundMusic.Stop();
            }

            if (sound != playAgainMusic &&
                playAgainMusic != null &&
                playAgainMusic.State == SoundState.Playing)
            {
                playAgainMusic.Stop();
            }

            if (sound.State != SoundState.Playing)
            {
                sound.Play();
            }
        }

        /// <summary>
        /// When the player reached the goal, there was some vibration.
        /// This method checks to see if you are in a close range of the 
        /// mouse position. If so, you do not need to move.
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public bool targetReached(float x, float y, float tolerance = 3)
        {
            return Math.Abs(currentPlayer.Position.X - x) < tolerance &&
                Math.Abs(currentPlayer.Position.Y - y) < tolerance;
        }

        /// <summary>
        /// Checks to make sure the click was within bounds of the game 
        /// window
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public bool validLeftClick(MouseState ms)
        {
            return (ms.LeftButton == ButtonState.Pressed &&
                ms.X > 0 && ms.X < screenWidth &&
                ms.Y > 0 && ms.Y < screenHeight);
        }

        /// <summary>
        /// Checks to make sure the click was within bounds of the game 
        /// window
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public bool validRightClick(MouseState ms)
        {
            return (ms.RightButton == ButtonState.Pressed &&
                ms.X > 0 && ms.X < screenWidth &&
                ms.Y > 0 && ms.Y < screenHeight);
        }

        /// <summary>
        /// Finds the distance between two points
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public float Distance(Vector2 a, Vector2 b)
        {
            float leg1 = a.X - b.X;
            float leg2 = a.Y - b.Y;
            float sumSqares = leg1 * leg1 + leg2 * leg2;
            return (float) Math.Sqrt(sumSqares);
        }
    }
}
