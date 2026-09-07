using System;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;

namespace labyrinth_gen
{
    public partial class Form1 : Form
    {
        public int size = 75; // всегда нечетный

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int seed = 1;

            int cellSize = 450 / size;

            Graphics g = pictureBox1.CreateGraphics();
            g.Clear(Color.White);

            //Pen pen = new Pen(Color.Black, 1);
            SolidBrush wallB = new SolidBrush(Color.Black);
            SolidBrush passB = new SolidBrush(Color.White);

            int isize = size - 2; //bcs of borders 
            var maze = Generator.Create(isize, seed);

            g.FillRectangle(wallB, 0, 0, 450, cellSize);
            g.FillRectangle(wallB, 0, 0, cellSize, 450);
            g.FillRectangle(wallB, 450-cellSize, 0, cellSize, 450);
            g.FillRectangle(wallB, 0, 450 - cellSize, 450, cellSize);

            for (int i = 0; i<isize; i++)
            {
                for (int j = 0; j < isize; j++)
                {
                    if (maze[i, j] == true)
                    {
                        g.FillRectangle(passB, (i+1) * cellSize, (j+1) * cellSize, cellSize, cellSize);
                    }
                    else
                    {
                        g.FillRectangle(wallB, (i+1) * cellSize, (j+1) * cellSize, cellSize, cellSize);
                    }
                }
            }
        }
    }
}
