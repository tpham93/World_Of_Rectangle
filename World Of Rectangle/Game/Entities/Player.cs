﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using sat.Shape;
using World_Of_Rectangle.Game;
using World_Of_Rectangle.Game.Entities.Weapons;

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
        protected const float MOVESPEED_PS = 500.0f;
        protected TimeSpan attackCooldown;
        protected int sp;
        private IWeapon weapon;

        private int keyNumber;

        public int KeyNumber
        {
            get { return keyNumber; }
            set { keyNumber = value; }
        }

        public IWeapon Weapon
        {
            get { return weapon; }
        }

        protected bool CanAttack
        {
            get { return attackCooldown < TimeSpan.Zero /*&& weapon != null && weapon.SP_Cost < SP*/; }
        }

        public bool isAttacking
        {
            get { return weapon.Attacking; }
        }

        public Player(Vector2 position, float rotation, Keys[] keys)
            : base(position, rotation, false, 0, 100 )
        {
            if (keys.Length != (int)ActionKeys.KeyCount)
            {
                throw new Exception("wrong number of keys!");
            }
            sp = 100;
            this.keys = keys;
        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            Texture = content.Load<Texture2D>(@"player");
            Vector2 size = new Vector2(Texture.Width, Texture.Height);
            TextureOrigin = size / 2f;
            Shape = new RectangleShape(new Rectangle(0,0,(int)size.X,(int)size.Y), Position);
            weapon = new Sword(Position, Rotation, false, 20f, float.PositiveInfinity);
            weapon.LoadContent(content);
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
                moveVector += Vector2.UnitX;
            }

            if (moveVector != Vector2.Zero)
            {
                moveVector.Normalize();
                Position += moveVector * MOVESPEED_PS * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            this.Rotation = (float)(sat.Etc.Helper.getAngleFromVector2(Input.mousePositionV() - Global.SCREENSIZE / 2) + Math.PI / 2);

            if (attackCooldown >= TimeSpan.Zero)
            {
                attackCooldown -= gameTime.ElapsedGameTime;
            }

            if (CanAttack && Input.mouseButtonClicked(Input.EMouseButton.LeftButton))
            {
                weapon.attack();
                attackCooldown = TimeSpan.FromSeconds(0.75f);
            }

            weapon.Update(gameTime, this);

            base.Update(gameTime);

            return Action.Nothing;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (weapon.Attacking)
                weapon.Draw(gameTime, spriteBatch);
            base.Draw(gameTime, spriteBatch);
        }
    }
}
