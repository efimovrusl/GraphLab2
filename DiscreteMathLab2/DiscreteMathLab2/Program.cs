using System;

namespace DiscreteMathLab2
{   // бессмысленно делать интерфейс - реализовал вывод в консоль
    class Program
    {
        static void Main(string[] args)
        {
            var adjacency_matrix = new int[6, 6]
            {  // x1 x2 x3 x4 x5 x6
                { 1, 1, 1, 1, 1, 1 }, // x1
                { 1, 1, 1, 1, 1, 0 }, // x2
                { 1, 1, 1, 1, 0, 0 }, // x3
                { 1, 1, 1, 0, 0, 0 }, // x4
                { 1, 1, 0, 0, 0, 0 }, // x5
                { 1, 0, 0, 0, 0, 0 }, // x6
            };
            Graph g = new Graph(ref adjacency_matrix, Graph.Type.Adjacency);
            try
            {
                g.Show();
            } 
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadLine();
            }
    }
        
    class Graph
    {
        private int[,] incidence_m;
        private int[,] adjacency_m;

        public int[,] Incidence
        {
            get => incidence_m;
        }
        public int[,] Adjacency
        {
            get => adjacency_m;
        }
        public enum Type
        {
            Adjacency,
            Incidency,
        };
        public Graph(ref int[,] matrix, Type type)
        { // non-oriented graph
            if (type == Type.Adjacency)
            {
                if (matrix?.GetLength(0) != matrix?.GetLength(1) || matrix?.GetLength(0) < 1)
                    throw new Exception("Incorrect ADJACENCY matrix!");
                
                // adjacency matrix
                adjacency_m = new int[matrix.GetLength(0), matrix.GetLength(1)];
                for (int i = 0; i < matrix.GetLength(0); i++)
                    for (int j = 0; j < matrix.GetLength(1); j++)
                        adjacency_m[i, j] = matrix[i, j];

                // incidency matrix
                int inc_width = 0;
                for (int i = 0; i < matrix.GetLength(0); i++)
                    for (int j = 0; j < matrix.GetLength(1); j++)
                        if (matrix[i, j] == 1 && (matrix[j, i] == 0 || j >= i))
                            inc_width++;
                incidence_m = new int[matrix.GetLength(0), inc_width];

                for (int i = 0, edge_index = 0; i < adjacency_m.GetLength(0); i++) 
                { 
                    for (int j = i; j < adjacency_m.GetLength(1); j++)
                    {
                        if (i == j && adjacency_m[i, j] == 1)
                        {
                            incidence_m[i, edge_index++] = 1;
                            continue;
                        }
                        if (matrix[i, j] == 1)
                        {
                            if (matrix[j, i] == 0)
                            {
                                incidence_m[i, edge_index] = 1;
                                incidence_m[j, edge_index++] = -1;
                            }
                            else if (j >= i && matrix[j, i] == 1)
                            {
                                incidence_m[i, edge_index] = 1;
                                incidence_m[j, edge_index++] = 1;
                            }
                        }
                    }
                }
            }
            else
            {
                if (matrix?.GetLength(0) < 1 || matrix?.GetLength(1) < 1)
                    throw new Exception("Incorrect INCIDENCY matrix!");
                for (int i = 0; i < 10; i++) Console.Beep();
                Console.WriteLine("Incidency matrix initialization aint implemented");
            }
        }
        public void Show()
        {
            for (int i = 0; i < adjacency_m.GetLength(0); i++)
                ShowVertex(i);
            DrawIntMatrix(Adjacency, "adjacency");
            DrawIntMatrix(Incidence, "incidence");

        }

        public void ShowVertex(int index)
        {
            if (index >= adjacency_m?.GetLength(0) || index < 0)
                throw new Exception("Incorrect vertex index!");
            int degree = 0;
            string repres = "{";
            string protot = "{";
            for (int i = 0; i < adjacency_m.GetLength(1); i++)
            {
                degree += adjacency_m[index, i];
                if (adjacency_m[index, i] == 1) repres += $"X{i}, ";
                if (adjacency_m[i, index] == 1) protot += $"X{i}, ";
            }
            if (repres.Length > 2) repres = repres.Remove(repres.Length - 2, 2);
            repres += "}";
            if (protot.Length > 2) protot = protot.Remove(protot.Length - 2, 2);
            protot += "}";

            Console.Write(
                $"\n Vertex X{index + 1}:\n" +
                $" <> Degree: {degree}\n" + // степень
                $" <> Representation: {repres}\n" + // образ
                $" <> Prototype: {protot}\n"); // прообраз
        }

        private void DrawIntMatrix(int[,] matrix, string name = "matrix")
        {
            Console.Write("\n ");
            int width = 2 + 3 * matrix.GetLength(1);
            for (int i = 0; i < Math.Floor((float)width / 2) - 1 -
                Math.Ceiling((float)(name.Length) / 2); i++)
                Console.Write(i == 0 ? "┌" : "─");
            Console.Write($" {name} ");
            double max = Math.Ceiling((float)width / 2) - 1 -
                Math.Floor((float)(name.Length) / 2);
            for (int i = 0; i < max; i++) 
                Console.Write(i + 1 == max ? "┐" : "─");
            Console.WriteLine();
            for (int i = 0; i < matrix.GetLength(0); i++) 
            {
                Console.Write(" │");
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(trimTo2(matrix[i, j]));
                    if (j < matrix.GetLength(1) - 1)
                        Console.Write(",");
                }
                Console.Write(" │\n");
            }
            string trimTo2(int num) { return num >= 0 ? $" {num}" : $"{num}"; }
        }
    }
}
