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

            public void Input(String input)
            {
                if (input == "+" || input == "-" || input == "*" || input == "/")
                {
                    this.operands.Push(input);
                    return;
                }
                if(input == "=")               
                    return;
                
                int number = Int32.Parse(input);
                this.numbers.Push(number);
                this.ExpressionSimplify();

            }

            private void ExpressionSimplify()
            {
                if (this.numbers.Count >= 2 && this.operands.Count >= 1)
                {
                    if (this.operands.Peek() == "*" || this.operands.Peek() == "/")
                    {
                        int secondNumber = numbers.Pop();
                        int firstNumber = numbers.Pop();

                        String operand = operands.Pop();

                        this.numbers.Push(this.CalculateTwoNumbers(firstNumber, secondNumber, operand));
                    }

                    else if (this.numbers.Count >= 3)
                    {
                        int numberForReturn = this.numbers.Pop();
                        int secondNumber = this.numbers.Pop();
                        int firstNumber = this.numbers.Pop();

                        String operandForReturn = this.operands.Pop();
                        String operand = this.operands.Pop();

                        this.operands.Push(operandForReturn);

                        this.numbers.Push(this.CalculateTwoNumbers(firstNumber, secondNumber, operand));
                        this.numbers.Push(numberForReturn);
                    }
                }

            }

            private int CalculateTwoNumbers(int firstNumber, int secondNumber, string operand)
            {
                switch (operand)
                {
                    case "+":
                        return firstNumber + secondNumber;
                    case "-":
                        return firstNumber - secondNumber;
                    case "*":
                        return firstNumber * secondNumber;
                    case "/":
                        return firstNumber / secondNumber;
                    default:
                        break;
                }
                return 0;
            }

            public void PrintResult()
            {
                if (this.operands.Count == 0)
                {
                    Console.WriteLine(numbers.Pop());
                    return;
                }
                int secondNumber = this.numbers.Pop();
                int firstNumber = this.numbers.Pop();

                String operand = this.operands.Pop();

                Console.WriteLine(this.CalculateTwoNumbers(firstNumber, secondNumber, operand));
            }
        }


        static void Main()
        {
            String line = "";

            InfixCalculator calculator = new();

            while(line != "=")
            {
                line = Console.ReadLine();
                calculator.Input(line);               
            }
            calculator.PrintResult();
        }
    }
}
