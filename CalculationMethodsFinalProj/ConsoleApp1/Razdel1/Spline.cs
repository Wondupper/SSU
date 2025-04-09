using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Spline
    {
        public static void SplineMethod()
        {
            Console.ResetColor();
            const int n = 4; // n - количество известных значений
            double[,] table = new double[2, n];
            for (int i = 0; i < 2; i++)
            {
                if (i == 0)
                    Console.Write("Введите значения x0, x1...xn: ");
                else if (i == 1)
                    Console.Write("Введите значения f0, f1...fn: ");
                string[] tmp = Console.ReadLine().Split(' ');
                for (int j = 0; j < n; j++)
                {
                    table[i, j] = double.Parse(tmp[j]);
                }
            }
            // вывод исходной таблицы
            Console.WriteLine("Исходная таблица:");
            for (int i = 0; i < 2; i++)
            {
                if (i == 0)
                    Console.Write("Знач. x:\t");
                else if (i == 1)
                    Console.Write("Знач. f:\t");
                for (int j = 0; j < n; j++)
                {
                    Console.Write("{0} ", table[i, j]);
                }
                Console.WriteLine();
            }

            int dim = 4 * (n - 1);
            double[,] slay = new double[dim, dim + 1];

            for (int i = 0; i < dim; i++)
            {
                for (int j = 0; j < dim; j++)
                {
                    if (i < dim / 4)
                    {
                        slay[i, n * i] = 1;
                        slay[i, dim] = table[1, i];
                    }
                    if (i >= dim / 4 && i < dim / 2)
                    {
                        for (int k = 0; k < n; k++)
                        {
                            if (k == 0)
                                slay[i, n * (i - dim / 4)] = 1;
                            else
                                slay[i, n * (i - dim / 4) + k] = Math.Pow(table[0, (i - dim / 4) + 1] - table[0, i - dim / 4], k);
                        }
                        slay[i, dim] = table[1, i - 2];
                    }
                    if (i >= dim / 2 && i < (dim / 2 + dim / 6))
                    {
                        for (int k = 0; k < n - 1; k++)
                        {
                            if (k == 0)
                                slay[i, n * (i - dim / 2) + 1] = 1;
                            else
                                slay[i, n * (i - dim / 2) + 1 + k] = (k + 1) * Math.Pow(table[0, (i - dim / 2) + 1] - table[0, i - dim / 2], k);
                            slay[i, n * (i - dim / 2 + 1) + 1] = -1;
                        }
                    }
                    if (i >= (dim / 2 + dim / 6) && i < (dim / 2 + 2 * (dim / 6)))
                    {
                        for (int k = 0; k < n - 1; k++)
                        {
                            slay[i, n * (i - dim / 2 - dim / 6) + 2] = 2;
                            slay[i, n * (i - dim / 2 - dim / 6) + 3] = 6 * (table[0, (i - dim / 2 - dim / 6) + 1] - table[0, (i - dim / 2 - dim / 6)]);
                            slay[i, n * (i - dim / 2 - dim / 6) + 2 + n] = -2;
                        }
                    }
                    if (i == dim - 2)
                    {
                        slay[i, n * (i - (dim - 2)) + 2] = 2;
                    }
                    else if (i == dim - 1)
                    {
                        slay[i, dim - 2] = 2;
                        slay[i, dim - 1] = 6 * (table[0, n - 1] - table[0, n - 2]);
                    }
                }
            }
            for (int i = 0; i < dim; i++)
            {
                for (int j = 0; j < dim + 1; j++)
                {
                    if (j != dim)
                        Console.Write("{0} ", slay[i, j]);
                    else
                        Console.Write("\t| {0}", slay[i, j]);
                }
                Console.WriteLine();
            }
        }
    }
}
