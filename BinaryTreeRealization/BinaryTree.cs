using System;
namespace Test
{
    internal class BinaryTree
    {
        Node tree;

        public int Inf
        {
            set { tree.inf = value; }
            get { return tree.inf; }
        }
        public BinaryTree()
        {
            tree = null;
        }
        private BinaryTree(Node r)
        {
            tree = r;
        }
        public void Add(int nodeInf)
        {
            Node.Add(ref tree, nodeInf);
        }
        public int F(int heigth)
        {
            int sum = 0;
            int count = 1;
            Node.F(tree,ref sum, count,  heigth); 
            return sum;
        }
        public void Preorder()
        {
            Node.Preorder(tree);
        }
    }
}