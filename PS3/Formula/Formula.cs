// Skeleton written by Joe Zachary for CS 3500, September 2013
// Read the entire skeleton carefully and completely before you
// do anything else!

// Version 1.1 (9/22/13 11:45 a.m.)

// Change log:
//  (Version 1.1) Repaired mistake in GetTokens
//  (Version 1.1) Changed specification of second constructor to
//                clarify description of how validation works

// (Daniel Kopta) 
// Version 1.2 (9/10/17) 

// Change log:
//  (Version 1.2) Changed the definition of equality with regards
//                to numeric tokens



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace SpreadsheetUtilities
{
    /// <summary>
    ///String extension class and the method signatures explained itself.
    ///
    /// </summary>
    /// <author>Yixiong Qin</author>
    public static class StringExtend
    {
   
      
        /// <summary>
        /// Parse the strings into tokens
        /// </summary>
        /// <param name="tokens">  intput string   </param>
        /// <returns>   an array of split tokens </returns>
        private static string[] substringsOfarray(string tokens)
        {

            string[] substrings = Regex.Split(tokens, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
            return substrings;

        }
        public static bool checkToken(this String temp)
        {
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            double number;
            if (Regex.IsMatch(temp, lpPattern) == false && Regex.IsMatch(temp, rpPattern) == false && Regex.IsMatch(temp, opPattern) == false
                && Regex.IsMatch(temp, varPattern) == false && Double.TryParse(temp, out number) == false)
            {
                return false;
            }
            else
            {

                return true;
            }
        }
        public static bool numCheck(this String temp)
        {
            double number;
            if (double.TryParse(temp,out number) == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool varCheck(this String temp)
        {
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            if (Regex.IsMatch(temp, varPattern))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //Opening parenthesis check.
        public static bool isOpenP(this String temp)
        {
            String lpPattern = @"\(";
            if (Regex.IsMatch(temp, lpPattern))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public static bool checkFirst(this String temp)
        {
            if (temp.numCheck() == false && temp.varCheck() == false && temp.isOpenP() == false)
            {
                return false;
            }
            else
            {

                return true;
            }
        }
        public static bool checkLast(this String temp)
        {
            String rpPattern = @"\)";
            if (temp.numCheck() == false && temp.varCheck() == false && Regex.IsMatch(temp,rpPattern) == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  The allowed symbols are non-negative numbers written using double-precision 
    /// floating-point syntax; variables that consist of a letter or underscore followed by 
    /// zero or more letters, underscores, or digits; parentheses; and the four operator 
    /// symbols +, -, *, and /.  
    /// 
    /// Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
    /// a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable; 
    /// and "x 23" consists of a variable "x" and a number "23".
    /// 
    /// Associated with every formula are two delegates:  a normalizer and a validator.  The
    /// normalizer is used to convert variables into a canonical form, and the validator is used
    /// to add extra restrictions on the validity of a variable (beyond the standard requirement 
    /// that it consist of a letter or underscore followed by zero or more letters, underscores,
    /// or digits.)  Their use is described in detail in the constructor and method comments.
    /// </summary>
    public class Formula
    {
        private Boolean isOperator(String token)
        {
            if (token == "+" || token == "-" || token == "*" || token == "/" || token == "(" || token == ")")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// The alogrithm for higher precedence arithmetic operations
        /// </summary>
        /// <param name="another">input values</param>
        /// <param name="operater">operator</param>
        private  object higherArithmetic(double another, String operater)
        {
            if (value.Count == 0)
            {
                return new FormulaError("there is no number to perform the operation for * or /");
            }
            else
            {
                if (operater == "*")
                {
                    double first = value.Pop();
                    operat.Pop();
                    double result = first * another;
                    value.Push(result);

                }
                else
                {
                    double first = value.Pop();
                    operat.Pop();
                    if (another != 0)
                    {
                        double result = first / another;
                        value.Push(result);
                    }
                    else
                    {
                        return  new FormulaError("divisor can not be zero");
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// Check whether the input token is a a valid variable
        /// </summary>
        /// <param name="token">input string</param>
        /// <returns>True if the token was passed in is a variable</returns>
        private static Boolean isVariable(String token)
        {

            return Regex.IsMatch(token, @"[a-zA-Z]+\d+");


        }
        /// <summary>
        /// This method performs the evaluation for the string that is passed in.
        /// </summary>
        /// <param name="exp">input string to be evaluated</param>
        /// <param name="variableEvaluator">parameter of the delegate method</param>
        /// <returns>Inter value</returns>

        /// <summary>
        /// this is a helper method while the token is a ), while the token is a * or /,
        /// we need to do calculation, but it's different with the normal * or / operation, 
        /// it need to pop the value stack two times to do the operation. but don't need to push the operator
        /// to the stack.
        /// </summary>
        private object dealWithHigherArithmetic()
        {
            if (operat.Count != 0)
            {
                if (operat.Peek() == "*" || operat.Peek() == "/")
                {
                    if (value.Count >= 2)
                    {
                        double right = value.Pop();
                        double left = value.Pop();
                        String getOperator = operat.Pop();
                        if (getOperator == "*")
                        {
                            double result = left * right;
                            value.Push(result);
                        }
                        else
                        {
                            if (right != 0)
                            {
                                double result = left / right;
                                value.Push(result);
                            }
                            else
                            {
                                return new FormulaError("Invalid operation");
                            }
                        }
                    }
                    else
                    {
                        return new FormulaError("Invalid operation");
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// this is a helper method while the token is a ), while the token is a + or -,
        /// we need to do calculation, but it's different with the normal + or - operation, 
        /// it need to pop the value stack two times to do the operation. but don't need to push the operator back
        /// </summary>
        private  object dealWithLowerArithmetic()
        {
            if (operat.Count != 0)
            {
                if (operat.Peek() == "+" || operat.Peek() == "-")
                {
                    if (value.Count >= 2)
                    {
                        double right = value.Pop();
                        double left = value.Pop();
                        String getOperator = operat.Pop();
                        if (getOperator == "+")
                        {
                            double result = left + right;
                            value.Push(result);
                        }
                        else
                        {
                            double result = left - right;
                            value.Push(result);
                        }
                    }
                    else
                    {
                        return  new FormulaError("Invalid operation");
                    }
                }
            }
            return true;


        }
        /// <summary>
        /// The alogrithm for lower precedence arithmetic operations.
        /// </summary>
        /// <param name="operater">operator</param>
        private object  lowerArithmetic(String operater)
        {
            if (operat.Count != 0)
            {
                if (operat.Peek() == "+" || operat.Peek() == "-")
                {
                    if (value.Count >= 2)
                    {
                        double right = value.Pop();
                        double left = value.Pop();
                        String getOperator = operat.Pop();
                        if (getOperator == "+")
                        {
                            double result = left + right;
                            value.Push(result);
                        }
                        else
                        {
                            double result = left - right;
                            value.Push(result);
                        }
                    }
                    else
                    {
                        return  new FormulaError("Invalid operation");
                    }
                }
            }
            operat.Push(operater);
            return true;
        }
        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically invalid,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer is the identity function, and the associated validator
        /// maps every string to true.  
        /// </summary>
        private IEnumerable<String> formula;
        private HashSet<String> varibles;
        private  Stack<double> value;
        private  Stack<String> operat;
        public Formula(String formula) :
            this(formula, s => s, s => true)
        {
        }
        private bool validCheck(String token)
        {
            if (token.numCheck() == true || token.varCheck() == true || token.isOpenP() == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool checkOP(String token)
        {
            if (token == "+" || token == "-" || token == "*" || token == "/")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool endCheck(String token)
        {
            if (checkOP(token) == true || token == ")")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically incorrect,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer and validator are the second and third parameters,
        /// respectively.  
        /// 
        /// If the formula contains a variable v such that normalize(v) is not a legal variable, 
        /// throws a FormulaFormatException with an explanatory message. 
        /// 
        /// If the formula contains a variable v such that isValid(normalize(v)) is false,
        /// throws a FormulaFormatException with an explanatory message.
        /// 
        /// Suppose that N is a method that converts all the letters in a string to upper case, and
        /// that V is a method that returns true only if a string consists of one letter followed
        /// by one digit.  Then:
        /// 
        /// new Formula("x2+y3", N, V) should succeed
        /// new Formula("x+y3", N, V) should throw an exception, since V(N("x")) is false
        /// new Formula("2x+y3", N, V) should throw an exception, since "2x+y3" is syntactically incorrect.
        /// </summary>
        public Formula(String formula, Func<string,string> normalize, Func<string,bool> isValid)
        {
            varibles = new HashSet<string>();
            var tokens = GetTokens(formula);
            String[] getToken=tokens.ToArray();
            Stack<String> checkparthese = new Stack<string>();
            if (getToken.Length < 1)
            {
                throw new FormulaFormatException("the formula has at lease one token");
            }
            for(int i=0;i<getToken.Length;i++)
            {
                if (getToken[i].checkToken() == false)
                {
                    throw new FormulaFormatException("contains unknown token");
                }
                else
                {
                    if (i == 0)
                    {
                        if (getToken[i].checkFirst() == false)
                        {
                            throw new FormulaFormatException("the first token is not valid");
                        }
                    }
                    if (i == getToken.Length - 1)
                    {
                        if (getToken[i].checkLast() == false)
                        {
                            throw new FormulaFormatException("the last token is not valid");
                        } 
                    }
                    if (getToken[i].varCheck() == true)
                    {
                        if (normalize(getToken[i]).varCheck() == false)
                        {
                            throw new FormulaFormatException("it's not a valid variable.");
                        }
                        else
                        {
                            if (isValid(normalize(getToken[i])) == false)
                            {
                                throw new FormulaFormatException("it's not a valid variable.");
                            }
                            else
                            {
                                getToken[i] = normalize(getToken[i]);
                                varibles.Add(getToken[i]);
                            }
                        }
                    }
                    if (getToken[i] == "(")
                    {
                        checkparthese.Push("(");
                      
                    }
                    if (getToken[i] == ")")
                    {   
                        if (checkparthese.Count != 0)
                        {
                            checkparthese.Pop();
                        }
                        else
                        {
                            throw new FormulaFormatException("miss a (");
                        }
                    }
                    if (getToken[i].isOpenP() == true || checkOP(getToken[i]) == true)
                    {
                        if (i + 1 != getToken.Length)
                        {
                            if(validCheck(getToken[i + 1])==false)
                            {
                                throw new FormulaFormatException("fail parathesis follow token");
                            }
                        }
                        else
                        {
                            throw new FormulaFormatException("only a ( with no other things");
                        }
                    }
                    if (getToken[i].numCheck() == true || getToken[i].varCheck() == true||getToken[i]==")")
                    {
                        if (i + 1 != getToken.Length)
                        {
                            if (endCheck(getToken[i + 1]) == false)
                            {
                                throw new FormulaFormatException("fail end check");
                            }
                        }
                    }





                }
            }
            if (checkparthese.Count != 0)
            {
                throw new FormulaFormatException("extra ( appears");
            }
            this.formula = getToken;

        }

        /// <summary>
        /// Evaluates this Formula, using the lookup delegate to determine the values of
        /// variables.  When a variable symbol v needs to be determined, it should be looked up
        /// via lookup(normalize(v)). (Here, normalize is the normalizer that was passed to 
        /// the constructor.)
        /// 
        /// For example, if L("x") is 2, L("X") is 4, and N is a method that converts all the letters 
        /// in a string to upper case:
        /// 
        /// new Formula("x+7", N, s => true).Evaluate(L) is 11
        /// new Formula("x+7").Evaluate(L) is 9
        /// 
        /// Given a variable symbol as its parameter, lookup returns the variable's value 
        /// (if it has one) or throws an ArgumentException (otherwise).
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, the value is returned.  Otherwise, a FormulaError is returned.  
        /// The Reason property of the FormulaError should have a meaningful explanation.
        ///
        /// This method should never throw an exception.
        /// </summary>
        public object Evaluate(Func<string, double> lookup)
        {
            this.value = new Stack<double>();
            this.operat = new Stack<string>();
            String[] expression = this.formula.ToArray();
            for (int i = 0; i < expression.Length; i++)
            {//current posiont in the array
                String tempToken = expression[i].Trim();
                if (tempToken == "")
                {
                    continue;
                }
                double inputValue;
                //Validates wheter the temToken is an inter value
                if (double.TryParse(tempToken, out inputValue) == true)
                {
                    if (operat.Count != 0)
                    {
                        if (operat.Peek() == "*" || operat.Peek() == "/")
                        {
                            higherArithmetic(inputValue, operat.Peek());
                        }
                        else
                        {
                            value.Push(inputValue);
                        }
                    }
                    else
                    {
                        value.Push(inputValue);
                    }
                }
                else if (isVariable(tempToken) == true)
                {
                    try
                    {
                        inputValue = lookup(tempToken);
                        if (operat.Count != 0)
                        {
                            if (operat.Peek() == "*" || operat.Peek() == "/")
                            {
                                higherArithmetic(inputValue, operat.Peek());
                            }
                            else
                            {
                                value.Push(inputValue);
                            }
                        }
                        else
                        {
                            value.Push(inputValue);
                        }
                    }
                    catch
                    {
                        return new FormulaError("undefined variable");
                    }
                }
                else if (isOperator(tempToken) == true)
                {
                    if (tempToken == "+" || tempToken == "-")
                    {
                        lowerArithmetic(tempToken);
                    }
                    else if (tempToken == "*" || tempToken == "/")
                    {
                        operat.Push(tempToken);
                    }
                    else if (tempToken == "(")
                    {
                        operat.Push(tempToken);
                    }
                    else if (tempToken == ")")
                    {
                        dealWithLowerArithmetic();
                        if (operat.Count != 0)
                        {
                            if (operat.Peek() == "(")
                            {
                                operat.Pop();
                            }
                            
                        }
                        
                        dealWithHigherArithmetic();
                    }


                }
                else
                {
                    return  new FormulaError("unknown token");
                }

            }
            if (operat.Count == 0)
            {
                if (value.Count == 1)
                {
                    double result = value.Pop();
                    return result;
                }
                else
                {
                    return  new FormulaError("Invalid operation");
                }
            }
            else
            {
                if (operat.Count == 1)
                {
                    if (value.Count == 2)
                    {
                        double right = value.Pop();
                        double left = value.Pop();
                        String getOperator = operat.Pop();
                        if (getOperator == "+")
                        {
                            double result = left + right;
                            return result;
                        }
                        else
                        {
                            double result = left - right;
                            return result;
                        }
                    }
                    else
                    {
                        return  new FormulaError("");
                    }
                }
                else
                {
                    return new FormulaError("There is no operand to perform the arithmetic");
                }
            }

        }

        /// <summary>
        /// Enumerates the normalized versions of all of the variables that occur in this 
        /// formula.  No normalization may appear more than once in the enumeration, even 
        /// if it appears more than once in this Formula.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x+y*z", N, s => true).GetVariables() should enumerate "X", "Y", and "Z"
        /// new Formula("x+X*z", N, s => true).GetVariables() should enumerate "X" and "Z".
        /// new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
        /// </summary>
        public IEnumerable<String> GetVariables()
        {
            return varibles;
        }

        /// <summary>
        /// Returns a string containing no spaces which, if passed to the Formula
        /// constructor, will produce a Formula f such that this.Equals(f).  All of the
        /// variables in the string should be normalized.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x + y", N, s => true).ToString() should return "X+Y"
        /// new Formula("x + Y").ToString() should return "x+Y"
        /// </summary>
        public override string ToString()
        {
            String result="";
            double number = 0.0;
            foreach(String temp in this.formula)
            {
                if (temp.numCheck())
                {
                    double.TryParse(temp, out number);
                    result += number;
                    continue;
                }
                result += temp;
            }
            return result;
           
        }

        /// <summary>
        /// If obj is null or obj is not a Formula, returns false.  Otherwise, reports
        /// whether or not this Formula and obj are equal.
        /// 
        /// Two Formulae are considered equal if they consist of the same tokens in the
        /// same order.  To determine token equality, all tokens are compared as strings 
        /// except for numeric tokens and variable tokens.
        /// Numeric tokens are considered equal if they are equal after being "normalized" 
        /// by C#'s standard conversion from string to double, then back to string. This 
        /// eliminates any inconsistencies due to limited floating point precision.
        /// Variable tokens are considered equal if their normalized forms are equal, as 
        /// defined by the provided normalizer.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        ///  
        /// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
        /// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
        /// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
        /// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj == null||obj is Formula==false)
            {
                return false;
            }
            Formula temp = (Formula)obj;
            double left = 0.0;
            double right = 0.0;
            string[] lefteq = this.formula.ToArray();
            string[] righteq = temp.formula.ToArray();
            if (lefteq.Length==righteq.Length)
            {
                for(int i = 0; i < lefteq.Length; i++)
                {
                    if (double.TryParse(lefteq[i],out left) == true)
                    {
                        if(double.TryParse(righteq[i],out right) == true)
                        {
                            if (left!=right)
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        if (lefteq[i] != righteq[i])
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
                
            
            
        }

        /// <summary>
        /// Reports whether f1 == f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return true.  If one is
        /// null and one is not, this method should return false.
        /// </summary>
        public static bool operator ==(Formula f1, Formula f2)
        {
            if (Object.ReferenceEquals(f1,null) && object.ReferenceEquals(f2,null))
            {
                return true;
            }
            if (Object.ReferenceEquals(f1, null) && !Object.ReferenceEquals(f2, null) || !Object.ReferenceEquals(f1, null) && Object.ReferenceEquals(f2, null))
            {
                return false;
            }
            if (f1.Equals(f2))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Reports whether f1 != f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return false.  If one is
        /// null and one is not, this method should return true.
        /// </summary>
        public static bool operator !=(Formula f1, Formula f2)
        {
            if (Object.ReferenceEquals(f1, null) && object.ReferenceEquals(f2, null))
            {
                return false;
            }
            if (Object.ReferenceEquals(f1, null) && !Object.ReferenceEquals(f2, null) || !Object.ReferenceEquals(f1, null) && Object.ReferenceEquals(f2, null))
            {
                return true;
            }
            if (f1.Equals(f2))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
        /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two 
        /// randomly-generated unequal Formulae have the same hash code should be extremely small.
        /// </summary>
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode() ;
        }

        /// <summary>
        /// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;
        /// right paren; one of the four operator symbols; a string consisting of a letter or underscore
        /// followed by zero or more letters, digits, or underscores; a double literal; and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }

        }
    }

    /// <summary>
    /// Used to report syntactic errors in the argument to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Used as a possible return value of the Formula.Evaluate method.
    /// </summary>
    public struct FormulaError
    {
        /// <summary>
        /// Constructs a FormulaError containing the explanatory reason.
        /// </summary>
        /// <param name="reason"></param>
        public FormulaError(String reason)
            : this()
        {
            Reason = reason;
        }

        /// <summary>
        ///  The reason why this FormulaError was created.
        /// </summary>
        public string Reason { get; private set; }
    }
}
