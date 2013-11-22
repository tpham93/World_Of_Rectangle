using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using World_Of_Rectangle.Game;
using World_Of_Rectangle.Game.Entities;

namespace World_Of_Rectangle.Gamestates
{
    class ActualGame : IGamestateElement
    {
        Player player;
        

        public ActualGame()
        {
            Keys[] keys = new Keys[(int)Player.ActionKeys.KeyCount];

            keys[(int)Player.ActionKeys.MoveForward] = Keys.W;
            keys[(int)Player.ActionKeys.MoveBackward] = Keys.S;
            keys[(int)Player.ActionKeys.MoveLeft] = Keys.L;
            keys[(int)Player.ActionKeys.MoveRight] = Keys.D;
            keys[(int)Player.ActionKeys.Attack] = Keys.Space;
            keys[(int)Player.ActionKeys.Inventory] = Keys.Escape;

            player = new Player( Vector2.Zero,0.0f,keys);
        }

        public void Load(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            player.LoadContent(content);
        }

        public EGameStates Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Action playerAction =  player.Update();

            switch (playerAction)
            {
                case Action.Inventory:
                    return EGameStates.Inventory;
            }

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
                Matrix.CreateTranslation(new Vector3(player.Position,0)) * Matrix.CreateScale(Global.scaleValues)
                );

            player.Draw(gameTime, spriteBatch);


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
