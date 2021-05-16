using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberMethodsSprint01.Methods
{
    class Jacobi
    {
        public static bool Solve(Matrix inputMatrix, float[] expectedOutcome, out float[] roots)
        {
            roots = new float[inputMatrix.Height];
            // cheching for roots' existance
            float[,] m = new float[inputMatrix.Height, inputMatrix.Width + 1];
            for (int i = 0; i < inputMatrix.Height; i++)
            {
                m[i, inputMatrix.Width] = expectedOutcome[i];
                for (int j = 0; j < inputMatrix.Width; j++)
                    m[i, j] = inputMatrix[i, j];
            }
            if (Check(m, ref roots)) return true;
            const int iterations = 1000;
            float[] solved = new float[inputMatrix.Height];
            for (int p = 0; p < iterations; p++)
            {
                for (int i = 0; i < inputMatrix.Height; i++)
                {
                    float sigma = 0;
                    for (int j = 0; j < inputMatrix.Width; j++)
                    {
                        if (j != i) sigma += inputMatrix[i, j] * solved[j];
                    }
                    solved[i] = (expectedOutcome[i] - sigma) / inputMatrix[i, i];
                }
            }
            for (int i = 0; i < inputMatrix.Height; i++) roots[i] = solved[i];
            return true;
        }


        private static bool Check(float[,] M, ref float[] roots)
        {
            int n = M.GetLength(0);
            for (int i = 0; i < n; i++) roots[i] = float.NaN;
            float[,] m = new float[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    m[i, j] = M[i, j];
            Matrix matrix = new Matrix(m);
            if (matrix.Determinant == 0) return false;
            for (int i = 0; i < M.GetLength(0); i++)
            {
                Matrix temp = (Matrix)matrix.Clone();
                for (int j = 0; j < M.GetLength(0); j++)
                    temp[j, i] = M[j, n];
                roots[i] = temp.Determinant / matrix.Determinant;
            }
            return true;
        }
    }
}
