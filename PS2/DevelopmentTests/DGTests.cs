using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;


namespace DevelopmentTests
{
  /// <summary>
  ///This is a test class for DependencyGraphTest and is intended
  ///to contain all DependencyGraphTest Unit Tests
  ///</summary>
  [TestClass()]
  public class DependencyGraphTest
  {

    /// <summary>
    ///Empty graph should contain nothing
    ///</summary>
    [TestMethod()]
    public void SimpleEmptyTest()
    {
      DependencyGraph t = new DependencyGraph();
      Assert.AreEqual(0, t.Size);
    }

        [TestMethod()]
        public void SimpleEmptyTestSecondOne()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.IsFalse(t.HasDependents(""));
        }
        [TestMethod()]
        public void SimpleEmptyTestSecond1()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.AreEqual(0,t["x"]);
        }


        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
    public void SimpleEmptyRemoveTest()
    {
      DependencyGraph t = new DependencyGraph();
      t.AddDependency("x", "y");
      Assert.AreEqual(1, t.Size);
      t.RemoveDependency("x", "y");
      Assert.AreEqual(0, t.Size);
    }
        [TestMethod()]
        public void SimpleEmptyTest1()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            t.AddDependency("a", "z");
            t.AddDependency("a", "z");
            Assert.IsTrue(t.HasDependees("y"));
            Assert.IsTrue(t.HasDependents("x"));
        }

    /// <summary>
    ///Empty graph should contain nothing
    ///</summary>
    [TestMethod()]
    public void SimpleEmptyTest2()
    {
      DependencyGraph t = new DependencyGraph();
      t.AddDependency("x", "y");
      IEnumerator<string> e1 = t.GetDependees("y").GetEnumerator();
      Assert.IsTrue(e1.MoveNext());
      Assert.AreEqual("x", e1.Current);
      IEnumerator<string> e2 = t.GetDependents("x").GetEnumerator();
      Assert.IsTrue(e2.MoveNext());
      Assert.AreEqual("y", e2.Current);
      t.RemoveDependency("x", "y");
      Assert.IsFalse(t.GetDependees("y").GetEnumerator().MoveNext());
      Assert.IsFalse(t.GetDependents("x").GetEnumerator().MoveNext());
      Assert.IsFalse(t.GetDependees("x").GetEnumerator().MoveNext());
        }


    /// <summary>
    ///Replace on an empty DG shouldn't fail
    ///</summary>
    [TestMethod()]
    public void SimpleReplaceTest()
    {
      DependencyGraph t = new DependencyGraph();
      t.AddDependency("x", "y");
      Assert.AreEqual(t.Size, 1);
      t.RemoveDependency("x", "y");
      t.ReplaceDependents("x", new HashSet<string>());
      t.ReplaceDependees("y", new HashSet<string>());
    }

        [TestMethod()]
        public void SimpleReplaceTest2()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            t.AddDependency("x", "a");
            t.AddDependency("x", "b");
            t.AddDependency("x", "c");
            LinkedList<string> newDependents = new LinkedList<string>();
            newDependents.AddLast("s");
            t.ReplaceDependees("s", new HashSet<string>());
            Assert.IsTrue(t.Size == 4);

        }
        [TestMethod()]
      public void EmptyTest1 ()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            t.AddDependency("x", "y");
            Assert.AreEqual(1, t.Size);
        }
        [TestMethod()]
        public void EmptyTest2()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            t.AddDependency("x", "z");
            t.AddDependency("f", "z");
            t.AddDependency("x", "e");
            HashSet<String> Dependeees1 = new HashSet<string>(t.GetDependees("x"));
            HashSet<String> Dependeees2 = new HashSet<string>(t.GetDependees("y"));
            HashSet<String> Dependeees3 = new HashSet<string>(t.GetDependees("z"));
            HashSet<String> Dependeees4 = new HashSet<string>(t.GetDependees("f"));
            HashSet<String> Dependeees5 = new HashSet<string>(t.GetDependees("e"));
            HashSet<String> Dependentts1 = new HashSet<string>(t.GetDependees("x"));
            HashSet<String> Dependentts2 = new HashSet<string>(t.GetDependees("y"));
            HashSet<String> Dependentts3 = new HashSet<string>(t.GetDependees("z"));
            HashSet<String> Dependentts4 = new HashSet<string>(t.GetDependees("f"));
            HashSet<String> Dependentts5 = new HashSet<string>(t.GetDependees("e"));
            Assert.IsTrue(Dependeees1.Count == 0);
            Assert.IsTrue(Dependentts1.Count == 0);






        }
        [TestMethod()]
        public void NonEmptyTest2 ()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            t.AddDependency("x", "z");
            t.AddDependency("f", "z");
            t.AddDependency("x", "e");
            Assert.AreEqual(0, t["x"]);
            Assert.AreEqual(1, t["y"]);
            Assert.AreEqual(2, t["z"]);
            Assert.AreEqual(0, t["f"]);
            
        }

        [TestMethod()]
        public void EmptyTest3()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.IsFalse(t.HasDependees("x"));
            
        }



        ///<summary>
        ///It should be possibe to have more than one DG at a time.
        ///</summary>
        [TestMethod()]
    public void StaticTest()
    {
      DependencyGraph t1 = new DependencyGraph();
      DependencyGraph t2 = new DependencyGraph();
      t1.AddDependency("x", "y");
      Assert.AreEqual(1, t1.Size);
      Assert.AreEqual(0, t2.Size);
    }
        



    /// <summary>
    ///Non-empty graph contains something
    ///</summary>
    [TestMethod()]
    public void SizeTest()
    {
      DependencyGraph t = new DependencyGraph();
      t.AddDependency("a", "b");
      t.AddDependency("a", "c");
      t.AddDependency("c", "b");
      t.AddDependency("b", "d");
      Assert.AreEqual(4, t.Size);
    }


    /// <summary>
    ///Non-empty graph contains something
    ///</summary>
    [TestMethod()]
    public void SizeTest2()
    {
      DependencyGraph t = new DependencyGraph();
      t.AddDependency("a", "b");
      t.AddDependency("a", "c");
      t.AddDependency("c", "b");
      t.AddDependency("b", "d");

      IEnumerator<string> e = t.GetDependees("a").GetEnumerator();
      Assert.IsFalse(e.MoveNext());

      e = t.GetDependees("b").GetEnumerator();
      Assert.IsTrue(e.MoveNext());
      String s1 = e.Current;
      Assert.IsTrue(e.MoveNext());
      String s2 = e.Current;
      Assert.IsFalse(e.MoveNext());
      Assert.IsTrue(((s1 == "a") && (s2 == "c")) || ((s1 == "c") && (s2 == "a")));

      e = t.GetDependees("c").GetEnumerator();
      Assert.IsTrue(e.MoveNext());
      Assert.AreEqual("a", e.Current);
      Assert.IsFalse(e.MoveNext());

      e = t.GetDependees("d").GetEnumerator();
      Assert.IsTrue(e.MoveNext());
      Assert.AreEqual("b", e.Current);
      Assert.IsFalse(e.MoveNext());
    }


    /// <summary>
    ///Non-empty graph contains something
    ///</summary>
    [TestMethod()]
    public void SizeTest3()
    {
      DependencyGraph t = new DependencyGraph();
      t.AddDependency("a", "b");
      t.AddDependency("a", "c");
      t.AddDependency("a", "b");
      t.AddDependency("c", "b");
      t.AddDependency("b", "d");
      t.AddDependency("c", "b");
      Assert.AreEqual(4, t.Size);
    }





    /// <summary>
    ///Non-empty graph contains something
    ///</summary>
    [TestMethod()]
    public void SizeTest4()
    {
      DependencyGraph t = new DependencyGraph();
      t.AddDependency("x", "y");
      t.AddDependency("a", "b");
      t.AddDependency("a", "c");
      t.AddDependency("a", "d");
      t.AddDependency("c", "b");
      t.RemoveDependency("a", "d");
      t.AddDependency("e", "b");
      t.AddDependency("b", "d");
      t.RemoveDependency("e", "b");
      t.RemoveDependency("x", "y");
      Assert.AreEqual(4, t.Size);
    }


    /// <summary>
    ///Non-empty graph contains something
    ///</summary>
    [TestMethod()]
    public void SizeTest5()
    {
      DependencyGraph t = new DependencyGraph();
      t.AddDependency("x", "b");
      t.AddDependency("a", "z");
      t.ReplaceDependents("b", new HashSet<string>());
      t.AddDependency("y", "b");
      t.ReplaceDependents("a", new HashSet<string>() { "c" });
      t.AddDependency("w", "d");
      t.ReplaceDependees("b", new HashSet<string>() { "a", "c" });
      t.ReplaceDependees("d", new HashSet<string>() { "b" });

      IEnumerator<string> e = t.GetDependees("a").GetEnumerator();
      Assert.IsFalse(e.MoveNext());

      e = t.GetDependees("b").GetEnumerator();
      Assert.IsTrue(e.MoveNext());
      String s1 = e.Current;
      Assert.IsTrue(e.MoveNext());
      String s2 = e.Current;
      Assert.IsFalse(e.MoveNext());
      Assert.IsTrue(((s1 == "a") && (s2 == "c")) || ((s1 == "c") && (s2 == "a")));

      e = t.GetDependees("c").GetEnumerator();
      Assert.IsTrue(e.MoveNext());
      Assert.AreEqual("a", e.Current);
      Assert.IsFalse(e.MoveNext());

      e = t.GetDependees("d").GetEnumerator();
      Assert.IsTrue(e.MoveNext());
      Assert.AreEqual("b", e.Current);
      Assert.IsFalse(e.MoveNext());
    }



    /// <summary>
    ///Using lots of data
    ///</summary>
    [TestMethod()]
    public void StressTest()
    {
      // Dependency graph
      DependencyGraph t = new DependencyGraph();

      // A bunch of strings to use
      const int SIZE = 200;
      string[] letters = new string[SIZE];
      for (int i = 0; i < SIZE; i++)
      {
        letters[i] = ("" + (char)('a' + i));
      }

      // The correct answers
      HashSet<string>[] dents = new HashSet<string>[SIZE];
      HashSet<string>[] dees = new HashSet<string>[SIZE];
      for (int i = 0; i < SIZE; i++)
      {
        dents[i] = new HashSet<string>();
        dees[i] = new HashSet<string>();
      }

      // Add a bunch of dependencies
      for (int i = 0; i < SIZE; i++)
      {
        for (int j = i + 1; j < SIZE; j++)
        {
          t.AddDependency(letters[i], letters[j]);
          dents[i].Add(letters[j]);
          dees[j].Add(letters[i]);
        }
      }

      // Remove a bunch of dependencies
      for (int i = 0; i < SIZE; i++)
      {
        for (int j = i + 4; j < SIZE; j += 4)
        {
          t.RemoveDependency(letters[i], letters[j]);
          dents[i].Remove(letters[j]);
          dees[j].Remove(letters[i]);
        }
      }

      // Add some back
      for (int i = 0; i < SIZE; i++)
      {
        for (int j = i + 1; j < SIZE; j += 2)
        {
          t.AddDependency(letters[i], letters[j]);
          dents[i].Add(letters[j]);
          dees[j].Add(letters[i]);
        }
      }

      // Remove some more
      for (int i = 0; i < SIZE; i += 2)
      {
        for (int j = i + 3; j < SIZE; j += 3)
        {
          t.RemoveDependency(letters[i], letters[j]);
          dents[i].Remove(letters[j]);
          dees[j].Remove(letters[i]);
        }
      }

      // Make sure everything is right
      for (int i = 0; i < SIZE; i++)
      {
        Assert.IsTrue(dents[i].SetEquals(new HashSet<string>(t.GetDependents(letters[i]))));
        Assert.IsTrue(dees[i].SetEquals(new HashSet<string>(t.GetDependees(letters[i]))));
      }
    }

  }
}



