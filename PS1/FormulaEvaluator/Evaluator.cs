using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FormulaEvaluator
{
    /// <summary>
    /// Evaluator class is used to build arithmetic expressions written using standard infix notation. And it will 
    /// throw exceptions if the expressions don't follow the premises. 
    /// </summary>
    public class Evaluator
    {/// <summary>
    /// Delegate lookup method turns input string into integer value
    /// </summary>
    /// <param name="v">input string</param>
    /// <returns>integer value</returns>
        public delegate int Lookup(String v);
        private static Stack<int> value;
        private static Stack<String> operat;
        /// <summary>
        /// The alogrithm for higher precedence arithmetic operations
        /// </summary>
        /// <param name="another">input values</param>
        /// <param name="operater">operator</param>
        private static void higherArithmetic(int another,String operater)
        {
            if (value.Count == 0)
            {
                throw new ArgumentException("there is no number to perform the operation for * or /");
            }
            else
            {
                if (operater == "*")
                {
                    int first = value.Pop();
                    operat.Pop();
                    int result = first * another;
                    value.Push(result);
                    
                }
                else
                {
                    int first = value.Pop();
                    operat.Pop();
                    if (another != 0)
                    {
                        int result = first / another;
                        value.Push(result);
                    }
                    else
                    {
                        throw new ArgumentException("divisor can not be zero");
                    }
                }
            }
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
        public static int Evaluate(String exp, Lookup variableEvaluator)
        {
            value = new Stack<int>();
            operat = new Stack<string>();
            String[] expression = substringsOfarray(exp);
            for(int i = 0; i < expression.Length; i++)
            {//current posiont in the array
                String tempToken = expression[i].Trim();
                if (tempToken == "")
                {
                    continue;
                }
                int inputValue;
                //Validates wheter the temToken is an inter value
                if (Int32.TryParse(tempToken,out inputValue) == true)
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
                }else if (isVariable(tempToken) == true)
                {
                    try
                    {
                        inputValue = variableEvaluator(tempToken);
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
                        throw new ArgumentException("At least two numbers is needed to perform the arithmetic");
                    }
                }else if (isOperator(tempToken) == true)
                {
                    if (tempToken == "+" || tempToken == "-")
                    {
                        lowerArithmetic(tempToken);
                    }
                    else if (tempToken == "*" || tempToken == "/")
                    {
                        operat.Push(tempToken);
                    }else if (tempToken == "(")
                    {
                        operat.Push(tempToken);
                    }else if (tempToken == ")")
                    {
                        dealWithLowerArithmetic();
                        if (operat.Count != 0)
                        {
                            if (operat.Peek() == "(")
                            {
                                operat.Pop();
                            }
                            else
                            {
                                throw new ArgumentException("");
                            }
                        }
                        else
                        {
                            throw new ArgumentException("Invalid operation!");
                        }
                        dealWithHigherArithmetic();
                    }


                }
                else
                {
                    throw new ArgumentException("unknown token");
                }

            }
            if (operat.Count == 0)
            {
                if (value.Count == 1)
                {
                    int result = value.Pop();
                    return result;
                }
                else
                {
                    throw new ArgumentException("Invalid operation");
                }
            }
            else
            {
                if (operat.Count == 1)
                {
                    if (value.Count == 2)
                    {
                        int right = value.Pop();
                        int left = value.Pop();
                        String getOperator = operat.Pop();
                        if (getOperator == "+")
                        {
                            int result = left + right;
                            return result;
                        }
                        else
                        {
                            int result = left - right;
                            return result;
                        }
                    }
                    else
                    {
                        throw new ArgumentException("");
                    }
                }
                else
                {
                    throw new ArgumentException("There is no operand to perform the arithmetic");
                }
            }

            
        }
        /// <summary>
        /// this is a helper method while the token is a ), while the token is a * or /,
        /// we need to do calculation, but it's different with the normal * or / operation, 
        /// it need to pop the value stack two times to do the operation. but don't need to push the operator
        /// to the stack.
        /// </summary>
        private static void dealWithHigherArithmetic()
        {
            if (operat.Count != 0)
            {
                if (operat.Peek() == "*" || operat.Peek() == "/")
                {
                    if (value.Count >= 2)
                    {
                        int right = value.Pop();
                        int left = value.Pop();
                        String getOperator = operat.Pop();
                        if (getOperator == "*")
                        {
                            int result = left * right;
                            value.Push(result);
                        }
                        else
                        {
                            if (right != 0)
                            {
                                int result = left / right;
                                value.Push(result);
                            }
                            else
                            {
                                throw new ArgumentException("Invalid operation");
                            }
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Invalid operation");
                    }
                }
            }
        }
        /// <summary>
        /// this is a helper method while the token is a ), while the token is a + or -,
        /// we need to do calculation, but it's different with the normal + or - operation, 
        /// it need to pop the value stack two times to do the operation. but don't need to push the operator back
        /// </summary>
        private static void dealWithLowerArithmetic()
        {
            if (operat.Count != 0)
            {
                if (operat.Peek() == "+" || operat.Peek() == "-")
                {
                    if (value.Count >= 2)
                    {
                        int right = value.Pop();
                        int left = value.Pop();
                        String getOperator = operat.Pop();
                        if (getOperator == "+")
                        {
                            int result = left + right;
                            value.Push(result);
                        }
                        else
                        {
                            int result = left - right;
                            value.Push(result);
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Invalid operation");
                    }
                }
            }
         

        }
        /// <summary>
        /// The alogrithm for lower precedence arithmetic operations.
        /// </summary>
        /// <param name="operater">operator</param>
        private static void lowerArithmetic(String operater)
        {
            if (operat.Count != 0)
            {
                if (operat.Peek() == "+" || operat.Peek() == "-")
                {
                    if (value.Count >= 2)
                    {
                        int right = value.Pop();
                        int left = value.Pop();
                        String getOperator = operat.Pop();
                        if (getOperator == "+")
                        {
                            int result = left + right;
                            value.Push(result);
                        }
                        else
                        {
                            int result = left - right;
                            value.Push(result);
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Invalid operation");
                    }
                }
            }
            operat.Push(operater);
        }
        /// <summary>
        /// Check whether the input string is a valid operator
        /// </summary>
        /// <param name="token">input string</param>
        /// <returns>True if the token is a valid operator</returns>
        private static Boolean isOperator(String token)
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
        /// Parse the strings into tokens
        /// </summary>
        /// <param name="tokens">  intput string   </param>
        /// <returns>   an array of split tokens </returns>
        private static string[] substringsOfarray (string tokens)
        {
            
                string[] substrings = Regex.Split(tokens, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
                return substrings; 
            
        }

    }
}
