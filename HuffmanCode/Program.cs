using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Text;
using System.Collections;
using System.IO;
using System.IO.Compression;

namespace Xaffman_forever
{
    class Program
    {
        static void Main()
        {
            string line;
            HuffmanTree huffmanTree = new HuffmanTree();
            using (StreamReader fileIn = new StreamReader("D:/Test/input.txt"))
            {
                line = fileIn.ReadToEnd();
                huffmanTree.Build(line);
                StringBuilder data = new StringBuilder((huffmanTree.Nodes()).ToString());
                data.Append("\r\n");
                List<Byte> bytes = new List<Byte>();
                foreach (char c in line)
                {
                    bytes.AddRange(huffmanTree.OutPut(c));

                }
                using (StreamWriter fileOut = new StreamWriter("D:/Test/2.bin"))
                {
                    fileOut.Write(huffmanTree.Data(data));
                    data.Clear();
                }

                using (FileStream fileOut = new FileStream("D:/Test/2.bin", FileMode.Append, FileAccess.Write))
                {
                    for (int i = 0; i < bytes.Count; i++)
                    {
                        fileOut.WriteByte(bytes[i]);
                    }
                    fileOut.Close();
                }

                using (FileStream fileOut = File.OpenRead("D:/Test/2.bin"))
                {
                    using (FileStream compressedFileStream = File.Create("D:/Test/1.gz"))
                    {
                        using (GZipStream gzipStream = new GZipStream(compressedFileStream, CompressionMode.Compress))
                        {
                            fileOut.CopyTo(gzipStream);
                        }
                    }
                }
                using (FileStream fileOut = File.OpenRead("D:/Test/1.gz"))
                {
                    using (FileStream decompressedFileStream = File.Create("D:/Test/22.bin"))
                    {
                        using (GZipStream gzipStream = new GZipStream(fileOut, CompressionMode.Decompress))
                        {
                            gzipStream.CopyTo(decompressedFileStream);
                        }
                    }
                }

                string line2;
                using (StreamReader file = new StreamReader("D:/Test/2.bin"))
                {
                    line = file.ReadLine();
                    line2 = file.ReadLine();
                    String[] mas = line2.Split('\t');
                    HuffmanTree huffmanTree1 = new HuffmanTree();
                    Node Root = huffmanTree1.InPutTable(mas);
                    line = file.ReadToEnd();
                    File.WriteAllText("D:/Test/3.txt", (huffmanTree1.Decode(Root, line)).ToString());
                }
            }
        }
    }
}