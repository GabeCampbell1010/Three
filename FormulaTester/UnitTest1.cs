using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;

namespace FormulaTester
{
    [TestClass]
    public class UnitTest1
    {

        //Func<string, double> lookup

        public static double SampleDelegateDouble(string s)
        {
            return 1;
        }

        [TestMethod]
        public void TestMethod1()
        {
        }

        /// <summary>
        ///Equals
        ///</summary>
        [TestMethod()]
        public void Equals()
        {
            Formula t = new Formula("A1 + 2");
            Formula a = t;
            Assert.IsTrue(a.Equals(t));
            //Assert.AreEqual(a,t);
        }

        /// <summary>
        ///EqualsFails
        ///</summary>
        [TestMethod()]
        public void EqualsFails()
        {
            Formula t = new Formula("A1 + 2");
            double a = 8;
            Assert.IsFalse(t.Equals(a));
            //Assert.AreEqual(a,t);
        }

        [TestMethod()]
        public void EqualsFailsEitherNull()
        {
            Formula t = new Formula("A1 + 2");
            Formula a = null;
            Assert.IsFalse(t.Equals(a));
            //Assert.AreEqual(a,t);
        }

        [TestMethod()]
        public void EqualsFailsDiffFormulae()
        {
            Formula t = new Formula("A1 + 2");
            Formula a = new Formula("A1 + 9");
            Assert.IsFalse(t.Equals(a));
            //Assert.AreEqual(a,t);
        }

        /// <summary>
        ///Equals Operator
        ///</summary>
        [TestMethod()]
        public void EqualsOperator()
        {
            Formula t = new Formula("A1 + 2");
            Formula a = t;
            Assert.IsTrue(a==t);
            //Assert.AreEqual(a,t);
        }

        /// <summary>
        ///Equals Operator one null
        ///</summary>
        [TestMethod()]
        public void EqualsOperatorFails1()
        {
            Formula t = new Formula("A1 + 2");
            Formula a = null;
            Assert.IsFalse(a == t);
            //Assert.AreEqual(a,t);
        }

        /// <summary>
        ///Equals Operator both null
        ///</summary>
        [TestMethod()]
        public void EqualsOperatorFails2()
        {
            Formula t = null;
            Formula a = null;
            Assert.IsTrue(a == t);
            //Assert.AreEqual(a,t);
        }

        /// <summary>
        ///Equals Operator diff strins
        ///</summary>
        [TestMethod()]
        public void EqualsOperatorFails3()
        {
            Formula t = new Formula("A1 + 9");
            Formula a = new Formula("A1 + 2");
            Assert.IsFalse(a == t);
            //Assert.AreEqual(a,t);
        }

        /// <summary>
        ///Equals
        ///</summary>
        [TestMethod()]
        public void NotEqualsOperator()
        {
            Formula t = new Formula("A1 + 2");
            Formula a = t;
            Assert.IsFalse(a != t);
            //Assert.AreEqual(a,t);
        }


        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void DivideByZero()
        {
            Formula a = new Formula("5 + 3 - 1/0");
            a.Evaluate(SampleDelegateDouble);
        }


        //Negative Parenthesis Tests
        /// <summary>
        ///parenthesis counts
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ParenCount1()
        {
            Formula t = new Formula(")A1 + 2");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ParenCount5()
        {
            Formula t = new Formula("(A1 + 2");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ParenCount2()
        {
            Formula a = new Formula(")A1 + 2(");
            a = new Formula("(A1)) + 2");
            a = new Formula("(A1 + 2))");
            a = new Formula("A1 + 2");
        }

        //Positive Parenthesis Tests
        [TestMethod()]
        public void ParenCount3()
        {
            Formula a = new Formula("(A1 + 2.1)");
            a = new Formula("(A1) + 2");
            a = new Formula("(A1 + 2)");
            a = new Formula("A1 + 2 - (8*8)");
        }

        //token count
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TokenCount()
        {
            Formula a = new Formula("");
        }

        /// <summary>
        /// variable space variable
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void VariableSpaces()
        {
            Formula a = new Formula("A1 A2");
        }

        //last variable
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void LastVariable()
        {
            Formula a = new Formula("8 +");
        }

        //token count
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void FirstVariable()
        {
            Formula a = new Formula("+ 8");
        }

        //follows an opening parenthesis
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void OpenParenFollow()
        {
            Formula a = new Formula("(+)");
            a = new Formula("()");

        }

        //follows a closing parenthesis
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void CloseParenFollow()
        {
            Formula a = new Formula("()4");
            a = new Formula("()");

        }

        //gethashcode
        [TestMethod()]
        public void GetHashCodeTest()
        {
            Formula a = new Formula("4");
            a.GetHashCode();

        }

        //long formula
        [TestMethod()]
        public void LongComplexFormulaToTestEvaluate()
        {
            Formula a = new Formula("4 + A5-(4/B35) - 1 - 2 / 4 * 6 +(8)-4*(5)/(8)-(9)+(8-3)-(8+5)");
            a.Evaluate(SampleDelegate);

        }

        //follows a closing parenthesis
        [TestMethod()]
        public void FormulaToTestEvaluate()
        {
            Formula a = new Formula("a10 * (A5 + A1)");
            a.Evaluate(SampleDelegate);

        }

        //follows a closing parenthesis
        //[TestMethod()]
        //[ExpectedException(typeof(FormulaFormatException))]
        //public void NullFormulaEval()
        //{
        //    //Formula a = new Formula("A1");
        //    //a = null;
        //    //a.Evaluate(SampleDelegate);
        //    //throws null ref exception before it can evaluate

        //}

        public double SampleDelegate(string s)
        {
            return 1;
        }

        //bad formulas
        //follows a closing parenthesis
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void BadFormula1()
        {
            Formula a = new Formula("??");
            a = new Formula("()");

        }

        //bad formulas
        //follows a closing parenthesis
        [TestMethod()]
        public void GetVariables()
        {
            Formula a = new Formula("A1");
            a.GetVariables();

        }

        //follows a closing parenthesis
        [TestMethod()]
        public void VariablesAll()
        {
            Formula a = new Formula("A1 + A2 - A3 / (A4 + A7)");
            a.Evaluate(SampleDelegateDouble);

        }

        //have three different tests for each block of code inside of the class, for each method and the constructor, and write more than that
        //get close to 100% code coverage
        //passing is 85, superior around 95
    }
}


//further specifications:
//You should make sure that your Formula type is immutable so that there is no way to change 
//a Formula after it has been created. For example (in the future spreadsheet), the only way to 
//modify a cell with a Formula in it is to build a new Formula object.

    //precision for floating points, making sure equals includes the circular to string then to double to account for 2.000000001

//am I supposed to use formula error somewhere?
