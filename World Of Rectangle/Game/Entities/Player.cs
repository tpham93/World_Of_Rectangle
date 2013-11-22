using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using sat.Shape;
using World_Of_Rectangle.Game;

namespace World_Of_Rectangle.Game.Entities
{
    class Player : IEntity
    {

        public enum ActionKeys
        {
            MoveForward,
            MoveLeft,
            MoveRight,
            MoveBackward,
            Attack,
            Inventory,
            KeyCount
        }

        protected Keys[] keys;
        protected const float MOVESPEED_PS = 10.0f;
        protected TimeSpan attackCooldown;
        protected int hp;
        protected int sp;

        protected bool CanAttack
        {
            get { return attackCooldown < TimeSpan.Zero /*&& weapon != null && weapon.SP_Cost < SP*/; }
        }

        public Player(Vector2 position, float rotation, Keys[] keys) : base(position, rotation)
        {
            if (keys.Length != (int)ActionKeys.KeyCount)
            {
                throw new Exception("wrong number of keys!");
            }
            hp = 100;
            sp = 100;
            this.keys = keys;
        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            Texture = content.Load<Texture2D>(@"Entities\Player.png");
            Vector2 size = new Vector2(Texture.Width, Texture.Height);
            TextureOrigin = size / 2f;
            Shape = new EdgeShape(EdgeShape.genCorners(size), size, Position);
        }

        public override Action Update(GameTime gameTime)
        {

            Vector2 moveVector = Vector2.Zero;
            if (Input.isKeyDown(keys[(int)ActionKeys.MoveForward]))
            {
                moveVector -= Vector2.UnitY;
            }
            if (Input.isKeyDown(keys[(int)ActionKeys.MoveBackward]))
            {
                moveVector += Vector2.UnitY;
            }
            if (Input.isKeyDown(keys[(int)ActionKeys.MoveLeft]))
            {
                moveVector -= Vector2.UnitX;
            }
            if (Input.isKeyDown(keys[(int)ActionKeys.MoveRight]))
            {
                moveVector += Vector2.UnitY;
            }

            moveVector.Normalize();
            Position += moveVector * MOVESPEED_PS * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(attackCooldown >= TimeSpan.Zero)
            {
                attackCooldown -= gameTime.ElapsedGameTime;
            }

            return (CanAttack)?Action.Attack:Action.Nothing;
        }
    }
}
