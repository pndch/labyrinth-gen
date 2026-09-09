using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace labyrinth_gen
{
    internal class Generator
    {
        
        public static bool[,] Create(int size, int seed) //prim's algo
        {
            int[] startPoint = [0, 0]; // x,y
            

            Random rnd = new Random(); //сюда можно вставить seed
            List<(int, int)> list = new List<(int, int)>();
            bool[,] maze = new bool[size, size];

            // prepare
            void paintWalls(int x, int y)
            {
                if (x != size - 1)
                {
                    list.Add((x + 1, y));
                }
                if (y != size - 1)
                {
                    list.Add((x, y + 1));
                }
                if (x != 0)
                {
                    list.Add((x - 1, y));
                }
                if (y != 0)
                {
                    list.Add((x, y - 1));
                }
            }

            maze[startPoint[0], startPoint[1]] = true;

            // generating
            paintWalls(startPoint[0], startPoint[1]);

            while (list.Count > 0)
            {
                int iIndex = rnd.Next(list.Count);
                var item = list[iIndex];
                list[iIndex] = list[list.Count-1];
                list.RemoveAt(list.Count - 1);
                try
                {
                    if (maze[item.Item1 - 1, item.Item2] != maze[item.Item1 + 1, item.Item2])
                    {
                        if (maze[item.Item1 - 1, item.Item2] == false)
                        {
                            maze[item.Item1 - 1, item.Item2] = true;
                            maze[item.Item1, item.Item2] = true;

                            paintWalls(item.Item1 - 1, item.Item2);
                        }
                        else if (maze[item.Item1 + 1, item.Item2] == false)
                        {
                            maze[item.Item1 + 1, item.Item2] = true;
                            maze[item.Item1, item.Item2] = true;

                            paintWalls(item.Item1 + 1, item.Item2);
                        }
                    }
                } catch { }
                try
                {
                    if (maze[item.Item1, item.Item2-1] != maze[item.Item1, item.Item2 + 1])
                    {
                        if (maze[item.Item1, item.Item2 - 1] == false)
                        {
                            maze[item.Item1, item.Item2 - 1] = true;
                            maze[item.Item1, item.Item2] = true;

                            paintWalls(item.Item1, item.Item2 - 1);
                        }
                        else if (maze[item.Item1, item.Item2 + 1] == false)
                        {
                            maze[item.Item1, item.Item2 + 1] = true;
                            maze[item.Item1, item.Item2] = true;

                            paintWalls(item.Item1, item.Item2 + 1);
                        }
                    }
                }
                catch { }
            }

            return maze;

        }
    }
}
