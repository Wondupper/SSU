
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Xaffman_forever
{
    public class HuffmanTree
    {
        public Node Root;

        public HuffmanTree() 
        {

        }

        private SortedSet<Node> nodes = new SortedSet<Node>();

        public Dictionary<char, int> frequencies = new Dictionary<char, int>();

        public void Build(string source)
        {
            for (int i = 0; i < source.Length; i++)
            {
                if (!frequencies.ContainsKey(source[i]))
                {
                    frequencies.Add(source[i], 0);
                }
                frequencies[source[i]]++;
            }

            foreach (KeyValuePair<char, int> symbol in frequencies)
            {
                Node a = new Node()
                {
                    letter = symbol.Key,
                    frequency = symbol.Value
                };

                nodes.Add(a);

            }
            nodes = new SortedSet<Node>(nodes);
            while (nodes.Count > 1)
            {
                Node SonL = nodes.First();
                nodes.Remove(SonL);
                Node SonR = nodes.First();
                nodes.Remove(SonR);
                Node parent = new Node(SonL, SonR);
                nodes.Add(parent);
            }
            BuildTable(nodes.First(), string.Empty);
        }


        public Dictionary<char, String> table = new Dictionary<char, String>();
        void BuildTable(Node Root, string code)
        {
            if (Root.left != null)
            {
                BuildTable(Root.left, code + '0');
            }
            if (Root.right != null)
            {
                BuildTable(Root.right, code + '1');
            }
            if (Root.letter != '\0')
            {
                table.Add(Root.letter, code);
                Root.letter = '\0';
            }
        }


        public StringBuilder Data(StringBuilder data)
        {
            foreach (KeyValuePair<char, String> symbol in table)
            {
                data.Append(symbol.Key);
                data.Append(symbol.Value);
                data.Append('\t');
            }
            data.Append("\r\n");
            return data;
        }
        public int Nodes()
        {
            return frequencies.Count();
        }
        public byte[] OutPut(char c)
        {
            return Encoding.ASCII.GetBytes(table[c]);
        }


        public void buildTree(Node node, string str, Dictionary<String, char> table)
        {
            if (table.ContainsKey(str))
            {
                node.letter = table[str];
            }
            else
            {
                buildTree(node.left = new Node('\0'), str + '0', table);
                buildTree(node.right = new Node('\0'), str + '1', table);
            }
        }
        public Node InPutTable(String[] mas)
        {
            Node Root = new Node('\0');
            Dictionary<String, char> table = new Dictionary<String, char>();
            foreach (string sb in mas)
            {
                if (sb != "")
                    table.Add(sb.Substring(1), sb[0]);
            }
            buildTree(Root, "", table);
            return Root;
        }
        public char DecodeSymbol(Node node, string line, int index, ref int lenght)
        {
            if (node.letter != '\0')
            {
                return node.letter;
            }
            else
            {
                lenght++;
                if (line[index] == '0')
                {
                    index++;
                    return DecodeSymbol(node.left, line, index, ref lenght);
                }
                else
                {
                    index++;
                    return DecodeSymbol(node.right, line, index, ref lenght);
                }
            }
        }
        public StringBuilder Decode(Node Root, string line)
        {
            StringBuilder s = new StringBuilder();
            int index = 0;
            while (index < line.Length)
            {
                int lenght = 0;
                s.Append(DecodeSymbol(Root, line, index, ref lenght));
                index += lenght;
            }
            return s;
        }
    }
}

