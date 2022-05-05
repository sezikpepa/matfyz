using System;
using System.Collections.Generic;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {

        class InfixCalculator
        {
            public Stack<int> numbers;
            public Stack<String> operands;
            public InfixCalculator()
            {
                this.numbers = new Stack<int>();
                this.operands = new Stack<String>();
            }

            public void input(String input)
            {
                if (input == "+" || input == "-" || input == "*" || input == "/")
                {
                    this.operands.Push(input);
                    return;
                }

                int number = Int32.Parse(input);
                this.numbers.Push(number);
                this.expressionSimplify();

            }

            private void expressionSimplify()
            {
                if (this.numbers.Count >= 2 && this.operands.Count >= 1)
                {
                    if (this.operands.Peek() == "*" || this.operands.Peek() == "/")
                    {
                        int secondNumber = numbers.Pop();
                        int firstNumber = numbers.Pop();

                        String operand = operands.Pop();

                        if (operand == "*")
                        {
                            this.numbers.Push(firstNumber * secondNumber);
                            return;
                        }
                        this.numbers.Push(firstNumber / secondNumber);

                    }

                    else if (this.numbers.Count >= 3)
                    {
                        int numberForReturn = this.numbers.Pop();
                        int secondNumber = this.numbers.Pop();
                        int firstNumber = this.numbers.Pop();

                        String operandForReturn = this.operands.Pop();
                        String operand = this.operands.Pop();

                        this.operands.Push(operandForReturn);

                        if (operand == "+")
                        {
                            this.numbers.Push(firstNumber + secondNumber);
                            this.numbers.Push(numberForReturn);
                            return;
                        }
                        this.numbers.Push(firstNumber - secondNumber);
                        this.numbers.Push(numberForReturn);
                    }
                }

            }

            public void printResult()
            {
                if (this.operands.Count == 0)
                {
                    Console.WriteLine(numbers.Pop());
                    return;
                }


                int secondNumber = this.numbers.Pop();
                int firstNumber = this.numbers.Pop();

                String operand = this.operands.Pop();

                switch (operand)
                {
                    case "+":
                        Console.WriteLine(firstNumber + secondNumber);
                        break;
                    case "-":
                        Console.WriteLine(firstNumber - secondNumber);
                        break;
                    case "*":
                        Console.WriteLine(firstNumber * secondNumber);
                        break;
                    case "/":
                        Console.WriteLine(firstNumber / secondNumber);
                        break;
                }
                return;
            }
        }


        static void Main(string[] args)
        {
            String line;

            InfixCalculator calculator = new InfixCalculator();

            while (true)
            {
                line = Console.ReadLine();

                if (line == "=")
                {
                    calculator.printResult();
                    break;
                }

                calculator.input(line);

                
            }
        }
    }
}
