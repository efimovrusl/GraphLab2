using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using System.Numerics;

namespace NumberMethodsSprint01
{
    public partial class Form1 : Form
    {
        Solution solution;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            trackBar1_Scroll(sender, e);
        }

        private void update()
        {
            ResizeTable();
            UpdateLabels();
        }

        private void scroll_update()
        {
            dataGridView1.ColumnCount = trackBar1.Value;
            dataGridView1.RowCount = trackBar1.Value;
            dataGridView2.RowCount = trackBar1.Value;
            dataGridView3.RowCount = trackBar1.Value;
            update();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            scroll_update();
        }



        private void ResizeTable()
        {
            int margin = 5;
            int fontSize;
            if (dataGridView1.Rows.Count < 10) fontSize = 14;
            else if (dataGridView1.Rows.Count < 12) fontSize = 11;
            else if (dataGridView1.Rows.Count < 14) fontSize = 9;
            else fontSize = 7;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView2.Rows[i].Cells[0].Style.Alignment =
                        DataGridViewContentAlignment.MiddleCenter;
                dataGridView2.Rows[i].Cells[0].Style.Font =
                    new Font("Verdana", fontSize);
                dataGridView3.Rows[i].Cells[0].Style.Alignment =
                        DataGridViewContentAlignment.MiddleCenter;
                dataGridView3.Rows[i].Cells[0].Style.Font =
                    new Font("Verdana", fontSize);
                for (int j = 0; j < dataGridView1.Columns.Count; j++)
                {
                    dataGridView1.Rows[j].Cells[i].Style.Alignment =
                        DataGridViewContentAlignment.MiddleCenter;
                    dataGridView1.Rows[j].Cells[i].Style.Font =
                        new Font("Verdana", fontSize);
                }
            }
            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                dataGridView1.Columns[i].Width = ((dataGridView1.Width - margin) / 
                    dataGridView1.Columns.Count) + ((i % 2 == 0) ? 1 : 0);
            }
            dataGridView2.Columns[0].Width = dataGridView2.Width;
            dataGridView3.Columns[0].Width = dataGridView3.Width;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Height = ((dataGridView1.Height - margin) /
                    dataGridView1.Rows.Count) + ((i % 2 == 0) ? 1 : 0);
                dataGridView2.Rows[i].Height = ((dataGridView2.Height - margin) /
                    dataGridView2.Rows.Count) + ((i % 2 == 0) ? 1 : 0);
                dataGridView3.Rows[i].Height = ((dataGridView3.Height - margin) /
                    dataGridView3.Rows.Count) + ((i % 2 == 0) ? 1 : 0);
            }
        }

        private void UpdateLabels()
        {
            label1.Text = $"SIZE: {dataGridView1.Rows.Count}";
        }

        private bool IsMatrixValid()
        {
            bool Error()
            {
                MessageBox.Show("INVALID INPUT!", "Sorry, you've born in Ukraine, son...");
                return false;
            }
            string value;
            float[,] matrix = new float[dataGridView1.RowCount, dataGridView1.ColumnCount];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    try
                    {
                        value = $"{dataGridView1.Rows[i].Cells[j].Value}" ?? "";

                        if (value.Trim().Length == 0)
                        {
                            dataGridView1.Rows[i].Cells[j].Value = "0";
                            j--; continue;
                        }
                        if (!float.TryParse(value, out matrix[i, j])) return Error();
                    } 
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }

                }
            }
            float[] free_terms = new float[dataGridView2.RowCount];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                value = $"{dataGridView2.Rows[i].Cells[0].Value}" ?? "";
                if (value.Trim().Length == 0)
                {
                    dataGridView2.Rows[i].Cells[0].Value = "0";
                    i--; continue;
                }
                if (!float.TryParse(value, out free_terms[i])) return Error();
            }
            float[] roots = new float[dataGridView3.RowCount];
            for (int i = 0; i < matrix.GetLength(0); i++)
            { 
                value = $"{dataGridView3.Rows[i].Cells[0].Value}" ?? "";
                if (value.Trim().Length == 0)
                {
                    dataGridView3.Rows[i].Cells[0].Value = "0";
                    i--; continue;
                }
                if (!float.TryParse(value, out roots[i])) return Error();
            }
            solution = new Solution(matrix, free_terms);
            solution.roots = roots;
            return true;
        }

        private void SetRoots(float[] roots)
        {
            for (int i = 0; i < roots.Length; i++)
                dataGridView3.Rows[i].Cells[0].Value = roots[i];
        }

        private void button1_Click(object sender, EventArgs e)
        { // метод Крамера
            if (!IsMatrixValid()) return;
            SetRoots(solution.Cramer());
        }

        private void button2_Click(object sender, EventArgs e)
        { // метод Гаусса
            if (!IsMatrixValid()) return;
            float[,] full_matrix;
            SetRoots(solution.Gauss(out full_matrix));
            for (int i = 0; i < full_matrix.GetLength(0); i++)
            {
                for (int j = 0; j < full_matrix.GetLength(0); j++)
                    dataGridView1.Rows[i].Cells[j].Value = full_matrix[i, j];
                dataGridView2.Rows[i].Cells[0].Value =
                    full_matrix[i, full_matrix.GetLength(0)];
            }
        }

        private void button3_Click(object sender, EventArgs e)
        { // метод Зейделя
            if (!IsMatrixValid()) return; 
            SetRoots(solution.Seidel());
        }

        private void button4_Click(object sender, EventArgs e)
        { // метод Гаусса-Жордана
            if (!IsMatrixValid()) return;
            float[,] full_matrix;
            SetRoots(solution.Gauss_Jordan(out full_matrix));
            for (int i = 0; i < full_matrix.GetLength(0); i++)
            {
                for (int j = 0; j < full_matrix.GetLength(0); j++)
                    dataGridView1.Rows[i].Cells[j].Value = full_matrix[i, j];
                dataGridView2.Rows[i].Cells[0].Value =
                    full_matrix[i, full_matrix.GetLength(0)];
            }
        }

        private void button5_Click(object sender, EventArgs e)
        { // метод Якоби
            if (!IsMatrixValid()) return;
            SetRoots(solution.Jacobi());
        }


        private bool Check_Roots(float[] roots)
        { // проверка корней
            int n = dataGridView1.RowCount;
            for (int i = 0; i < n; i++)
            {
                float sum = 0;
                for (int j = 0; j < n; j++)
                    sum += roots[j] * solution.matrix[i, j];
                if (Math.Abs(sum - solution.free_terms[i]) > 0.01) return false;
            }
            return true;
        }

        private void button6_Click(object sender, EventArgs e)
        { // проверка корней
            if (!IsMatrixValid()) return;
            string showup = "";
            if (Check_Roots(solution.roots)) showup += "Roots are correct.\n";
            else showup += "Roots are INCORRECT!\n";
            // проверка на вырожденность
            if (solution.matrix.Determinant == 0) showup += "Matrix is DEGENERATE!";
            else showup += "Matrix is non-degenerate.";
            MessageBox.Show(showup);
        }

        private void button7_Click(object sender, EventArgs e)
        { // данные из файла
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = ".";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                        MessageBox.Show(fileContent);

                        string[] words = fileContent.Split(' ', '\t', '\n', '\v', '\f', '\r');
                        var vals = new List<float>();
                        float shit;
                        foreach (var word in words)
                            if (float.TryParse(word, out shit))
                                vals.Add(shit);
                        if (vals.Count() != vals.ToArray()[0] * (vals.ToArray()[0] + 1) + 1)
                        {
                            MessageBox.Show("Incorrect file!");
                            return;
                        }
                        trackBar1.Value = (int)vals.First();
                        scroll_update();
                        int n = (int)vals.First();
                        vals.RemoveAt(0);
                        float[] arr = vals.ToArray();
                        for (int i = 0; i < n; i++)
                        {
                            for (int j = 0; j < n; j++)
                                dataGridView1.Rows[i].Cells[j].Value = 
                                    $"{arr[(n + 1) * i + j]}";
                            dataGridView2.Rows[i].Cells[0].Value =
                                $"{arr[(n + 1) * i + n]}";
                        }
                    }
                }
            }
        }
    }
}
