using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using SS;



namespace SpreadsheetTests
{
    /// <summary>
    /// Test cases for spreadsheet. Method names are self documanted.
    /// </summary>
    [TestClass]
    public class SStestCases
    {
        [TestMethod]
        public void ValidityCheckTest()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("x1", "=2.0");
            Assert.AreEqual(2.0, sheet.GetCellValue("x1"));
        }

        [TestMethod]
        public void SetContentsOfCellTest()
        {
            
            AbstractSpreadsheet sss = new Spreadsheet();
            sss.SetContentsOfCell("a1", "aa");

        }
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]

        public void SetContentsOfCellValidationTest1()
        {
            
            AbstractSpreadsheet sss = new Spreadsheet();
            sss.SetContentsOfCell("1a", "a");

        }
        [TestMethod]
        
        
        public void SetContentsOfCellTest2()
        {

            AbstractSpreadsheet sss = new Spreadsheet();
            sss.SetContentsOfCell("XX1", "=A1+B1");

        }
        
        [TestMethod]


       

        public void SetCellContentsConditionTest1()
        {

            AbstractSpreadsheet sss = new Spreadsheet();
            sss.SetContentsOfCell("x1", "xxx");
            Assert.AreEqual("xxx", sss.GetCellContents("x1"));

        }


        [TestMethod]
        public void NormalizationTest1()
        {
            AbstractSpreadsheet sss = new Spreadsheet(ss => true, ss => ss.ToUpper(),"") ;
            sss.SetContentsOfCell("x1", "xxx");
            Assert.AreEqual("xxx", sss.GetCellContents("X1"));
            
            }

        [TestMethod]

        
        public void NormalizationTest2()
        {
            AbstractSpreadsheet sss = new Spreadsheet();
            sss.SetContentsOfCell("X1", "1.11");
            sss.SetContentsOfCell("x1", "aaa");
            sss.SetContentsOfCell("Y1", "=X1");
            sss.SetContentsOfCell("B1", "1.1122");
            Assert.AreEqual(1.11, (double)sss.GetCellValue("Y1"));
            Assert.AreEqual(1.1122, (double)sss.GetCellValue("B1"));

        }
        [TestMethod]


        public void GetCellContentsTest()
        {
            AbstractSpreadsheet sss = new Spreadsheet();
            sss.SetContentsOfCell("x1", "5");
            sss.SetContentsOfCell("X1", "aaa");
            sss.SetContentsOfCell("x2", "=s1");
            
            Assert.AreEqual(5.0, (double)sss.GetCellValue("x1"));
           

        }
        
        [TestMethod]


        public void NormalizationTest3()
        {
            AbstractSpreadsheet sss = new Spreadsheet();
            Assert.IsFalse(sss.Changed);
            sss.SetContentsOfCell("x1", "x");
            Assert.IsTrue(sss.Changed);
           

        }
        
        [TestMethod]


        public void FormulaTest0()
        {
            AbstractSpreadsheet sss = new Spreadsheet();
            sss.SetContentsOfCell("x11", "1.1");
            sss.SetContentsOfCell("x22", "1.1");
            sss.SetContentsOfCell("x33", "=x11 / x22");
            sss.GetCellValue("x33");


        }
        
        [TestMethod]


        public void FormulaTest1()
        {
            AbstractSpreadsheet sss = new Spreadsheet();
            sss.SetContentsOfCell("x11", "2.0");
            sss.SetContentsOfCell("x22", "0.0");
            sss.SetContentsOfCell("x33", "=x11/x22");
            sss.GetCellValue("x33");


        }
        
        [TestMethod]


        public void FormulaTest2()
        {
            AbstractSpreadsheet sss = new Spreadsheet();
            sss.SetContentsOfCell("x11", "2.0");
            sss.SetContentsOfCell("x22", "0.0");
            sss.SetContentsOfCell("x33", "=x22/x11");
            sss.GetCellValue("x33");


        }
        

        [TestMethod]


        public void FormulaTest3()
        {
            AbstractSpreadsheet sss = new Spreadsheet();
            sss.SetContentsOfCell("x11", "2.0");
            sss.SetContentsOfCell("x22", "a");
            sss.SetContentsOfCell("x33", "=x22/x11");
            sss.GetCellValue("x33");


        }


        
        [TestMethod]


        public void FormulaTest4()
        {
            AbstractSpreadsheet sss = new Spreadsheet();
            sss.SetContentsOfCell("x11", "2.0");
            sss.SetContentsOfCell("x33", "=x22/x11");
            sss.GetCellValue("x33");


        }
        
        [ExpectedException(typeof(InvalidNameException))]
        [TestMethod]
        public void SetCellContentsTest2()
        {
            Spreadsheet sss = new Spreadsheet();
            ISet<string> temp = sss.SetContentsOfCell(null, "abc");
            
        }
        
        
        /// <summary>
        /// Test valid cellname that is not in the current spreadsheet
        /// </summary>
        [TestMethod]
        public void TestGetDirectDependentsNewCellName()
        {
            Spreadsheet s = new Spreadsheet();
            PrivateObject sheet = new PrivateObject(s);
            sheet.Invoke("GetDirectDependents", new String[1] { "a1" });//should work and cause no errors
        }
        [TestMethod]
        public void NormalizationTest()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "1.0");
            sheet.SetContentsOfCell("A2", "2.0");
            sheet.SetContentsOfCell("A3", "3.0");
            sheet.SetContentsOfCell("A4", "4.0");
            sheet.SetContentsOfCell("A5", "12.0");
            sheet.SetContentsOfCell("A6", "24.0");
            sheet.SetContentsOfCell("A7", "abc");
            sheet.SetContentsOfCell("X1", "=A1*A2");
            sheet.SetContentsOfCell("X2", "=A6/A5");
            sheet.SetContentsOfCell("X3", "=A1-A6");
            sheet.SetContentsOfCell("X4", "=A3+A4");
            Assert.AreEqual(2.0, sheet.GetCellValue("X1"));
            Assert.AreEqual(2.0, sheet.GetCellValue("X2"));
            Assert.AreEqual(-23.0, sheet.GetCellValue("X3"));
            Assert.AreEqual(7.0, sheet.GetCellValue("X4"));
        }

        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void SaveTest()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.Save("c:\\reposs\\file.txt");
        }

        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void SaveTest1()
        {
            AbstractSpreadsheet sss = new Spreadsheet("c:\\reposs\\file.txt", s => true, s => s, "");
        }

        [TestMethod()]
        public void SetContentOfCellTryCatchTest()
        {
            AbstractSpreadsheet sss = new Spreadsheet();
            sss.SetContentsOfCell("x1", "=1/0");
            Assert.IsTrue(sss.GetCellValue("x1") is FormulaError);
            sss.SetContentsOfCell("x2", "=x1");
            sss.SetContentsOfCell("x1", "=2");
            Assert.AreEqual(sss.GetCellValue("x2"), sss.GetCellValue("x1"));
            Assert.AreEqual(2.0, sss.GetCellValue("x2"));
        }

        [TestMethod()]
        
        public void SaveTest2()
        {
            AbstractSpreadsheet sss = new Spreadsheet();
            sss.SetContentsOfCell("A1", "x");
            sss.Save("filename.txt");
            sss = new Spreadsheet("filename.txt", s => true, s => s, "default");
            Assert.AreEqual("x", sss.GetCellContents("A1"));
        }
         [ExpectedException(typeof(SpreadsheetReadWriteException))]
         [TestMethod()]
         public void SaveTest3()
         {
             AbstractSpreadsheet sss = new Spreadsheet();
             sss.SetContentsOfCell("A1", "x");
             sss.Save("filename.txt");
             sss = new Spreadsheet("filename.txt", s => true, s => s, "version");
             
         }
        [TestMethod]
        [ExpectedException(typeof(TargetInvocationException))]
        public void SetCellContentsTest00()
        {
            Spreadsheet sss = new Spreadsheet();
            PrivateObject pss = new PrivateObject(sss);
            Type[] myTypes = new Type[] { typeof(string), typeof(Formula) };
            object[] args = new object[] { "a1", null };
            pss.Invoke("SetCellContents", myTypes, args);
        }
        [TestMethod]
        [ExpectedException(typeof(TargetInvocationException))]
        public void GetDirectDependencyTest01()
        {
            Spreadsheet sss = new Spreadsheet();
            PrivateObject pss = new PrivateObject(sss);
            Type[] myTypes = new Type[] { typeof(string) };
            object[] args = new object[] { null };
            pss.Invoke("GetDirectDependents", myTypes,args);
        }
        [TestMethod]
        [ExpectedException(typeof(TargetInvocationException))]
        public void GetDirectDependencyTest02()
        {
            Spreadsheet sss = new Spreadsheet();
            PrivateObject pss = new PrivateObject(sss);
            Type[] myTypes = new Type[] { typeof(string) };
            object[] args = new object[] { "xmltest1.xml" };
            pss.Invoke("GetSavedVersion", myTypes, args);
        }
        [TestMethod]
        [ExpectedException(typeof(TargetInvocationException))]
        public void GetDirectDependencyTest03()
        {
            Spreadsheet sss = new Spreadsheet();
            PrivateObject pss = new PrivateObject(sss);
            Type[] myTypes = new Type[] { typeof(string) };
            object[] args = new object[] { "xmltest2.xml" };
            pss.Invoke("GetSavedVersion", myTypes, args);
        }
        [TestMethod]
        [ExpectedException(typeof(TargetInvocationException))]
        public void SetCellContentsTest01()
        {
            Spreadsheet sss = new Spreadsheet();
            PrivateObject pss = new PrivateObject(sss);
            sss.SetContentsOfCell("a1", "=3");
            Formula f = (Formula)sss.GetCellContents("a1");
            Type[] myTypes = new Type[] { typeof(string), typeof(Formula) };
            object[] args = new object[] { null, f };
            pss.Invoke("SetCellContents", myTypes, args);
        }
        [TestMethod]
        [ExpectedException(typeof(TargetInvocationException))]
        public void SetCellContentsTest3()
        {
            Spreadsheet sss = new Spreadsheet();
            PrivateObject pss = new PrivateObject(sss);
            Type[] myTypes = new Type[] { typeof(string), typeof(string) };
            object[] args = new object[] { "a1", null };
            pss.Invoke("SetCellContents", myTypes, args);
        }
        [TestMethod]
        [ExpectedException(typeof(TargetInvocationException))]
        public void SetCellContentsTest4()
        {
            Spreadsheet sss = new Spreadsheet();
            PrivateObject pss = new PrivateObject(sss);
            Type[] myTypes = new Type[] { typeof(string), typeof(string) };
            object[] args = new object[] { null, "%%" };
            pss.Invoke("SetCellContents", myTypes, args);
        }

        [TestMethod]
        [ExpectedException(typeof(TargetInvocationException))]
        public void SetCellContentsTest()
        {
            Spreadsheet sss = new Spreadsheet();
            PrivateObject pss = new PrivateObject(sss);
            Type[] myTypes = new Type[] { typeof(string), typeof(double) };
            object[] args = new object[] { "$#@", 0.1 };
            pss.Invoke("SetCellContents", myTypes, args);
        }
        [TestMethod]
        [ExpectedException(typeof(TargetInvocationException))]
        public void SetCellContentsTest8()
        {
            Spreadsheet sss = new Spreadsheet();
            PrivateObject pss = new PrivateObject(sss);
            Type[] myTypes = new Type[] { typeof(string), typeof(double) };
            object[] args = new object[] { null, 1.0 };
            pss.Invoke("SetCellContents", myTypes, args);
        }




        [TestMethod()]
        public void SaveTest4()
        {
            AbstractSpreadsheet sss = new Spreadsheet(s => true, s => s, "Yixiong");
            sss.Save("PS5file.txt");
            Assert.AreEqual("Yixiong", new Spreadsheet().GetSavedVersion("PS5file.txt"));
        }
        

        [ExpectedException(typeof(InvalidNameException))]
        [TestMethod]
        public void SetContentsOfCellTest3()
        {
            Spreadsheet sss = new Spreadsheet();
            sss.SetContentsOfCell(null, "1+2");
        }
        [ExpectedException(typeof(InvalidNameException))]
        [TestMethod]
        public void GetNameTest()
        {
            Spreadsheet sss = new Spreadsheet();
            sss.GetCellValue(null);
        }
       
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        [TestMethod]
        public void FileNotFooundTest()
        {
            Spreadsheet sss = new Spreadsheet("ss.xml", s => true, s => s, "1.0");
            
        }
        [TestMethod]
        public void GetContentsEmptyTest()
        {
            Spreadsheet sss = new Spreadsheet();
            Assert.IsTrue(sss.GetCellContents("X1").ToString() == "");

        }
        [ExpectedException(typeof(InvalidNameException))]
        [TestMethod]
        public void GetCellContentTest()
        {
            Spreadsheet sss = new Spreadsheet();
            sss.GetCellContents(null);
        }
        [ExpectedException(typeof(InvalidNameException))]
        [TestMethod]
        public void GetCellContentTest2()
        {
            Spreadsheet sss = new Spreadsheet();
            sss.GetCellContents("6");
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void SetCellContentsTest1()
        {
            Spreadsheet sss = new Spreadsheet();
            string s = null;
            sss.SetContentsOfCell("X1", s);
        }
        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void SetCellContentsTest2e()
        {
            Spreadsheet sss = new Spreadsheet();
            List<string> result = new List<string>(sss.SetContentsOfCell("12", null));
        }
       
        }
}

