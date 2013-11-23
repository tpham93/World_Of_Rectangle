using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace World_Of_Rectangle.Gamestates
{
    class Inventory : IGamestateElement
    {
        IGamestateElement game;

        public Inventory(IGamestateElement game)
        {
            this.game = game;
        }

        public void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            throw new NotImplementedException();
        }

        public EGameStates Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public void Draw(Microsoft.Xna.Framework.GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            game.Draw(gameTime, spriteBatch);
        }

        public void Unload()
        {
            throw new NotImplementedException();
        }

        public EGameStates getGameState()
        {
            throw new NotImplementedException();
        }
    }
}
