using System;
using System.IO;


namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Argument Error");
                return;
            }

            int wordCount = 0;

            string textFile = args[0];
            try
            {
                using (StreamReader sr = new StreamReader(textFile))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string newLine = line.Replace('\t', ' ');
                        newLine = newLine.Replace('\n', ' ');

                        foreach (string word in newLine.Split(' '))
                        {
                            string wordWithoutNonLetters = word.Trim();

                            if (wordWithoutNonLetters.Length == 0)
                            {
                                continue;
                            }

                            wordCount++;
                            //Console.WriteLine(wordWithoutNonLetters);

                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine("File Error");
                return;
            }

            Console.WriteLine(wordCount);

        }
    }
}