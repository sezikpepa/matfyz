using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

namespace MyApp // Note: actual namespace depends on the project name.
{
    class HuffmanNode
    {
        public string text;
        public long value;
        public HuffmanNode leftChild { get; set; }
        public HuffmanNode rightChild { get; set; }

        public HuffmanNode(string text, long value)
        {
            this.text = text;
            this.value = value;
        }
        public void setLeftChild(HuffmanNode node)
        {
            this.leftChild = node;
        }

        public void setRightChild(HuffmanNode node)
        {
            this.rightChild = node;
        }
    }

    class HuffmanTree
    {
        public HuffmanNode root { get; set; }

        public HuffmanTree(HuffmanNode root)
        {
            this.root = root;
        }

        public static bool operator <(HuffmanTree tree1, HuffmanTree tree2)
        {
            if (tree1.root.value < tree2.root.value)
            {
                return true;
            }
            else if (tree1.root.value > tree2.root.value)
            {
                return false;
            }

            else
            {
                if (tree1.root.text != "" && tree2.root.text != "")
                {
                    if (Int32.Parse(tree1.root.text) < Int32.Parse(tree2.root.text))
                        return true;
                    return false;
                }

                else if (tree1.root.text == "" && tree2.root.text == "")
                {
                    return true;
                }

                else
                {
                    if (tree1.root.text != "")
                        return true;
                    return false;
                }
            }
        }

        public static bool operator >(HuffmanTree tree1, HuffmanTree tree2)
        {
            throw new NotImplementedException();
            //not needed
        }

        public void printTree()
        {
            if (root == null)
            {
                return;
            }

            if (root.text == "")
            {
                Console.Write(this.root.value);
                Console.Write(" ");

                HuffmanTree tree1 = new HuffmanTree(this.root.leftChild);
                HuffmanTree tree2 = new HuffmanTree(this.root.rightChild);

                tree1.printTree();
                tree2.printTree();
            }
            else
            {
                Console.Write("*" + this.root.text + ":" + this.root.value);
                Console.Write(" ");
            }

        }
    }


    class HuffmanForest
    {
        public List<HuffmanTree> trees { get; set; }

        public HuffmanForest()
        {
            this.trees = new List<HuffmanTree>();
        }

        public void addTree(HuffmanTree tree)
        {
            this.trees.Add(tree);
        }

        private void bubbleSortIteration()
        {
            for (int i = 0; i < this.trees.Count - 1; i++)
            {
                if (!(this.trees[i + 1] < this.trees[i]))
                {
                    HuffmanTree tree = this.trees[i];

                    this.trees[i] = this.trees[i + 1];
                    this.trees[i + 1] = tree;
                }
            }
        }

        private void mergeTwoLowestTrees()
        {
            HuffmanTree tree1 = this.trees[^1];
            HuffmanTree tree2 = this.trees[^2];

            this.trees.Remove(tree1);
            this.trees.Remove(tree2);

            HuffmanTree mergedTree = new HuffmanTree(new HuffmanNode("", tree1.root.value + tree2.root.value));
            mergedTree.root.setLeftChild(tree1.root);
            mergedTree.root.setRightChild(tree2.root);

            this.trees.Insert(0, mergedTree);
            this.bubbleSortIteration();
        }

        private void BubbleTwoLowest()
        {
            this.bubbleSortIteration();
            this.bubbleSortIteration();
        }

        public void mergeTrees()
        {
            while (this.trees.Count > 1)
            {
                this.BubbleTwoLowest();
                this.mergeTwoLowestTrees();
            }
        }
    }

    class StatisticMaker
    {
        private FileStream reader;
        public Dictionary<int, int> statics;

        public StatisticMaker(string fileName)
        {
            this.reader = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            this.statics = new();
        }

        public bool ProcessFile()
        {
            int part;
            try
            {
                while (true)
                {
                    part = reader.ReadByte();
                    if (part == -1)
                        break;

                    this.processPart(part);
                }
                return true;
            }
            catch
            {
                return false;
            }

        }

        private void processPart(int part)
        {
            if (this.statics.ContainsKey(part))
            {
                this.statics[part]++;
            }
            else
            {
                this.statics.Add(part, 1);
            }
        }
    }

    class HuffmanEncoder
    {
        public HuffmanTree huffmanTree;
        private HuffmanNode currentNode;
        public List<long> encodedNodes = new List<long>();
        public HuffmanEncoder(HuffmanTree tree)
        {
            this.huffmanTree = tree;
            this.currentNode = tree.root;
        }

        public void encodeSubTree(HuffmanNode root)
        {
            this.encodedNodes.Add(this.encodeNode(root));

            if (root.leftChild != null)
                this.encodeSubTree(root.leftChild);
            if (root.rightChild != null)
                this.encodeSubTree(root.rightChild);
        }

        private long encodeNode(HuffmanNode node)
        {
            long result;

            if (node.text == "") //mělo by fungovat dobře
            {
                result = 0;
                result = result << 56;

                //váha uzlu
                long value = node.value;
                value = value << 1;

                //vnitřní uzel
                long type = 0;

                //merge
                result = result | value;
                result = result | type;
            }


            else
            {
                result = Convert.ToInt32(node.text);

                result = result << 56;

                //váha uzlu
                long value = node.value;
                value = value << 1;

                //vnitřní uzel
                long type = 1;

                //merge
                result = result | value;
                result = result | type;
            }

            return result;
        }


    }

    class HuffmanWriter
    {
        private string fileName;
        private BinaryWriter writer;
        private byte[] header = { 0x7B, 0x68, 0x75, 0x7C, 0x6D, 0x7D, 0x66, 0x66 };
        private byte[] headerEnd = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        private HuffmanEncoder huffmanEncoder;
        private Dictionary<int, List<bool>> hashmap;
        private FileStream reader;
        private ByteParser byteParser = new();

