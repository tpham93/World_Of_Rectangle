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
        readonly TimeSpan MAX_ATTACKINGTIME = TimeSpan.FromSeconds(0.5f);
        const float MAX_ROTATION = (float)Math.PI/4 * 3;

        float ownRotation;

        public Sword(Vector2 position, float rotation, bool moveable, float contactDamage, float hp, Team team = Team.Good)
            : base(position,rotation,moveable,contactDamage,hp,team)
        {
            ownRotation = (float)-Math.PI/2;
        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            Texture = content.Load<Texture2D>(@"sword");
            Vector2 size = new Vector2(Global.TILE_SIZE * 0.5f, Global.TILE_SIZE * 3);
            Shape = new EdgeShape(EdgeShape.genCorners(size),size,new Vector2(size.Y*0.25f,size.X * 0.9f),Position,false);
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
                    Rotation = ownRotation+ (float)Math.PI/2 + owner.Rotation;
                    Position = (-sat.Etc.Helper.rotateVector2(owner.Shape.MiddlePoint,owner.Rotation)/2f+ owner.Position );
                }
                else
                {
                    attacking = false;
                }
            }
        }
    }
}
