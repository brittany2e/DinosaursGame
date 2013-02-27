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
    class MainScreen : Screen
    {
        GameData data;
        
        // Textures
        Texture2D background;

        // Sounds


        // Actors
        List<Actor> players;
        int numberOfPlayers;

        List<Actor> enemies;
        int numberOfEnemies;

        List<Actor> plants;
        int numberOfPlants;

        // Cursor
        Cursor cursor;
        Marker marker;

        bool restart;

        public MainScreen(GameData theData, bool Restart = true)
        {
            data = theData;
            restart = Restart;
            cursor = new Cursor();
            marker = new Marker();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
        }

        /// <summary>
        /// Loads the necessary textures for all of the components
        /// in the main area
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            background =
                content.Load<Texture2D>("textures/mainBackground");

            SetUpPlants(content);
            SetUpPlayers(content);
            SetUpEnemies(content);

            // Load the texture for the cursor
            cursor.Load(content);
            marker.Load(content);

            // Background music
            if (data.backgroundMusic == null)
            {
                data.backgroundMusic =
                    content.Load<SoundEffect>("sounds/mainMusic").CreateInstance();
                data.backgroundMusic.Volume = 0.2f;
                data.backgroundMusic.IsLooped = true;
            }
            data.changeMusic(data.backgroundMusic);
        }

        /// <summary>
        /// Determine the size and location of all the plants
        /// </summary>
        /// <param name="content"></param>
        private void SetUpPlants(ContentManager content)
        {
            numberOfPlants = 5;
            plants = new List<Actor>();
            for (int i = 0; i < numberOfPlants; i++)
            {
                plants.Add(new Plant(data));
                
                Plant temp = (Plant)plants[i];
                temp.LoadContent(content);

                if (i == 3)
                {
                    temp.hasFlower = true;
                }
            }

            // Disperse plants around the area.

            //plants[0].SetPosition(
            //    screenWidth * 2 / 3,
            //    screenHeight * 2 / 3);
            plants[0].SetPosition(
                data.screenWidth / 6,
                data.screenHeight / 2);
            plants[1].SetPosition(
                data.screenWidth * 1 / 3,
                data.screenHeight * 3 / 4);
            plants[2].SetPosition(
                data.screenWidth * 2 / 5,
                data.screenHeight * 1 / 7);
            plants[3].SetPosition(
                data.screenWidth * 3 / 7,
                data.screenHeight * 2 / 5);
            plants[4].SetPosition(
                data.screenWidth * 3 / 4,
                data.screenHeight * 1 / 8);
        }

        /// <summary>
        /// Determine the starting location of the players
        /// </summary>
        /// <param name="content"></param>
        private void SetUpPlayers(ContentManager content)
        {
            numberOfPlayers = 1;
            players = new List<Actor>();
            
            if (data.currentPlayer == null || restart)
            {
                data.currentPlayer = new Player(data);
            }
            players.Add(data.currentPlayer);

            // Set the initial position
            data.currentPlayer.SetPosition(
                data.screenWidth / 6,
                data.screenHeight / (numberOfPlayers + 1));

            data.currentPlayer.LoadContent(content);

            if (numberOfPlayers >= 2)
            {
                players.Add(new Player(data));

                // Set the initial position
                data.currentPlayer.SetPosition(
                    data.screenWidth / 6,
                    data.screenHeight / (numberOfPlayers + 1) * 2);
            }

        }

        /// <summary>
        /// Determine the starting location of the enemies
        /// </summary>
        /// <param name="content"></param>
        private void SetUpEnemies(ContentManager content)
        {
            numberOfEnemies = 3;
            enemies = new List<Actor>();
            for (int i = 0; i < numberOfEnemies; i++)
            {
                enemies.Add(new Enemy(data));
                
                enemies[i].SetPosition(
                    data.screenWidth / 2,
                    data.screenHeight / (numberOfEnemies + 1) * (i + 1));

                Enemy temp = (Enemy)enemies[i];
                temp.LoadContent(content, i * 2);
            }
        }

        /// <summary>
        /// Update the current state of affairs in the main area
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            // Process Input
            ProcessKeyboard();
            ProcessMouse();

            // Game Logic
            doLogic(gameTime);

            // Update all of the actors.
            for (int i = 0; i < numberOfPlants; i++)
                plants[i].Update(gameTime);
            for (int i = 0; i < numberOfPlayers; i++)
            {
                players[i].Update(gameTime);
                Player temp = (Player)players[i];
                temp.life.Update();
            }
            for (int i = 0; i < numberOfEnemies; i++)
                enemies[i].Update(gameTime);
        }

        /// <summary>
        /// Draw the components in the main area to the sceen
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw scenery
            spriteBatch.Draw(background, Vector2.Zero, Color.White);

            // Draw Plants
            foreach (Plant plant in plants)
            {
                plant.Draw(spriteBatch);
            }

            // Draw the marker
            marker.Draw(spriteBatch);

            // Draw Players
            foreach( Player player in players)
            {
                player.Draw(spriteBatch);
                player.life.Draw(spriteBatch);
            }

            // Draw Enemies
            foreach (Enemy enemy in enemies)
            {
                enemy.Draw(spriteBatch);
            }

            // Draw the cursor on top of everything
            cursor.Draw(spriteBatch);

        }

        /// <summary>
        /// Use the gametime to determine if animations need to be updated and 
        /// collisions need to be checked
        /// </summary>
        /// <param name="gameTime"></param>
        private void doLogic(GameTime gameTime)
        {
            //CheckCollisions(gameTime);
            for (int i = 0; i < numberOfEnemies; i++)
            {
                enemies[i].KeepInBounds(data.screenWidth, data.screenHeight, true);
            }

            if (gameTime.TotalGameTime.Milliseconds % 400 == 0)
            {
                // Player animation
                for (int i = 0; i < numberOfPlayers; i++)
                {
                    if (players[i].Direction.Equals(Vector2.Zero))
                    {
                        players[i].Texture = players[i].Animation.reset();
                    }
                    else
                    {
                        players[i].Texture = players[i].Animation.next();
                    }
                }


                // Enemy animation and collision detection
                for (int i = 0; i < numberOfEnemies; i++)
                {
                    enemies[i].Texture = enemies[i].Animation.next();

                    bool collisionDetected =
                        data.currentPlayer.CheckPlayersCollision(enemies[i]);

                    if (collisionDetected)
                    {
                        // You are eaten! For now, you only lose points.
                        data.currentPlayer.Score -= enemies[i].Score;
                        //enemies[i].IsAlive = false;
                        Enemy temp = (Enemy)enemies[i];
                        temp.Roar();
                    }

                    //if (enemies[i].Texture.Equals(Content.Load<Texture2D>("textures/TrexGray1")))
                    //{
                    //    enemies[i].Direction = Vector2.Zero;
                    //}
                }

            }

            if (gameTime.TotalGameTime.Milliseconds % 1000 == 0)
            {
                CheckPlantCollisions();
            }
        }

        /// <summary>
        /// Check to see if the players are colliding with any plants, and 
        /// reacts accordingly
        /// </summary>
        private void CheckPlantCollisions()
        {
            for (int i = 0; i < numberOfPlants; i++)
            {
                bool collisionDetected =
                        data.currentPlayer.CheckPlayersCollision(plants[i]);

                if (collisionDetected)
                {
                    //System.Console.WriteLine("Collision Detected: " + gameTime.TotalGameTime.Seconds);

                    // Eat the plant and gain the points
                    Plant plant = (Plant)plants[i];
                    data.currentPlayer.Score += plant.eat();

                    if (plant.Score == 0 && plant.hasFlower)
                    {
                        data.main2bonus = true;
                    }
                }
            }
        }

        /// <summary>
        /// Checks the state of the keyboard and reacts accordingly
        /// </summary>
        private void ProcessKeyboard()
        {
            KeyboardState kb = Keyboard.GetState();

            // Check the states first
            if (kb.IsKeyDown(Keys.B))
            {
                data.main2bonus = true;
            }

            if (kb.IsKeyDown(Keys.Escape))
            {
                data.main2playagain = true;
            }

            if (data.currentPlayer.Score <= 0)
            {
                data.main2playagain = true;
            }    
        }

        /// <summary>
        /// Checks the state of the mouse and reacts accordingly
        /// </summary>
        private void ProcessMouse()
        {
            MouseState mouseState = Mouse.GetState();

            // Update the position of the cursor
            cursor.Update(mouseState.X, mouseState.Y);

            // Player 1 uses the mouse for control
            if (data.validLeftClick(mouseState) &&
                !data.targetReached(mouseState.X, mouseState.Y))
            {
                data.currentPlayer.GoTo(mouseState.X, mouseState.Y);
                marker.setPosition(mouseState.X, mouseState.Y);
            }

            Vector2 dst = data.currentPlayer.getDestination();
            if(data.targetReached(dst.X, dst.Y, 5))
            {
                marker.Disable();
            }
            
        }
    }
}
