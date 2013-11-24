using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using World_Of_Rectangle.Game.Entities.Weapons;

using sat.Etc;
using sat.Shape;

namespace World_Of_Rectangle.Game.Entities
{
    enum Action
    {
        Nothing,
        Attack,
        Inventory,
        Die
    }

    enum Team
    {
        Good,
        Evil,
    }

    abstract class IEntity
    {

        private Shape2D shape;
        private Texture2D texture;
        private Vector2 textureOrigin;
        private Vector2 position;
        private float rotation;
        private float hp;
        private float contactDamage;
        private bool moveable;
        private Team team;

        public Shape2D Shape
        {
            get { return shape; }
            set
            {
                shape = value;
            }
        }
        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }
        public Vector2 TextureOrigin
        {
            get { return textureOrigin; }
            set { textureOrigin = value; }
        }
        public Vector2 Position
        {
            get { return position; }
            set
            {
                position = value;
                shape.Position = value;
            }
        }
        public float Rotation
        {
            get { return rotation; }
            set
            {
                rotation = value;
                shape.Rotation = value;
            }
        }
        public float Hp
        {
            get { return hp; }
            set { hp = value; }
        }
        public bool stillLiving
        {
            get { return hp > 0; }
        }
        public bool Moveable
        {
            get { return moveable; }
            set
            {
                moveable = value;
                shape.Moveable = value;
            }
        }

        protected IEntity(Vector2 position, float rotation, bool moveable, float contactDamage, float hp, Team team =  Team.Good)
        {
            this.shape = null;
            this.texture = null;
            this.textureOrigin = Vector2.Zero;
            this.position = position;
            this.rotation = rotation;
            this.moveable = moveable;
            this.contactDamage = contactDamage;
            this.hp = hp;
            this.team = team;
        }

        public virtual void receiveDamageFrom(IEntity entity)
        {
            hp -= entity.contactDamage;
        }

        public virtual void receiveDamageFrom(IWeapon weapon)
        {
            hp -= weapon.ContactDamage;
        }

        public abstract void LoadContent(ContentManager content);
        public abstract Action Update(GameTime gameTime);
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, null, Color.White, Rotation, textureOrigin, 1, SpriteEffects.None, 0);
        }
    }
}
