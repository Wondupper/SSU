using System;

namespace Xaffman_forever
{
    public class Node : IComparable<Node>
    {
        public char letter;
        public int frequency;
        public int height;
        public Node right;
        public Node left;

        public Node()
        { 

        }
        public Node(Node l, Node r)
        {
            left = l;
            right = r;
            height++;
            frequency = l.frequency + r.frequency;
        }
        public Node(char a)
        {
            letter = a;
            left = null;
            right = null;
        }

        public int CompareTo(Node name2)
        {
            if (frequency > name2.frequency)
            {
                return 1;
            }
            if (frequency < name2.frequency)
            {
                return -1;
            }
            else
            {
                if (height > name2.height)
                {
                    return 1;
                }
                else if (height < name2.height)
                {
                    return -1;
                }
                else
                {
                    if (letter > name2.letter)
                    {
                        return 1;
                    }
                    else if (letter < name2.letter)
                    {
                        return -1;
                    }
                    return 0;
                }

            }
        }

    }
}
