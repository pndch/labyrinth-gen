using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms.VisualStyles;
using static System.Net.Mime.MediaTypeNames;

namespace labyrinth_gen
{
    public partial class Form1 : Form
    {
        public static int size = 75; // всегда нечетный
        static int seed = 1;
        static bool pFlag = true;
        static bool[,] maze;
        public static int cellSize = 450 / size;
        public static (int, int) startPos = (1, 1);
        public static (int, int) endPos = (1, 1);
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            maze = Generator.Create(size - 2, seed);
            startPos = (0, 0);
            endPos = (0, 0);

            Graphics g = pictureBox1.CreateGraphics();
            g.Clear(Color.White);

            //Pen pen = new Pen(Color.Black, 1);
            SolidBrush wallB = new SolidBrush(Color.Black);
            SolidBrush passB = new SolidBrush(Color.White);

            int isize = size - 2; //bcs of borders 
            

            g.FillRectangle(wallB, 0, 0, 450, cellSize);
            g.FillRectangle(wallB, 0, 0, cellSize, 450);
            g.FillRectangle(wallB, 450 - cellSize, 0, cellSize, 450);
            g.FillRectangle(wallB, 0, 450 - cellSize, 450, cellSize);

            for (int i = 0; i < isize; i++)
            {
                for (int j = 0; j < isize; j++)
                {
                    if (maze[i, j] == true) 
                    { 
                        g.FillRectangle(passB, (i + 1) * cellSize, (j + 1) * cellSize, cellSize, cellSize);
                    }
                    else
                    {
                        g.FillRectangle(wallB, (i + 1) * cellSize, (j + 1) * cellSize, cellSize, cellSize);
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e) //solve
        {
            (int, int, int)[,] aStarMaze = new (int, int, int)[size - 2, size - 2];
            HashSet<(int, int)> openSet = new HashSet<(int, int)>();
            HashSet<(int, int)> closedSet = new HashSet<(int, int)>();

            int G, H, F, tmpF; //G - price of real route, H - evristic price, F = G + H
            (int, int) tmpXY = startPos;
            aStarMaze[tmpXY.Item1, tmpXY.Item2] = (tmpXY.Item1, tmpXY.Item2, 0);
            

            void restorePath()
            {
                Debug.WriteLine("Succes");
                Graphics g = pictureBox1.CreateGraphics();
                SolidBrush truePath = new SolidBrush(Color.Yellow);
                (int, int) next = tmpXY;

                for (int i = 0; i < aStarMaze[tmpXY.Item1, tmpXY.Item2].Item3; i++)
                {
                    if (next != startPos && next != endPos)
                    {
                        Thread.Sleep(25);
                        g.FillRectangle(truePath, (next.Item1 + 1) * cellSize, (next.Item2 + 1) * cellSize, cellSize, cellSize);
                    }
                    next = (aStarMaze[next.Item1, next.Item2].Item1, aStarMaze[next.Item1, next.Item2].Item2);
                }
            }

            openSet.Add(startPos);
            
            Graphics g = pictureBox1.CreateGraphics();
            SolidBrush pathTemp = new SolidBrush(Color.LightYellow);



            while (openSet.Count > 0)
            {
                if (tmpXY != startPos && tmpXY != endPos)
                {
                    Thread.Sleep(3);
                    g.FillRectangle(pathTemp, (tmpXY.Item1 + 1) * cellSize, (tmpXY.Item2 + 1) * cellSize, cellSize, cellSize);
                }
                //Debug.WriteLine(tmpXY);

                tmpF = 99999;
                foreach ((int,int) xy in openSet)
                {
                    if (tmpF > aStarMaze[xy.Item1, xy.Item2].Item3 + Math.Sqrt(Math.Pow((endPos.Item1 - xy.Item1),2) + Math.Pow((endPos.Item2 - xy.Item2),2)))
                    {
                        tmpF = aStarMaze[xy.Item1, xy.Item2].Item3 + (int)Math.Sqrt(Math.Pow((endPos.Item1 - xy.Item1), 2) + Math.Pow((endPos.Item2 - xy.Item2), 2));
                        tmpXY = xy;
                    }
                } 

                if (tmpXY == endPos)
                {
                    restorePath();
                    break;
                }


                openSet.Remove(tmpXY);
                closedSet.Add(tmpXY);

                //добавление соседей в список
                try
                {
                    if (!closedSet.Contains((tmpXY.Item1 + 1, tmpXY.Item2)) && maze[tmpXY.Item1 + 1, tmpXY.Item2] == true)
                    {
                        aStarMaze[tmpXY.Item1 + 1, tmpXY.Item2] = (tmpXY.Item1, tmpXY.Item2, 1 + aStarMaze[tmpXY.Item1, tmpXY.Item2].Item3);
                        openSet.Add((tmpXY.Item1 + 1, tmpXY.Item2));
                    }
                } catch { }
                try
                {
                    if (!closedSet.Contains((tmpXY.Item1 - 1, tmpXY.Item2)) && maze[tmpXY.Item1 - 1, tmpXY.Item2] == true)
                    {
                        aStarMaze[tmpXY.Item1 - 1, tmpXY.Item2] = (tmpXY.Item1, tmpXY.Item2, 1 + aStarMaze[tmpXY.Item1, tmpXY.Item2].Item3);
                        openSet.Add((tmpXY.Item1 - 1, tmpXY.Item2));
                    }
                } catch { }
                try
                {
                    if (!closedSet.Contains((tmpXY.Item1, tmpXY.Item2 + 1)) && maze[tmpXY.Item1, tmpXY.Item2 + 1] == true)
                    {
                        aStarMaze[tmpXY.Item1, tmpXY.Item2 + 1] = (tmpXY.Item1, tmpXY.Item2, 1 + aStarMaze[tmpXY.Item1, tmpXY.Item2].Item3);
                        openSet.Add((tmpXY.Item1, tmpXY.Item2 + 1));
                    }
                } catch { }
                try
                {
                    if (!closedSet.Contains((tmpXY.Item1, tmpXY.Item2 - 1)) && maze[tmpXY.Item1, tmpXY.Item2 - 1] == true)
                    {
                        aStarMaze[tmpXY.Item1, tmpXY.Item2 - 1] = (tmpXY.Item1, tmpXY.Item2, 1 + aStarMaze[tmpXY.Item1, tmpXY.Item2].Item3);
                        openSet.Add((tmpXY.Item1, tmpXY.Item2 - 1));
                    }
                } catch { }
            }
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            Graphics g = pictureBox1.CreateGraphics();

            SolidBrush brsh = new SolidBrush(Color.Green);
            SolidBrush wht = new SolidBrush(Color.White);
            (int, int) pos;
            pos.Item1 = (PointToClient(Cursor.Position).X - 12) / cellSize -1; //PointToClient переводит с позиции на мониторе на позициб на фрейме
            pos.Item2 = (PointToClient(Cursor.Position).Y - 12) / cellSize -1;
            
            try
            {
                if (maze[pos.Item1, pos.Item2] != false)
                {
                    if (pFlag == true)
                    {
                        pFlag = false;
                        g.FillRectangle(wht, (startPos.Item1+1) * cellSize, (startPos.Item2+1) * cellSize, cellSize, cellSize);
                        startPos.Item1 = pos.Item1; 
                        startPos.Item2 = pos.Item2;
                        g.FillRectangle(brsh, (startPos.Item1 + 1) * cellSize, (startPos.Item2+1) * cellSize, cellSize, cellSize);
                        Debug.WriteLine("Start: " + startPos);
                    }
                    else
                    {
                        brsh = new SolidBrush(Color.Red);
                        pFlag = true;
                        g.FillRectangle(wht, (endPos.Item1+1) * cellSize, (endPos.Item2+1) * cellSize, cellSize, cellSize);
                        endPos.Item1 = pos.Item1; 
                        endPos.Item2 = pos.Item2;
                        g.FillRectangle(brsh, (endPos.Item1+1) * cellSize, (endPos.Item2+1) * cellSize, cellSize, cellSize);
                        Debug.WriteLine("End: " + endPos);
                    }
                }
            }
            catch { }
        }
    }
}
