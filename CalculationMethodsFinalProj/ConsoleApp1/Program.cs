using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    public class Program
    {
        public static void Main()
        {
            Console.WriteLine("gjrf");

            ////Progonka example
            //List<double[]> A = new List<double[]>();
            //double[] mas1 = { 7, 0.07, 0 };
            //double[] mas2 = { 0.08, 8, 0.08 };
            //double[] mas3 = { 0, 0.09, 9 };
            //A.Add(mas1);
            //A.Add(mas2);
            //A.Add(mas3);
            //List<double[]> stolbec = new List<double[]>();
            //double[] mas4 = { 7 };
            //double[] mas5 = { 8 };
            //double[] mas6 = { 9 };
            //stolbec.Add(mas4);
            //stolbec.Add(mas5);
            //stolbec.Add(mas6);
            //List<double[]> b = Matrix.MultiplicationMatrix(A, stolbec);
            //List<double> result = Matrix.Progonka(A, b);
            //foreach (double el in result)
            //{
            //    Console.Write("{0} ", el);
            //}


            ////Gaus example
            //List<double[]> A = new List<double[]>();
            //double[] mas1 = { 7, 0.07, 0 };
            //double[] mas2 = { 0.08, 8, 0.08 };
            //double[] mas3 = { 0, 0.09, 9 };
            //A.Add(mas1);
            //A.Add(mas2);
            //A.Add(mas3);
            //List<double[]> stolbec = new List<double[]>();
            //double[] mas4 = { 7 };
            //double[] mas5 = { 8 };
            //double[] mas6 = { 9 };
            //stolbec.Add(mas4);
            //stolbec.Add(mas5);
            //stolbec.Add(mas6);
            //List<double[]> b = Matrix.MultiplicationMatrix(A, stolbec);
            //List<double> result = Matrix.Gaus(A, b);
            //foreach (double el in result)
            //{
            //    Console.Write("{0} ", el);
            //}


            ////ODU 1 Porydok
            //Diffuri1.EilersMethod(1, 7, 13, 0.001);
            //Console.WriteLine();
            //Console.WriteLine();
            //Diffuri1.EilersMethod1(1, 7, 13, 0.001);
            //Console.WriteLine();
            //Console.WriteLine();
            //Diffuri1.EilersMethod2(1, 7, 13, 0.001);


            ////ODU 2 Raznostny Method
            //Diffuri2.RaznostnySxema(0, 25);


            ////ODU 2 Neopredelenie coafi
            //Diffuri2.NeoprCoaf(0, 10);

            ////Reshenie uravnenia Fredgolma s virozdennim idrom
            //IntegralnieUravnenia.IntUravFredWithVirYadro();

            ////Rvadraturny Method
            //IntegralnieUravnenia.KvadraturnyMethod();
        }
    }
}