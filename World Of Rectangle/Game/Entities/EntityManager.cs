using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using sat.Shape;
using sat.Etc;

namespace World_Of_Rectangle.Game.Entities
{

    class EntityManager
    {

        class Chunk
        {
            public Chunk(RectangleShape shape, int x, int y)
            {
                passableEntities = new List<IEntity>();
                solidEntities = new List<IEntity>();
                enemies = new List<IEntity>();
                spawner = new List<Spawner>();
                this.shape = shape;
                this.x = x;
                this.y = y;
            }

            int x;
            public int X
            {
                get { return x; }
            }

            int y;
            public int Y
            {
                get { return y; }
            }

            List<IEntity> passableEntities;
            public List<IEntity> PassableEntities
            {
                get { return passableEntities; }
            }

            List<IEntity> solidEntities;
            public List<IEntity> SolidEntities
            {
                get { return solidEntities; }
            }

            List<IEntity> enemies;
            public List<IEntity> Enemies
            {
                get { return enemies; }
            }

            private List<Spawner> spawner;
            public List<Spawner> Spawner
            {
                get { return spawner; }
            }

            RectangleShape shape;
            public RectangleShape Shape
            {
                get { return shape; }
            }

            bool hasPlayer;
            public bool HasPlayer
            {
                get { return hasPlayer; }
                set { hasPlayer = value; }
            }

            public void Reset()
            {
                enemies.Clear();
                hasPlayer = false;
            }
        }

        private static List<IEntity> passableEntities;
        public static List<IEntity> PassableEntities
        {
            get { return EntityManager.passableEntities; }
        }

        private static List<IEntity> solidEntities;
        public static List<IEntity> SolidEntities
        {
            get { return EntityManager.solidEntities; }
        }

        private static List<IEntity> enemies;
        public static List<IEntity> Enemies
        {
            get { return EntityManager.enemies; }
        }

        private static Player player;
        public static Player Player
        {
            get { return EntityManager.player; }
        }

        private static Point chunkSize;
        private static Chunk[,] map;

        private static List<PassableEntities> keyShapes;

        public static List<PassableEntities> KeyPositions
        {
            get { return keyShapes; }
            set { keyShapes = value; }
        }

        public static void Initialize(Point chunkSize, Point mapSize, Vector2 startPosition)
        {
            passableEntities = new List<IEntity>();
            solidEntities = new List<IEntity>();
            enemies = new List<IEntity>();
            keyShapes = new List<PassableEntities>();
            EntityManager.chunkSize = chunkSize;
            map = new Chunk[(int)Math.Ceiling(mapSize.X / (float)chunkSize.X), (int)Math.Ceiling(mapSize.Y / (float)chunkSize.Y)];
            for (int x = 0; x <= map.GetUpperBound(0); ++x)
            {
                for (int y = 0; y <= map.GetUpperBound(1); ++y)
                {
                    map[x, y] = new Chunk(new RectangleShape(new Rectangle(0, 0, chunkSize.X * Global.TILE_SIZE, chunkSize.Y * Global.TILE_SIZE), new Vector2((x + 0.5f) * chunkSize.X * Global.TILE_SIZE, (y + 0.5f) * chunkSize.Y * Global.TILE_SIZE)), x, y);
                }
            }


            Keys[] keys = new Keys[(int)Player.ActionKeys.KeyCount];

            keys[(int)Player.ActionKeys.MoveForward] = Keys.W;
            keys[(int)Player.ActionKeys.MoveBackward] = Keys.S;
            keys[(int)Player.ActionKeys.MoveLeft] = Keys.A;
            keys[(int)Player.ActionKeys.MoveRight] = Keys.D;
            keys[(int)Player.ActionKeys.Attack] = Keys.Space;
            keys[(int)Player.ActionKeys.Inventory] = Keys.Escape;

            player = new Player(startPosition, 0.0f, keys);
        }

