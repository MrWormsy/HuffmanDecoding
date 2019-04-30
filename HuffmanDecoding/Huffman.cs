using System;
using System.IO;
using System.Diagnostics;

namespace Huffman
{
    class Huffman
    {

        //Main method
        static void Main(string[] args)
        {
            //The stopwatch object needed to calculate the time both the encoding and the decoding part take
            Stopwatch stopwatch = new Stopwatch();

            //The streamwriter where we will write the encoded data
            StreamWriter encodedTextWriter = new StreamWriter("..\\..\\data.txt");

            //The streamwriter where we will write the decoded data
            StreamWriter decodedTextWriter = new StreamWriter("..\\..\\decodedText.txt");

            //The Huffman tree Object
            Tree tree = new Tree();

            //The path of the file to decode
            String pathDataToEncode = "..\\..\\dataToEncode.txt";

            //Launch the stopwatch to get the time in millisecond between the begining and the end of the encoding part
            stopwatch.Start();

            //The root of the Huffman tree from the encoded file
            Node root = Tree.getTreeFromFileToEncode(encodedTextWriter, pathDataToEncode);

            //Encode the file from the root of the tree
            Tree.encodeFile(encodedTextWriter, root, pathDataToEncode);

            //Close the encodedTextWriter (we no longer need it) 
            encodedTextWriter.Close();

            //Stop the stopwatch
            stopwatch.Stop();

            //Convert the encoded file to binary
            Utils.convertFileToBinary("..\\..\\data.txt");

            //Warn the operator that the file has been encoded
            Console.WriteLine("----- File has been encoded (in " + stopwatch.ElapsedMilliseconds + "ms)-----");

            //Launch the stopwatch to get the time in millisecond between the begining and the end of the decoding part
            stopwatch.Start();

            //We use a StreamReader to read the encoded file
            using (var reader = new StreamReader("..\\..\\data.txt"))
            {
                //We build the tree from the encoded file which will be used to decode the file
                tree.buildTreeFromEncodedTextFile(reader);

                //We decode each char using the tree and write it into the decodedTextWriter
                tree.decode(decodedTextWriter, reader, "");
            }

            //Close the decodedTextWriter (we no longer need it) 
            decodedTextWriter.Close();

            //Stop the stopwatch
            stopwatch.Stop();

            //Warn the operator that the file has been decoded
            Console.WriteLine("----- File has been decoded (in " + stopwatch.ElapsedMilliseconds + "ms)-----");

            //Wait for the operator to close the console
            Console.Write("\nType any key to close the console...");
            Console.ReadKey();
        }
    }
}