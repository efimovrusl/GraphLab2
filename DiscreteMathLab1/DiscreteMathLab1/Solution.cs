using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// DEBUG WRITELINE
using System.Diagnostics;

namespace DiscreteMathLab1
{
    public static class Solution
    {
        private static char[] 
            input1 = Array.Empty<char>(), 
            input2 = Array.Empty<char>();
        public static char[] Input1 { 
            get => input1; 
            set
            {
                input1 = value;
                Array.Sort(input1);
            }
        }
        public static char[] Input2 { 
            get => input2; 
            set
            {
                input2 = value;
                Array.Sort(input2);
            }
        }
        public static String Task1()
        { // вхождение
            bool result = true;
            foreach (Char c in Input1)
                if (!Input2.Contains(c)) result = false;
            return $"{result}";
        }
        public static String Task2()
        { // объединение
            String output = "";
            foreach (Char c in Input1) if (!output.Contains(c)) output += c;
            foreach (Char c in Input2) if (!output.Contains(c)) output += c;
            Char[] buffer = output.ToArray();
            Array.Sort(buffer);
            return new String(buffer);
        }
        public static String Task3()
        { // пересечение
            String output = "";
            char[] buffer = new char[Input2.Length];
            Array.Copy(Input2, buffer, Input2.Length);
            for (int i = 0; i < Input1.Length; i++)
                for (int j = 0; j < Input2.Length; j++)
                    if (buffer[j] == Input1[i])
                    {
                        buffer[j] = '#';
                        output += Input1[i];
                        break;
                    }
            return output;
        }
        public static String Task4()
        { // разность
            String output = "";
            foreach (Char c in Input1)
                if (!Input2.Contains(c)) output += c;
            return output;
        }
        public static String Task5()
        { // симметричная разность (убираем общие элементы)
            String output = "";
            String intersection = Task3();
            foreach (Char c in Input1.Concat(Input2)) 
                if (!output.Contains(c) && 
                    !intersection.Contains(c)) output += c;
            Char[] buffer = output.ToArray();
            Array.Sort(buffer);
            return new String(buffer);
        }
    }
}
