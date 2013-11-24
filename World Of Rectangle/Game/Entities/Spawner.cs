using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using World_Of_Rectangle.Game.Entities.Enemies;

namespace World_Of_Rectangle.Game.Entities
{
    enum Enemies_1x1
    {
        Rat,
        Kobold,
        Irrlicht,
        Pixy,
        Imp,



        Count,
    }
    enum Enemies_1x2
    {
        Dwarf,
        Creature,
        Goblin,





        Count,
    }
    enum Enemies_1x3
    {
        Skeletton,
        Bandit,
        DarkElve,
        DarkKnight,
        Elve,
        Gnoll,
        Hobgoblin,
        Lich,
        Orc,
        Vampire,
        Werewolf,

       // Skeletton,Bandit,DarkElve,DarkKnight, Elve,Gnoll,Hobgoblin,Lich,Orc,Vampire,Werewolf,


        Count,
    }
    enum Enemies_1x4
    {



        Count,
    }
    enum Enemies_2x2
    {
        Slime,



        Count,
    }
    enum Enemies_2x3
    {
        Zombie,
        Mummy,
        Ghul,
        Bear,
        Basilisk,
        Ghostwolf,
        Warg,
        Wolf,
        Zombiehound,

//Zombie,Mummy,Ghul,Bear,Basilisk,Ghostwolf,Warg,Wolf,Zombiehound,






        Count,
    }
    enum Enemies_2x4
    {



        Count,
    }
    enum Enemies_3x3
    {



        Count,
    }
    enum Enemies_3x4
    {



        Count,
    }
    enum Enemies_4x4
    {
        Dragon_Skeletton,


        Count,
    }

    class Spawner
    {

        static int[] enemyNumbers = {   (int)Enemies_1x1.Count, (int)Enemies_1x2.Count, (int)Enemies_1x3.Count, (int)Enemies_1x4.Count,
                                        (int)Enemies_2x2.Count, (int)Enemies_2x3.Count, (int)Enemies_2x4.Count,
                                        (int)Enemies_3x3.Count, (int)Enemies_3x4.Count,
                                        (int)Enemies_4x4.Count
                                    };

        static Random random;

        float spawnRate;
        int possibilitieCount;
        Point[] sizes;

        Vector2 position;

        static ContentManager content;

        public Vector2 Position
        {
            get { return position; }
        }

        public static void Initialize(ContentManager content)
        {
            Spawner.content = content;
            random = new Random();
        }

        public Spawner(float spawnRate, Vector2 position, Point[] sizes)
        {
            this.position = position;
            this.spawnRate = spawnRate;
            this.sizes = sizes;
            this.possibilitieCount = getPossibilityNumber(sizes);
        }

        private static int getPossibilityNumber(Point[] sizes)
        {
            int result = 0;
            for (int i = 0; i < sizes.Length; ++i)
            {
                int min = Math.Min(sizes[i].X, sizes[i].Y);
                int max = sizes[i].X + sizes[i].Y - min;
                bool found = false;
                for (int x = 1, counter = 0; x <= 4 && !found; ++x)
                {
                    for (int y = x; y <= 4 && !found; ++y, ++counter)
                    {
                        if (x == min && y == max)
                        {
                            result += enemyNumbers[counter];
                            found = true;
                            break;
                        }
                    }
                }
            }
            return result;
        }

        public static IEntity spawnEntity(Point[] sizes, int index, Vector2 position)
        {
            for (int i = 0; i < sizes.Length; ++i)
            {
                int min = Math.Min(sizes[i].X, sizes[i].Y);
                int max = sizes[i].X + sizes[i].Y - min;
                bool found = false;
                for (int x = 1, counter = 0; x <= 4 && !found; ++x)
                {
                    for (int y = x; y <= 4 && !found; ++y, ++counter)
                    {
                        if (x == min && y == max)
                        {
                            if (index >= enemyNumbers[counter])
                            {
                                index -= enemyNumbers[counter];
                                found = true;
                            }
                            else
                            {
                                return spawnEntity(min, max,position, index);
                            }
                            break;
                        }
                    }
                }

            }

            return null;
        }

