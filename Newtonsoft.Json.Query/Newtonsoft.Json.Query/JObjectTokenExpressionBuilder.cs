using System;
using System.Collections.Generic;
using Newtonsoft.Json.Query.TokenExpressions;

namespace Newtonsoft.Json.Query
{
    internal static class JObjectTokenExpressionBuilder
    {
        private static readonly HashSet<char> Operators = new HashSet<char> { '&', '|' };

        private static readonly HashSet<string> Comparators = new HashSet<string>
        {
            "=", //equal to
            ">", //greater than 
            "<", //less than
            "<=", //less than equal to
            ">=", //greater than equal to
            "!=", //not equal to
            "^=", //starts with
            "$=", //ends with
            "*=" //contains
        };

        internal static IJObjectTokenExpression GetOperatorLogic(string query)
        {
            //clean the string
            query = query.Trim();
            
            if(query.StartsWith("(") && query.EndsWith(")") && query.LastIndexOf("(", StringComparison.InvariantCultureIgnoreCase) == 0)
                query = query[new Range(1, query.Length - 1)];
            if(query.StartsWith("[") && query.EndsWith("]") && query.LastIndexOf("[", StringComparison.InvariantCultureIgnoreCase) == 0)
                query = query[new Range(1, query.Length - 1)];

            //if we find any & or | symbols split the string at this point and create logical operators
            if (ParseJObjectLogicalOperationTokenExpression(query, out var expression)) return expression;

            if ((query.StartsWith('(') && query.EndsWith(')')) || (query.StartsWith('[') && query.EndsWith(']')))
                query = query[new Range(1, query.Length - 1)];

            //now check to see if the string ends with a ")" which may be a method
            if (ParseJObjectMethodTokenExpression(query, out var operatorLogic)) return operatorLogic;

            //now check to see if the string ends with a ")" which may be an array operation
            if (ParseJObjectArrayTokenExpression(query, out var jObjectArrayTokenExpression)) return jObjectArrayTokenExpression;

            //finally check if we are doing any comparisons and return those
            if (ParseJObjectOperationTokenExpression(query, out var jObjectOperationTokenExpression)) return jObjectOperationTokenExpression;

            //all else fails it must be low enough to be a value
            return new JObjectValueTokenExpression(query);
        }

        private static bool ParseJObjectLogicalOperationTokenExpression(string query, out IJObjectTokenExpression expression)
        {
            expression = null;

            //need to on finding a bracket keep going until we find the end bracket then set that as a value logic
            //the value logic can then be used and evaluated in the same way
            var bracketCount = 0;
            var inArrayAction = 0;
            char? currentOperator = null;
            int splitAt = 0;


            //split & | operators
            for (var i = 0; i < query.Length; i++)
            {
                var charValue = query[i];

                //handle brackets to ignore nested &|
                bracketCount += charValue == '(' ? 1 : 0;
                bracketCount -= charValue == ')' ? 1 : 0;
                inArrayAction += charValue == '[' ? 1 : 0;
                inArrayAction -= charValue == ']' ? 1 : 0;

                //if its not a logic operator outside of brackets carry on
                if (currentOperator == null && Operators.Contains(charValue) && bracketCount == 0 && inArrayAction == 0)
                {
                    currentOperator = charValue;
                    splitAt = i;
                }

                ;
                if (currentOperator == null || bracketCount != 0 || inArrayAction != 0) continue;

                var l = query[..splitAt].Trim();
                var o = currentOperator.ToString();
                var r = query[(splitAt+1)..].Trim();

                expression = new JObjectOperationTokenExpression(l, o, r);
                return true;
            }

            return false;
        }

        private static bool ParseJObjectOperationTokenExpression(string query, out IJObjectTokenExpression expression)
        {
            expression = null;

            //only worth checking if string length > 3 (ie 1=1)
            if (query.Length <= 2) return false;
            for (var i = 0; i < query.Length - 1; i++)
            {
                if (Comparators.Contains(query.Substring(i, 2))) //check current and next char
                {
                    {
                        expression = new JObjectOperationTokenExpression(query[..i], query.Substring(i, 2), query[(i + 2)..]);
                        return true;
                    }
                }

                if (Comparators.Contains(query.Substring(i, 1))) //check current and next char
                {
                    {
                        expression = new JObjectOperationTokenExpression(query[..i], query.Substring(i, 1), query[(i + 1)..]);
                        return true;
                    }
                }
            }

            return false;
        }

        private static bool ParseJObjectArrayTokenExpression(string query, out IJObjectTokenExpression expression)
        {
            expression = null;

            //if it ends with a square bracket we know these have been trimmed from both ends so it must be an array method
            if (!query.EndsWith("]"))
                return false;

            var queryStart = query.IndexOf('[');
            if (queryStart > -1)
            {
                var path = query[..queryStart];
                var argument = query[queryStart..];
                expression = new JObjectArrayTokenExpression(path, argument);
                return true;
            }

            return false;
        }


        private static bool ParseJObjectMethodTokenExpression(string query, out IJObjectTokenExpression expression)
        {
            expression = null;
            //if it ends with a round bracket we know these have been trimmed from both ends so it must be an array method
            if (!query.EndsWith(")"))
                return false;

            if (query.StartsWith("Any("))
            {
                var method = query[..3];
                var argument = query[3..];
                {
                    expression = new JObjectMethodTokenExpression(method, argument);
                    return true;
                }
            }

            return false;
        }
    }
}