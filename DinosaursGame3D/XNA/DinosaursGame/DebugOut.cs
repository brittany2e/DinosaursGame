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
    /** Draws useful debug information on the screen **/
    class DebugOut : DrawableGameComponent
    {
        SpriteBatch sb;
        SpriteFont CourierNew;
        
        bool isShowing;
        Keys toggleKey;
        Dictionary<string, string> dictionary;

        KeyboardState prevState;

        public DebugOut(Game game, Keys toggleKey = Keys.LeftControl, bool onByDefault = true) 
            : base(game)
        {
            this.toggleKey = toggleKey;
            isShowing = onByDefault;
            dictionary = new Dictionary<string, string>();
        }

        public override void Initialize()
        {
            base.Initialize();

            prevState = Keyboard.GetState();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            sb = new SpriteBatch(this.Game.GraphicsDevice);
            CourierNew = Game.Content.Load<SpriteFont>("Debug");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            KeyboardState curState = Keyboard.GetState();

            // On key up event, toggle the visibility of the output.
            if (prevState.IsKeyUp(toggleKey) && curState.IsKeyDown(toggleKey))
            {
                // Toggle if the text is showing or not
                isShowing = !isShowing;
            }
            prevState = curState;
        }

        public override void Draw(GameTime gameTime)
        {
            if (isShowing)
            {
                sb.Begin();

                // Create string from the dictionary
                string output = "";
                foreach (KeyValuePair<string, string> element in dictionary)
                {
                    output = output + element.Key + ": " + element.Value + "\n";
                }

                // Draw the string
                sb.DrawString(CourierNew, output, Vector2.Zero, Color.Black);
                //Note: Change "CourierNew" to "current" above to enable switching

                sb.End();
            }

            base.Draw(gameTime);
        }

        public void setData(string key, string value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }
        }
    }
}