        public HuffmanWriter(string fileName, HuffmanTree tree, Dictionary<int, List<bool>> hashmap)
        {
            this.fileName = fileName;
            this.reader = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            this.writer = new BinaryWriter(new FileStream(fileName + ".huff", FileMode.Create));
            this.huffmanEncoder = new(tree);
            this.hashmap = hashmap;
        }

        public void WriteEncodedFile()
        {
            using (this.writer)
            {
                //writes the data to the stream
                //this.writer.Write(this.header);
                //Console.WriteLine("Successfully Written");

                this.writer.Write(header);

                this.huffmanEncoder.encodeSubTree(this.huffmanEncoder.huffmanTree.root);
                foreach (var toWrite in this.huffmanEncoder.encodedNodes)
                {
                    this.writer.Write(toWrite);
                }
                this.writer.Write(this.headerEnd);
                //start with file encoding
                int nextByte;
                List<bool> result = new List<bool>();
                while (true)
                {
                    nextByte = this.reader.ReadByte();
                    //Console.WriteLine(nextByte);
                    if (nextByte == -1)
                    {
                        break;
                    }

                    result = this.hashmap[nextByte];
                    //Console.WriteLine(result);
                    this.byteParser.input(result);

                    while (this.byteParser.createdBytes.Count > 0)
                    {
                        //Console.WriteLine("byte:" + this.byteParser.createdBytes[0]);
                        this.writer.Write(this.byteParser.createdBytes[0]);
                        this.byteParser.createdBytes.RemoveAt(0);
                    }
                }
                bool yes = this.byteParser.finishByte();

                if (yes)
                {
                    this.byteParser.checkForFullByte();
                    this.writer.Write(this.byteParser.createdBytes[0]);
                }


            }

        }
    }

    class ByteParser
    {
        public List<byte> createdBytes = new();
        private bool[] currentValue = new bool[8];
        public int stackSize = 0;

        public ByteParser()
        {

        }

        public void checkForFullByte()
        {
            int result = 0;
            int counter = 0;

            foreach (var value in this.currentValue)
            {
                //Console.WriteLine(value);
                if (value == true)
                {
                    int shiftValue = 1;
                    shiftValue = shiftValue << counter;
                    result = result | shiftValue;
                    //Console.Write(1);
                }
                else
                {
                    //Console.Write(0);
                }
                counter++;
            }
            //Console.WriteLine();

            this.createdBytes.Add(Convert.ToByte(result));


            //this.currentValue.Clear();
            this.stackSize = 0;

        }

        public bool finishByte()
        {
            if (this.stackSize == 0)
                return false;
            while (this.stackSize != 8)
            {
                this.currentValue[this.stackSize] = false;
                this.stackSize++;
            }
            return true;
        }

        public void input(List<bool> values)
        {
            foreach (var value in values)
            {
                //Console.Write(value);
                this.currentValue[this.stackSize] = value;
                this.stackSize++;
                if(this.stackSize == 8)
                    this.checkForFullByte();
            }
            //Console.WriteLine();
        }

    }

    class HuffmanHashMapCreator
    {
        public HuffmanTree tree;
        public Dictionary<int, List<bool>> hashmap;
        public HuffmanHashMapCreator(HuffmanTree tree)
        {
            this.tree = tree;
            this.hashmap = new();
        }

        public void exploreTree(HuffmanNode node, List<bool> pathToNode)
        {
            if (node != null)
            {
                if (node.text != "")
                {
                    this.hashmap[Int32.Parse(node.text)] = pathToNode;
                    //Console.WriteLine("done");
                }
                else
                {
                    List<bool> pathToNodeLeft = new List<bool>(pathToNode);
                    pathToNodeLeft.Add(false);

                    List<bool> pathToNodeRight = new List<bool>(pathToNode);
                    pathToNodeRight.Add(true);

                    this.exploreTree(node.leftChild, pathToNodeLeft);
                    this.exploreTree(node.rightChild, pathToNodeRight);
                }

            }
        }
    }


    internal class Program
    {
        static void Main(string[] args)
        {
            //Stopwatch sw = new Stopwatch();

            //sw.Start();
            if (args.Length != 1)
            {
                Console.WriteLine("Argument Error");
                return;
            }
            string inputFile = args[0];

            StatisticMaker statisticMaker;
            try
            {
                statisticMaker = new(inputFile);
                bool result = statisticMaker.ProcessFile();
                if (result == false)
                {
                    Console.WriteLine("File Error");
                    return;
                }
            }
            catch
            {
                Console.WriteLine("File Error");
                return;
            }


            HuffmanForest huffmanForest = new HuffmanForest();

            if (statisticMaker.statics.Count == 0)
            {
                return;
            }

            foreach (var element in statisticMaker.statics)
            {
                huffmanForest.addTree(new HuffmanTree(new HuffmanNode(element.Key.ToString(), element.Value)));
            }


            huffmanForest.mergeTrees();

            //hashmap, letter values
            HuffmanHashMapCreator hashmapCreator = new(huffmanForest.trees[0]);
            hashmapCreator.exploreTree(huffmanForest.trees[0].root, new List<bool>());

            HuffmanWriter huffmanWriter = new HuffmanWriter(inputFile, huffmanForest.trees[0], hashmapCreator.hashmap);

            huffmanWriter.WriteEncodedFile();

            //sw.Stop();

            //Console.WriteLine("Elapsed={0}", sw.Elapsed);

        }
    }
}