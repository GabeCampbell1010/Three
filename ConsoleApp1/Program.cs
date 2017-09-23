using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadsheetUtilities;


namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {



            Formula form = new Formula("A1 + A2 - A3 / (A4 + A7)");
            var answer = form.Evaluate(SampleDelegateDouble2);
            Console.WriteLine(answer);
            Console.ReadKey();
        }

        public static double SampleDelegateDouble2(string s)
        {
            return 1;
        }
    }
}
