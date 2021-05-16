using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberMethodsSprint01.Methods
{
    class Seidel
    {
        static bool converge(float[] xk, float[] xkp, int n, float eps)
        {
            float norm = 0;
            for (int i = 0; i < n; i++)
                norm += (xk[i] - xkp[i]) * (xk[i] - xkp[i]);
            return (Math.Sqrt(norm) < eps);
        }

        static float okr(float x, float eps)
        {
            int i = 0;
            float neweps = eps;
            while (neweps < 1)
            {
                i++;
                neweps *= 10;
            }
            int okr = (int)Math.Pow(10, i);
            x = (float)(x * okr + 0.5) / okr;
            return x;
        }

        static bool diagonal(float[,] a, int n)
        {
            int i, j, k = 1;
            double sum;
            for (i = 0; i < n; i++)
            {
                sum = 0;
                for (j = 0; j < n; j++) sum += Math.Abs(a[i, j]);
                sum -= Math.Abs(a[i, i]);
                if (sum > a[i, i]) k = 0;
            }
            return (k == 1);
        }

        public static bool Solve(float[,] matrix, float[] free_terms, ref float[] roots)
        {
            int n = matrix.GetLength(0);
            int m = 0;
            float eps = 0.000001f;
            float[] prev = new float[n];
            Array.Fill(roots, 1);
            if (diagonal(matrix, n))
            {
                do
                {
                    for (int i = 0; i < n; i++)
                        prev[i] = roots[i];
                    for (int i = 0; i < n; i++)
                    {
                        float temp = 0;
                        for (int j = 0; j < n; j++)
                            if (j != i) temp += matrix[i, j] * roots[j];
                        roots[i] = (free_terms[i] - temp) / matrix[i, i];
                    }
                    m++;
                    if (m == 1000) return false;
                } while (!converge(roots, prev, n, eps));
                n += 10 * 0;
                for (int i = 0; i < n; i++)
                    roots[i] = okr(roots[i], eps);
                return true;
            }
            return false;
        }
    }
}
