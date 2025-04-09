using System;
using System.Collections.Specialized;
using System.Reflection.Metadata.Ecma335;

namespace Test
{
    internal class Node
    {
        public int inf;
        public Node left;
        public Node rigth;

        public Node(int nodeInf)
        {
            inf = nodeInf;
            left = null;
            rigth = null;
        }
        public static void Add(ref Node r, int nodeInf)
        {
            if (r == null)
            {
                r = new Node(nodeInf);
            }
            else
            {
                if (((IComparable)(r.inf)).CompareTo(nodeInf) > 0)
                {
                    Add(ref r.left, nodeInf);
                }
                else
                {
                    Add(ref r.rigth, nodeInf);
                }
            }
        }
        public static void F(Node t, ref int sum, int count, int heigth)
        {
            if (t != null && count == heigth)
            {
                if ((t.inf % 2) == 0)
                {
                    sum += t.inf;
                }
            }
            if (t != null && count < heigth)
            {
                count++;
                F(t.left, ref sum, count, heigth);
                F(t.rigth, ref sum, count, heigth);

            }
        }
        public static void Preorder(Node r)
        {
            if (r != null)
            {
                Console.Write("{0} ", r.inf);
                Preorder(r.left);
                Preorder(r.rigth);
            }
        }
    }
}
