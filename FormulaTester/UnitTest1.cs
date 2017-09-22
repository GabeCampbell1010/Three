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
        ///Equals
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
