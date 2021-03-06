﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace World_Of_Rectangle.Gamestates
{
    enum EGameStates
    {
        Nothing,
        Menu,
        Intro,
        Game,
        Credits,
        Inventory,
        Close,
    }

    interface IGamestateElement
    {
        void LoadContent(ContentManager content);
        EGameStates Update(GameTime gameTime);
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        void Unload();
        EGameStates getGameState();
    }
}
