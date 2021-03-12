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
        Graph graph;

        public Form()
        {
            InitializeComponent();
            tableGraphics = tablePanel1.CreateGraphics();
            mainG = panel2.CreateGraphics();
            graph = new Graph(mainG, panel2);
            graph.DrawBorders();
        }
        private void universalButtonClick(List<float> xValues)
        {
            fillTable(xValues, Task.Function);
            List<PointF> points = new List<PointF>();
            for (int i = 0; i < xValues.Count(); i++)
                points.Add(new PointF(xValues.ElementAt(i), Task.Function(xValues.ElementAt(i))));
            graph.Refresh();
            graph.FocusOnPoints(points);
            graph.DrawPoints(points, true, Color.Black, Color.LightGreen);
            graph.DrawBorders();
        }
        private void button1_Click(object sender, EventArgs e)
        { // bisection method
            universalButtonClick(Task.BisectionMethod(Task.Function, Task.interval, 0.00001f));
        }

        private void button2_Click(object sender, EventArgs e)
        { // chord method
            universalButtonClick(Task.ChordMethod(Task.Function, Task.interval, 0.00001f));
        }

        private void button3_Click(object sender, EventArgs e)
        { // newton method
            universalButtonClick(Task.NewtonMethod(Task.Function, Task.interval, 0.00001f));
        }

        private void button4_Click(object sender, EventArgs e)
        { // chord + newton method
            universalButtonClick(Task.CombinedMethod(Task.Function, Task.interval, 0.00001f));
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
    }

    class Graph
    {
        Font fontArial8 = new Font("Arial", 8);
        SolidBrush brushBlack = new SolidBrush(Color.Black);
        SolidBrush brushRed = new SolidBrush(Color.Red);
        const float margin = 15;
        Graphics g;
        Panel canvas;
        float xMinFocus, yMinFocus, xMaxFocus, yMaxFocus;
        float widthOfFocus, heightOfFocus;
        public Graph(Graphics g, Panel canvas)
        {
            this.g = g;
            this.canvas = canvas;
            widthOfFocus = this.canvas.Width - margin * 2;
            heightOfFocus = this.canvas.Height - margin * 2;
            FocusOnPoints(null);
        }
        public void DrawBorders()
        {
            g.DrawLines(new Pen(Color.Black, 2), new PointF[] {
                new PointF(margin, margin),
                new PointF(margin, margin + heightOfFocus),
                new PointF(margin + widthOfFocus, margin + heightOfFocus),
                new PointF(margin + widthOfFocus, margin),
                new PointF(margin, margin),
            });
            float perfectStep(float range)
            {
                float curr = 1, prev = 1;
                do
                {
                    prev = curr;
                    if (curr / range > 1)
                        curr /= 10;
                    else
                        curr *= 10;
                } while (Math.Abs(range / 10 - curr) < Math.Abs(range / 10 - prev));
                prev /= 10;
                float perfect = prev;
                while (range / 7 > perfect)
                {
                    perfect += prev;
                }
                return perfect;
            }
            float xStep = perfectStep(xMaxFocus - xMinFocus);
            float yStep = perfectStep(yMaxFocus - yMinFocus);
            for (int i = 1; xStep * i + xMinFocus <= xMaxFocus; i++)
            {
                g.DrawLine(new Pen(Color.Black, 2),
                    TransX(xStep * i + xMinFocus), margin,
                    TransX(xStep * i + xMinFocus), margin / 2);
                g.DrawString($"{Math.Round(xStep * i + xMinFocus, 6)}", fontArial8, brushBlack, 
                    TransX(xStep * i + xMinFocus) + 2, 0);
            }
            Console.WriteLine(xStep);
            for (int i = 1; yStep * i + yMinFocus <= yMaxFocus; i++)
            {
                g.DrawLine(new Pen(Color.Black, 2),
                    margin, TransY(yStep * i + yMinFocus),
                    margin / 2, TransY(yStep * i + yMinFocus));
                g.DrawString($"{Math.Round(yStep * i + yMinFocus, 6)}", fontArial8, brushBlack,
                    margin + 2, TransY(yStep * i + yMinFocus) + 2);
            }
        }
        public void Refresh() { canvas.Refresh(); }
        public void DrawPoints(List<PointF> points, bool connect = false, 
            Color lineColor = default, Color lastPointColor = default)
        {
            if (lineColor == default) _ = Color.Black;
            void DrawPoint(PointF point, Color _pointColor = default)
            {
                Brush tempBrush = new SolidBrush(_pointColor);
                float rectWidth = 5, rectHeight = 6;
                g.FillEllipse(brushBlack, TransX(point.X) - (rectWidth + 2) / 2, 
                    TransY(point.Y) - (rectHeight + 2) / 2, (rectWidth + 2), (rectHeight + 2));
                g.FillEllipse(_pointColor == default ? brushRed : tempBrush, TransX(point.X) - rectWidth / 2,
                    TransY(point.Y) - rectHeight / 2, rectWidth, rectHeight);
            }
            if (connect)
            {
                List<PointF> transformedPoints = new List<PointF>();
                foreach (PointF p in points) transformedPoints.Add(Trans(p));
                g.DrawCurve(new Pen(lineColor, 2), transformedPoints.ToArray());
            }
            foreach (PointF p in points) DrawPoint(p);
            if (lastPointColor != default) DrawPoint(points.Last(), lastPointColor);

        }
        private PointF Trans(PointF point)
        {
            return new PointF(TransX(point.X), TransY(point.Y));
        }
        private float TransX(float x)
        {
            return (x - xMinFocus) / (xMaxFocus - xMinFocus) * widthOfFocus + margin;
        }
        private float TransY(float y)
        {
            return (1 - (y - yMinFocus) / (yMaxFocus - yMinFocus)) * heightOfFocus + margin;
        }
        public void FocusOnPoints(List<PointF> points)
        {
            if (points == null || points.Count() == 0)
            {
                xMinFocus = -2;
                yMinFocus = -2;
                xMaxFocus = 2;
                yMaxFocus = 2;
                return;
            }
            xMinFocus = points.First().X;
            yMinFocus = points.First().Y;
            xMaxFocus = points.First().X;
            yMaxFocus = points.First().Y;
            foreach (PointF p in points)
            {
                if (p.X > xMaxFocus) xMaxFocus = p.X;
                if (p.X < xMinFocus) xMinFocus = p.X;
                if (p.Y > yMaxFocus) yMaxFocus = p.Y;
                if (p.X < yMinFocus) yMinFocus = p.Y;
            }
            float xMargin = (xMaxFocus - xMinFocus) / 10;
            float yMargin = (yMaxFocus - yMinFocus) / 10;
            xMaxFocus += xMargin;
            xMinFocus -= xMargin;
            yMaxFocus += yMargin;
            yMinFocus -= yMargin;
            if (xMaxFocus - xMinFocus == 0)
            {
                xMinFocus--;
                xMaxFocus++;
            }
            if (yMaxFocus - yMinFocus == 0)
            {
                yMinFocus--;
                yMaxFocus++;
            }

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
