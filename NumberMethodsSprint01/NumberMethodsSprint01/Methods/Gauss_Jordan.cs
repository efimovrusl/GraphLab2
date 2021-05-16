using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberMethodsSprint01.Methods
{
    class Gauss_Jordan
    {
        /// <summary>
        /// Метод Гаусса-Жордана (Обратная матрица)
        /// </summary>
        /// <param name="Matrix">Начальная матрица</param>
        /// <returns></returns>
        public static bool Solve(float[,] M, ref float[] roots)
        {
            if (Gauss.Solve(M, ref roots))
            {
                int n = M.GetLength(0);
                for (int i = n - 1; i > 0; i--)
                {
                    M[i - 1, n] -= M[i - 1, i] * M[i, n];
                    M[i - 1, i] = 0;
                }
                return true;
            }
            return false;
        }
    }
}
