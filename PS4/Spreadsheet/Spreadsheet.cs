using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using SpreadsheetUtilities;
/// <summary>
/// name: Yixiong Qin
/// </summary>
namespace SS
{
    public class Spreadsheet : AbstractSpreadsheet
    {
        class cell
        {
            private String name;
            private object element;

            public cell(String name, object element)
            {
                this.name = name;
                this.element = element;

            }
            public String getName()
            {
                return name;
            }
            public object getElement()
            {
                return element;

            }
            public object value
            {
                set;
                get;
            }
        }
        private bool checkName(String token)
        {
            return Regex.IsMatch(token, @"^[a-zA-Z]+[1-9]{1}[0-9]*$");
        }
        /// <summary>
        /// a constructor of Spreadsheet
        /// 
        /// </summary>
        private DependencyGraph graph;
        private Dictionary<String, cell> dictionary;



        public override bool Changed
        {
            get;
            protected set;
        }

        public Spreadsheet()
            : base(s => true, s => s, "default")
        {
            Changed = false;
            graph = new DependencyGraph();
            this.dictionary = new Dictionary<string, cell>();

        }
        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version) : base(isValid, normalize, version)
        {
            Changed = false;
            this.IsValid = isValid;
            graph = new DependencyGraph();
            this.dictionary = new Dictionary<string, cell>();
        }
        public Spreadsheet(String path, Func<string, bool> isValid, Func<string, string> normalize, string version)
            : base(isValid, normalize, version)
        {

            this.IsValid = isValid;
            this.Version = version;
            this.Normalize = normalize;
            graph = new DependencyGraph();
            this.dictionary = new Dictionary<string, cell>();
            getSaved(path);
        }
        public void getSaved(String path)
        {
            try
            {
                using (XmlReader reader = XmlReader.Create(path))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            if (reader.Name == "spreadsheet")
                            {
                                String version = reader["version"];
                                if (this.Version != version)
                                {
                                    throw new SpreadsheetReadWriteException("");
                                }



                                continue;
                            }
                            if (reader.Name == "cell")
                            {
                                reader.Read();
                                String name = reader.ReadElementContentAsString();
                                String content = reader.ReadElementContentAsString();
                                SetContentsOfCell(name, content);

                            }
                        }

                    }
                }
            }
            catch
            {
                throw new SpreadsheetReadWriteException("");
            }

            //throw new SpreadsheetReadWriteException("");
        }
        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        public override object GetCellContents(string name)
        {
            if (name == null || checkName(name) == false)
            {
                throw new InvalidNameException();

            }
            else
            {
                if (dictionary.ContainsKey(getName(name)))
                {
                    return dictionary[getName(name)].getElement();
                }
                else
                {
                    return "";
                }
            }

        }
        /// <summary>
        /// Enumerates the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            HashSet<string> result = new HashSet<string>();
            foreach (cell temp in dictionary.Values)
            {
                if (temp != null)
                {
                    result.Add(temp.getName());
                }
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public double lookup(String name)
        {
            //     double result;
            //record:using cast directly in lookup instead of using double.tryparse. 
            if (dictionary.ContainsKey(name))
            {
                if (dictionary[name].value is double)
                {
                    return (double)(dictionary[name].value);
                }
            }

            throw new ArgumentException();

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object calculate(String name)
        {
            if (dictionary[name].getElement() is Formula)
            {
                return (new Formula(this.dictionary[name].getElement().ToString())).Evaluate(lookup);
            }

            else
            {
                return dictionary[name].getElement();
            }
        }
        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes number.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        protected override ISet<string> SetCellContents(string name, double number)
        {
            if (name == null || checkName(name) == false)
            {
                throw new InvalidNameException();
            }
            else
            {
                cell temp = new cell(name, number);
                temp.value = number;
                HashSet<String> result = new HashSet<string>();
                graph.ReplaceDependents(name, result);
                HashSet<String> check = new HashSet<string>(GetCellsToRecalculate(name));
                dictionary[name] = temp;
                //result.Add(name);
                //dictionary[name].value = number;
                foreach (String token in GetCellsToRecalculate(name))
                {
                    if (dictionary.ContainsKey(token))
                    {
                        try
                        {
                            dictionary[token].value = calculate(token);
                        }
                        catch
                        {
                            dictionary[token].value = new FormulaError();
                        }
                    }



                }
                Changed = true;
                return check;

            }


        }
        /// <summary>
        /// If text is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes text.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        protected override ISet<string> SetCellContents(string name, string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException();
            }
            if (name == null || checkName(name) == false)
            {
                throw new InvalidNameException();
            }
            else
            {
                HashSet<String> result = new HashSet<string>();

                cell temp = new cell(name, text);

                //temp.value = text;
                graph.ReplaceDependents(name, result);
                HashSet<String> check = new HashSet<string>(GetCellsToRecalculate(name));
                dictionary[name] = temp;
                // dictionary[name].value = text;
                foreach (String token in GetCellsToRecalculate(name))
                {
                    if (dictionary.ContainsKey(token))
                    {
                        try
                        {
                            dictionary[token].value = calculate(token);
                        }
                        catch
                        {
                            dictionary[token].value = new FormulaError();
                        }
                    }



                }

                Changed = true;
                return check;


            }


        }
        /// <summary>
        /// If the formula parameter is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if changing the contents of the named cell to be the formula would cause a 
        /// circular dependency, throws a CircularException.  (No change is made to the spreadsheet.)
        /// 
        /// Otherwise, the contents of the named cell becomes formula.  The method returns a
        /// Set consisting of name plus the names of all other cells whose value depends,
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        protected override ISet<string> SetCellContents(string name, Formula formula)
        {
            if (formula == null)
            {
                throw new ArgumentNullException();
            }
            if (name == null || checkName(name) == false)
            {
                throw new InvalidNameException();
            }
            else
            {

                HashSet<String> variables = new HashSet<string>(formula.GetVariables());
                graph.ReplaceDependents(name, variables);
                HashSet<String> result = new HashSet<string>(GetCellsToRecalculate(name));
                cell temp = new cell(name, formula);
                dictionary[name] = temp;
                // result.Add(name);
                foreach (String token in GetCellsToRecalculate(name))
                {
                    if (dictionary.ContainsKey(token))
                    {
                        try
                        {
                            dictionary[token].value = calculate(token);
                        }
                        catch
                        {
                            dictionary[token].value = new FormulaError();
                        }
                    }



                }
                Changed = true;
                return result;
            }


        }
        /// <summary>
        /// If name is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name isn't a valid cell name, throws an InvalidNameException.
        /// 
        /// Otherwise, returns an enumeration, without duplicates, of the names of all cells whose
        /// values depend directly on the value of the named cell.  In other words, returns
        /// an enumeration, without duplicates, of the names of all cells that contain
        /// formulas containing name.
        /// 
        /// For example, suppose that
        /// A1 contains 3
        /// B1 contains the formula A1 * A1
        /// C1 contains the formula B1 + A1
        /// D1 contains the formula B1 - C1
        /// The direct dependents of A1 are B1 and C1
        /// </summary>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            if (name == null)
            {
                throw new ArgumentException();
            }
            if (checkName(name) == false)
            {
                throw new InvalidNameException();
            }
            else
            {

                HashSet<String> result = new HashSet<string>(graph.GetDependees(name));
                return result;
            }

        }
        // ADDED FOR PS5
        /// <summary>
        /// Returns the version information of the spreadsheet saved in the named file.
        /// If there are any problems opening, reading, or closing the file, the method
        /// should throw a SpreadsheetReadWriteException with an explanatory message.
        /// </summary>
        public override string GetSavedVersion(string filename)
        {

            using (XmlReader reader = XmlReader.Create(filename))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        if (reader.Name == "spreadsheet")
                        {
                            return reader["version"];
                        }
                        else
                        {
                            throw new SpreadsheetReadWriteException("");
                        }
                    }
                    else
                    {
                        throw new SpreadsheetReadWriteException("");
                    }
                }
            }


            throw new SpreadsheetReadWriteException("");



        }
        // ADDED FOR PS5
        /// <summary>
        /// Writes the contents of this spreadsheet to the named file using an XML format.
        /// The XML elements should be structured as follows:
        /// 
        /// <spreadsheet version="version information goes here">
        /// 
        /// <cell>
        /// <name>
        /// cell name goes here
        /// </name>
        /// <contents>
        /// cell contents goes here
        /// </contents>    
        /// </cell>
        /// 
        /// </spreadsheet>
        /// 
        /// There should be one cell element for each non-empty cell in the spreadsheet.  
        /// If the cell contains a string, it should be written as the contents.  
        /// If the cell contains a double d, d.ToString() should be written as the contents.  
        /// If the cell contains a Formula f, f.ToString() with "=" prepended should be written as the contents.
        /// 
        /// If there are any problems opening, writing, or closing the file, the method should throw a
        /// SpreadsheetReadWriteException with an explanatory message.
        /// </summary>
        public override void Save(string filename)
        {
            try
            {
                using (XmlWriter writer = XmlWriter.Create(filename))
                {
                    writer.WriteStartDocument();
                    Changed = false;
                    writer.WriteStartElement("spreadsheet");
                    writer.WriteAttributeString("version", Version);
                    foreach (String temp in GetNamesOfAllNonemptyCells())
                    {
                        writer.WriteStartElement("cell");
                        writer.WriteElementString("name", temp);
                        if (dictionary[temp].getElement() is Formula)
                        {
                            writer.WriteElementString("contents", "=" + dictionary[temp].getElement());
                        }
                        else
                        {
                            writer.WriteElementString("contents", dictionary[temp].getElement().ToString());
                        }
                        writer.WriteFullEndElement();

                    }
                    writer.WriteEndElement();
                    writer.WriteEndDocument();

                }
            }
            catch
            {
                throw new SpreadsheetReadWriteException("");
            }
        }
        // ADDED FOR PS5
        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the value (as opposed to the contents) of the named cell.  The return
        /// value should be either a string, a double, or a SpreadsheetUtilities.FormulaError.
        /// </summary>
        public override object GetCellValue(string name)
        {

            if (name == null)
            {
                throw new InvalidNameException();
            }
            String newName = getName(name);
            if (dictionary.ContainsKey(newName))
            {
                return dictionary[newName].value;
            }
            else
            {
                return "";
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string getName(String name)
        {
            if (checkName(name) == true)
            {

                if (this.IsValid(name) == true)
                {
                    return this.Normalize(name);
                }
                else
                {
                    throw new InvalidNameException();
                }


            }
            else
            {
                throw new InvalidNameException();
            }
        }
        // ADDED FOR PS5
        /// <summary>
        /// If content is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if content parses as a double, the contents of the named
        /// cell becomes that double.
        /// 
        /// Otherwise, if content begins with the character '=', an attempt is made
        /// to parse the remainder of content into a Formula f using the Formula
        /// constructor.  There are then three possibilities:
        /// 
        ///   (1) If the remainder of content cannot be parsed into a Formula, a 
        ///       SpreadsheetUtilities.FormulaFormatException is thrown.
        ///       
        ///   (2) Otherwise, if changing the contents of the named cell to be f
        ///       would cause a circular dependency, a CircularException is thrown.
        ///       
        ///   (3) Otherwise, the contents of the named cell becomes f.
        /// 
        /// Otherwise, the contents of the named cell becomes content.
        /// 
        /// If an exception is not thrown, the method returns a set consisting of
        /// name plus the names of all other cells whose value depends, directly
        /// or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetContentsOfCell(string name, string content)
        {
            if (content == null)
            {
                throw new ArgumentNullException();
            }
            else if (name == null || checkName(name) == false)
            {
                throw new InvalidNameException();
            }
            String newName = getName(name);
            double number;
            if (content == "")
            {
                return new HashSet<String>();
            }
            if (double.TryParse(content, out number) == true)
            {

                return this.SetCellContents(newName, number);

            }
            else if (content[0] == '=')
            {
                String formula = content.Substring(1);
                Formula newForm = new Formula(formula, this.Normalize, this.IsValid);
                return this.SetCellContents(newName, newForm);

            }
            else
            {
                return this.SetCellContents(newName, content);
            }

        }

    }
}
