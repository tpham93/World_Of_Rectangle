using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using World_Of_Rectangle.Game;
using World_Of_Rectangle.Game.Entities;

namespace World_Of_Rectangle.Gamestates
{
    class ActualGame : IGamestateElement
    {
        World world;
        

        public ActualGame()
        {

            world = new World(@"Maps\map_1.png");
        }

        public void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            world.LoadContent(content);
        }

        public EGameStates Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            //Action playerAction =  player.Update(gameTime);

            //switch (playerAction)
            //{
            //    case Action.Inventory:
            //        return EGameStates.Inventory;
            //}

            world.Update(gameTime);

            return EGameStates.Game;
        }

        public void Draw(Microsoft.Xna.Framework.GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {

            spriteBatch.Begin(
                SpriteSortMode.Immediate,
                null,
                null,
                null,
                null,
                null,
                Matrix.CreateTranslation(-new Vector3(world.CamPosition, 0) + new Vector3(Global.REFERENCE_SCREENSIZE / 2, 0)) * Matrix.CreateScale(Global.scaleValues));
            //spriteBatch.Begin(
            //    SpriteSortMode.Immediate,
            //    null,
            //    null,
            //    null,
            //    null,
            //    null,
            //    Matrix.CreateTranslation(-new Vector3(world.CamPosition, 0) + new Vector3(Global.REFERENCE_SCREENSIZE / 2, 0)));

            //player.Draw(gameTime, spriteBatch);
            world.Draw(gameTime, spriteBatch);


            spriteBatch.End();
        }

        public void Unload()
        {

        }

        public EGameStates getGameState()
        {
            return EGameStates.Game;
        }
    }
}
