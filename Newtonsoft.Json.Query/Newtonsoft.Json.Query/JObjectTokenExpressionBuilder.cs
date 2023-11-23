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

        internal static IJObjectTokenExpression GetOperatorLogic(ReadOnlySpan<char> query)
        {
            //clean the string
            query = query.Trim();
            query = EscapeClosingBracketPairs(query, '(', ')');
            query = EscapeClosingBracketPairs(query, '[', ']');

            //if we find any & or | symbols split the string at this point and create logical operators
            if (ParseJObjectLogicalOperationTokenExpression(query, out var expression)) return expression;

            //now check to see if the string ends with a ")" which may be a method
            if (JObjectMethodTokenExpression.TryParse(query, out var operatorLogic)) return operatorLogic;

            //now check to see if the string ends with a ")" which may be an array operation
            if (JObjectArrayTokenExpression.TryParse(query, out var jObjectArrayTokenExpression)) return jObjectArrayTokenExpression;

            //finally check if we are doing any comparisons and return those
            if (ParseJObjectOperationTokenExpression(query, out var jObjectOperationTokenExpression)) return jObjectOperationTokenExpression;

            //all else fails it must be low enough to be a value
            return new JObjectValueTokenExpression(query);
        }

        private static ReadOnlySpan<char> EscapeClosingBracketPairs(ReadOnlySpan<char> query, char openingChar, char closingChar)
        {
            //loop through and check if the outer brackets of the query are paired, if so strip them for further processing
            while (true)
            {
                if (query.Length < 2) return query;
                if (query[0] != openingChar && query[..^1][0] != closingChar) return query;

                var bracketCount = 0;
                for (var i = 0; i < query.Length - 1; i++)
                {
                    var charValue = query[i];

                    //check nesting
                    bracketCount += charValue == openingChar ? 1 : 0;
                    bracketCount -= charValue == closingChar ? 1 : 0;

                    //if we escape the brackets return
                    if (bracketCount == 0)
                    {
                        return query;
                    }
                }

                query = query[1..^1];
            }
        }

        private static bool ParseJObjectLogicalOperationTokenExpression(ReadOnlySpan<char> query, out IJObjectTokenExpression expression)
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

                if (currentOperator == null || bracketCount != 0 || inArrayAction != 0) continue;

                var l = query[..splitAt].Trim();
                var o = currentOperator.ToString();
                var r = query[(splitAt+1)..].Trim();

                expression = new JObjectOperationTokenExpression(l, o, r);
                return true;
            }

            return false;
        }

        private static bool ParseJObjectOperationTokenExpression(ReadOnlySpan<char> query, out IJObjectTokenExpression expression)
        {
            expression = null;

            //only worth checking if string length > 3 (ie 1=1)
            if (query.Length <= 2) return false;
            for (var i = 0; i < query.Length - 1; i++)
            {
                if (Comparators.Contains(query.Slice(i, 2).ToString())) //check current and next char
                {
                    {
                        expression = new JObjectOperationTokenExpression(query[..i], query.Slice(i, 2), query[(i + 2)..]);
                        return true;
                    }
                }

                if (Comparators.Contains(query.Slice(i, 1).ToString())) //check current and next char
                {
                    {
                        expression = new JObjectOperationTokenExpression(query[..i], query.Slice(i, 1), query[(i + 1)..]);
                        return true;
                    }
                }
            }

            return false;
        }
    }
}