using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;

namespace Huffman
{

    //The Node class
    class Node
    {
        //The variables
        private int value;
        private Node left;
        private Node right;
        private char theChar;

        //The Node constructor
        public Node(char theChar)
        {
            this.value = 0;
            this.left = null;
            this.right = null;
            this.theChar = theChar;
        }

        //Getters and Setters
        public int Value { get => value; set => this.value = value; }
        public char TheChar { get => theChar; set => theChar = value; }
        internal Node Left { get => left; set => left = value; }
        internal Node Right { get => right; set => right = value; }

        // Get the Node according to the path
        public Node getNodeFromPath(String path)
        {
            if (path == null)
            {
                return this;
            }

            if (path.Length == 0)
            {
                return this;
            }

            if (path[0] == '0')
            {
                return this.Right.getNodeFromPath(path.Substring(1));
            }
            else
            {
                return this.Left.getNodeFromPath(path.Substring(1));
            }
        }

        //Get the path according to the Node
        public String getPathFromNode(char theChar)
        {
            if (this.TheChar == theChar)
            {
                return "";
            }

            if (this.Left == null && this.Right == null)
            {
                return null;
            }

            if (this.Right != null)
            {
                String str = this.Right.getPathFromNode(theChar);

                if (str != null)
                {
                    return str + "1";
                }
            }

            if (this.Left != null)
            {
                String str = this.Left.getPathFromNode(theChar);

                if (str != null)
                {
                    return str + "0";
                }
            }
            return null;
        }

        // Build the tree from the dictionary gathered earlier
        public void buildTree(Dictionary<string, char> dictionary, String theString, int maxLenght)
        {
            if (theString.Length > maxLenght)
            {
                return;
            }

            char charValue = '\0';

            //If the disctionary has the the key, that means we have to build a leaf
            if (dictionary.ContainsKey(theString))
            {
                charValue = dictionary[theString];
            }

            //If the char value is '\0' that means that we are not at a leaf
            if (charValue == '\0')
            {
                this.Left = new Node('\0');
                this.Right = new Node('\0');

                this.Right.buildTree(dictionary, theString + '0', maxLenght);
                this.Left.buildTree(dictionary, theString + '1', maxLenght);
            }

            //Else we are a leaf and we set the current char to the char corresponding to theString
            else
            {
                this.TheChar = charValue;
            }
        }
    }

    //The Tree class
    class Tree
    {

        //Variables
        private Node root;
        private Dictionary<String, char> dictionary;
        private int maxLenght;

        //The Tree constructor
        public Tree()
        {
            this.Root = null;
            this.Dictionary = new Dictionary<string, char>();
            this.MaxLenght = 0;
        }

        //Getters and Setters
        public Dictionary<string, char> Dictionary { get => dictionary; set => dictionary = value; }
        public int MaxLenght { get => maxLenght; set => maxLenght = value; }
        internal Node Root { get => root; set => root = value; }

        //Build the decoded tree from the decoded file
        public void buildTreeFromEncodedTextFile(StreamReader reader)
        {
            String line;

            while (true)
            {
                line = reader.ReadLine();

                //Here we have to cases, the one where we have printable characters and the one we do not have
                if (line.Length > 2)
                {
                    if (line[1] == '\\' && line[2] == 'n')
                    {
                        this.Dictionary.Add(Regex.Replace(line, "('.*' )", ""), '\n');
                    }
                    else
                    {
                        this.Dictionary.Add(Regex.Replace(line, "('.*' )", ""), line[1]);
                    }
                }
                else
                {
                    break;
                }
            }

            this.MaxLenght = this.getMaxLenghtOfTheDictionary();

            this.root = new Node('\0');

            this.root.buildTree(this.Dictionary, "", this.maxLenght);
        }

