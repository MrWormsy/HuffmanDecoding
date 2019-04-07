using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace HuffmanDecoding {
    class Node {
        private int value;
        private Node left;
        private Node right;
        private String theChar;

        public Node(String theChar) {
            this.value = 0;
            this.left = null;
            this.right = null;
            this.theChar = theChar;
        }

        public int Value { get => value; set => this.value = value; }
        public string TheChar { get => theChar; set => theChar = value; }
        internal Node Left { get => left; set => left = value; }
        internal Node Right { get => right; set => right = value; }

        // Get the Node according to the path
        public Node getNodeFromPath(String path)
        {            
            if(path == "")
            {
                return this;
            }

            if(path[0] == '0')
            {
                if(this.Right == null)
                {
                    return null;
                }

                return this.Right.getNodeFromPath(path.Substring(1));
            } else
            {
                if (this.Left == null)
                {
                    return null;
                }

                return this.Left.getNodeFromPath(path.Substring(1));
            }
        }

        // Build the tree
        public void buildTree(Dictionary<string, string> dictionary, String theString, int maxLenght)
        {

            if (theString.Length > maxLenght)
            {
                return;
            }

            String charValue = "-1";

            if (dictionary.ContainsKey(theString))
            {
                charValue = dictionary[theString];
            }

            if (charValue == "-1")
            {
                this.Left = new Node("-1");
                this.Right = new Node("-1");

                this.Right.buildTree(dictionary, theString + '0', maxLenght);
                this.Left.buildTree(dictionary, theString + '1', maxLenght);

                return;

            } else
            {
                this.TheChar = charValue;

                return;
            }
        }

        /*

        public Node buildTree(Node theNode, Dictionary<string, string> dictionary, String theString, int maxLenght) {

            // If the length of the string is higher than the maxLenght we know that we need to stop the algorithm there
            if (theString.Length > maxLenght)
            {
                return null;
            }

            String charValue = "-1";

            // First we need to check if we are at a leaf, if we can gather the letter from the theString that is to say we are at a string

            if (dictionary.ContainsKey(theString))
            {
                charValue = dictionary[theString];
            }

            // If the charValue is different from -1 that is to say we are at a leaf and we can thus stop the algorithm for this node
            if (charValue != "-1")
            {
                Console.WriteLine(String.Concat(Enumerable.Repeat("-", theString.Length)) + charValue + " " + theString);
                return new Node(charValue);
            }
            else
            { 
                Console.WriteLine(String.Concat(Enumerable.Repeat("-", theString.Length)) + "* " + theString);
                theNode.Left = buildTree(new Node("-1"), dictionary, theString + "1", maxLenght);
                theNode.Right = buildTree(new Node("-1"), dictionary, theString + "0", maxLenght);

                return theNode;
            }

        }

        */

        public void display(int i)
        {
            if(this == null)
            {
                return;
            }

            Console.WriteLine(String.Concat(Enumerable.Repeat("-", i)) + this.theChar);

            if(this.Left != null)
            {
                this.Left.display(i++);
            }

            if(this.Right != null)
            {
                this.Right.display(i++);
            }

            
        }


    }

    class Tree {
        private Node root;
        private Dictionary<String, String> dictionary;
        private int maxLenght;
        private String encodedData;

        public Tree() {
            this.Root = null;
            this.Dictionary = new Dictionary<string, string>();
            this.MaxLenght = 0;
            this.EncodedData = "";
        }

        public Dictionary<string, string> Dictionary { get => dictionary; set => dictionary = value; }
        public int MaxLenght { get => maxLenght; set => maxLenght = value; }
        public string EncodedData { get => encodedData; set => encodedData = value; }
        internal Node Root { get => root; set => root = value; }

        public void buildTreeFromTextFile() {

            String path = "..\\..\\data.txt";

            String letter = "";

            //We build the dictionary from the data.txt file
            if (File.Exists(path)) {
                String[] lines = File.ReadAllLines(path);

                Boolean isEncodedData = false;

                foreach (String line in lines) {
              
                    if (isEncodedData)
                    {
                        this.EncodedData = line;
                        break;
                    }

                    // If we get a blank that is to say this is the end of the dictionary part
                    if (line.Length <= 2) { isEncodedData = true; }

                    if(!isEncodedData) {

                        letter = line.Split(' ')[0];

                        if (letter == "'")
                        {
                            this.Dictionary[line.Split(' ')[2]] = " ";
                        } else
                        {
                            this.Dictionary[line.Split(' ')[1]] = Regex.Replace(letter, "[']", "");
                        }

                        
                    }
                }
            }

            this.MaxLenght = this.getMaxLenghtOfTheDictionary();

            String theString = "";

            this.root = new Node("-1");

            this.root.buildTree(this.Dictionary, theString, maxLenght);
        }

        /*

        internal void decode(Node node)
        {

            // If encodedData is empty we have finished to decode the code
            if (this.EncodedData == "" || node == null)
            {
                return;
            }

            if (node.TheChar != "-1")
            {
                Console.WriteLine(node.TheChar + " " + this.EncodedData);
                this.EncodedData = this.EncodedData.Substring(1);
                this.decode(this.Root);
            }

            // If the first character is a 1 that is to say we need to go to the right else to the left
            if (this.EncodedData[0] == '0')
            {
                //We remove the first character
                this.EncodedData = this.EncodedData.Substring(1);

                this.decode(node.Right);
            }
            else
            {
                //We remove the first character
                this.EncodedData = this.EncodedData.Substring(1);

                this.decode(node.Left);
            }
        }


        */

        public void decode()
        {

            if (this.EncodedData == "")
            {
                return;
            }

            String path = "";

            while (this.Root.getNodeFromPath(path).TheChar == "-1")
            {
                path += this.EncodedData[path.Length];
            }

            Console.Write(this.Root.getNodeFromPath(path).TheChar);

            this.EncodedData = EncodedData.Substring(path.Length);

            this.decode();

        }

        private int getMaxLenghtOfTheDictionary() {

            int maxLenght = 0;

            foreach (KeyValuePair<string, string> entry in this.Dictionary)
            {
                if(entry.Key.Length >= maxLenght) {
                    maxLenght = entry.Key.Length;
                }
            }

            return maxLenght;
        }
    }
}
