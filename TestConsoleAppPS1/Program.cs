using SpreadsheetUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsoleAppPS1
{
    /// <summary>
    /// this is the tester file with the program class for executing the evaluate method
    /// </summary>
    class Program
    {
        /// <summary>
        /// main method that runs program
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //using test delegate, need to replace SampleDelegate with the actual variable test delegate
            //Console.WriteLine(FormulaEvaluatorPS1.Evaluator.Evaluate("A1 + 7", SampleDelegate));//print out the returned value

            Formula formula = new Formula("A1 + 5");

            formula.Evaluate(SampleDelegateDouble);

            Console.ReadKey();

        }

        /// <summary>
        /// this test delegate function is currently being fed into formula evaluator above, but needs to be replaced by the actual variable lookup delegate,
        /// I guess by the grader?
        /// </summary>
        /// <param name="s">test variable string s</param>
        /// <returns></returns>
        public static int SampleDelegate(string s)
        {
            return 1;
        }

        public static double SampleDelegateDouble(string s)
        {
            return 1;
        }


        //valid:
        /// <summary>
        /// test method of the type described in the lectures, i did not actually use this though.
        /// </summary>
        /// <param name="expression">input string to be evaluated</param>
        /// <param name="L">the lookup delegate</param>
        /// <param name="expected">expected answer, if after being evaluated the input string and lookup do not equal this then throw an exception</param>
        /// <returns></returns>
        public static bool ValidTest(string expression, FormulaEvaluatorPS1.Evaluator.Lookup L, int expected)//i don't think that Lookup<string, string> is of the correct form, Lookup alone gives an error,
        {
            try
            {
                return FormulaEvaluatorPS1.Evaluator.Evaluate(expression, L) == expected;
            }
            catch (Exception)
            {
                return false;
            }
        }
        //and then will want to run this test like:
        //ValidTest("1+7", "A4/5",...,"(3-4)/0"); and so on

        //invalid tests:
        /// <summary>
        /// invalidity test, also not acutally used, designed to make sure that invalid input does return false and is not permitted to pass
        /// </summary>
        /// <param name="exp">inputed expression</param>
        /// <param name="L">lookup</param>
        /// <returns></returns>
        public static bool InvalidTest(string exp, FormulaEvaluatorPS1.Evaluator.Lookup L)
        {
            try
            {
                FormulaEvaluatorPS1.Evaluator.Evaluate(exp, L);
                return false;
            }
            catch (ArgumentException)
            {
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }



    }
    /// <summary>
    /// i just used manual if loop checks if a stack was empty before popping or peeking it, i didn't use the below extension
    /// </summary>
    public static class Extensions
    {
        //will often need something like:
        //if(stack.Count > 0 && stack.Peek()...)//I used this and not the below isontop method
        //instead make custom stack with class isOnTop(T x) using an extension method
        public static bool isOnTop<T>(this Stack<T> s, T val)
        {
            return true;//supposed to put code in here that simply checks that the stack is not empty and the top value is of some type
        }
    }
}