        //Get the tree from the file we want to encode
        public static Node getTreeFromFileToEncode(StreamWriter writer, String path)
        {
            Dictionary<char, int> myDictionary = new Dictionary<char, int>();

            String[] lines = File.ReadAllLines(path);

            int count = 0;

            foreach (String line in lines)
            {
                foreach (char myChar in line)
                {
                    if (myDictionary.ContainsKey(myChar))
                    {
                        myDictionary[myChar]++;
                    }
                    else
                    {
                        myDictionary[myChar] = 1;
                    }
                    count++;
                }

                if (myDictionary.ContainsKey('\n'))
                {
                    myDictionary['\n']++;
                }
                else
                {
                    myDictionary['\n'] = 1;
                }
            }

            Dictionary<char, int> sortedDictionary = new Dictionary<char, int>();
            var sortedDict = from entry in myDictionary orderby entry.Value ascending select entry;

            foreach (KeyValuePair<char, int> entry in sortedDict)
            {
                sortedDictionary[entry.Key] = myDictionary[entry.Key];
            }

            List<Node> nodes = new List<Node>();
            List<char> charList = new List<char>();
            Node node;

            foreach (KeyValuePair<char, int> entry in sortedDictionary)
            {
                node = new Node(entry.Key);
                node.Value = entry.Value;
                nodes.Add(node);
                charList.Add(entry.Key);
            }

            //The comparator object to sort my list
            GFG gg = new GFG();

            nodes.Sort(gg);

            // We are building the tree
            while (nodes.Count != 1)
            {
                node = new Node('\0');
                node.Left = nodes.First();
                nodes.Remove(nodes.First());
                node.Right = nodes.First();
                nodes.Remove(nodes.First());
                node.Value = node.Right.Value + node.Left.Value;

                // Add the new Node and sort the list
                nodes.Add(node);
                nodes.Sort(gg);
            }

            // We begin to write the data into the txt file, this data is all the char with their code
            foreach (Char theChar in charList)
            {
                if (theChar == '\n')
                {
                    writer.WriteLine("'" + "\\n" + "' " + new String(nodes.First().getPathFromNode(theChar).Reverse().ToArray()));
                }
                else
                {
                    writer.WriteLine("'" + theChar + "' " + new String(nodes.First().getPathFromNode(theChar).Reverse().ToArray()));
                }
            }

            //We write a line between the dictionary and the encoded text
            writer.WriteLine("");

            // Now we can return the root of the tree 
            return nodes.First();

        }

        // This method is used to write the encoded text into a txt file from an Huffman tree
        public static void encodeFile(StreamWriter writer, Node root, String path)
        {
            // We begin to write the data into the txt file, this data is all the char with their code
            String[] lines = File.ReadAllLines(path);

            foreach (String line in lines)
            {
                foreach (char myChar in line)
                {
                    writer.Write(new String(root.getPathFromNode(myChar).Reverse().ToArray()));
                }

                writer.Write(new String(root.getPathFromNode('\n').Reverse().ToArray()));
            }
        }

        //Decode the encoded file 'reader' using the huffman tree into the decoded file 'writer'
        public void decode(StreamWriter writer, StreamReader reader, String path)
        {
            while (!reader.EndOfStream)
            {
                while (this.Root.getNodeFromPath(path).TheChar == '\0')
                {
                    path += ((char)reader.Read());
                }

                writer.Write(this.Root.getNodeFromPath(path).TheChar);

                path = "";
            }
        }

        //Get the maximum lenght of the keys of the dictionary
        private int getMaxLenghtOfTheDictionary()
        {
            int maxLenght = 0;

            foreach (KeyValuePair<string, char> entry in this.Dictionary)
            {
                if (entry.Key.Length >= maxLenght)
                {
                    maxLenght = entry.Key.Length;
                }
            }

            return maxLenght;
        }
    }

    //The IComparer class used to compare two nodes (in term of their value)
    class GFG : IComparer<Node>
    {
        public int Compare(Node x, Node y)
        {
            //If one the nodes is null, they are considered as equal (this should not happen, but needed)
            if (x == null || y == null)
            {
                return 0;
            }

            //Else we compare the values of the two nodes (0 means the two nodes are equal, 1 means x is greater than y and -1 means x is less than y)

            if (x.Value < y.Value)
            {
                return -1;
            }

            if (x.Value > y.Value)
            {
                return 1;
            }

            return 0;
        }
    }
}