        public static IEntity spawnEntity(int x, int y,Vector2 position, int index)
        {
            switch (x)
            {
                case 1:
                    switch (y)
                    {
                        case 1:
                            switch ((Enemies_1x1)index)
                            {
                                case Enemies_1x1.Rat:
                                    return new BasicEnemy(position, 5, 10, 50, content.Load<Texture2D>(@"Enemies\E_Rat"));
                                case Enemies_1x1.Imp:
                                    return new BasicEnemy(position, 5, 10, 50, content.Load<Texture2D>(@"Enemies\E_Imp"));
                                case Enemies_1x1.Kobold:
                                    return new BasicEnemy(position, 5, 10, 50, content.Load<Texture2D>(@"Enemies\E_Kobold"));
                                case Enemies_1x1.Pixy:
                                    return new BasicEnemy(position, 5, 10, 50, content.Load<Texture2D>(@"Enemies\E_Pixy"));
                                case Enemies_1x1.Irrlicht:
                                    return new BasicEnemy(position, 5, 10, 50, content.Load<Texture2D>(@"Enemies\E_Irrlicht"));
                            }
                            break;
                        case 2:
                            switch ((Enemies_1x2)index)
                            {
                                case Enemies_1x2.Dwarf:
                                    return new BasicEnemy(position, 5, 10, 50, content.Load<Texture2D>(@"Enemies\E_Dwarf"));
                                case Enemies_1x2.Creature:
                                    return new BasicEnemy(position, 5, 10, 50, content.Load<Texture2D>(@"Enemies\E_Creature"));
                                case Enemies_1x2.Goblin:
                                    return new BasicEnemy(position, 5, 10, 50, content.Load<Texture2D>(@"Enemies\E_Goblin"));
                           
                            }
                            break;
                        case 3:
                            switch ((Enemies_1x3)index)
                            {
                                case Enemies_1x3.Skeletton:
                                    return new BasicEnemy(position, 5, 10, 50, content.Load<Texture2D>(@"Enemies\E_Skeletton"));
                                case Enemies_1x3.Bandit:
                                    return new BasicEnemy(position, 5, 10, 50, content.Load<Texture2D>(@"Enemies\E_Bandit"));
                                case Enemies_1x3.DarkElve:
                                    return new BasicEnemy(position, 5, 10, 50, content.Load<Texture2D>(@"Enemies\E_DarkElve"));
                                case Enemies_1x3.DarkKnight:
                                    return new BasicEnemy(position, 5, 10, 50, content.Load<Texture2D>(@"Enemies\E_DarkKnight"));
                                case Enemies_1x3.Elve:
                                    return new BasicEnemy(position, 5, 10, 50, content.Load<Texture2D>(@"Enemies\E_Elve"));
                                case Enemies_1x3.Gnoll:
                                    return new BasicEnemy(position, 5, 10, 50, content.Load<Texture2D>(@"Enemies\E_Gnoll"));
                                case Enemies_1x3.Hobgoblin:
                                    return new BasicEnemy(position, 5, 10, 50, content.Load<Texture2D>(@"Enemies\E_Hobgoblin"));
                                case Enemies_1x3.Lich:
                                    return new BasicEnemy(position, 5, 10, 50, content.Load<Texture2D>(@"Enemies\E_Lich"));
                                case Enemies_1x3.Orc:
                                    return new BasicEnemy(position, 5, 10, 50, content.Load<Texture2D>(@"Enemies\E_Orc"));
                                case Enemies_1x3.Vampire:
                                    return new BasicEnemy(position, 5, 10, 50, content.Load<Texture2D>(@"Enemies\E_Vampire"));
                                case Enemies_1x3.Werewolf:
                                    return new BasicEnemy(position, 5, 10, 50, content.Load<Texture2D>(@"Enemies\E_Werewolf"));
                            }
                            break;
                        case 4:
                            switch ((Enemies_1x4)index)
                            {

                            }
                            break;
                    }
                    break;
                case 2:
                    switch (y)
                    {
                        case 2:
                            switch ((Enemies_2x2)index)
                            {
                                case Enemies_2x2.Slime:
                                    return new BasicEnemy(position, 5, 10, 50, content.Load<Texture2D>(@"Enemies\E_Slime"));
                            }
                            break;
                        case 3:
                            switch ((Enemies_2x3)index)
                            {
                                //Zombie,Mummy,Ghul,Bear,Basilisk,Ghostwolf,Warg,Wolf,Zombiehound,
                                case Enemies_2x3.Zombie:
                                    return new BasicEnemy(position, 5, 10, 50, content.Load<Texture2D>(@"Enemies\E_Zombie"));
                                case Enemies_2x3.Mummy:
                                    return new BasicEnemy(position, 5, 10, 50, content.Load<Texture2D>(@"Enemies\E_Mummy"));
                                case Enemies_2x3.Ghul:
                                    return new BasicEnemy(position, 5, 10, 50, content.Load<Texture2D>(@"Enemies\E_Ghul"));
                                case Enemies_2x3.Bear:
                                    return new BasicEnemy(position, 5, 10, 50, content.Load<Texture2D>(@"Enemies\E_Bear"));
                                case Enemies_2x3.Basilisk:
                                    return new BasicEnemy(position, 5, 10, 50, content.Load<Texture2D>(@"Enemies\E_Basilisk"));
                                case Enemies_2x3.Ghostwolf:
                                    return new BasicEnemy(position, 5, 10, 50, content.Load<Texture2D>(@"Enemies\E_Warg"));
                                case Enemies_2x3.Warg:
                                    return new BasicEnemy(position, 5, 10, 50, content.Load<Texture2D>(@"Enemies\E_Warg"));
                                case Enemies_2x3.Wolf:
                                    return new BasicEnemy(position, 5, 10, 50, content.Load<Texture2D>(@"Enemies\E_Wolf"));
                                case Enemies_2x3.Zombiehound:
                                    return new BasicEnemy(position, 5, 10, 50, content.Load<Texture2D>(@"Enemies\E_Zombiehound"));

                            }
                            break;
                        case 4:
                            switch ((Enemies_2x3)index)
                            {

                            }
                            break;
                    }
                    break;
                case 3:
                    switch (y)
                    {
                        case 3:
                            switch ((Enemies_3x3)index)
                            {

                            }
                            break;
                        case 4:
                            switch ((Enemies_3x4)index)
                            {

                            }
                            break;
                    }
                    break;
                case 4:
                    switch (y)
                    {
                        case 4:
                            switch ((Enemies_4x4)index)
                            {
                                case Enemies_4x4.Dragon_Skeletton:
                                    return new BasicEnemy(position, 5, 10, 50, content.Load<Texture2D>(@"Enemies\E_Dragon_Skeletton"));

                            }
                            break;
                    }
                    break;
            }

            return null;
        }

        public IEntity Update(GameTime gameTime)
        {
            if (random.NextDouble() <= spawnRate)
            {
                int index = random.Next(possibilitieCount);
                return spawnEntity(sizes, index, position);
            }
            return null;
        }
    }
}
