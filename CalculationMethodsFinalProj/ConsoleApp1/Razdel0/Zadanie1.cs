using System;
namespace ConsoleApp1
{
    public class Zadanie1
    {
        public static double fact(int n)
        {
            if (n < 0) return 0;
            if (n == 0) return 1;
            else return n * fact(n - 1);
        }
        public static double f(int x, int n)
        {
            return Math.Pow(x, n) / fact(n);
        }
        public static void LocalMain()
        {
            double sum; int i;
            int n = 1;
            Console.WriteLine("Введите a и b:");
            int a = int.Parse(Console.ReadLine());
            int b = int.Parse(Console.ReadLine());
            double eps = double.Parse(Console.ReadLine());
            Console.WriteLine("x\tf(x)\t\t\tn");
            for (int x = a; x < b; x++)
            {
                sum = 0;
                n = 1;
                i = 0;
                while (Math.Abs(f(x, n)) > eps)
                {
                    sum += Math.Pow(-1, i) * f(x, n);
                    n += 2;
                    i++;
                }
                Console.WriteLine("{0}\t{1}\t{2}", x, sum, n);
            }
        }
    }
}
