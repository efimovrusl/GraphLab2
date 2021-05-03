using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLab2
{
    static class Task
    {
        public struct Interval
        {
            public float a, b;

            public Interval(float a, float b)
            {
                this.a = a;
                this.b = b;
            }
        }

        public static Interval interval = new Interval(0, 1);
        public static float Function(float x)
        {
            return (float)Math.Exp(x) - 10 * x;
        }
        private static float TESTder1(float x)
        { // isn't used in calculations
            return (float)Math.Exp(x) - 10;
        }
        private static float TESTder2(float x)
        { // isn't used in calculations
            return (float)Math.Exp(x);
        }
        private static float der(Func<float, float> func, float x, float eps, int power = 1)
        {
            if (power > 1)
                return (der(func, x + eps, eps, power - 1) -
                    der(func, x - eps, eps, power - 1)) / (eps * 2);
            else
                return (func(x + eps) - func(x - eps)) / (eps * 2);
        }
        public static List<float> BisectionMethod(Func<float, float> func, Interval ab, float error)
        { // метод половинного деления
            if (func(ab.a) * func(ab.b) >= 0)
                throw new Exception("Potentially no roots!");
            List<float> results = new List<float>();
            float c = ab.a;
            while ((ab.b - ab.a >= error))
            {
                c = (ab.a + ab.b) / 2;
                if (func(c) == 0)
                    break;
                if (func(c) * func(ab.a) < 0)
                    ab.b = c;
                else
                    ab.a = c;
                results.Add(c);
            }
            return results;
        }
        public static List<float> ChordMethod(Func<float, float> func, Interval ab, float error)
        { // метод хорд
            List<float> results = new List<float>();
            while (Math.Abs(ab.b - ab.a) > error)
            {
                ab.a = ab.b - (ab.b - ab.a) *
                    func(ab.b) / (func(ab.b) - func(ab.a));
                ab.b = ab.a - (ab.a - ab.b) *
                    func(ab.a) / (func(ab.a) - func(ab.b));
                results.Add(Math.Abs(ab.a + ab.b) / 2);
            }
            return results;
        }
        public static List<float> NewtonMethod(Func<float, float> func, Interval ab, float error)
        { // метод ньютона (касательных)
            List<float> results = new List<float>();
            ab.b = ab.a - func(ab.a) / der(func, ab.a, error);
            while (Math.Abs(ab.b - ab.a) / 2 > error)
            {
                ab.a = ab.b;
                ab.b = ab.a - func(ab.a) / der(func, ab.a, error);
                results.Add(ab.b);
            }
            return results;
        }
        public static List<float> CombinedMethod(Func<float, float> func, Interval ab, float error)
        { // комбинированный метод хорд и касательных (Ньютона)
            List<float> results = new List<float>();
            do
            {
                if (func(ab.a) * der(func, ab.a, error, 2) < 0)
                    ab.a += (ab.b - ab.a) / (func(ab.a) - func(ab.b)) * func(ab.a);
                else
                    ab.a -= func(ab.a) / der(func, ab.a, error, 1);
                if (func(ab.b) * der(func, ab.b, error, 2) < 0)
                    ab.b += (ab.a - ab.b) / (func(ab.b) - func(ab.a)) * func(ab.b);
                else
                    ab.b -= func(ab.b) / der(func, ab.b, error, 1);
                results.Add(Math.Abs(ab.a + ab.b) / 2);
            } while (Math.Abs(ab.a - ab.b) > 2 * error);
            return results;
        }
    }
}
