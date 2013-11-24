using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using World_Of_Rectangle.Game.Entities;

using sat.Etc;

namespace World_Of_Rectangle.Game
{
    class World
    {
        static ContentManager content;

        public Vector2 CamPosition
        {
            get { return EntityManager.Player.Position; }
        }

        //solid colours
        readonly static Color WallColor = new Color(0, 0, 0);
        readonly static Color PillarColor = new Color(0, 255, 0);
        readonly static Color TreeColor = new Color(0, 230, 0);
        readonly static Color BushColor = new Color(0, 210, 0);
        readonly static Color TableColor = new Color(0, 190, 0);
        readonly static Color BookshelfColor = new Color(0, 170, 0);
        readonly static Color OvenColor = new Color(0, 150, 0);
        readonly static Color BasketColor = new Color(0, 130, 0);
        readonly static Color Table_4x4Color = new Color(0, 110, 0);
        readonly static Color Table_GiantColor = new Color(0, 90, 0);
        readonly static Color Door_brokenColor = new Color(0, 70, 0);
        readonly static Color Bed_LargeColor = new Color(0, 50, 0);
        readonly static Color BedColor = new Color(0, 30, 0);
        readonly static Color ObstacleColor = new Color(0, 10, 0);

        readonly static Color ChimneyColor = new Color(0, 240, 0);
        readonly static Color ForgeColor = new Color(0, 220, 0);
        readonly static Color AnvilColor = new Color(0, 200, 0);
        readonly static Color WhetstoneColor = new Color(0, 180, 0);
        readonly static Color WatertrugColor = new Color(0, 160, 0);
        readonly static Color CthulhuColor = new Color(0, 140, 0);
        readonly static Color PondColor = new Color(0, 120, 0);
        readonly static Color HorseColor = new Color(0, 100, 0);
        readonly static Color Horse_InvisibleColor = new Color(0, 80, 0);
        readonly static Color GateColor = new Color(0, 60, 0);
        readonly static Color BoxesColor = new Color(0, 40, 0);
        



        readonly static Color Door_1Color = new Color(255, 250, 0);
        readonly static Color Door_2Color = new Color(255, 230, 0);
        readonly static Color ChandelierColor = new Color(255, 210, 0);
        readonly static Color Chair_GroupColor = new Color(255, 190, 0);
        readonly static Color ChickenColor = new Color(255, 170, 0);
        readonly static Color BenchColor = new Color(255, 150, 0);
        readonly static Color BookColor = new Color(255, 110, 0);
        readonly static Color WebColor = new Color(255, 90, 0);
        readonly static Color MasterswordColor = new Color(255, 70, 0);
        readonly static Color HoardColor = new Color(255, 50, 0);
        readonly static Color Door_BossColor = new Color(255, 30, 0);
        readonly static Color JewelryColor = new Color(255, 10, 0);

        readonly static Color KeyColor = new Color(100, 100, 100);


        public World(string filepath)
        {
            Texture2D map = sat.Etc.Helper.loadImage(filepath);
            Color[] pixel = new Color[map.Width * map.Height];
            map.GetData<Color>(pixel);

            Vector2 startPoint = Vector2.Zero;
            Spawner.Initialize(content);
            EntityManager.Initialize(new Point(map.Width / 5, map.Height / 5), new Point(map.Width, map.Height), Vector2.Zero);

            for (int i = 0; i < pixel.Length; ++i)
            {
                int x = i % map.Width;
                int y = i / map.Width;
                Color color = pixel[i];
                if (color.B == 0 && color.G == 0 && color.R > 0)
                {
                    Spawner spawner = colorToSpawner(color, new Vector2(x, y) * Global.TILE_SIZE);
                    if (spawner != null)
                    {
                        EntityManager.addSpawnerToChunk(spawner);
                    }
                }
                else
                {

                    IEntity entity = colorToEntitiy(color, new Vector2(x, y) * Global.TILE_SIZE);
                    if (entity != null)
                    {
                        if (entity.Moveable)
                        {
                            EntityManager.addPassableEntities(entity);
                        }
                        else
                        {
                            EntityManager.addSolidEntityToChunks(entity);
                        }
                    }
                    else
                    {
                        if (pixel[i] == new Color(0, 0, 255))
                        {
                            startPoint = new Vector2(x, y) * Global.TILE_SIZE;
                        }
                    }
                }
            }
            EntityManager.LoadContent(content);
            EntityManager.Player.Position = startPoint;
        }

        public static void Initialize(ContentManager content)
        {
            World.content = content;
        }

        public void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
        }

