using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Graphics;

using sat.Etc;
using sat.Shape;

namespace World_Of_Rectangle.Game.Entities.Weapons
{
    class Sword : IWeapon
    {
        TimeSpan attackingTime;
        readonly TimeSpan MAX_ATTACKINGTIME = TimeSpan.FromSeconds(0.3f);
        const float MAX_ROTATION = (float)Math.PI * 9 / 10f;

        float ownRotation;

        public Sword(Vector2 position, float rotation, bool moveable, float contactDamage, float hp, Team team = Team.Good)
            : base(position,rotation,moveable,contactDamage,hp,team)
        {
            ownRotation = 0;
        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            Texture = content.Load<Texture2D>(@"sword");
            Vector2 size = new Vector2(Texture.Width,Texture.Height);
            TextureOrigin = new Vector2(size.X*0.5f,size.Y * 0.9f);
            Shape = new RectangleShape(new Rectangle(0,0,(int)size.X,(int)size.Y),Position,false);
        }

        public override void attack()
        {
            attacking = true;
            attackingTime = TimeSpan.Zero;
        }

        public override void Update(GameTime gameTime, IEntity owner)
        {
            if (Attacking)
            {
                if (attackingTime <= MAX_ATTACKINGTIME)
                {
                    attackingTime += gameTime.ElapsedGameTime;
                    ownRotation = MAX_ROTATION * (float)(attackingTime.TotalMilliseconds / MAX_ATTACKINGTIME.TotalMilliseconds);
                    Rotation = ownRotation - (float)Math.PI/2 + owner.Rotation;
                    Position = (-sat.Etc.Helper.rotateVector2(owner.Shape.MiddlePoint,owner.Rotation)+ owner.Position );
                }
                else
                {
                    attacking = false;
                }
            }
        }
    }
}
