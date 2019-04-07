using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HuffmanDecoding {
    class Program
    {
        static void Main(string[] args)
        {

            Tree tree = new Tree();
            tree.buildTreeFromTextFile();

            Console.WriteLine("Encoded sentence : " + tree.EncodedData);

            Console.Write("Decoded sentence : ");

            tree.decode();

            Console.ReadKey();

        }
    }
}
