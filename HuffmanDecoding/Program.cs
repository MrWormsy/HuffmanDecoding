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

            StreamWriter writer = new StreamWriter("..\\..\\data.bin");
            StreamWriter decodedText = new StreamWriter("..\\..\\decodedText.txt");

            //StreamReader reader = new StreamReader("..\\..\\data.txt");

            Tree tree = new Tree();            

            String pathDataToEncode = "..\\..\\dataToEncode.txt";

            Node root = Tree.getTreeFromEncodedFile(writer, pathDataToEncode);

            Tree.encodeFile(writer, root, pathDataToEncode);

            writer.Close();

            Console.WriteLine("----- File has been encoded -----");

            using (var reader = new StreamReader("..\\..\\data.bin"))
            {
                tree.buildTreeFromTextFile(reader);
                tree.decode(decodedText, reader, "");
            }

            decodedText.Close();
            //reader.Close();

            Console.WriteLine("----- File has been decoded -----");

            Console.ReadKey();

        }
    }
}