        private static IEntity colorToEntitiy(Color color, Vector2 position)
        {
            IEntity result = null;
            if (color.A == 0)
            {
                return null;
            }
            else if (color == WallColor)
            {
                Texture2D texture = content.Load<Texture2D>(@"Levelgrafiken PNG\Wall");
                result = new SolidEntities(position + new Vector2(texture.Width / 2, texture.Height / 2), content.Load<Texture2D>(@"Levelgrafiken PNG\Wall"));
            }
            else if (color == PillarColor)
            {
                Texture2D texture = content.Load<Texture2D>(@"Levelgrafiken PNG\Pillar_2x2");
                result = new SolidEntities(position + new Vector2(texture.Width / 2, texture.Height / 2), content.Load<Texture2D>(@"Levelgrafiken PNG\Pillar_2x2"));
            }
            else if (color == TreeColor)
            {
                Texture2D texture = content.Load<Texture2D>(@"Levelgrafiken PNG\Tree");
                result = new SolidEntities(position + new Vector2(texture.Width / 2, texture.Height / 2), content.Load<Texture2D>(@"Levelgrafiken PNG\Tree"));
            }
            else if (color == BushColor)
            {
                Texture2D texture = content.Load<Texture2D>(@"Levelgrafiken PNG\Bush");
                result = new SolidEntities(position + new Vector2(texture.Width / 2, texture.Height / 2), content.Load<Texture2D>(@"Levelgrafiken PNG\Bush"));
            }
            else if (color == TableColor)
            {
                Texture2D texture = content.Load<Texture2D>(@"Levelgrafiken PNG\Table_Round");
                result = new SolidEntities(position + new Vector2(texture.Width / 2, texture.Height / 2), content.Load<Texture2D>(@"Levelgrafiken PNG\Table_Round"));
            }
            else if (color == BookshelfColor)
            {
                Texture2D texture = content.Load<Texture2D>(@"Levelgrafiken PNG\Bookshelf");
                result = new SolidEntities(position + new Vector2(texture.Width / 2, texture.Height / 2), content.Load<Texture2D>(@"Levelgrafiken PNG\Bookshelf"));
            }
            else if (color == OvenColor)
            {
                Texture2D texture = content.Load<Texture2D>(@"Levelgrafiken PNG\Oven");
                result = new SolidEntities(position + new Vector2(texture.Width / 2, texture.Height / 2), content.Load<Texture2D>(@"Levelgrafiken PNG\Oven"));
            }
            else if (color == BasketColor)
            {
                Texture2D texture = content.Load<Texture2D>(@"Levelgrafiken PNG\Basket");
                result = new SolidEntities(position + new Vector2(texture.Width / 2, texture.Height / 2), content.Load<Texture2D>(@"Levelgrafiken PNG\Basket"));
            }
            else if (color == Table_4x4Color)
            {
                Texture2D texture = content.Load<Texture2D>(@"Levelgrafiken PNG\Table_4x4");
                result = new SolidEntities(position + new Vector2(texture.Width / 2, texture.Height / 2), content.Load<Texture2D>(@"Levelgrafiken PNG\Table_4x4"));
            }
            else if (color == Table_GiantColor)
            {
                Texture2D texture = content.Load<Texture2D>(@"Levelgrafiken PNG\Table_Giant");
                result = new SolidEntities(position + new Vector2(texture.Width / 2, texture.Height / 2), content.Load<Texture2D>(@"Levelgrafiken PNG\Table_Giant"));
            }
            else if (color == Door_brokenColor)
            {
                Texture2D texture = content.Load<Texture2D>(@"Levelgrafiken PNG\Door_broken");
                result = new SolidEntities(position + new Vector2(texture.Width / 2, texture.Height / 2), content.Load<Texture2D>(@"Levelgrafiken PNG\Door_broken"));
            }
            else if (color == Bed_LargeColor)
            {
                Texture2D texture = content.Load<Texture2D>(@"Levelgrafiken PNG\Bed_Large");
                result = new SolidEntities(position + new Vector2(texture.Width / 2, texture.Height / 2), content.Load<Texture2D>(@"Levelgrafiken PNG\Bed_Large"));
            }
            else if (color == BedColor)
            {
                Texture2D texture = content.Load<Texture2D>(@"Levelgrafiken PNG\Bed");
                result = new SolidEntities(position + new Vector2(texture.Width / 2, texture.Height / 2), content.Load<Texture2D>(@"Levelgrafiken PNG\Bed"));
            }
            else if (color == ObstacleColor)
            {
                Texture2D texture = content.Load<Texture2D>(@"Levelgrafiken PNG\Obstacle");
                result = new SolidEntities(position + new Vector2(texture.Width / 2, texture.Height / 2), content.Load<Texture2D>(@"Levelgrafiken PNG\Obstacle"));
            }
            else if (color == ChimneyColor)
            {
                Texture2D texture = content.Load<Texture2D>(@"Levelgrafiken PNG\Chimney");
                result = new SolidEntities(position + new Vector2(texture.Width / 2, texture.Height / 2), content.Load<Texture2D>(@"Levelgrafiken PNG\Chimney"));
            }
            else if (color == ForgeColor)
            {
                Texture2D texture = content.Load<Texture2D>(@"Levelgrafiken PNG\Forge");
                result = new SolidEntities(position + new Vector2(texture.Width / 2, texture.Height / 2), content.Load<Texture2D>(@"Levelgrafiken PNG\Forge"));
            }
            else if (color == AnvilColor)
            {
                Texture2D texture = content.Load<Texture2D>(@"Levelgrafiken PNG\Anvil");
                result = new SolidEntities(position + new Vector2(texture.Width / 2, texture.Height / 2), content.Load<Texture2D>(@"Levelgrafiken PNG\Anvil"));
            }
            else if (color == WhetstoneColor)
            {
                Texture2D texture = content.Load<Texture2D>(@"Levelgrafiken PNG\Whetstone");
                result = new SolidEntities(position + new Vector2(texture.Width / 2, texture.Height / 2), content.Load<Texture2D>(@"Levelgrafiken PNG\Whetstone"));
            }
            else if (color == WatertrugColor)
            {
                Texture2D texture = content.Load<Texture2D>(@"Levelgrafiken PNG\Watertrug");
                result = new SolidEntities(position + new Vector2(texture.Width / 2, texture.Height / 2), content.Load<Texture2D>(@"Levelgrafiken PNG\Watertrug"));
            }
            else if (color == CthulhuColor)
            {
                Texture2D texture = content.Load<Texture2D>(@"Levelgrafiken PNG\Cthulhu");
                result = new SolidEntities(position + new Vector2(texture.Width / 2, texture.Height / 2), content.Load<Texture2D>(@"Levelgrafiken PNG\Cthulhu"));
            }
            else if (color == PondColor)
            {
                Texture2D texture = content.Load<Texture2D>(@"Levelgrafiken PNG\Pond");
                result = new SolidEntities(position + new Vector2(texture.Width / 2, texture.Height / 2), content.Load<Texture2D>(@"Levelgrafiken PNG\Pond"));
            }
            else if (color == HorseColor)
            {
                Texture2D texture = content.Load<Texture2D>(@"Levelgrafiken PNG\Horse");
                result = new SolidEntities(position + new Vector2(texture.Width / 2, texture.Height / 2), content.Load<Texture2D>(@"Levelgrafiken PNG\Horse"));
            }
            else if (color == Horse_InvisibleColor)
            {
                Texture2D texture = content.Load<Texture2D>(@"Levelgrafiken PNG\Horse_Invisible");
                result = new SolidEntities(position + new Vector2(texture.Width / 2, texture.Height / 2), content.Load<Texture2D>(@"Levelgrafiken PNG\Horse_Invisible"));
            }
            else if (color == GateColor)
            {
                Texture2D texture = content.Load<Texture2D>(@"Levelgrafiken PNG\Gate");
                result = new SolidEntities(position + new Vector2(texture.Width / 2, texture.Height / 2), content.Load<Texture2D>(@"Levelgrafiken PNG\Gate"));
            }
            else if (color == BoxesColor)
            {
                Texture2D texture = content.Load<Texture2D>(@"Levelgrafiken PNG\Boxes");
                result = new SolidEntities(position + new Vector2(texture.Width / 2, texture.Height / 2), content.Load<Texture2D>(@"Levelgrafiken PNG\Boxes"));
            }




       //Passable objects
            else if (color == Door_1Color)
            {
                Texture2D texture = content.Load<Texture2D>(@"Levelgrafiken PNG\Door_1");
                result = new PassableEntities(position + new Vector2(texture.Width / 2, texture.Height / 2), content.Load<Texture2D>(@"Levelgrafiken PNG\Door_1"));
            }
            else if (color == Door_2Color)
            {
                Texture2D texture = content.Load<Texture2D>(@"Levelgrafiken PNG\Door_2");
                result = new PassableEntities(position + new Vector2(texture.Width / 2, texture.Height / 2), content.Load<Texture2D>(@"Levelgrafiken PNG\Door_2"));
            }
            else if (color == ChandelierColor)
            {
                Texture2D texture = content.Load<Texture2D>(@"Levelgrafiken PNG\Chandelier");
                result = new PassableEntities(position + new Vector2(texture.Width / 2, texture.Height / 2), content.Load<Texture2D>(@"Levelgrafiken PNG\Chandelier"));
            }
            else if (color == Chair_GroupColor)
            {
                Texture2D texture = content.Load<Texture2D>(@"Levelgrafiken PNG\Chair_Group");
                result = new PassableEntities(position + new Vector2(texture.Width / 2, texture.Height / 2), content.Load<Texture2D>(@"Levelgrafiken PNG\Chair_Group"));
            }
            else if (color == ChickenColor)
            {
                Texture2D texture = content.Load<Texture2D>(@"Levelgrafiken PNG\Chicken");
                result = new PassableEntities(position + new Vector2(texture.Width / 2, texture.Height / 2), content.Load<Texture2D>(@"Levelgrafiken PNG\Chicken"));
            }
            else if (color == BenchColor)
            {
                Texture2D texture = content.Load<Texture2D>(@"Levelgrafiken PNG\Bench");
                result = new PassableEntities(position + new Vector2(texture.Width / 2, texture.Height / 2), content.Load<Texture2D>(@"Levelgrafiken PNG\Bench"));
            }
            else if (color == BookColor)
            {
                Texture2D texture = content.Load<Texture2D>(@"Levelgrafiken PNG\Book");
                result = new PassableEntities(position + new Vector2(texture.Width / 2, texture.Height / 2), content.Load<Texture2D>(@"Levelgrafiken PNG\Book"));
            }
            else if (color == WebColor)
            {
                Texture2D texture = content.Load<Texture2D>(@"Levelgrafiken PNG\Web");
                result = new PassableEntities(position + new Vector2(texture.Width / 2, texture.Height / 2), content.Load<Texture2D>(@"Levelgrafiken PNG\Web"));
            }
            else if (color == MasterswordColor)
            {
                Texture2D texture = content.Load<Texture2D>(@"Levelgrafiken PNG\Mastersword");
                result = new PassableEntities(position + new Vector2(texture.Width / 2, texture.Height / 2), content.Load<Texture2D>(@"Levelgrafiken PNG\Mastersword"));
            }
            else if (color == HoardColor)
            {
                Texture2D texture = content.Load<Texture2D>(@"Levelgrafiken PNG\Hoard");
                result = new PassableEntities(position + new Vector2(texture.Width / 2, texture.Height / 2), content.Load<Texture2D>(@"Levelgrafiken PNG\Hoard"));
            }
            else if (color == Door_BossColor)
            {
                Texture2D texture = content.Load<Texture2D>(@"Levelgrafiken PNG\Door_Boss");
                result = new PassableEntities(position + new Vector2(texture.Width / 2, texture.Height / 2), content.Load<Texture2D>(@"Levelgrafiken PNG\Door_Boss"));
            }
            else if (color == JewelryColor)
            {
                Texture2D texture = content.Load<Texture2D>(@"Levelgrafiken PNG\Jewelry");
                result = new PassableEntities(position + new Vector2(texture.Width / 2, texture.Height / 2), content.Load<Texture2D>(@"Levelgrafiken PNG\Jewelry"));
            }
            else if (color == KeyColor)
            {
                Texture2D texture = content.Load<Texture2D>(@"Levelgrafiken PNG\Key");
                result = new PassableEntities(position + new Vector2(texture.Width / 2, texture.Height / 2), content.Load<Texture2D>(@"Levelgrafiken PNG\Key"));
            }

            return result;

        }

        private static Spawner colorToSpawner(Color color, Vector2 position)
        {
            Spawner result = null;
            if (color.R == 250)
            {
                result = new Spawner(0.01f,position,new Point[]{new Point(1,1)});
            }
            if (color.R == 230)
            {
                result = new Spawner(0.01f, position, new Point[] { new Point(1, 2) });
            }
            if (color.R == 210)
            {
                result = new Spawner(0.01f, position, new Point[] { new Point(1, 3) });
            }
            if (color.R == 190)
            {
                result = new Spawner(0.01f, position, new Point[] { new Point(2, 2) });
            }
            if (color.R == 170)
            {
                result = new Spawner(0.01f, position, new Point[] { new Point(2, 3) });
            }
            if (color.R == 90)
            {
                result = new Spawner(0.01f, position, new Point[] { new Point(3, 4) });
            }
            if (color.R == 70)
            {
                result = new Spawner(0.01f, position, new Point[] { new Point(4, 4) });
            }

            return result;

        }

        public void Update(GameTime gameTime)
        {

            EntityManager.Update(gameTime);

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            EntityManager.Draw(gameTime, spriteBatch);
        }


    }
}
