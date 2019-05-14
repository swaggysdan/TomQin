using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
/// <summary>
/// A series of test cases on Formula class. And the method names explained itself.
/// </summary>
/// <author> Yixiong Qin </author>
namespace FormulaTester
{
    [TestClass]
    public class FormulaTestCases
    {
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConstructorTest1()

        {
            Formula x = new Formula("$");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConstructorTest2()

        {
            Formula x = new Formula("");
        }
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConstructorTest3()

        {
            Formula x = new Formula("(8+8))");
        }
        [TestMethod]
        
        public void ConstructorTest4()

        {
            Formula x = new Formula("(8+8)");
        }
        [TestMethod]
        public void ConstructorTest5()

        {
            Formula x = new Formula("X5");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConstructorTest6()

        {
            Formula x = new Formula(")(8+8))");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConstructorTest7()

        {
            Formula x = new Formula("(8+8))(");
        }
        [TestMethod]
        public void ConstructorTest8()

        {
            Formula x = new Formula("(X5)");
        }
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConstructorTest9()

        {
            Formula x = new Formula("()8+8)");
        }
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConstructorTest11()

        {
            Formula x = new Formula("***");
        }
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConstructorTest12()

        {
            Formula x = new Formula("1*");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConstructorTest10()

        {
            Formula x = new Formula("1 2");
        }
        [TestMethod]
       
        public void ToStringTest1()

        {
            Formula x = new Formula("1+2");
            Assert.AreEqual("1+2", x.ToString());
        }
        [TestMethod]
        public void ToStringTest2()

        {
            Formula x = new Formula("1+   2");
            Assert.AreEqual("1+2", x.ToString());
        }
        [TestMethod]
        public void EvaluateTest1()

        {
            Formula x = new Formula("x1");
            Assert.AreEqual(3.0,(double)x.Evaluate(s => 3.0));
        }
        [TestMethod]
        public void Normalizer()

        {
            Formula x = new Formula("(1+a1)",s=>"A1",s=>true);
            Assert.AreEqual("(1+A1)", x.ToString());
        }
        [TestMethod]
        public void EvaluateTest2()

        {
            Formula x = new Formula("(x1)+3+16+(X2)");
            Assert.AreEqual(25.0, (double)x.Evaluate(s => 3.0));
        }
        [TestMethod]
        public void EvaluateTest3()

        {
            Formula x = new Formula("(((x1)+3+9+(X2))*X3)/a1");
            Assert.AreEqual(18.0, (double)x.Evaluate(s => 3.0));
        }
        [TestMethod]
        
        public void EvaluateTest4()

        {
            Formula x = new Formula("(((x1)+3+9+(X2))*1111)/a1");
            Assert.IsInstanceOfType(x.Evaluate(s => 0), typeof(FormulaError));
            
        }

        [TestMethod]

        public void EqualsTest()

        {
            Formula x = new Formula("1.0");
            Formula y = new Formula("1.000001");
            Assert.IsFalse(x.Equals(y));


        }
        [TestMethod]

        public void EqualsTest2()

        {
            Formula x = new Formula("1.0");
            Formula y = new Formula("1.00000");
            Assert.IsTrue(x.Equals(y));


        }
        [TestMethod]

        public void GetHashCodeTest()

        {
            Formula x = new Formula("1.0+     1.0");
            Formula y = new Formula("1.0+1.0");
            Assert.IsTrue(x.GetHashCode()==y.GetHashCode());


        }

        [TestMethod]

        public void GetHashCodeTest2()

        {
            Formula x = new Formula("1.0+     1.0-     1.0*1.0");
            Formula y = new Formula("1.0+1.0-1.0*1.0");
            Assert.IsTrue(x.GetHashCode() == y.GetHashCode());


        }

        [TestMethod]

        public void VariablesTest()

        {
            Formula x = new Formula("a1+A2-A3*x5");
            List<string> ennum = new List<string>(x.GetVariables());
            Assert.AreEqual(4, ennum.Count);
            Assert.IsTrue(ennum.Contains("a1"));
            


        }
        [TestMethod]

        public void DivisionTest()

        {
            Formula x = new Formula("a1/x5");
            Assert.AreEqual(1.0, x.Evaluate(s => 2), "1e-6");



        }
        [TestMethod]
        public void OperatorTest()
        {
            Formula a = new Formula("1+2");
            Formula x = new Formula("1+2");
            Formula y = new Formula("1-2");
            Formula z = new Formula("1*2");
            Formula k = new Formula("1/2");
            Formula l = null;
            Formula b = null;
            Assert.IsFalse ( (x == y));
            Assert.IsTrue((l == b));
            Assert.IsTrue((a == x));
            Assert.IsTrue((z != k));


        }
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void operatorTest()

        {
            Formula x = new Formula("2-");
            



        }
        [TestMethod]
        
        public void ValidatorTest()

        {
            Formula x = new Formula("a1+A2",normalizer,s=>true);
            Assert.AreEqual(1.0, x.Evaluate(s => 0.5));



        }
        
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ValidatorTest2()

        {
            Formula x = new Formula("a1+A 2", normalizer, s => true);
          



        }
        [TestMethod]

        public void EvaluatorTest()

        {
            Formula x = new Formula("(a1+B2*(3+1-2))/1");
            Assert.AreEqual(6.0, x.Evaluate(s => 2), "1e-6");



        }
        [TestMethod]

        public void EvaluatorTest2()

        {
            Formula x = new Formula("10+2-1*5/1");
            Assert.AreEqual(7.0, (double)x.Evaluate(s => 2));



        }
        [TestMethod]

        public void EvaluatorTest3()

        {
            Formula x = new Formula("2/2");
            Assert.AreEqual(1.0, (double)x.Evaluate(s => 2));



        }
        private string normalizer(string token)
        {
            return token.ToUpper();
        }
        [TestMethod]
        
        public void EvaluateTest()

        {
            Formula x = new Formula("(a1+1)/(a2-1)");
            Assert.IsInstanceOfType(x.Evaluate(s => 1), typeof(FormulaError));
        }
        [TestMethod]

        public void EvaluateTest5()

        {
            Formula x = new Formula("((1/2)/2)");
            Assert.AreEqual(0.25,(double)x.Evaluate(null));
        }

    }
}
