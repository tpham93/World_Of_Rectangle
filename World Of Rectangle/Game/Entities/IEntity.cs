using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

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

        private EdgeShape shape;
        private Texture2D texture;
        private Vector2 textureOrigin;
        private Vector2 position;
        private float rotation;
        private float hp;
        private float contactDamage;
        private bool moveable;
        private Team team;

        private Texture2D shapeTexture;


        private Vector2[] corners;

        public EdgeShape Shape
        {
            get { return shape; }
            set
            {
                shape = value;
                corners = new Vector2[4];

                for (int i = 0; i < 4; ++i)
                {
                    corners[i] = shape.Corners[i] + shape.MiddlePoint;
                }

                shapeTexture = EdgeShape.genTexture(corners, corners[0] + new Vector2(1), texture.Width, texture.Height, Color.Red);
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

        //static Texture2D pixel = Helper.genRectangle(1, 1, Color.Green, Color.Green);
        public abstract void LoadContent(ContentManager content);
        public abstract Action Update(GameTime gameTime);
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(shapeTexture, shape.Position, null, Color.White, shape.Rotation, shape.MiddlePoint, 1, SpriteEffects.None, 0);
            spriteBatch.Draw(texture, shape.Position, null, Color.White, Rotation, textureOrigin, 1, SpriteEffects.None, 0);

            //for (int i = 0; i < shape.CurrentCorners.Length; ++i)
            {
                //spriteBatch.Draw(pixel, shape.CurrentCorners[i], null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            }
        }
    }
}
