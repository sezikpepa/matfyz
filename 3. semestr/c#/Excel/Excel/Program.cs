using System;
using System.Collections.Generic;
using System.IO;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {

        class Cell
        {
            public string text;
            public bool finalValue;

            public int operativeValue;

            public string error = "";

            public string[] formula = new string[3];

            public int firstDependencyRowIndex;
            public int secondDependencyRowIndex;
            public int firstDependencyColumnIndex;
            public int secondDependencyColumnIndex;

            public Cell(string text)
            {
                this.text = text;

                try
                {
                    int result = Int32.Parse(text);
                    this.finalValue = true;
                    this.operativeValue = result;
                }
                catch
                {
                    this.ParseInput(text);                    
                }
            }

            private void ParseInput(string text)
            {
                if(text == "[]")
                {
                    this.finalValue = true;
                    this.operativeValue = 0;
                    return;
                }

                if(text[0] != '=')// empty string solution not included
                {
                    this.finalValue = true;
                    this.error = "#INVVAL";
                    this.text = "#INVVAL";
                    return;
                }

                string toEvaluate = text.Remove(0, 1);
                this.ParsePotentionalFormula(toEvaluate);
            }

            private void ParsePotentionalFormula(string text)
            {
                string firstPart = "";
                string secondPart = "";
                string operation = "";
                int indexToContinue = text.Length;
                for(int i = 0; i < text.Length; i++)
                {
                    if(text[i] != '+' && text[i] != '-' && text[i] != '*' && text[i] != '/')
                    {
                        firstPart += text[i].ToString();
                    }
                    else
                    {
                        indexToContinue = i + 1;
                        operation = text[i].ToString();
                        break;
                    }                
                }

                for(int i = indexToContinue; i < text.Length; i++)
                {
                    secondPart += text[i].ToString();
                }

                this.formula[0] = firstPart;
                this.formula[1] = operation;
                this.formula[2] = secondPart;

                this.checkFormulaValidity();
            }

            private void checkFormulaValidity()
            {
                if(this.formula[1] != "+" && this.formula[1] != "-" && this.formula[1] != "*" && this.formula[1] != "/")
                {
                    this.error = "#MISSOP";
                    this.finalValue = true;
                    this.text = "#MISSOP";
                    return;
                }
            }

            public void evaluateCell()
            {

            }
        }

        class ExcelTable
        {
            private List<List<Cell>> table = new();


            private List<Cell> row = new();

            private List<Cell> evalutionQueue = new();

            public ExcelTable()
            {

            }

            public void input(string value)
            {
                this.row.Add(new Cell(value));
            }

            public void newRow()
            {
                this.table.Add(this.row);
                //Console.WriteLine(this.table[0][0].text);
                this.row = new();
            }

            public int getColumnIndex(string columnName)
            {
                int current26power = 1;
                int result = 0;
                
                foreach(var letter in columnName.Reverse())
                {
                    Console.WriteLine(letter);
                    current26power *= 26;
                }
                
                return result;
            }

            private int getRowIndex(int rowNumber)
            {
                return rowNumber - 1;
            }

            public void evaluateTable()
            {
                bool run = true;
                while(run)
                {
                    run = false;
                    for (int i = 0; i < this.table.Count; i++)
                    {
                        for (int j = 0; j < this.table[i].Count; j++)
                        {
                            if (this.table[i][j].finalValue == false)
                            {
                                run = true;
                                this.table[i][j].operativeValue = this.evaluateBox(this.table[i][j]);
                                this.table[i][j].finalValue = true;
                                this.table[i][j].text = this.table[i][j].operativeValue.ToString();
                            }
                        }
                    }
                }          
            }

            private int evaluateBox(Cell cell)
            {
                string box1 = cell.formula[0];
                string operation = cell.formula[1];
                string box2 = cell.formula[2];

                int[] box1Indexes = this.getBoxIndexes(box1);
                int[] box2Indexes = this.getBoxIndexes(box2);

                int firstNumber = this.table[box1Indexes[0]][box1Indexes[1]].operativeValue;
                int secondNumber = this.table[box2Indexes[0]][box2Indexes[1]].operativeValue;

                if(operation == "+")
                    return firstNumber + secondNumber;
                else if(operation == "-")
                    return firstNumber - secondNumber;
                else if (operation == "*")
                    return firstNumber * secondNumber;
                else if(operation == "/")
                    return firstNumber / secondNumber;
                return 0;
            }

            private int[] getBoxIndexes(string text)
            {
                string column = "";
                string row = "";
                bool numberFound = false;
                foreach(var character in text)
                {
                    if ("QWERTZUIOPLKJHGFDSAYXCVBNM".Contains(character) == true && numberFound == false)
                    {
                        column += character;
                    }
                    else if ("0123456789".Contains(character) == true)
                    {
                        numberFound = true;
                        row += character;
                    }
                    else
                    {
                        Console.WriteLine("chyba, vyřešit pak");
                    }
                }
                int[] toReturn = { Int32.Parse(row) - 1, this.getColumnIndex(column) };
                return toReturn;
            }

            public void printTable()
            {
                //Console.WriteLine("test");
                foreach(var row in this.table)
                {
                    //Console.WriteLine("radka");
                    //Console.WriteLine(row.Count());
                    for(int i = 0; i < row.Count - 1; i++)
                    {
                        //Console.WriteLine("sloupec");
                        Console.Write(row[i].text);
                        Console.Write(" ");
                    }
                    Console.Write(row[row.Count - 1].text);
                    Console.WriteLine();
                }
            }
        }

        class FileReader
        {
            private string fileName;

            public FileReader(string fileName)
            {
                this.fileName = fileName;
            }
        }

        class InputController
        {
            public ExcelTable table = new();
            private int currentRowIndex = 0;
            private int currentColumnIndex = 0;
            public InputController()
            {

            }

            public void input(string line)
            {
                string[] parts = line.Split(' ');
                foreach(var part in parts)
                {
                    //Console.WriteLine(part);
                    if (part != "")
                        this.table.input(part);
                }
                this.table.newRow();
            }
        }




        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Argument Error");
                return;
            }

            string inputFile = args[0];
            string outputFile = args[1];

            InputController inputController = new InputController();
            FileStream fileReader = new FileStream(inputFile, FileMode.Open, FileAccess.Read);


            using (var fileStream = File.OpenRead(inputFile))
            {
                using (var streamReader = new StreamReader(fileStream, System.Text.Encoding.UTF8, true, 128))
                {
                    String line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        inputController.input(line);
                    }
                }
            }

            //inputController.table.evaluateTable();

            inputController.table.printTable();
        }
    }
}