using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Diffuri2
    {

        public static int v = 7;
        public static int T = 7;

        public static double f(double x)
        {
            if (x == 0 || x == T)
            {
                return 0;
                
            }
            else
            {
                return 4 * v * x * x * x * x - 3 * T * v * x * x * x + 6 * v * x - 2 * T * v;
            }
        }

        public static double p(double x)
        {
            return x * x;
        }

        public static double q(double x)
        {
            return x ;
        }

        public static double k1(double x,double h)
        {
            return 1.0 / Math.Pow(h, 2) - Math.Pow(x, 2) / (2.0 * h);
        }
        public static double k2(double x, double h)
        {
            return -2.0 / Math.Pow(h, 2) + x;
        }
        public static double k3(double x, double h)
        {
            return 1.0 / Math.Pow(h, 2) + Math.Pow(x, 2) / (2.0 * h);
        }

        public static double fi(double x, int i)
        {
            return Math.Pow(x, i + 1) * (x - T);
        }

        public static double d1fi(double x, int i)
        {
            return Math.Pow(x, i) * ((i + 2) * x - (i + 1) * T);
        }
        public static double d2fi(double x, int i)
        {
            return Math.Pow(x, i-1) * ((i + 2)*(i+1) * x - (i + 1)*i * T);
        }

        public static void RaznostnySxema(double x,int n)
        {
            List<double> X = new List<double>();
            List<double[]> matrix = new List<double[]>();
            List<double[]> b = new List<double[]>();
            double h = (double)T / n;
            n = n - 1;
            x += h;
            X.Add(x);
            for (int i = 1; i <= n; i++)
            {
                List<double> mas = new List<double>();
                if (i == 1)
                {
                    mas.Add(k2(x,h));
                    mas.Add(k3(x, h));
                    while (mas.Count < n )
                    {
                        mas.Add(0);
                    }

                }
                else if (i==n)
                {
                    while (mas.Count < n-2)
                    {
                        mas.Add(0);
                    }
                    mas.Add(k1(x, h));
                    mas.Add(k2(x, h));
                }
                else
                {
                    for (int j = 1; j < i-1 ; j++)
                    {
                        mas.Add(0);
                    }
                    mas.Add(k1(x, h));
                    mas.Add(k2(x, h));
                    mas.Add(k3(x, h));
                    while(mas.Count < n )
                    {
                        mas.Add(0);
                    }
                }
                double[] temp = new double[1];
                temp[0] = f(x);
                b.Add(temp);
                matrix.Add(mas.ToArray());
                x += h;
                X.Add(x);
            }
            List<double> result = Matrix.Progonka(matrix, b);
            for (int i = 0; i < result.Count; i++)
            {
                Console.WriteLine("xk= {0}, yk= {1}, y_tochnoe = {2} ", X[i], result[i], 7 * X[i] * X[i] * (X[i]-T));
            }

        }

        public static void NeoprCoaf(double x, int n)
        {
            List<double> X = new List<double>();
            List<double[]> matrix = new List<double[]>();
            List<double[]> b = new List<double[]>();
            double h = (double)T / n;
            Dictionary<double,List<double>> FI = new Dictionary<double,List<double>>();
            Dictionary<double, List<double>> D1FI = new Dictionary<double, List<double>>();
            Dictionary<double, List<double>> D2FI = new Dictionary<double, List<double>>();
            n--;
            for (int i = 1; i <= n; i++)
            {
                x += h;
                X.Add(x);
                List<double> fi_temp = new List<double>();
                List<double> d1fi_temp = new List<double>();
                List<double> d2fi_temp = new List<double>();
                for (int j = 1; j <= n; j++) 
                {
                    fi_temp.Add(fi(x, j));
                    d1fi_temp.Add(d1fi(x, j));
                    d2fi_temp.Add(d2fi(x, j));
                }
                FI.Add(x, fi_temp);
                D1FI.Add(x, d1fi_temp);
                D2FI.Add(x, d2fi_temp);
            }
            for (int j = 1; j <= n;j++)
            {
                double[] row = new double[n];
                for (int k = 1; k <= n; k++)
                {
                    row[k - 1] = (D2FI[X[j - 1]])[k - 1] + p(X[j - 1]) * (D1FI[X[j - 1]])[k - 1] + q(X[j - 1]) * (FI[X[j - 1]])[k - 1];
                }
                matrix.Add(row);
                double[] el = new double[1];
                el[0] = f(X[j-1]);
                b.Add(el);
            }

            List<double> Findak = Matrix.Gaus(matrix, b);
            List<double> result = new List<double>();
            for (int i = 1; i <= n; i++)
            {
                double y = 0;
                for (int k = 1;k <= n; k++) 
                {
                    y += Findak[k - 1] * (FI[X[i - 1]])[k - 1];
                }
                result.Add(y);
            }
            for (int i = 1; i <= n; i++)
            {
                Console.WriteLine("xi = {0} y= {1} y_tochnoe = {2}", X[i - 1], result[i-1], v*Math.Pow(X[i - 1],2)*(X[i - 1]-T));
            }
        }

    }
}