using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphLab2
{
    public partial class Form : System.Windows.Forms.Form
    {
        public Form()
        {
            InitializeComponent();

            float shit;
            
            shit = Task.BisectionMethod(Task.Function, Task.interval, 0.0001f);
            Console.WriteLine(shit + ", " + Task.Function(shit));


            shit = Task.ChordMethod(Task.Function, Task.interval, 0.0001f);
            Console.WriteLine(shit + ", " + Task.Function(shit));

            shit = Task.NewtonMethod(Task.Function, Task.interval, 0.0001f);
            Console.WriteLine(shit + ", " + Task.Function(shit));


        }
    }




    class Table
    {
        Graphics g;
        Font fontArial = new Font("Arial", 7);
        SolidBrush brushBlack = new SolidBrush(Color.Black);
        Pen grayPen = new Pen(Color.DarkGray);
        Pen blackPen = new Pen(Color.Black);
        int rows, columns, height, width;
        public String[,] values;
        public Table(Graphics g, int rows, int columns, int height, int width)
        {
            this.g = g;
            this.rows = rows;
            this.columns = columns;
            this.height = height;
            this.width = width;
            values = new String[rows, columns];
        }
        public void drawTable()
        {
            drawBorders();
            fillText();
        }
        private void fillText()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    g.DrawString(values[i, j], fontArial, brushBlack,
                        (float)width / columns * j, (float)height / rows * i + 7);
                }
            }
        }
        private void drawBorders()
        {
            for (int i = 1; i < rows; i++)
            {
                g.DrawLine(grayPen,
                    new PointF(0, (float)height / rows * i),
                    new PointF(width, (float)height / rows * i));
            }
            for (int i = 1; i < columns; i++)
            {
                g.DrawLine(grayPen,
                    new PointF((float)width / columns * i, 0),
                    new PointF((float)width / columns * i, height));
            }
        }

    }

}
