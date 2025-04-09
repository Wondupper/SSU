using System;

namespace ConsoleApp1
{
    public class Lagrange
    {
        public static void LagrangeMethod()
        {
            const int size = 5;
            var xValues = new double[size];
            var yValues = new double[size];
            int k = 0;
            for (int i = -2; i < 3; i++)
            {
                xValues[k] = i;
                yValues[k] = TestF(i);
                k++;
            }
            Console.Write(xValues[0]);
            Console.Write(" ");
            Console.Write((xValues[0] + xValues[1]) / 2);
            Console.Write(" ");
            Console.Write(xValues[1]);
            Console.Write(" ");
            Console.Write((xValues[1] + xValues[2]) / 2);
            Console.Write(" ");
            Console.Write(xValues[2]);
            Console.Write(" ");
            Console.Write((xValues[2] + xValues[3]) / 2);
            Console.Write(" ");
            Console.Write(xValues[3]);
            Console.Write(" ");
            Console.Write((xValues[3] + xValues[4]) / 2);
            Console.Write(" ");
            Console.Write(xValues[4]);
            Console.WriteLine();
            Console.Write(LagrangeDo(xValues[0], xValues, yValues, size));
            Console.Write(" ");
            Console.Write(LagrangeDo((xValues[0] + xValues[1]) / 2, xValues, yValues, size));
            Console.Write(" ");
            Console.Write(LagrangeDo(xValues[1], xValues, yValues, size));
            Console.Write(" ");
            Console.Write(LagrangeDo((xValues[1] + xValues[2]) / 2, xValues, yValues, size));
            Console.Write(" ");
            Console.Write(LagrangeDo(xValues[2], xValues, yValues, size));
            Console.Write(" ");
            Console.Write(LagrangeDo((xValues[2] + xValues[3]) / 2, xValues, yValues, size));
            Console.Write(" ");
            Console.Write(LagrangeDo(xValues[3], xValues, yValues, size));
            Console.Write(" ");
            Console.Write(LagrangeDo((xValues[3] + xValues[4]) / 2, xValues, yValues, size));
            Console.Write(" ");
            Console.Write(LagrangeDo(xValues[4], xValues, yValues, size));
        }

        static double LagrangeDo(double x, double[] xValues, double[] yValues, int size)
        {
            double lagrangePol = 0;

            for (int i = 0; i < size; i++)
            {
                double basicsPol = 1;
                for (int j = 0; j < size; j++)
                {
                    if (j != i)
                    {
                        basicsPol *= (x - xValues[j]) / (xValues[i] - xValues[j]);
                    }
                }
                lagrangePol += basicsPol * yValues[i];
            }

            return lagrangePol;
        }

        static double TestF(double x)
        {
            return x * x * x;
        }
    }
}