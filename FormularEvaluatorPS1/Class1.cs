using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FormulaEvaluatorPS1
{
    /// <summary>
    /// this file contains the evaluator class with the evaluator method and asociated helper methods for adding, subtracting, multiplying and dividing
    /// </summary>

    public static class Evaluator
    {
        public delegate int Lookup(String v);


        /// <summary>
        /// helper method for when I need to add or subtract, passes as parameters the two operands and their operator, checks to convert variables if needed, and also checks if correct
        /// type of input string was passed
        /// </summary>
        /// <param name="x">operand</param>
        /// <param name="y">operand</param>
        /// <param name="sign">operator</param>
        /// <param name="lookup">delegate for lookup</param>
        /// <returns></returns>
        public static int AddOrSubtract(string x, string y, string sign, Lookup lookup)
        {
            int X = 0;
            int Y = 0;

            if (Regex.IsMatch(x, @"^[a-zA-Z]+\d+$"))
            {
                try { X = lookup(x); }
                catch (ArgumentException)
                {
                    throw new ArgumentException("invalid variable");
                }

            }
            else
            {
                X = Convert.ToInt32(x);
            }
            if (Regex.IsMatch(y, @"^[a-zA-Z]+\d+$"))
            {
                try { Y = lookup(x); }
                catch (ArgumentException)
                {
                    throw new ArgumentException("invalid variable");
                }

            }
            else
            {
                Y = Convert.ToInt32(y);
            }

            switch (sign)
            {
                case "+": return X + Y;
                case "-": return Y - X;
                default: throw new Exception("invalid operator");
            }
        }
        /// <summary>
        /// helper method for multiplying and dividing, in addition to doing lookups and checking sign type validity, also checks for divide by zero
        /// </summary>
        /// <param name="x">operand, denominator if dividing, returns error if equal to zero</param>
        /// <param name="y">operand, numerator if dividing</param>
        /// <param name="sign">multiply * or divide /</param>
        /// <param name="lookup">lookup delegate</param>
        /// <returns></returns>
        public static int MultiplyOrDivide(string x, string y, string sign, Lookup lookup)
        {
            int X = 0;
            int Y = 0;
            //need to check if the variable does not have a value in the table
            if (Regex.IsMatch(x, @"^[a-zA-Z]+\d+$"))
            {
                try { X = lookup(x); }
                catch (ArgumentException)
                {
                    throw new ArgumentException("invalid variable");
                }

            }
            else
            {
                X = Convert.ToInt32(x);
            }
            if (Regex.IsMatch(y, @"^[a-zA-Z]+\d+$"))
            {
                try { Y = lookup(x); }
                catch (ArgumentException)
                {
                    throw new ArgumentException("invalid variable");
                }
            }
            else
            {
                Y = Convert.ToInt32(y);
                //Y = Convert.ToDouble(y);
            }

            switch (sign)
            {
                case "*": return X * Y;
                case "/": if (X == 0) { throw new ArgumentException("divide by zero"); } else { return Y / X; }//should perhaps replace these conversions to tryparse to test to make sure that math can be done with them
                default: throw new ArgumentException("invalid operator");
            }
        }
        /// <summary>
        /// main evaluator method that takes in the lookup delegate and the users input string
        /// </summary>
        /// <param name="_formula">users input string</param>
        /// <param name="lookup">lookup delegate</param>
        /// <returns></returns>
        public static int Evaluate(string _formula, Lookup lookup)
        {
            ///stacks for operands and operators
            Stack<string> values = new Stack<string>();//seems to work with string stacks, why do i need to use generics here?
            Stack<string> operators = new Stack<string>();

            ///various checks for input validity, including checking for null, spaces and no operator between operands, correct characters, trimming whitespace, etc...
            if (_formula == null)//check for null
                throw new ArgumentException("input value cannot be null string");

            //check for spaces between operands
            if (Regex.IsMatch(_formula, @"[a-zA-Z]*\d+\s+[a-zA-Z]*\d+"))
            {
                throw new ArgumentException("there are two operands with a space between them");
            }

            if (Regex.IsMatch(_formula, @"[a-zA-Z]+[^0-9]$"))//trying to test for one or more letters and no numbers
            {
                throw new ArgumentException("invalid variable, letters without a number");
            }

            //use regular expressions to replace all the whitespace in input with nothing
            _formula = Regex.Replace(_formula, @"\s+", "");

            //this will throw an exception if any of the input characters are invalid
            if (Regex.IsMatch(_formula, @"[^0-9A-Za-z()*/+-]"))
            {
                throw new ArgumentException("there are invalid characters in the input");
            }
            string[] substrings = Regex.Split(_formula, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

            foreach (string variable in substrings)
            {
                if (!Regex.IsMatch(variable, @"[+|\-|*|/|%|(|)|]") && variable != "")
                {
                    if (!Regex.IsMatch(variable, @"^[a-zA-Z]*\d+$"))
                    {
                        throw new Exception("invalid input type");
                    }
                }
            }

            for (int i = 0; i < substrings.Length; i++)
            {
                //if empty string
                if (substrings[i] == "")//go on to the next loop if its an empty string
                    continue;
                //if value string
                if (Regex.IsMatch(substrings[i], @"[a-zA-Z]*\d+"))
                {
                    if (values.Count == 0)
                    {
                        values.Push(substrings[i]);
                        continue;
                    }

                    if (values.Count != 0)  //check there is a value at the top of values, if not throw an error, if so pop it, the thing you popoff is the numerator, and do calculation, that is in Calculate method, then put that returned value back into values as a string
                    {
                        if (operators.Peek() == "/" || operators.Peek() == "*")//peek the operator stack, if it has a * or \ then do some evaluating, otherwise add the value to the value stack
                        {

                            string y = values.Pop();
                            string x = substrings[i];
                            string sign = operators.Pop();
                            values.Push(MultiplyOrDivide(x, y, sign, lookup).ToString());
                        }
                        else//just push to the values stack if the operator stack does not have an operator for it
                        {
                            values.Push(substrings[i]);
                        }
                    }
                    else
                    {
                        throw new ArgumentException("no values on the value stack to multiply or divide by");
                    }
                }
                //if * or / operator or ( left parenthesis then push onto the operator stack
                if (substrings[i] == "*" || substrings[i] == "/" || substrings[i] == "(")
                    operators.Push(substrings[i]);

                //if + or - operator
                if (substrings[i] == "+" || substrings[i] == "-")
                {
                    if (operators.Count == 0)
                    {
                        operators.Push(substrings[i]);
                        continue;//so i don't get an error on the first operator
                    }
                    if (operators.Peek() == "+" || operators.Peek() == "-")
                    {
                        if (values.Count > 1)//if - or + on top of operators stack
                        {
                            string x = values.Pop();
                            string y = values.Pop();
                            string sign = operators.Pop();
                            values.Push(AddOrSubtract(x, y, sign, lookup).ToString());//do calculation and push result back onto values stack as a string

                        }
                        else
                        {
                            throw new ArgumentException("tried to pop off 2 values from value stack for + or - operator but there were fewer than 2 values, could be caused by extra operator");
                        }
                    }
                    operators.Push(substrings[i]);//push + or - onto the operator stack either way, regardless of whether above to conditions are met
                }

                if (substrings[i] == ")")
                {
                    if (operators.Count == 0)
                        throw new Exception("no operators on the stack to pop after a right parenthesis, perhaps your parentheses are not in the proper order");

                    if (operators.Peek() == "+" || operators.Peek() == "-")
                    {
                        if (values.Count > 1)
                        {
                            string x = values.Pop();
                            string y = values.Pop();
                            string sign = operators.Pop();
                            values.Push(AddOrSubtract(x, y, sign, lookup).ToString());//do calculation and push result back onto values stack as a string

                        }
                        else
                        {
                            throw new ArgumentException("tried to pop off 2 values from value stack for + or - operator but there were fewer than 2 values, perhaps a right parenthesis is out of order or superfluous");
                        }
                    }
                    if (operators.Peek() == "(")
                    {
                        operators.Pop();
                    }
                    else
                    {
                        throw new ArgumentException("the top of the operator stack was supposed to be a '(' but it was a: " + operators.Peek());
                    }
                    if (operators.Count != 0 && (operators.Peek() == "/" || operators.Peek() == "*"))//peek the operator stack, if it has a * or / then do some evaluating, otherwise add the value to the value stack
                    {

                        if (values.Count > 1)//check if enough values to do below calculation
                        {
                            string x = values.Pop();
                            string y = values.Pop();
                            string sign = operators.Pop();
                            values.Push(MultiplyOrDivide(x, y, sign, lookup).ToString());
                        }
                        else
                        {
                            throw new ArgumentException("tried to pop off 2 values from value stack for * or / operator but there were fewer than 2 values on value stack");
                        }

                    }
                    else
                    {
                        //i think don't do anything here?
                    }
                }
            }

            if (operators.Count == 0)
            {
                if (values.Count == 1)//if there is only one remaining value on the stack 
                {

                    int X = 0;

                    if (Regex.IsMatch(values.Peek(), @"^[a-zA-Z]+\d+$"))
                    {
                        try { X = lookup(values.Pop()); }
                        catch (ArgumentException)
                        {
                            throw new ArgumentException("could not convert variable");
                        }
                        return X;

                    }
                    else
                    {
                        return Convert.ToInt32(values.Pop());
                    }
                }
            }
            else
            {
                if (operators.Count == 1 && values.Count == 2 && (operators.Peek() == "+" || operators.Peek() == "-"))
                {
                    string x = values.Pop();
                    string y = values.Pop();
                    string sign = operators.Pop();
                    values.Push(AddOrSubtract(x, y, sign, lookup).ToString());
                    return Convert.ToInt32(values.Pop());//try parse?
                }
                else
                {
                    throw new ArgumentException("more than one operator left on the stack after string has been evaluated, or one remaining operator is not a + or -, or not exactly 2 values left on the value stack");
                }
            }


            //return 0;
            //not really sure if below is necessary or helpful
            int result;
            if (Int32.TryParse(_formula, out result))
                return result;
            else
                throw new Exception("the result could not be converted to an integer");


        }



    }
}

