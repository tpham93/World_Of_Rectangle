using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using sat.Shape;

namespace World_Of_Rectangle.Game.Entities
{
    class SolidEntities : IEntity
    {

        public SolidEntities(Vector2 position, Texture2D texture) : base(position,0,false,0,float.PositiveInfinity)
        {
            Texture = texture;
            Point size = new Point(texture.Width, texture.Height);
            Shape = new RectangleShape(new Rectangle(0,0,size.X,size.Y),position);
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
