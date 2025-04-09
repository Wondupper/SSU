using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Matrix
    {
        public static int v = 7;
        public static List<double> Gaus(List<double[]> matrix, List<double[]> b)
        {
            List<double> result = new List<double>();
            for (int k = 0; k<matrix.Count-1; k++)
            {
                double temp = (matrix[k])[k];
                if (temp != 0)
                {
                    continue;
                }
                else
                {
                    bool flag = false;
                    for (int i_finder = k + 1; i_finder < matrix.Count; i_finder++)
                    {
                        if ((matrix[i_finder])[k] != 0)
                        {
                            (matrix[k], matrix[i_finder]) = (matrix[i_finder], matrix[k]);
                            flag = true;
                            break;
                        }
                    }
                    if (!flag)
                    {
                        return null;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            for (int k = 0; k < matrix.Count - 1; k++)
            {
                double temp = (matrix[k])[k];
                for (int j = k; j < matrix.Count; j++)
                {
                    (matrix[k])[j] *= 1.0 / temp;
                }
                b[k][0] *= 1.0 / temp;
                for (int i = k + 1; i < matrix.Count; i++)
                {
                    temp = (matrix[i])[k];
                    for (int j = k; j < matrix.Count; j++)
                    {
                        (matrix[i])[j] = (matrix[i])[j] - (matrix[k])[j] * temp;
                    }
                    b[i][0] -= b[k][0]*temp;
                }
            }
            result.Add(b.Last()[0] / (matrix.Last()[matrix.Last().Length-1]));
            for (int i = matrix.Count - 2; i >= 0; i--)
            {
                double res = 0;
                for (int j = 0; j < result.Count + 1; j++)
                {
                    if (j == 0)
                    {
                        res = b[i][0];
                    }
                    else
                    {
                        res -= (matrix[i])[matrix.Count  - j] * result[j - 1];
                    }
                }
                res /= (matrix[i])[i];
                result.Add(res);
            }
            result.Reverse();
            return result;
        }


        public static List<double[]> MultiplicationMatrix(List<double[]> A, List<double[]> B)
        {
            if ((A.First()).Length == B.Count)
            {
                List<double[]> C = new List<double[]>();
                foreach (double[] row in A)
                {
                    double[] rowC = new double[(B.First()).Length];
                    for (int j = 0; j < (B.First()).Length; j++)
                    {
                        double el = 0;
                        for (int i = 0; i < (A.First()).Length; i++)
                        {
                            el += row[i] * (B[i])[j];
                        }
                        rowC[j] = el;
                    }
                    C.Add(rowC);
                }
                return C;
            }
            else
            {
                return null;
            }
        }

        public static List<double> Progonka(List<double[]> A, List<double[]> b)
        {
            int n = A.Count;
            List<double> Result = new List<double>();
            List<double[]> CoafMas = new List<double[]>();
            for (int i = 0; i < n; i++)
            {
                double[] row = new double[2];
                if (i == 0)
                {
                    row[0] = (A[i])[1] / (-1 * (A[i])[0]);
                    row[1] = (-1 * (b[i])[0]) / (-1 * (A[i])[0]);
                }
                else if (i == n - 1)
                {
                    row[0] = 0;
                    row[1] = ((A[i])[n-2] * (CoafMas[i - 1])[1] - (b[i])[0]) / (-1 * (A[i])[n-1] - (A[i])[n-2] * (CoafMas[i - 1])[0]);
                }
                else
                {
                    row[0] = A[i][i+1] / (-1 * A[i][i] - A[i][i-1] * CoafMas[i - 1][0]);
                    row[1] = (A[i][i-1] * CoafMas[i - 1][1] - b[i][0]) / (-1 * A[i][i] - A[i][i-1] * CoafMas[i - 1][0]);
                }
                CoafMas.Add(row);
            }
            CoafMas.Reverse();
            for (int i = 0; i < n; i++)
            {
                if (i == 0)
                {
                    Result.Add(CoafMas[i][1]);
                }
                else
                {
                    Result.Add(CoafMas[i][0] * Result.Last() + CoafMas[i][1]);
                }
            }
            Result.Reverse();
            return Result;
        }
    }
}