        public static void LoadContent(ContentManager content)
        {
            player.LoadContent(content);
        }

        public static void Update(GameTime gameTime)
        {
            HashSet<IEntity> deadEnemies = new HashSet<IEntity>();

            player.Update(gameTime);
            addPlayerToChunks(player);

            for (int i = 0; i < enemies.Count; ++i)
            {
                enemies[i].Update(gameTime);
                addEnemyToChunks(enemies[i]);
            }

            foreach (Chunk chunk in map)
            {
                List<IEntity> chunkEnemies = chunk.Enemies;
                List<IEntity> chunkSolidEntities = chunk.SolidEntities;
                List<IEntity> chunkPassableEntities = chunk.PassableEntities;

                if (chunk.HasPlayer)
                {
                    foreach (SolidEntities solidEntity in chunkSolidEntities)
                    {
                        if (!solidEntity.canPass(player))
                        {
                            IntersectData intersectData = solidEntity.Shape.intersects(player.Shape);
                            if (intersectData.Intersects)
                            {
                                player.Position += intersectData.Mtv * intersectData.Distance;
                            }
                        }
                    }
                }
                else
                {
                    foreach (Spawner spawner in chunk.Spawner)
                    {
                        IEntity entity = spawner.Update(gameTime);
                        if (entity != null)
                        {
                            addEnemy(entity);
                        }
                    }
                }
                if (chunk.HasPlayer)
                {
                    for (int i = 0; i < chunkEnemies.Count; ++i)
                    {
                        IEntity enemy1 = chunkEnemies[i];
                        foreach (IEntity solidEntity in chunkSolidEntities)
                        {
                            IntersectData intersectData = enemy1.Shape.intersects(solidEntity.Shape);
                            if (intersectData.Intersects)
                            {
                                enemy1.Position -= intersectData.Mtv * intersectData.Distance;
                            }
                        }
                        foreach (PassableEntities passableEntities in chunkPassableEntities)
                        {
                            IntersectData intersectData = enemy1.Shape.intersects(passableEntities.Shape);
                            if (intersectData.Intersects)
                            {
                                enemy1.Position -= intersectData.Mtv * intersectData.Distance;
                            }
                        }

                        if (chunk.HasPlayer)
                        {
                            IntersectData intersectData = enemy1.Shape.intersects(player.Shape);
                            if (intersectData.Intersects)
                            {
                                enemy1.Position -= intersectData.Mtv * intersectData.Distance / 2;
                                player.Position += intersectData.Mtv * intersectData.Distance / 2;
                                player.receiveDamageFrom(enemy1);
                            }
                            if (player.isAttacking)
                            {
                                intersectData = enemy1.Shape.intersects(player.Weapon.Shape);
                                if (intersectData.Intersects)
                                {
                                    enemy1.receiveDamageFrom(player.Weapon);
                                    if (!enemy1.stillLiving)
                                    {
                                        deadEnemies.Add(enemy1);
                                    }
                                }
                            }
                        }

                        for (int j = 0; j < chunkEnemies.Count; ++j)
                        {
                            if (j != i)
                            {
                                IEntity enemy2 = chunkEnemies[j];
                                IntersectData intersectData = enemy1.Shape.intersects(enemy2.Shape);
                                if (intersectData.Intersects)
                                {
                                    enemy1.Position -= intersectData.Mtv * intersectData.Distance / 2;
                                    enemy2.Position += intersectData.Mtv * intersectData.Distance / 2;
                                }
                            }
                        }
                    }
                }
            }
            PassableEntities keyDeleted = null;
            for (int i = 0; i < keyShapes.Count; ++i)
            {
                if (player.Shape.intersects(keyShapes[i].Shape).Intersects)
                {
                    keyDeleted = keyShapes[i];
                }
            }
            if (keyDeleted != null)
            {
                keyShapes.Remove(keyDeleted);
                passableEntities.Remove(keyDeleted);
                player.KeyNumber++;
            }

            for (int x = 0; x <= map.GetUpperBound(0); ++x)
            {
                for (int y = 0; y <= map.GetUpperBound(1); ++y)
                {
                    Chunk chunk = map[x, y];
                    chunk.Reset();
                }
            }
            foreach (IEntity deadEnemy in deadEnemies)
            {
                enemies.Remove(deadEnemy);
            }

        }

