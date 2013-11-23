using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using World_Of_Rectangle.Game;

namespace World_Of_Rectangle.Gamestates
{
    class Intro : IGamestateElement
    {
        enum IntroState
        {
            Textboxes,
            Blending,
            Animation,
            Ending,
        }

        protected static float textboxWidth = Global.REFERENCE_SCREENSIZE.X;
        protected static float textboxHeight = Global.REFERENCE_SCREENSIZE.Y / 5;

        protected static float castleWidth = Global.REFERENCE_SCREENSIZE.X * 0.8f;
        protected static float castleHeight = Global.REFERENCE_SCREENSIZE.Y - textboxHeight;

        protected static float playerWidth = Global.REFERENCE_SCREENSIZE.X * 0.2f;
        protected static float playerHeight = Global.REFERENCE_SCREENSIZE.X * 0.2f;

        protected static float oldManWidth = Global.REFERENCE_SCREENSIZE.X * 0.1f;
        protected static float oldManHeight = 2 * oldManWidth;

        protected static Vector2 SPACE_WINDOW_TEXT = new Vector2(Global.REFERENCE_SCREENSIZE.X / 2 - textboxWidth / 2, Global.REFERENCE_SCREENSIZE.Y - textboxHeight);

        Texture2D playerHeadTexture;
        Rectangle playerHeadRect;

        Texture2D playerTexture;
        Rectangle playerRect;

        Texture2D oldManTexture;
        Rectangle oldManRect;

        Texture2D castleTexture;
        Rectangle castleRect;

        int textBoxState;
        Texture2D[] textboxes;
        Rectangle textboxRect;

        TimeSpan waitTime;
        IntroState currentState;

        public Intro()
        {
            textBoxState = 0;
            textboxes = new Texture2D[10];
            waitTime = TimeSpan.FromSeconds(3);
        }

        public void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {

            castleTexture = content.Load<Texture2D>("Castle");
            castleRect = new Rectangle((int)(Global.REFERENCE_SCREENSIZE.X * 0.2f), 0, (int)castleWidth, (int)castleHeight);

            playerHeadTexture = content.Load<Texture2D>("player_head");
            playerHeadRect = new Rectangle((int)(Global.REFERENCE_SCREENSIZE.X / 20), (int)(castleHeight - playerHeight), (int)playerWidth, (int)playerHeight);

            playerTexture = content.Load<Texture2D>("player");
            playerRect = new Rectangle((int)(castleRect.Width * 0.5f) + castleRect.X, (int)(Global.REFERENCE_SCREENSIZE.Y), (int)(Global.REFERENCE_SCREENSIZE.X * 0.12f), (int)(Global.REFERENCE_SCREENSIZE.X * 0.04f));

            oldManTexture = content.Load<Texture2D>("oldman");
            oldManRect = new Rectangle((int)(Global.REFERENCE_SCREENSIZE.X * 0.55f), (int)((castleHeight - playerHeight) * 0.5f), (int)playerWidth, (int)playerHeight);

            Texture2D clearTextbox = content.Load<Texture2D>("textbox_clear");

            textboxes[0] = content.Load<Texture2D>("textbox_story");
            textboxes[1] = clearTextbox;

            textboxes[2] = content.Load<Texture2D>("textbox_morestory");
            textboxes[3] = clearTextbox;

            textboxes[4] = content.Load<Texture2D>("textbox_references");
            textboxes[5] = clearTextbox;

            textboxes[6] = content.Load<Texture2D>("textbox_clue");
            textboxes[7] = clearTextbox;

            textboxes[8] = content.Load<Texture2D>("textbox_samestory");
            textboxes[9] = clearTextbox;

            textboxRect = new Rectangle(0, (int)SPACE_WINDOW_TEXT.Y, (int)textboxWidth, (int)textboxHeight);
        }

        public EGameStates Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            switch (currentState)
            {
                case IntroState.Textboxes:
                    if (Input.mouseButtonClicked(Input.EMouseButton.LeftButton))
                    {
                        if (textBoxState >= textboxes.Length - 1)
                        {
                            currentState = IntroState.Blending;
                        }
                        else
                        {
                            ++textBoxState;
                        }
                    }
                    break;
                case IntroState.Blending:
                    waitTime -= gameTime.ElapsedGameTime;
                    if (waitTime <= TimeSpan.FromSeconds(-3f))
                    {
                        currentState = IntroState.Ending;
                        //waitTime = TimeSpan.FromSeconds(-3f);
                    }
                    break;
                //case IntroState.Animation:
                //    playerRect.Y = (int)Global.REFERENCE_SCREENSIZE.Y - (int)(-waitTime.TotalSeconds * (playerRect.Height + textboxHeight) / 2);
                //    waitTime += gameTime.ElapsedGameTime;
                //    if (waitTime >= TimeSpan.Zero)
                //    {
                //        currentState = IntroState.Ending;
                //    }
                //    break;
                case IntroState.Ending:
                    if (Input.mouseButtonClicked(Input.EMouseButton.LeftButton))
                    {
                        return EGameStates.Game;
                    }
                    break;
            }
            return EGameStates.Intro;
        }

        public void Draw(Microsoft.Xna.Framework.GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Matrix.CreateScale(Global.scaleValues));


            switch (currentState)
            {
                case IntroState.Textboxes:
                    spriteBatch.Draw(textboxes[textBoxState], textboxRect, Color.White);
                    spriteBatch.Draw(playerHeadTexture, playerHeadRect, Color.White);
                    spriteBatch.Draw(oldManTexture, oldManRect, Color.White);
                    break;
                case IntroState.Blending:
                    spriteBatch.Draw(castleTexture, castleRect, Color.White * ((1 - (float)(waitTime.TotalSeconds / 1.5f)) / 2));
                    spriteBatch.Draw(textboxes[textBoxState], textboxRect, Color.White * (((float)(waitTime.TotalSeconds / 1.5f)) / 2));
                    spriteBatch.Draw(playerHeadTexture, playerHeadRect, Color.White * (((float)(waitTime.TotalSeconds / 1.5f)) / 2));
                    spriteBatch.Draw(oldManTexture, oldManRect, Color.White * (((float)(waitTime.TotalSeconds / 1.5f)) / 2));
                    break;
                case IntroState.Animation:
                    spriteBatch.Draw(castleTexture, castleRect, Color.White);
                    spriteBatch.Draw(playerTexture,playerRect, Color.White);
                    break;
                case IntroState.Ending:
                    break;

            }

            spriteBatch.End();
        }

        public void Unload()
        {
        }

        public EGameStates getGameState()
        {
            return EGameStates.Intro;
        }
    }
}
