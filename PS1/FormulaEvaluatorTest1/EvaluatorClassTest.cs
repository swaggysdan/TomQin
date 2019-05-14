using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FormulaEvaluator;

namespace FormulaEvaluatorTest1
{
    [TestClass]
    public class EvaluatorClassTest
    {
        [TestMethod()]
        public void Test1()
        {
            Assert.AreEqual(9, Evaluator.Evaluate("8+1", s => 0));
        }

        [TestMethod()]
        public void Test2()
        {
            Assert.AreEqual(8, Evaluator.Evaluate("B2-2", s => 10));
        }

        [TestMethod()]
        public void Test3()
        {
            Assert.AreEqual(-2, Evaluator.Evaluate("6-8", s => 0));
        }

        [TestMethod()]
        public void Test4()
        {
            Assert.AreEqual(1, Evaluator.Evaluate("A3", s => 1));
        }

        [TestMethod()]
        public void Test5()
        {
            Assert.AreEqual(1815, Evaluator.Evaluate("55*33", s => 0));
        }

        [TestMethod()]
        public void Test6()
        {
            Assert.AreEqual(1, Evaluator.Evaluate("5/5", s => 0));
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void Test7()
        {
            Evaluator.Evaluate("(10*10*", s => 0);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void Test8()
        {
            Evaluator.Evaluate("8+8*8)", s => 0);
        }

        [TestMethod()]
        public void Test9()
        {
            Assert.AreEqual(6, Evaluator.Evaluate("1*2*3", s => 0));
        }

        [TestMethod()]
        public void Test10()
        {
            Assert.AreEqual(625, Evaluator.Evaluate("5*5*5*5", s => 0));
        }

        [TestMethod()]
        public void Test11()
        {
            Assert.AreEqual(0, Evaluator.Evaluate("s7-s7*s7/s7", s => 1));
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void Test12()
        {
            Evaluator.Evaluate("++2", s => 0);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void Test13()
        {
            Evaluator.Evaluate("++-18)", s => 0);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void Test14()
        {
            Evaluator.Evaluate("", s => 0);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void Test15()
        {
            Evaluator.Evaluate("8++*22", s => 0);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void Test16()
        {
            Evaluator.Evaluate("as", s => 0);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void Test17()
        {
            Evaluator.Evaluate("0/0", s => 0);
        }

        [TestMethod()]
        
        public void Test18()
        {
            Assert.AreEqual(18, Evaluator.Evaluate("x1+(1+(2+(x2+(13))))", s => 1));
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void Test19()
        {
            Evaluator.Evaluate("55--44++22**18", s => 0);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void Test20()
        {
            Evaluator.Evaluate("(A1)4+88", s => 0);
        }

        
        

    }
}