        public static void addSpawnerToChunk(Spawner spawner)
        {
            Point baseChunkCoords = new Point((int)Math.Floor(Math.Ceiling(spawner.Position.X / Global.TILE_SIZE) / chunkSize.X), (int)Math.Floor(Math.Ceiling(spawner.Position.Y / Global.TILE_SIZE) / chunkSize.Y));
            map[baseChunkCoords.X, baseChunkCoords.Y].Spawner.Add(spawner);
        }

        public static void addEnemy(IEntity enemy)
        {
            enemies.Add(enemy);
            addEnemyToChunks(enemy);
        }
        public static void addEnemyToChunks(IEntity enemy)
        {
            List<Chunk> chunks = getChunk(enemy);
            foreach (Chunk chunk in chunks)
            {
                chunk.Enemies.Add(enemy);
            }
        }
        public static void addPlayerToChunks(IEntity player)
        {
            List<Chunk> chunks = getChunk(player);
            foreach (Chunk chunk in chunks)
            {
                chunk.HasPlayer = true;
            }
        }
        public static void addSolidEntityToChunks(IEntity solidEntity)
        {
            List<Chunk> chunks = getChunk(solidEntity);
            foreach (Chunk chunk in chunks)
            {
                chunk.SolidEntities.Add(solidEntity);
            }
            solidEntities.Add(solidEntity);
        }
        public static void addPassableEntities(IEntity passableEntity)
        {
            List<Chunk> chunks = getChunk(passableEntity);
            foreach (Chunk chunk in chunks)
            {
                chunk.PassableEntities.Add(passableEntity);
            }
            passableEntities.Add(passableEntity);
        }

        private static List<Chunk> getChunk(IEntity entity)
        {
            List<Chunk> c = new List<Chunk>();

            Vector2 baseChunkCoords = new Vector2((int)Math.Floor(Math.Ceiling(entity.Position.X / Global.TILE_SIZE) / chunkSize.X), (int)Math.Floor(Math.Ceiling(entity.Position.Y / Global.TILE_SIZE) / chunkSize.Y));

            c.Add(map[(int)baseChunkCoords.X, (int)baseChunkCoords.Y]);

            for (int y = -1; y <= 1; ++y)
            {
                for (int x = -1; x <= 1; ++x)
                {
                    if (!(x == 0 && y == 0))
                    {
                        Vector2 chunkCoords = new Vector2(x, y) + baseChunkCoords;
                        if (chunkCoords.X >= 0 && chunkCoords.Y >= 0 && chunkCoords.X <= map.GetUpperBound(0) && chunkCoords.Y <= map.GetUpperBound(1))
                        {
                            Chunk currentChunk = map[(int)chunkCoords.X, (int)chunkCoords.Y];
                            if (entity.Shape.intersects(currentChunk.Shape).Intersects)
                            {
                                c.Add(currentChunk);
                            }
                        }
                    }
                }
            }
            return c;
        }

        public static void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            player.Draw(gameTime, spriteBatch);

            for (int i = 0; i < enemies.Count; ++i)
            {
                enemies[i].Draw(gameTime, spriteBatch);
            }
            for (int i = 0; i < passableEntities.Count; ++i)
            {
                passableEntities[i].Draw(gameTime, spriteBatch);
            }
            for (int i = 0; i < solidEntities.Count; ++i)
            {
                solidEntities[i].Draw(gameTime, spriteBatch);
            }
        }

    }
}
