using System;

namespace Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start Calculator...");
            Console.Write("Input: ");
            String input = Console.ReadLine();

            checkInput(input);

            Calculator calculator = new Calculator(input);
            Console.WriteLine("Result: " + calculator.Calc());
        }

        private static void checkInput(String input)
        {
            int openBrackets = 0;
            int closingBrackets = 0;

            foreach(Char element in input)
            {
                if (element == '(')
                {
                    openBrackets++;
                }

                if (element == ')')
                {
                    closingBrackets++;
                }
            }

            if (openBrackets != closingBrackets)
            {
                Console.WriteLine("ERROR: Open and closing brackets must be the same amount!");
                System.Environment.Exit(1);
            }
        }
    }
}
