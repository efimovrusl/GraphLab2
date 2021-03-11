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
        Graphics tableGraphics, mainG;
        Table table;

        public Form()
        {
            InitializeComponent();
            tableGraphics = tablePanel1.CreateGraphics();
            mainG = panel1.CreateGraphics();

            List<float> govno = Task.CombinedMethod(Task.Function, Task.interval, 0.00001f);
            foreach (float shit in govno)
            {
                Console.WriteLine($"Govno: {shit}");
            }
        }
        private void fillTable(List<float> arr, Func<float, float> func)
        {
            tablePanel1.Refresh();
            table = new Table(tableGraphics, 2, arr.Count(), tablePanel1.Height, tablePanel1.Width);
            for (int i = 0; i < arr.Count; i++)
            {
                table.values[0, i] =
                    String.Format("{0:0.0000}", arr.ToArray()[i]);
                table.values[1, i] =
                    String.Format("{0:0.0000}", Task.Function(arr.ToArray()[i]));
            }
            table.drawTable();
        }
        private void button1_Click(object sender, EventArgs e)
        { // bisection method
            List<float> currentTableX = Task.BisectionMethod(Task.Function, Task.interval, 0.00001f);
            fillTable(currentTableX, Task.Function);
        }

        private void button2_Click(object sender, EventArgs e)
        { // chord method
            List<float> currentTableX = Task.ChordMethod(Task.Function, Task.interval, 0.00001f);
            fillTable(currentTableX, Task.Function);
        }

        private void button3_Click(object sender, EventArgs e)
        { // newton method
            List<float> currentTableX = Task.NewtonMethod(Task.Function, Task.interval, 0.00001f);
            fillTable(currentTableX, Task.Function);
        }

        private void button4_Click(object sender, EventArgs e)
        { // chord + newton method
            List<float> currentTableX = Task.CombinedMethod(Task.Function, Task.interval, 0.00001f);
            fillTable(currentTableX, Task.Function);
        }
    }

    class Graph
    {
        public Graph()
        {

        }
    }

    class Table
    {
        Graphics g;
        Font fontArial = new Font("Arial", 8);
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
