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

        public static float BisectionMethod(Func<float, float> func, Interval ab, float error)
        { // метод половинного деления
            if (func(ab.a) * func(ab.b) >= 0)
                throw new Exception("Potentially no roots!");
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
            }
            return c;
        }
        public static float ChordMethod(Func<float, float> func, Interval ab, float error)
        { // метод хорд
            while (Math.Abs(ab.b - ab.a) > error)
            {
                ab.a = ab.b - (ab.b - ab.a) *
                    func(ab.b) / (func(ab.b) - func(ab.a));
                ab.b = ab.a - (ab.a - ab.b) *
                    func(ab.a) / (func(ab.a) - func(ab.b));
            }
            return ab.b;
        }
        public static float NewtonMethod(Func<float, float> func, Interval ab, float error)
        {
            float der(Func<float, float> _func, float _x)
            {
                return (_func(_x + error) - _func(_x - error)) / (error * 2);
            }
            float x0, x1 = ab.a;
            do
            {
                x0 = x1;
                x1 = x0 - func(x0) / der(func, x0);
            } while (Math.Abs(x1 - x0) > error);
            return x1;
        }


    }
}
