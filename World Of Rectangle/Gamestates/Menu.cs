using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using World_Of_Rectangle.Game;

namespace World_Of_Rectangle.Gamestates
{
    class Menu : IGamestateElement
    {

        protected static int nameWidth = (int)(Global.REFERENCE_SCREENSIZE.X / 2 * Global.scaleValues.X);
        protected static int nameHeight = (int)(Global.REFERENCE_SCREENSIZE.Y / 5 * Global.scaleValues.Y);

        protected static int buttonWidth = (int)(Global.REFERENCE_SCREENSIZE.X / 4 * Global.scaleValues.X);
        protected static int buttonHeight = (int)(Global.REFERENCE_SCREENSIZE.Y / 10 * Global.scaleValues.Y);

        protected static Vector2 SPACE_WINDOW_NAME = new Vector2(Global.REFERENCE_SCREENSIZE.X / 2 - nameWidth / 2, Global.REFERENCE_SCREENSIZE.Y / 10f);

        protected static Vector2 SPACE_WINDOW_BUTTON = new Vector2(Global.REFERENCE_SCREENSIZE.X / 2 - buttonWidth / 2, Global.REFERENCE_SCREENSIZE.Y / 2.5f);
        protected static Vector2 SPACE_BUTTON_BUTTON = new Vector2(0, Global.REFERENCE_SCREENSIZE.Y / 25);


        Texture2D nameTexture;
        Rectangle nameRectangle;

        EGameStates[] buttonGamestates;
        Rectangle[] buttonRectangles;
        Texture2D[] buttonTextures;

        Vector3 scaleValues;

        public Menu()
        {
            buttonGamestates = new EGameStates[] { EGameStates.Game, EGameStates.Menu, EGameStates.Close };
            buttonTextures = new Texture2D[buttonGamestates.Length];
            buttonRectangles = new Rectangle[buttonGamestates.Length];
            scaleValues = Global.scaleValues;
        }

        public void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            nameTexture = content.Load<Texture2D>(@"button_name");
            Vector2 namePosition = Vector2.Transform(SPACE_WINDOW_NAME, Matrix.CreateScale(Global.scaleValues));
            nameRectangle = new Rectangle((int)namePosition.X, (int)namePosition.Y, nameWidth, nameHeight);
            
            buttonTextures[0] = content.Load<Texture2D>(@"button_start");
            buttonTextures[1] = content.Load<Texture2D>(@"button_credits");
            buttonTextures[2] = content.Load<Texture2D>(@"button_exit");


            Vector2 position = Vector2.Transform(SPACE_WINDOW_BUTTON, Matrix.CreateScale(Global.scaleValues));
            

            for (int i = 0; i < buttonTextures.Length; ++i)
            {
                Vector2 buttonSize = Global.REFERENCE_SCREENSIZE / 20;
                buttonRectangles[i] = new Rectangle((int)position.X, (int)position.Y, buttonWidth, buttonHeight);
                position += Vector2.Transform(SPACE_BUTTON_BUTTON + new Vector2(0, buttonSize.Y), Matrix.CreateScale(Global.scaleValues));
                position += SPACE_BUTTON_BUTTON + new Vector2(0, buttonSize.Y);
            }
        }

        public EGameStates Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (Input.mouseButtonPressed(Input.EMouseButton.LeftButton))
            {
                for (int i = 0; i < buttonRectangles.Length; ++i)
                {
                    if (buttonRectangles[i].Contains(Input.mousePosition()))
                    {
                        return buttonGamestates[i];
                    }
                }
            }
            return EGameStates.Menu;
        }

        public void Draw(Microsoft.Xna.Framework.GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(nameTexture, nameRectangle, Color.White);

            for (int i = 0; i < buttonTextures.Length; ++i)
            {
                spriteBatch.Draw(buttonTextures[i], buttonRectangles[i], Color.White);
            }

            spriteBatch.End();
        }

        public void Unload()
        {
            throw new NotImplementedException();
        }

        public EGameStates getGameState()
        {
            return EGameStates.Menu;
        }
    }
}
