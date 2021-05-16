using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NumberMethodsSprint01
{
    public class Solution
    {
        public Matrix matrix = null;
        public float[] free_terms = null;
        public float[] roots = null;

        //public float[] Roots
        //{
        //    get => 
        //}
        
        public Solution(float[,] matrix, float[] free_terms)
        {
            this.matrix = new Matrix(new float[matrix.GetLength(0), matrix.GetLength(1)]);
            this.free_terms = new float[matrix.GetLength(0)];
            this.roots = new float[matrix.GetLength(0)];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                    this.matrix[i, j] = matrix[i, j];
                this.free_terms[i] = free_terms[i];
                this.roots[i] = 0;
            }
        }

        public float[] Cramer()
        {
            cleanRoots();
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                Matrix temp = (Matrix)matrix.Clone();
                for (int j = 0; j < matrix.GetLength(1); j++)
                    temp[j, i] = free_terms[j];
                roots[i] = temp.Determinant / matrix.Determinant;
            }
            return roots;
        }

        public float[] Gauss(out float[,] m)
        {
            cleanRoots();
            m = new float[matrix.Height, matrix.Width + 1];
            for (int i = 0; i < matrix.Height; i++)
            {
                m[i, matrix.Width] = free_terms[i];
                for (int j = 0; j < matrix.Width; j++)
                    m[i, j] = matrix[i, j];
            }
            if (!Methods.GaussJordanElimination.Solve(m, ref roots))
            {
                MessageBox.Show("Gauss can't solve!");
                float[] r = new float[matrix.Height];
                for (int i = 0; i < matrix.Height; i++) r[i] = float.NaN;
                return r;
            }
            return roots;
        }

        public float[] Seidel()
        {
            cleanRoots();
            if (!Methods.Seidel.Solve(matrix.AsArray(), free_terms, ref roots))
            {
                MessageBox.Show("Seidel can't solve!");
                float[] r = new float[matrix.Height];
                for (int i = 0; i < matrix.Height; i++) r[i] = float.NaN;
                return r;
            }
            return roots;
        }

        public float[] Gauss_Jordan(out float[,] m)
        {
            cleanRoots();
            m = new float[matrix.Height, matrix.Width + 1];
            for (int i = 0; i < matrix.Height; i++)
            {
                m[i, matrix.Width] = free_terms[i];
                for (int j = 0; j < matrix.Width; j++)
                    m[i, j] = matrix[i, j];
            }
            if (!Methods.Gauss_Jordan.Solve(m, ref roots))
            {
                MessageBox.Show("Gauss can't solve!");
                float[] r = new float[matrix.Height];
                for (int i = 0; i < matrix.Height; i++) r[i] = float.NaN;
                return r;
            }
            return roots;
        }

        public float[] Jacobi()
        {
            cleanRoots();
            if (!Methods.Jacobi.Solve(matrix, free_terms, out roots))
            {
                MessageBox.Show("Jacobi can't solve!");
                float[] r = new float[matrix.Height];
                for (int i = 0; i < matrix.Height; i++) r[i] = float.NaN;
                return r;
            }
            return roots;
        }

        private void cleanRoots() { for (int i = 0; i < roots.Length; i++) roots[i] = float.NaN; }
    }





}
