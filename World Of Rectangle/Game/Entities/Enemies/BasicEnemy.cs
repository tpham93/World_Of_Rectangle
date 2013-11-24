using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using sat.Etc;
using sat.Shape;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace World_Of_Rectangle.Game.Entities.Enemies
{
    class BasicEnemy : IEntity
    {
        static Random random = new Random();
        float radius;

        public BasicEnemy(Vector2 position, float contactDamage, float hp, float sightRadius, Texture2D texture)
            : base(position, 0, true, contactDamage, hp, Team.Evil)
        {
            Texture = texture;
            Point size = new Point(texture.Width, texture.Height);
            Shape = new RectangleShape(new Rectangle(0,0,size.X,size.Y),position);
            TextureOrigin = new Vector2(texture.Width/2,texture.Height/2);
            radius = sightRadius;
        }
        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
        }

        public override Action Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Vector2 moveVector = (EntityManager.Player.Position - Position);

            if (moveVector.LengthSquared() > radius * radius)
            {
                moveVector = new Vector2((float)random.NextDouble() - 0.5f, (float)random.NextDouble() - 0.5f);
            }

            moveVector.Normalize();
            Position += moveVector * 2;

            base.Update(gameTime);

            return Action.Nothing;
        }
    }
}
