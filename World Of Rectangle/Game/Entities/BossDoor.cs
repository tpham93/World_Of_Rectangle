using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace World_Of_Rectangle.Game.Entities
{
    class BossDoor : SolidEntities
    {
        public BossDoor(Vector2 position, Texture2D texture)
            : base(position,texture)
        {
        }

        public override bool canPass(Player entity)
        {
            return entity.KeyNumber >=3;
        }
    }
}
