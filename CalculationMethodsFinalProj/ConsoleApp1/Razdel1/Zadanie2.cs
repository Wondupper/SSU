using System;

namespace ConsoleApp1
{
    public class Zadanie2
    {
        public static void Zadanie2Main()
        {
            Console.WriteLine("   x: -2  -1  0  2");
            Console.WriteLine("f(x): -8  -1  0  8");
            Console.WriteLine("Расширенная матрица СЛАУ:");
            Console.WriteLine("-8  4  -2  1  -8");
            Console.WriteLine("-1  1  -1  1  -1");
            Console.WriteLine(" 0  0   0  1   0");
            Console.WriteLine(" 8  4   2  1   8");
            Console.WriteLine("Коэффициенты матрицы:");
            Console.WriteLine("a3 = 1, a2 = 0, a1 = 0, a0 = 0");
            Console.WriteLine("Интерполяционный многочлен:");
            Console.WriteLine("P(x) = x^3");
            Console.WriteLine("Значения многочлена в искомых точках:");
            Console.WriteLine("P((x0+x1)/2) = -27/8");
            Console.WriteLine("P((x1+x2)/2) = -1/8");
            Console.WriteLine("P((x2+x3)/2) = 1");
        }
    }
}
