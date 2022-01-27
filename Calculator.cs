using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace Calculator
{
    public class Calculator
    {
        String formula;

        ArrayList operators = new ArrayList();
        ArrayList operands = new ArrayList();

        public Calculator(String input)
        {
            formula = input.Replace(" ", "");

            //Split input string into operands and operators
            operators.AddRange(RemoveEmptyEntries(new ArrayList(Regex.Split(formula, ""))));
            operators = RemoveNonOperators(operators);

            operands.AddRange(RemoveEmptyEntries(new ArrayList(Regex.Split(formula, "[()+/*-]"))));
        }

        /// <summary>
        /// Calculates the result of the given input formula
        /// </summary>
        /// <returns>Result in double</returns>
        public double Calc()
        {
            return Calc(operators, operands);
        }

        /// <summary>
        /// Calculates the result of the forumula from 2 lists
        /// </summary>
        /// <param name="operators"></param>
        /// <param name="operands"></param>
        /// <returns>Result in double</returns>
        public double Calc(ArrayList operators, ArrayList operands)
        {
            //Hadle brackets
            if (operators.Contains("("))
            {
                //find bracket pair
                int openIndex = operators.IndexOf("(");
                int closeIndex = operators.LastIndexOf(")");

                //calc recursiv bracket and replace bracket pair with result
                ArrayList subOperators = operators.GetRange(openIndex + 1, closeIndex - 1);
                int remainigBracketsInSub = countBrackets(subOperators);

                ArrayList subOperands = operands.GetRange(openIndex, closeIndex - remainigBracketsInSub);
                Calc(subOperators, subOperands);

                operators.RemoveAt(operators.IndexOf("("));
                operators.RemoveAt(operators.LastIndexOf(")"));
            }

            //Check if operators cointains multiplication
            if (operators.Contains("*"))
            {
                int index = operators.IndexOf("*");
                if (index != -1)
                {
                    double firstOperand;
                    double secondOperand;
                    HandlerUserInputs(operands, index, out firstOperand, out secondOperand);

                    //Calculate result and replace/remove operands with result
                    double result = firstOperand * secondOperand;
                    operands[index] = result.ToString();
                    operands.RemoveAt(index + 1);
                    operators.RemoveAt(index);

                    //Check if only final result is in list, if not recusive call Calc()
                    if (operands.Count > 1)
                    {
                        Calc(operators, operands);
                    }
                }
            }

            //Check if operators cointains division
            if (operators.Contains("/"))
            {
                int index = operators.IndexOf("/");
                if (index != -1)
                {
                    double firstOperand;
                    double secondOperand;
                    HandlerUserInputs(operands, index, out firstOperand, out secondOperand);

                    //Calculate result and replace/remove operands with result
                    double result = firstOperand / secondOperand;
                    operands[index] = result.ToString();
                    operands.RemoveAt(index + 1);
                    operators.RemoveAt(index);

                    //Check if only final result is in list, if not recusive call Calc()
                    if (operands.Count > 1)
                    {
                        Calc(operators, operands);
                    }
                }
            }

            //Calculate all remaining parts in order from left to right
            while (operands.Count > 1)
            {
                String nextOperator = (string)operators[0];

                //Check if next operator is addition
                if (nextOperator == "+")
                {
                    double firstOperand;
                    double secondOperand;
                    HandlerUserInputs(operands, 0, out firstOperand, out secondOperand);

                    //Calculate result and replace/remove operands with result
                    double result = firstOperand + secondOperand;
                    operands[0] = result.ToString();
                    operands.RemoveAt(1);
                    operators.RemoveAt(0);

                    continue;
                }

                //Check if next operator if subtraction
                if (nextOperator == "-")
                {
                    double firstOperand;
                    double secondOperand;
                    HandlerUserInputs(operands, 0, out firstOperand, out secondOperand);

                    //Calculate result and replace/remove operands with result
                    double result = firstOperand - secondOperand;
                    operands[0] = result.ToString();
                    operands.RemoveAt(1);
                    operators.RemoveAt(0);
                }
            }

            return double.Parse((string)operands[0]);
        }

        /// <summary>
        /// Remove empty list entries
        /// </summary>
        /// <param name="list"></param>
        /// <returns>Cleared list</returns>
        private ArrayList RemoveEmptyEntries(ArrayList list)
        {
            ArrayList converted = new ArrayList();
            foreach (String element in list)
            {
                if (element != "")
                {
                    converted.Add(element);
                }
            }

            return converted;
        }

        /// <summary>
        /// Add all operators to a list and return it
        /// </summary>
        /// <param name="list"></param>
        /// <returns>List containing only operators</returns>
        private ArrayList RemoveNonOperators(ArrayList list)
        {
            ArrayList converted = new ArrayList();
            foreach (String element in list)
            {
                if (Regex.IsMatch(element, "[()+*/-]"))
                {
                    converted.Add(element);
                }
            }

            return converted;
        }

        /// <summary>
        /// Parse operands to double if possible.
        /// If not possible, user will be ask for variable input
        /// </summary>
        /// <param name="operands"></param>
        /// <param name="index"></param>
        /// <param name="firstOperand"></param>
        /// <param name="secondOperand"></param>
        private void HandlerUserInputs(ArrayList operands, int index, out double firstOperand, out double secondOperand)
        {
            //Check if operands are nums ore variables, if variable ask for user input
            if (!double.TryParse((string)operands[index], out firstOperand))
            {
                firstOperand = GetVariableInput((string)operands[index]);
            }
            if (!double.TryParse((string)operands[index + 1], out secondOperand))
            {
                secondOperand = GetVariableInput((string)operands[index + 1]);
            }
        }

        /// <summary>
        /// Asks the user for a value for a variable in the console
        /// </summary>
        /// <param name="variable"></param>
        /// <returns>User input as double</returns>
        private double GetVariableInput(String variable)
        {
            Console.WriteLine("Please enter a number for variable " + variable + ":");
            String input_string = Console.ReadLine();

            double input;
            if (double.TryParse(input_string, out input))
            {
                return input;
            }

            Console.WriteLine("This ist not a number!");
            return GetVariableInput(variable);
        }

        /// <summary>
        /// Calc sum of remaining brackets in operators list
        /// </summary>
        /// <param name="operators"></param>
        /// <returns>Sum of brackets remaining in list</returns>
        private int countBrackets(ArrayList operators)
        {
            int count = 0;
            foreach(String element in operators)
            {
                if (element == "(" || element == ")")
                {
                    count++;
                }
            }

            return count;
        }
    }
}
