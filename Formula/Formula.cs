// Skeleton written by Joe Zachary for CS 3500, September 2013
// Read the entire skeleton carefully and completely before you
// do anything else!

// Version 1.1 (9/22/13 11:45 a.m.)

// Change log:
//  (Version 1.1) Repaired mistake in GetTokens
//  (Version 1.1) Changed specification of second constructor to
//                clarify description of how validation works

// (Daniel Kopta) 
// Version 1.2 (9/10/17) 

// Change log:
//  (Version 1.2) Changed the definition of equality with regards
//                to numeric tokens


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SpreadsheetUtilities
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  The allowed symbols are non-negative numbers written using double-precision 
    /// floating-point syntax; variables that consist of a letter or underscore followed by 
    /// zero or more letters, underscores, or digits; parentheses; and the four operator 
    /// symbols +, -, *, and /.  
    /// 
    /// Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
    /// a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable; 
    /// and "x 23" consists of a variable "x" and a number "23".
    /// 
    /// Associated with every formula are two delegates:  a normalizer and a validator.  The
    /// normalizer is used to convert variables into a canonical form, and the validator is used
    /// to add extra restrictions on the validity of a variable (beyond the standard requirement 
    /// that it consist of a letter or underscore followed by zero or more letters, underscores,
    /// or digits.)  Their use is described in detail in the constructor and method comments.
    /// </summary>
    public class Formula
    {

        private string _formula;

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically invalid,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer is the identity function, and the associated validator
        /// maps every string to true.  
        /// </summary>
        public Formula(String formula) :
            this(formula, s => s, s => true)
        {
            //do nothing here
        }

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically incorrect,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer and validator are the second and third parameters,
        /// respectively.  
        /// 
        /// If the formula contains a variable v such that normalize(v) is not a legal variable, 
        /// throws a FormulaFormatException with an explanatory message. 
        /// 
        /// If the formula contains a variable v such that isValid(normalize(v)) is false,
        /// throws a FormulaFormatException with an explanatory message.
        /// 
        /// Suppose that N is a method that converts all the letters in a string to upper case, and
        /// that V is a method that returns true only if a string consists of one letter followed
        /// by one digit.  Then:
        /// 
        /// new Formula("x2+y3", N, V) should succeed
        /// new Formula("x+y3", N, V) should throw an exception, since V(N("x")) is false
        /// new Formula("2x+y3", N, V) should throw an exception, since "2x+y3" is syntactically incorrect.
        /// </summary>
        public Formula(String formula, Func<string, string> normalize, Func<string, bool> isValid)
        {
            //number 1 parsing is already covered by GetTokens?

            //number 2 token count rule
            if(GetTokens(formula).Count() < 1)
            {
                throw new FormulaFormatException("no tokens present");
            }

            //do parenthesis check here, balanced and right, numbers 3 and 4

            //end parenthesis check

            

            var tokenArray = GetTokens(formula).ToArray<string>();//use this to check

            // 5 and 6 starting and ending token check
            //The first token of an expression must be a number, a variable, or an opening parenthesis.
            if (!(Regex.IsMatch(tokenArray[0], @"\(") || Regex.IsMatch(tokenArray[0], @"[a-zA-Z_](?: [a-zA-Z_]|\d)*") || Regex.IsMatch(tokenArray[0], @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?")))
            {
                throw new FormulaFormatException("first token is not a number, variable or opening parenthesis");
            }
            if (!(Regex.IsMatch(tokenArray[tokenArray.Length - 1], @"\(") || Regex.IsMatch(tokenArray[tokenArray.Length - 1], @"[a-zA-Z_](?: [a-zA-Z_]|\d)*") || Regex.IsMatch(tokenArray[tokenArray.Length - 1], @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?")))
            {
                throw new FormulaFormatException("last token is not a number, variable or closing parenthesis");
            }

            
            for (int i = 0; i < tokenArray.Length; i++)//could probably just do everything inthis for loop
            {
                //number 7
                //Any token that immediately follows an opening parenthesis or an operator must be either a number, a variable, or an opening parenthesis.
                if (tokenArray[i] == "(" || Regex.IsMatch(tokenArray[i], @"[\+\-*/]"))
                {
                    if (!(Regex.IsMatch(tokenArray[i+1], @"\(") || Regex.IsMatch(tokenArray[i + 1], @"[a-zA-Z_](?: [a-zA-Z_]|\d)*") || Regex.IsMatch(tokenArray[i + 1], @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?")))
                    {
                        throw new FormulaFormatException("token that immediately follows an opening parenthesis or an operator is not either a number, a variable, or an opening parenthesis");
                    }
                }
                //number 8 extra following
                //Any token that immediately follows a number, a variable, or a closing parenthesis must be either an operator or a closing parenthesis.
                if (Regex.IsMatch(tokenArray[i], @"\)") || Regex.IsMatch(tokenArray[i], @"[a-zA-Z_](?: [a-zA-Z_]|\d)*") || Regex.IsMatch(tokenArray[i], @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?"))
                {
                    if (!(tokenArray[i+1] == ")" || Regex.IsMatch(tokenArray[i+1], @"[\+\-*/]")))
                    {
                        throw new FormulaFormatException("token that immediately follows a number, a variable, or a closing parenthesis is not either an operator or a closing parenthesis");
                    }
                }


            }

            //Any token that immediately follows a number, a variable, or a closing parenthesis must be either an operator or a closing parenthesis.

            foreach (string item in GetTokens(formula))
            {
                //use normalize and isvalid forthe variables, not the operators or doubles but just the variables
                //if its a vairable then normalize like cpaitlaize, and then check for validity
                //is this a valid vairable by our standards, write privatefunction for that, then check their startndards with isvalid
                
                string itemNormalized;//this stores the normalized version of the item

                if (Regex.IsMatch(item, @"[a-zA-Z_](?: [a-zA-Z_]|\d)*"))//check if variable, this from getokens below, excudes numbers (floats) and operatoors and parentheses
                {
                    try//variables consist of a letter or underscore followed by zero or more letters, underscores, or digits;
                    {
                        itemNormalized = normalize(item);//if the item is a variable then try to normalize it, if it can't be normalized throw an exception
                    }
                    catch(Exception)
                    {
                        throw new FormulaFormatException("item cannot be normalized");
                    }

                    if (!isValid(itemNormalized))//check if valid
                    {
                        throw new FormulaFormatException("item is not valid");
                    }
                    _formula += itemNormalized;
                    continue;//maybe not finished here 
                }

                //do more work here for 1-8 startingwith parsing

                _formula += item;//maybe do as stringbuilder but this is ok
            }



            //check the 8 steps thatgo in the constructor staring with 1.parsing
            //build up a string representation of formaula that is normalized and so on and keep as aprivte member variabel 
        }

        /// <summary>
        /// Evaluates this Formula, using the lookup delegate to determine the values of
        /// variables.  When a variable symbol v needs to be determined, it should be looked up
        /// via lookup(normalize(v)). (Here, normalize is the normalizer that was passed to 
        /// the constructor.)
        /// 
        /// For example, if L("x") is 2, L("X") is 4, and N is a method that converts all the letters 
        /// in a string to upper case:
        /// 
        /// new Formula("x+7", N, s => true).Evaluate(L) is 11
        /// new Formula("x+7").Evaluate(L) is 9
        /// 
        /// Given a variable symbol as its parameter, lookup returns the variable's value 
        /// (if it has one) or throws an FormulaFormatException (otherwise).
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, the value is returned.  Otherwise, a FormulaError is returned.  
        /// The Reason property of the FormulaError should have a meaningful explanation.
        ///  
        /// This method should never throw an exception.
        /// </summary>
        public object Evaluate(Func<string, double> lookup)
        {
            #region Evaluate
            ///stacks for operands and operators
            Stack<string> values = new Stack<string>();//seems to work with string stacks, why do i need to use generics here?
            Stack<string> operators = new Stack<string>();

            ///various checks for input validity, including checking for null, spaces and no operator between operands, correct characters, trimming whitespace, etc...
            if (_formula == null)//check for null
                throw new FormulaFormatException("input value cannot be null string");

            //check for spaces between operands
            if (Regex.IsMatch(_formula, @"[a-zA-Z]*\d+\s+[a-zA-Z]*\d+"))
            {
                throw new FormulaFormatException("there are two operands with a space between them");
            }

            if (Regex.IsMatch(_formula, @"[a-zA-Z]+[^0-9]$"))//trying to test for one or more letters and no numbers
            {
                throw new FormulaFormatException("invalid variable, letters without a number");
            }

            //use regular expressions to replace all the whitespace in input with nothing
            _formula = Regex.Replace(_formula, @"\s+", "");

            //this will throw an exception if any of the input characters are invalid
            if (Regex.IsMatch(_formula, @"[^0-9A-Za-z()*/+-]"))
            {
                throw new FormulaFormatException("there are invalid characters in the input");
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
                        throw new FormulaFormatException("no values on the value stack to multiply or divide by");
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
                            throw new FormulaFormatException("tried to pop off 2 values from value stack for + or - operator but there were fewer than 2 values, could be caused by extra operator");
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
                            throw new FormulaFormatException("tried to pop off 2 values from value stack for + or - operator but there were fewer than 2 values, perhaps a right parenthesis is out of order or superfluous");
                        }
                    }
                    if (operators.Peek() == "(")
                    {
                        operators.Pop();
                    }
                    else
                    {
                        throw new FormulaFormatException("the top of the operator stack was supposed to be a '(' but it was a: " + operators.Peek());
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
                            throw new FormulaFormatException("tried to pop off 2 values from value stack for * or / operator but there were fewer than 2 values on value stack");
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

                    double X = 0;

                    if (Regex.IsMatch(values.Peek(), @"^[a-zA-Z]+\d+$"))
                    {
                        try { X = lookup(values.Pop()); }
                        catch (FormulaFormatException)
                        {
                            throw new FormulaFormatException("could not convert variable");
                        }
                        return X;

                    }
                    else
                    {
                        return Convert.ToDouble(values.Pop());
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
                    return Convert.ToDouble(values.Pop());//try parse?
                }
                else
                {
                    throw new FormulaFormatException("more than one operator left on the stack after string has been evaluated, or one remaining operator is not a + or -, or not exactly 2 values left on the value stack");
                }
            }


            //return 0;
            //not really sure if below is necessary or helpful
            double result;
            if (Double.TryParse(_formula, out result))
                return result;
            else
                throw new Exception("the result could not be converted to an integer");

            #endregion
            return null;
        }
        #region AddOrSubtract
        /// <summary>
        /// helper method for when I need to add or subtract, passes as parameters the two operands and their operator, checks to convert variables if needed, and also checks if correct
        /// type of input string was passed
        /// </summary>
        /// <param name="x">operand</param>
        /// <param name="y">operand</param>
        /// <param name="sign">operator</param>
        /// <param name="lookup">delegate for lookup</param>
        /// <returns></returns>
        public static double AddOrSubtract(string x, string y, string sign, Func<string, double> lookup)
        {
            double X = 0;
            double Y = 0;

            if (Regex.IsMatch(x, @"^[a-zA-Z]+\d+$"))
            {
                try { X = lookup(x); }
                catch (FormulaFormatException)
                {
                    throw new FormulaFormatException("invalid variable");
                }

            }
            else
            {
                X = Convert.ToDouble(x);
            }
            if (Regex.IsMatch(y, @"^[a-zA-Z]+\d+$"))
            {
                try { Y = lookup(x); }
                catch (FormulaFormatException)
                {
                    throw new FormulaFormatException("invalid variable");
                }

            }
            else
            {
                Y = Convert.ToDouble(y);
            }

            switch (sign)
            {
                case "+": return X + Y;
                case "-": return Y - X;
                default: throw new Exception("invalid operator");
            }
        }
        #endregion
        #region MultiplyOrDivide
        /// <summary>
        /// helper method for multiplying and dividing, in addition to doing lookups and checking sign type validity, also checks for divide by zero
        /// </summary>
        /// <param name="x">operand, denominator if dividing, returns error if equal to zero</param>
        /// <param name="y">operand, numerator if dividing</param>
        /// <param name="sign">multiply * or divide /</param>
        /// <param name="lookup">lookup delegate</param>
        /// <returns></returns>
        public static double MultiplyOrDivide(string x, string y, string sign, Func<string, double> lookup)
        {
            double X = 0;
            double Y = 0;
            //need to check if the variable does not have a value in the table
            if (Regex.IsMatch(x, @"^[a-zA-Z]+\d+$"))
            {
                try { X = lookup(x); }
                catch (FormulaFormatException)
                {
                    throw new FormulaFormatException("invalid variable");
                }

            }
            else
            {
                X = Convert.ToDouble(x);
            }
            if (Regex.IsMatch(y, @"^[a-zA-Z]+\d+$"))
            {
                try { Y = lookup(x); }
                catch (FormulaFormatException)
                {
                    throw new FormulaFormatException("invalid variable");
                }
            }
            else
            {
                Y = Convert.ToDouble(y);
            }

            switch (sign)
            {
                case "*": return X * Y;
                case "/": if (X == 0) { throw new FormulaFormatException("divide by zero"); } else { return Y / X; }//should perhaps replace these conversions to tryparse to test to make sure that math can be done with them
                default: throw new FormulaFormatException("invalid operator");
            }
        }
        #endregion

        /// <summary>
        /// Enumerates the normalized versions of all of the variables that occur in this 
        /// formula.  No normalization may appear more than once in the enumeration, even 
        /// if it appears more than once in this Formula.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x+y*z", N, s => true).GetVariables() should enumerate "X", "Y", and "Z"
        /// new Formula("x+X*z", N, s => true).GetVariables() should enumerate "X" and "Z".
        /// new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
        /// </summary>
        public IEnumerable<String> GetVariables()
        {
            return null;
        }

        /// <summary>
        /// Returns a string containing no spaces which, if passed to the Formula
        /// constructor, will produce a Formula f such that this.Equals(f).  All of the
        /// variables in the string should be normalized.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x + y", N, s => true).ToString() should return "X+Y"
        /// new Formula("x + Y").ToString() should return "x+Y"
        /// </summary>
        public override string ToString()
        {
            return null;
        }

        /// <summary>
        /// If obj is null or obj is not a Formula, returns false.  Otherwise, reports
        /// whether or not this Formula and obj are equal.
        /// 
        /// Two Formulae are considered equal if they consist of the same tokens in the
        /// same order.  To determine token equality, all tokens are compared as strings 
        /// except for numeric tokens and variable tokens.
        /// Numeric tokens are considered equal if they are equal after being "normalized" 
        /// by C#'s standard conversion from string to double, then back to string. This 
        /// eliminates any inconsistencies due to limited floating point precision.
        /// Variable tokens are considered equal if their normalized forms are equal, as 
        /// defined by the provided normalizer.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        ///  
        /// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
        /// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
        /// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
        /// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
        /// </summary>
        public override bool Equals(object obj)
        {
            return false;
        }

        /// <summary>
        /// Reports whether f1 == f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return true.  If one is
        /// null and one is not, this method should return false.
        /// </summary>
        public static bool operator ==(Formula f1, Formula f2)
        {
            return false;
        }

        /// <summary>
        /// Reports whether f1 != f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return false.  If one is
        /// null and one is not, this method should return true.
        /// </summary>
        public static bool operator !=(Formula f1, Formula f2)
        {
            return false;
        }

        /// <summary>
        /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
        /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two 
        /// randomly-generated unequal Formulae have the same hash code should be extremely small.
        /// </summary>
        public override int GetHashCode()
        {
            return 0;
        }

        /// <summary>
        /// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;
        /// right paren; one of the four operator symbols; a string consisting of a letter or underscore
        /// followed by zero or more letters, digits, or underscores; a double literal; and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";//?: does some kind of grouping, so this is 1 or more digits followed by a period followed by zero or more digits, 
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))//if its not a space then return it
                {
                    //Console.Write(s + " ");//remove this
                    yield return s;
                }
            }

        }
    }

    /// <summary>
    /// Used to report syntactic errors in the argument to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Used as a possible return value of the Formula.Evaluate method.
    /// </summary>
    public struct FormulaError
    {
        /// <summary>
        /// Constructs a FormulaError containing the explanatory reason.
        /// </summary>
        /// <param name="reason"></param>
        public FormulaError(String reason)
            : this()
        {
            Reason = reason;
        }

        /// <summary>
        ///  The reason why this FormulaError was created.
        /// </summary>
        public string Reason { get; private set; }
    }
}
