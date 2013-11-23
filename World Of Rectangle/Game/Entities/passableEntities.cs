using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using sat.Shape;

namespace World_Of_Rectangle.Game.Entities
{
    class PasseableEntities : IEntity
    {

        public PasseableEntities(Vector2 position, Texture2D texture) : base(position, 0, true, 0, float.PositiveInfinity)
        {
            Texture = texture;
            Vector2 size = new Vector2(texture.Width,texture.Height);
            Shape = new EdgeShape(EdgeShape.genCorners(size),size,position);
            TextureOrigin = new Vector2(texture.Width/2,texture.Height/2);
        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
        }

        public override Action Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            return Action.Nothing;
        }
    }
}
