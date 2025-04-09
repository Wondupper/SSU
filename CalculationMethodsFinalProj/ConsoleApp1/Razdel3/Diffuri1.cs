using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Diffuri1
    {

        public static int v = 7;
        public static int T = 7;

        //EilersMethods
        public static double f(double xk, double y_xk)
        {
            return 2*v*xk + v*xk*xk;
        }
        public static double y_newE(double yk, double xk, double h)
        {
            return yk + h * f(xk, yk);
        }
        public static double y_newE1(double yk, double xk, double h)
        {
            return yk + h * f(xk, y_newE(yk, xk, h / 2));
        }
        public static double y_newE2(double yk, double xk, double h)
        {
            return yk + h / 2 * (f(xk, yk) + f(xk + h, y_newE(yk, xk, h)));
        }
        public static void EilersMethod(double x0, double y0, int n, double h)
        {
            Console.WriteLine("i = {0}, xi = {1}, yi = {2}, y_tochnoe = {3}", 0, x0, y0,  v*x0*x0);
            for (int i = 1; i < n; i++)
            {
                y0 = y_newE(y0, x0, h);
                x0 += h;
                Console.WriteLine("i = {0}, xi = {1}, yi = {2}, y_tochnoe = {3}", i, x0, y0, v * x0 * x0);
            }
        }
        public static void EilersMethod1(double x0, double y0, int n, double h)
        {
            Console.WriteLine("i = {0}, xi = {1}, yi = {2}, y_tochnoe = {3}", 0, x0, y0, v * x0 * x0);
            for (int i = 1; i < n; i++)
            {
                y0 = y_newE1(y0, x0, h);
                x0 += h;
                Console.WriteLine("i = {0}, xi = {1}, yi = {2}, y_tochnoe = {3}", i, x0, y0, v * x0 * x0);
            }
        }
        public static void EilersMethod2(double x0, double y0, int n, double h)
        {
            Console.WriteLine("i = {0}, xi = {1}, yi = {2}, y_tochnoe = {3}", 0, x0, y0, v * x0 * x0);
            for (int i = 1; i < n; i++)
            {
                y0 = y_newE2(y0, x0, h);
                x0 += h;
                Console.WriteLine("i = {0}, xi = {1}, yi = {2}, y_tochnoe = {3}", i, x0, y0, v * x0 * x0);
            }
        }

    }
}