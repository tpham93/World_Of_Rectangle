using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using World_Of_Rectangle.Game.Entities;



namespace World_Of_Rectangle.Game
{
    class World
    {
        static ContentManager content;

        Player player;

        List<IEntity> passebleEntities;
        List<IEntity> solidEntities;

        List<IEntity> enemies;


        public Vector2 CamPosition
        {
            get { return player.Position; }
        }

        public List<IEntity> PassebleEntities
        {
            get { return passebleEntities; }
            set { passebleEntities = value; }
        }
        public List<IEntity> SolidEntities
        {
            get { return solidEntities; }
            set { solidEntities = value; }
        }

        readonly static Color PillarColor = new Color(0,0,0);


        public World(string filepath)
        {
            passebleEntities = new List<IEntity>();
            solidEntities = new List<IEntity>();

            Texture2D map = sat.Etc.Helper.loadImage(filepath);
            Color[] pixel = new Color[map.Width *  map.Height];
            map.GetData<Color>(pixel);

            Vector2 startPoint = Vector2.Zero;

            for (int i = 0; i < pixel.Length; ++i)
            {
                int x = i % map.Width;
                int y = i / map.Width;
                IEntity entity = colorToEntitiy(pixel[i], new Vector2(x,y) * Global.TILE_SIZE);
                if (entity != null)
                {
                    if (entity.Moveable)
                    {
                        passebleEntities.Add(entity);
                    }
                    else
                    {
                        solidEntities.Add(entity);
                    }
                }
                else
                {
                    if (pixel[i] == new Color(0,0,255))
                    {
                        startPoint = new Vector2((x * Global.TILE_SIZE), (y * Global.TILE_SIZE));
                    }
                }
            }


            Keys[] keys = new Keys[(int)Player.ActionKeys.KeyCount];

            keys[(int)Player.ActionKeys.MoveForward] = Keys.W;
            keys[(int)Player.ActionKeys.MoveBackward] = Keys.S;
            keys[(int)Player.ActionKeys.MoveLeft] = Keys.A;
            keys[(int)Player.ActionKeys.MoveRight] = Keys.D;
            keys[(int)Player.ActionKeys.Attack] = Keys.Space;
            keys[(int)Player.ActionKeys.Inventory] = Keys.Escape;

            player = new Player(startPoint, 0.0f, keys);

        }

        public static void Initialize(ContentManager content)
        {
            World.content = content;
        }


        public void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            player.LoadContent(content);
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

        public void Update(GameTime gameTime)
        {
            player.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            player.Draw(gameTime,spriteBatch);

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
