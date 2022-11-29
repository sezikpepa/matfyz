using System;
using System.IO;
using System.Collections.Generic;


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

            SortedDictionary<string, int> wordFrequency = new SortedDictionary<string, int>();

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

                            if (wordFrequency.ContainsKey(wordWithoutNonLetters)){
                                wordFrequency[wordWithoutNonLetters]++;
                            }
                            else
                            {
                                wordFrequency[wordWithoutNonLetters] = 1;
                            }

                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine("File Error");
                return;
            }

            foreach(var element in wordFrequency)
            {
                Console.WriteLine(element.Key + ": " + element.Value);
            }

        }
    }
}