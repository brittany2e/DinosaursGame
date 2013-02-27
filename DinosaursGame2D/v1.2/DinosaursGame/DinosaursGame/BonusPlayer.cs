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
    class BonusPlayer : Actor
    {
        private Texture2D body;
        private Texture2D neck;
        private Texture2D head;
        private bool isLeft; // Use for determining drawing direction

        // All positions are top-left corner of texture in screen coords
        private Vector2 startingPos; 
        private Vector2 neckStartPos;
        private Vector2 headStartPos;
        private Vector2 bodyStartPos;

        /// <summary>
        /// Constructs a bonus player and adds it to the game in the 
        /// bonus area
        /// </summary>
        /// <param name="theData"></param>
        public BonusPlayer(GameData theData)
        {
            data = theData;
            Position = new Vector2(
                data.screenWidth / 2,
                data.screenHeight / 2);
            destination = new Vector2();
            Direction = new Vector2();
            
            isLeft = false;
        }

        /// <summary>
        /// Loads the textures which make up this dinosaur and calculates
        /// the starting positions
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            head = content.Load<Texture2D>("textures/bonusHead");
            neck = content.Load<Texture2D>("textures/bonusNeck");
            body = content.Load<Texture2D>("textures/bonusBody");

            // Figure out starting positions of textures.
            startingPos = Position;
            headStartPos = Position - new Vector2(head.Width / 2, head.Height / 2);
            neckStartPos = Position;
            neckStartPos.X -= neck.Width / 2;
            neckStartPos.Y += (int)(head.Height / 2 * 0.75);
            bodyStartPos = Position;
            bodyStartPos.X -= body.Width / 2;
            bodyStartPos.Y += (int)(1.25 * head.Height);

            // Keep the bounding box around the head
            Bound = new Rectangle(
                (int)headStartPos.X,
                (int)headStartPos.Y, 
                head.Width, 
                head.Height);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // Move toward destination
            if (isCloseEnough(Position, destination))
            {
                Direction = Vector2.Zero;
            }
            else
            {
                Position += Direction;
                isLeft = (Direction.X < 0);
            }
        }

        /// <summary>
        /// Displays the bonus dinosaur on the screen in the correct
        /// position.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            // Logic for left or right facing
            SpriteEffects direction = SpriteEffects.None;
            if (isLeft)
            {
                direction = SpriteEffects.FlipHorizontally;
            }
            
            // Draw each part of the body individually.
            
            // Calculate the correct positions of the textures
            Vector2 delta = Position - startingPos;
            Vector2 headPos = headStartPos + delta;
            Vector2 neckPos = neckStartPos + delta;
            Vector2 bodyPos = bodyStartPos;
            bodyPos.X += delta.X; // Only change the X position of the body (left/right)
            
            // I think this is the destination size of the rectangles.
            // This step is important because the size of the neck changes.
            Rectangle headRect = new Rectangle(
                (int)headPos.X, (int)headPos.Y, 
                head.Width, head.Height);
            Rectangle neckRect = new Rectangle(
                (int)neckPos.X, (int)neckPos.Y,
                neck.Width, (int)(bodyPos.Y - headPos.Y));
            Rectangle bodyRect = new Rectangle(
                (int)bodyPos.X, (int)bodyPos.Y,
                body.Width, body.Height);

            // The neck needs to be drawn first because it should be 
            // behind the head and body.
            spriteBatch.Draw(neck, neckRect, null, Color.White, 0, Vector2.Zero, direction, 0);
            spriteBatch.Draw(head, headRect, null, Color.White, 0, Vector2.Zero, direction, 0);
            spriteBatch.Draw(body, bodyRect, null, Color.White, 0, Vector2.Zero, direction, 0);
            
            // Make sure to update the bounding box according to the new position of the head.
            Bound.X = (int)headPos.X;
            Bound.Y = (int)headPos.Y;
        }

        /// <summary>
        /// Determines whether the bonus dinosaur has gotten close enough to the
        /// objective or not
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public bool reachedDestination(Vector2 pos)
        {
            Vector2 playerPos = 
                Position - new Vector2(head.Width / 2, head.Height / 2);
            
            return Math.Abs(playerPos.X - pos.X) < 20 &&
                   Math.Abs(playerPos.Y - pos.Y) < 20;
        }

    }

}
