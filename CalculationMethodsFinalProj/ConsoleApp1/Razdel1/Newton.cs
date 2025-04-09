using System;

namespace ConsoleApp1
{
    class Newton
    {
        public static double NewtonDo(double x, int n, double[] MasX, double[] MasY)
        {
            double[,] matrix = new double[n, n];
            int k = 1;
            bool flag = true;
            for (int j = 0; j < n; j++)
            {
                for (int i = 0; i < n - j; i++)
                {
                    if (j == 0)
                    {
                        matrix[i, j] = MasY[i + j];
                    }
                    else
                    {
                        matrix[i, j] = (matrix[i + 1, j - 1] - matrix[i, j - 1]) / (MasX[i + k] - MasX[i]);
                    }
                }
                if (flag == false)
                {
                    k++;
                }
                if (flag == true)
                {
                    flag = false;
                }
            }
            double res = 0;
            double pr = 1;
            for (int j = 0; j < n; j++)
            {
                if (j == 0)
                {
                    res += matrix[0, j];
                }
                else
                {
                    pr = pr * (x - MasX[j - 1]);
                    res += pr * matrix[0, j];
                }
            }
            return res;
        }
        public static void NewtonMethod()
        {
            int n = 5;
            double[] MasX = new double[] { -4, -3, 0, 1, 5 };
            double[] MasY = new double[] { 16, 1, 0, 1, 16 };
            double[] newMasX = new double[9];
            int k = 0;
            for (int i = 0; i < 9; i++)
            {
                if (i % 2 == 0)
                {
                    Console.Write("{0} ", MasX[i - k]);
                    k++;
                }
                else
                {
                    Console.Write("{0} ", (MasX[i - k + 1] - MasX[i - k]) / 2);
                }
            }
            Console.WriteLine();
            k = 0;
            for (int i = 0; i < 9; i++)
            {
                if (i % 2 == 0)
                {
                    Console.Write("{0} ", NewtonDo(MasX[i - k], n, MasX, MasY));
                    k++;
                }
                else
                {
                    Console.Write("{0} ", NewtonDo((MasX[i - k + 1] - MasX[i - k]) / 2, n, MasX, MasY));
                }
            }
        }
    }
}