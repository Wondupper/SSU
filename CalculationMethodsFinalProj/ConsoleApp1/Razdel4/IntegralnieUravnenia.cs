using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class IntegralnieUravnenia
    {
        public static int lambda = 1;
        public static int a = 0;
        public static int b = 1;
        public static int n = 3;
        public static int v = 7;
        public static double h = (double)(b-a)/n;
        public static double fx(double x)
        {
            return v*(4.0 / 3.0 * x + 0.25 * x * x + 0.2 * x * x * x);
        }

        public static double y_tochnoe(double x)
        {
            return v*x;
        }
        public static double ak_x(double x,int i) 
        {
            return Math.Pow(x,i);
        }
        public static double bk_x(double t, int i)
        {
            return Math.Pow(t, i);
        }

        public static double Akj(int k,int j)
        {
            return Math.Pow(b,k+j+1)/(k+j+1) - Math.Pow(a, k + j+1) / (k + j + 1);
        }

        public static double A_xt(double x, double t)
        {
            return (x * t + x * x * t * t + x * x * x * t * t * t);
        }

        public static double FIj(int j)
        {
            return (v * 4.0 / 3.0) * ((double)(Math.Pow(b, j + 2) / (j + 2.0)) - (double)(Math.Pow(a, j + 2) / (j + 2.0))) + (v/4.0) * ((double)(Math.Pow(b, j + 3) / (j + 3.0)) - (double)(Math.Pow(a, j + 3) / (j + 3.0))) + (v/5.0) * ((double)(Math.Pow(b, j + 4) / (j + 4.0)) - (double)(Math.Pow(a, j + 4) / (j + 4.0)));
        }

        public static void IntUravFredWithVirYadro()
        {
            List<double> X = new List<double>();
            X.Add(0.25);
            X.Add(0.5);
            X.Add(0.75);
            List<double[]> matrix = new List<double[]>();
            List<double[]> b = new List<double[]>();
            for (int i = 1; i <= n; i++)
            {
                double[] row = new double[n];
                for (int j = 1; j <= n; j++)
                {
                    if (i == j)
                    {
                        row[j - 1] = 1.0 + lambda * Akj(j, i);
                    }
                    else
                    {
                        row[j - 1] = lambda * Akj(j, i);
                    }
                }
                matrix.Add(row);
                double[] el = new double[1];
                el[0] = FIj(i);
                b.Add(el);
            }
            List<double> result = Matrix.Gaus(matrix,b);
            for (int i = 1; i <= n; i++)
            {
                double sum = 0;
                for (int k = 1; k <= n; k++)
                {
                    sum += result[k-1] * ak_x(X[i-1],k);
                }
                double y = fx(X[i - 1]) - lambda * sum;
                double x = X[i - 1];
                Console.WriteLine("x = {0} | y = {1} | y_tochnoe = {2} ", x, y, y_tochnoe(x));
            }
        }

        public static void KvadraturnyMethod()
        {
            List<double> X = new List<double>();
            List<double> T = new List<double>();
            List<double[]> matrix = new List<double[]>();
            List<double[]> b = new List<double[]>();
            for (int i = 1;i <= n;i++) 
            {
                if (X.Count == 0 && T.Count == 0)
                {
                    X.Add(0);
                    T.Add(0);
                }
                else
                {
                    X.Add(h + X.Last());
                    T.Add(h + T.Last());
                }
            }
            for (int i = 1; i <= n; i++)
            {
                double[] row = new double[n];
                for (int j = 1; j <= n; j++)
                {
                    if (i == j)
                    {
                        row[j - 1] = 1.0 + lambda * A_xt(X[i - 1], T[j-1]);
                    }
                    else
                    {
                        row[j - 1] = lambda * A_xt(X[i - 1], T[j - 1]);
                    }
                }
                matrix.Add(row);
                double[] el = new double[1];
                el[0] = fx(X[i-1]);
                b.Add(el);
            }
            List<double> result = Matrix.Gaus(matrix, b);
            for (int i = 1; i <= n; i++)
            {
                Console.WriteLine("x = {0} | y = {1} | y_tochnoe = {2} ", X[i-1], result[i-1], y_tochnoe(X[i-1]));
            }
        }
    }
}
