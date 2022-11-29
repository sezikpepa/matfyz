using System;
using System.Collections.Generic;
using System.IO;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void printOutputFile(string outputFile, string text, bool newLine = false)
        {
            using (StreamWriter writetext = new StreamWriter(outputFile, true))
            {
                writetext.Write(text);
                if (newLine == true)
                {
                    writetext.WriteLine();
                }
            }
        }

        class RowMaker
        {
            public List<string> words;
            public int spaceLength;
            public int currentLength;
            public int maxLength;
            public int remainingExtendedSpace;
            public string outputFile;
            public string spaceString;
            public string lineEndString;
            public bool somethingWritten = false;
            public bool highlightSpaces;
            public RowMaker(int maxLength, string outputFile, bool highlightSpaces=false)
            {
                this.words = new List<string>();
                this.maxLength = maxLength;
                this.currentLength = 0;
                this.spaceLength = 0;
                this.remainingExtendedSpace = 0;
                this.outputFile = outputFile;
                this.spaceString = " ";
                this.lineEndString = "";
                this.highlightSpaces = highlightSpaces;
                if (highlightSpaces == true)
                {
                    this.spaceString = ".";
                    this.lineEndString = "<-";
                }
            }

            public void WordInput(string word)
            {
                if (this.currentLength + word.Length > this.maxLength)
                {
                    this.printLine();
                    this.reset();
                    this.AddWord(word);
                    return;
                }

                this.AddWord(word);

            }

            private void AddWord(string word)
            {
                this.words.Add(word);
                this.currentLength += word.Length + 1;
            }

            public void CalculateSpaceLength()
            {
                this.currentLength--;

                int textLength = 0;
                foreach (string word in this.words)
                {
                    textLength += word.Length;
                }
                if (this.words.Count == 1)
                    this.spaceLength = 0;
                else
                    this.spaceLength = (this.maxLength - textLength) / (this.words.Count - 1);

                this.remainingExtendedSpace = this.maxLength;
                foreach (string word in this.words)
                {
                    this.remainingExtendedSpace -= word.Length;
                    this.remainingExtendedSpace -= this.spaceLength;
                }
                this.remainingExtendedSpace += this.spaceLength;

            }

            public void printLine()
            {
                
                this.CalculateSpaceLength();
                if (this.words.Count == 0)
                    return;
                this.somethingWritten = true;
                string toPrint = "";

                for (int i = 0; i < this.words.Count - 1; i++)
                {

                    toPrint = toPrint + this.words[i];
                    for (int j = 0; j < this.spaceLength; j++)
                    {
                        toPrint = toPrint + this.spaceString;
                    }

                    if (this.remainingExtendedSpace > 0)
                    {

                        toPrint = toPrint + this.spaceString;
                        this.remainingExtendedSpace--;
                    }
                }

                toPrint = toPrint + this.words[^1];
                toPrint = toPrint + this.lineEndString;
                printOutputFile(this.outputFile, toPrint, true);

            }

            public void PrintRest()
            {              
                if (this.words.Count == 0)
                    return;
                this.somethingWritten = true;
                for (int i = 0; i < this.words.Count - 1; i++)
                {
                    printOutputFile(this.outputFile, this.words[i]);
                    printOutputFile(this.outputFile, this.spaceString);
                }
                printOutputFile(this.outputFile, this.words[^1] + this.lineEndString);
            }

            public void reset()
            {
                this.words.Clear();
                this.spaceLength = 0;
                this.currentLength = 0;
                this.remainingExtendedSpace = 0;
            }
        }


        static void Main(string[] args)
        {
            int fileStartIndex = 0;

            if (args.Length < 3)
            {
                Console.WriteLine("Argument Error");
                return;
            }

            int maxRowLenght;
            //string inputTextFile;// = args[0];
            string outputTextFile = args[^2];
            RowMaker rowMaker;

            

            try
            {
                maxRowLenght = int.Parse(args[^1]);
            }
            catch
            {
                Console.WriteLine("Argument Error");
                return;
            }

            if (maxRowLenght <= 0)
            {
                Console.WriteLine("Argument Error");
            }

            if (args[0] == "--highlight-spaces")
            {
                fileStartIndex = 1;
                rowMaker = new RowMaker(maxRowLenght, outputTextFile, true);
                if (args.Length < 4)
                {
                    Console.WriteLine("Argument Error");
                    return;
                }
            }
            else
            {
                rowMaker = new RowMaker(maxRowLenght, outputTextFile, false);
            }

            int nextChar;
            
            string currentWord = "";
            int rowEndsFound = 0;

            for(int fileIndex = fileStartIndex; fileIndex < args.Length - 2; fileIndex++)
            {
                try
                {
                    using (StreamReader sr = new StreamReader(args[fileIndex]))
                    {

                        while ((nextChar = sr.Read()) != -1)
                        {
                            char characterNextChar = ((char)nextChar);
                            if (characterNextChar == '\r')
                                continue;

                            if (characterNextChar != ' ' && characterNextChar != '\n' && characterNextChar != '\t')
                            {
                                if (rowEndsFound >= 2)
                                {
                                    rowMaker.PrintRest();
                                    rowMaker.reset();
                                    printOutputFile(outputTextFile, "", true);
                                    printOutputFile(outputTextFile, "", true);

                                }
                                rowEndsFound = 0;
                                currentWord += ((char)nextChar);
                            }

                            else
                            {
                                if (characterNextChar == '\n')
                                {
                                    rowEndsFound++;
                                }
                                if (currentWord != "")
                                    rowMaker.WordInput(currentWord);
                                currentWord = "";
                            }
                        }

                    }
                    if(nextChar == -1)
                    {
                        if (currentWord != "")
                            rowMaker.WordInput(currentWord);

                        currentWord = "";
                    }
                    
                    

                    

                }
                catch (IOException)
                {
                    //Console.WriteLine("File Error");
                    //return;
                }

                

            }

            if (currentWord != "")
                rowMaker.WordInput(currentWord);

            rowMaker.PrintRest();

            if (rowMaker.somethingWritten == false && rowMaker.highlightSpaces == true)
                printOutputFile(outputTextFile, "<-", true);
            else
            {
                printOutputFile(outputTextFile, "", true);
            }


            

        }
    }
}
