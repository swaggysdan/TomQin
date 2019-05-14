// Skeleton implementation written by Joe Zachary for CS 3500, September 2013.
// Version 1.1 (Fixed error in comment for RemoveDependency.)
// Version 1.2 - Daniel Kopta 
//               (Clarified meaning of dependent and dependee.)
//               (Clarified names in solution/project structure.)
// Author Yixiong Qin

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpreadsheetUtilities
{

    /// <summary>
    /// (s1,t1) is an ordered pair of strings
    /// t1 depends on s1; s1 must be evaluated before t1
    /// 
    /// A DependencyGraph can be modeled as a set of ordered pairs of strings.  Two ordered pairs
    /// (s1,t1) and (s2,t2) are considered equal if and only if s1 equals s2 and t1 equals t2.
    /// Recall that sets never contain duplicates.  If an attempt is made to add an element to a 
    /// set, and the element is already in the set, the set remains unchanged.
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that (s,t) is in DG is called dependents(s).
    ///        (The set of things that depend on s)    
    ///        
    ///    (2) If s is a string, the set of all strings t such that (t,s) is in DG is called dependees(s).
    ///        (The set of things that s depends on) 
    //
    // For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    //     dependents("a") = {"b", "c"}
    //     dependents("b") = {"d"}
    //     dependents("c") = {}
    //     dependents("d") = {"d"}
    //     dependees("a") = {}
    //     dependees("b") = {"a"}
    //     dependees("c") = {"a"}
    //     dependees("d") = {"b", "d"}
    /// </summary>
    public class DependencyGraph
    {
        /// <summary>
        /// Creates an empty DependencyGraph.
        /// </summary>
        private Dictionary<String, HashSet<String>> dependents;
        private Dictionary<String, HashSet<String>> dependees;
        public DependencyGraph()
        {
            this.dependents = new Dictionary<string, HashSet<String>>();
            this.dependees = new Dictionary<string, HashSet<String>>();
        }


        /// <summary>
        /// The number of ordered pairs in the DependencyGraph.
        /// dependents[key]
        /// </summary>
        public int Size
        {
            get
            {
                int count = 0;

                foreach (String temp in this.dependents.Keys)
                {
                    count += dependents[temp].Count;
                }
                return count;
            }
        }


        /// <summary>
        /// The size of dependees(s).
        /// This property is an example of an indexer.  If dg is a DependencyGraph, you would
        /// invoke it like this:
        /// dg["a"]
        /// It should return the size of dependees("a")
        /// </summary>
        public int this[string s]
        {
            get
            {
                if (dependees.ContainsKey(s))
                {
                    return dependees[s].Count;
                }
                else
                {
                    return 0;
                }

            }
        }


        /// <summary>
        /// Reports whether dependents(s) is non-empty.
        /// </summary>
        public bool HasDependents(string s)
        {
            if (dependents.ContainsKey(s))
            {
                if (dependents[s].Count != 0)
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Reports whether dependees(s) is non-empty.
        /// </summary>
        public bool HasDependees(string s)
        {
            if (dependees.ContainsKey(s))
            {
                if (dependees[s].Count != 0)
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Enumerates dependents(s).
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            if (dependents.ContainsKey(s))
            {
                return dependents[s];
            }
            return new HashSet<String>();
        }

        /// <summary>
        /// Enumerates dependees(s).
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            if (dependees.ContainsKey(s))
            {
                return dependees[s];
            }
            return new HashSet<String>();
        }
        /// <summary>
        /// Helper method for adding dependent to dependee.
        /// </summary>
        /// <param name="s">dependee</param>
        /// <param name="t">dependent</param>
        private void addDependent(String s, String t)
        {
            HashSet<String> temp = new HashSet<string>();
            temp.Add(t);
            dependents[s] = temp;
        }
        /// <summary>
        /// Helper method for adding depenee to dependent.
        /// </summary>
        /// <param name="s">dependee</param>
        /// <param name="t">dependent</param>
        private void addDependee(String s, String t)
        {
            if (dependees.ContainsKey(t))
            {
                HashSet<String> temp = dependees[t];
                temp.Add(s);
                dependees[t] = temp;
            }
            else
            {
                HashSet<String> temp = new HashSet<string>();
                temp.Add(s);
                dependees[t] = temp;
            }
        }
        /// <summary>
        /// <para>Adds the ordered pair (s,t), if it doesn't exist</para>
        /// 
        /// <para>This should be thought of as:</para>   
        /// 
        ///   t depends on s
        ///
        /// </summary>
        /// <param name="s"> s must be evaluated first. T depends on S</param>
        /// <param name="t"> t cannot be evaluated until s is</param>        /// 
        public void AddDependency(string s, string t)
        {
            if (dependents.ContainsKey(s))
            {
                HashSet<String> temp = dependents[s];
                temp.Add(t);
                addDependee(s, t);
            }
            else
            {
                addDependent(s, t);
                addDependee(s, t);
            }
        }


        /// <summary>
        /// Removes the ordered pair (s,t), if it exists
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        public void RemoveDependency(string s, string t)
        {
            if (dependents.ContainsKey(s))
            {
                HashSet<String> temp = dependents[s];
                if (temp.Contains(t))
                {
                    temp.Remove(t);
                    dependents[s] = temp;
                    HashSet<String> dees = dependees[t];
                    dees.Remove(s);
                }
            }
        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (s,r).  Then, for each
        /// t in newDependents, adds the ordered pair (s,t).
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            if (dependents.ContainsKey(s))
            {
                HashSet<String> temp = dependents[s];
                foreach (String getToken in temp)
                {
                    dependees[getToken].Remove(s);

                }
                dependents[s] = new HashSet<string>();
                
            }
            
            
                foreach (String newToken in newDependents)
                {
                    AddDependency(s, newToken);
                }
            
        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (r,s).  Then, for each 
        /// t in newDependees, adds the ordered pair (t,s).
        /// </summary>
        public void ReplaceDependees(string s, IEnumerable<string> newDependees)
        {
            if (dependees.ContainsKey(s))
            {
                HashSet<String> temp = dependees[s];
                String[] check=temp.ToArray();
                foreach (String getToken in check)
                {
                    RemoveDependency(getToken, s);
                   

                }
                
            }
            
                foreach (String newToken in newDependees)
                {
                    AddDependency(newToken, s);
                }
            
        }

    }

}

