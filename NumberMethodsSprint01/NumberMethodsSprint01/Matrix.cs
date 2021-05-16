using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberMethodsSprint01
{
    public class Matrix : ICloneable
    {
        private float[,] matrix;

        public float this[int i, int j]
        {
            get => matrix[i, j];
            set => matrix[i, j] = value;
        }

        public Matrix(float[,] matrix)
        {
            this.matrix = new float[matrix.GetLength(0), matrix.GetLength(1)];
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                    this.matrix[i, j] = matrix[i, j];
        }

        public float Determinant
        {
            get {
                if (Height != Width) 
                    throw new Exception("Non-square matrices do not have determinants");
                if (Height == 1) return matrix[0, 0];
                if (Height == 2) return
                        matrix[0, 0] * matrix[1, 1] -
                        matrix[1, 0] * matrix[0, 1];
                float res = 0;
                for (int i = 0; i < Height; i++)
                {
                    float temp1 = 1;
                    float temp2 = -1;
                    for (int k = 0; k < Width; k++)
                    {
                        temp1 *= matrix[(i + k) % Height, k % Width];
                        temp2 *= matrix[(i + k) % Height, (Width - k) % Width];
                    }
                    res += temp1 + temp2;
                }
                return res;
            }
        }

        public int GetLength(int i) => matrix.GetLength(i);

        public int Height { get => matrix.GetLength(0); }

        public int Width { get => matrix.GetLength(1); }

        public float[,] AsArray()
        {
            float[,] arr = new float[Height, Width];
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                    arr[i, j] = matrix[i, j];
            return arr;
        }


        public void ForEach(Func<float, float> deleg)
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    matrix[i, j] = deleg(matrix[i, j]);
                }
            }
        }

        public object Clone() { return new Matrix(matrix); }

    }


}
