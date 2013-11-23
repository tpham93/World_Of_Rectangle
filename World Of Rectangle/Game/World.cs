﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using World_Of_Rectangle.Game.Entities;



namespace World_Of_Rectangle.Game
{
    class World
    {
        static ContentManager content;

        List<IEntity> passebleEntities;
        List<IEntity> solidEntities;

        readonly static Color PillarColor = new Color(0,0,0);


        public World(string filepath)
        {
            passebleEntities = new List<IEntity>();
            solidEntities = new List<IEntity>();

            Texture2D map = sat.Etc.Helper.loadImage(filepath);
            Color[] pixel = new Color[map.Width *  map.Height];
            map.GetData<Color>(pixel);

            for (int i = 0; i < pixel.Length; ++i)
            {
                int x = i % map.Width;
                int y = i / map.Width;
                IEntity entity = colorToEntitiy(pixel[i], new Vector2(x,y) * Global.TILE_SIZE);
                if (entity != null)
                {
                    if (entity.Moveable)
                    {

                    }
                    else
                    {
                        solidEntities.Add(entity);
                    }
                }
            }
        }

        public static void Initialize(ContentManager content)
        {
            World.content = content;
        }

        private static IEntity colorToEntitiy(Color color, Vector2 position)
        {
            if(color == PillarColor)
            {
                Texture2D texture = content.Load<Texture2D>(@"Levelgrafiken PNG\Pillar_2x2");
                return new SolidEntities(position + new Vector2(texture.Width/2,texture.Height/2),content.Load<Texture2D>(@"Levelgrafiken PNG\Pillar_2x2"));
            }
            return null;
        }

        public void Draw(GameTime gameTime,  SpriteBatch spriteBatch)
        {
            for (int i = 0; i < passebleEntities.Count; ++i)
            {
                passebleEntities[i].Draw(gameTime, spriteBatch);
            }
            for (int i = 0; i < solidEntities.Count; ++i)
            {
                solidEntities[i].Draw(gameTime, spriteBatch);
            }
        }


    }
}
