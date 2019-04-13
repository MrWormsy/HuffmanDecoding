using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Huffman {
    class Program
    {
        static void Main(string[] args)
        {

            StreamWriter writer = new StreamWriter("..\\..\\data.txt");
            StreamWriter decodedText = new StreamWriter("..\\..\\decodedText.txt");


            Tree tree = new Tree();

            String path = "..\\..\\data.txt";

            String pathDataToEncode = "..\\..\\dataToEncode.txt";

            Node root = Tree.getTreeFromEncodedFile(writer, pathDataToEncode);

            Tree.encodeFile(writer, root, pathDataToEncode);

            writer.Close();

            Console.WriteLine("");

            tree.buildTreeFromTextFile(path);

            //Console.WriteLine("Encoded sentence : " + tree.EncodedData);

            Console.WriteLine("");

            Console.Write("Decoded sentence : ");

            tree.decode(decodedText, 0);

            decodedText.Close();

            Console.ReadKey();

        }
    }
}
