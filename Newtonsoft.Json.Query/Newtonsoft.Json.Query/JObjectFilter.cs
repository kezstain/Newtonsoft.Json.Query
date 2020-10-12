using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Newtonsoft.Json.Query
{
    public class JObjectFilter : IJObjectTokenExpression
    {
        private static readonly HashSet<char> Operators =new HashSet<char> { '&','|' };

        private static readonly HashSet<string> Comparators =new HashSet<string>
        {
            "=", //equal to
            ">", //greater than 
            "<", //less than
            "<=", //less than equal to
            ">=", //greater than equal to
            "!=", //not equal to
            "*=", //starts with
            "?=", //ends with
            "~=" //contains
        };

        private IJObjectTokenExpression BaseLogic { get; set; }

        public JObjectFilter(string query)
        {
            BaseLogic = GetOperatorLogic(query);
        }

        private static IJObjectTokenExpression GetOperatorLogic(string query)
        {
            query = query.Trim();
            if (query.StartsWith('(') && query.EndsWith(')') || query.StartsWith('[') && query.EndsWith(']'))
                query = query[new Range(1, query.Length - 1)];

            //need to on finding a bracket keep going until we find the end bracket then set that as a value logic
            //the value logic can then be used and evaluated in the same way
            var bracketCount = 0;
            var inArrayAction = 0;
            //split & | operators
            for (var i = 0; i < query.Length; i++)
            {
                var charValue = query[i];

                //handle brackets to ignore nested &|
                switch (charValue)
                {
                    case '(': bracketCount++; break;
                    case ')': bracketCount--; break;
                    case '[': inArrayAction++; break;
                    case ']': inArrayAction--; break;
                }

                //if its not a logic operator outside of brackets carry on
                if (!Operators.Contains(charValue) || bracketCount!=0 || inArrayAction!=0) continue;

                var l = query[new Range(0, i)];
                var o = charValue.ToString();
                var r = query[new Range(i + 1, query.Length)];
                return new JQueryFilterOperatorLogic(l, o, r);
            }

            if (JQueryFilterLogic(query, out var operatorLogic)) return operatorLogic;

            if (query.EndsWith("]")) //cannot start with as they are removed so must be an array function
            {
                var queryStart = query.IndexOf('[');
                if (queryStart > -1)
                {
                    var path = query[new Range(0, queryStart)];
                    var argument = query[new Range(queryStart, query.Length)];
                    return new JQueryFilterArrayLogic(path, argument);
                }
            }
            
            //only worth checking if string length > 3 (ie 1=1)
            if (query.Length > 2)
            {
                for (var i = 0; i < query.Length-1; i++)
                {
                    if (Comparators.Contains(query.Substring(i,2))) //check current and next char
                    {
                        return new JQueryFilterOperatorLogic(query.Substring(0, i), query.Substring(i, 2 ), query.Substring(i + 2));
                    }
                    if (Comparators.Contains(query.Substring(i,1))) //check current and next char
                    {
                        return new JQueryFilterOperatorLogic(query.Substring(0,i), query.Substring(i,1), query.Substring(i + 1));
                    }
                }
            }

            //need to split between "=,>,<," in a different way
            return new JQueryFilterValueLogic(query);
        }

        private static bool JQueryFilterLogic(string query, out IJObjectTokenExpression operatorLogic)
        {
            operatorLogic = null;
            if (!query.EndsWith(")")) 
                return false;
            var queryStart = query.IndexOf('(');
            if (queryStart > -1)
            {
                var method = query[new Range(0, queryStart)];
                var argument = query[new Range(queryStart, query.Length)];
                {
                    operatorLogic = new JQueryFilterMethodLogic(method, argument);
                    return true;
                }
            }

            return false;
        }

        private class JQueryFilterValueLogic : IJObjectTokenExpression
        {
            private readonly string _query;

            public JQueryFilterValueLogic(string query)
            {
                _query = query;
            }

            public JToken Evaluate(JObject jObject)
            {
                if (_query.StartsWith("."))
                {
                    var tokenPath = _query.Substring(1);
                    return jObject.SelectToken(tokenPath);
                }

                //escape string values
                return JToken.Parse(_query);
            }
        }

        private class JQueryFilterOperatorLogic : IJObjectTokenExpression
        {
            private readonly IJObjectTokenExpression _leftSideLogic;
            private readonly string _oper;
            private readonly IJObjectTokenExpression _rightSideLogic;

            public JQueryFilterOperatorLogic(string leftSide, string @operator, string rightSide)
            {
                _leftSideLogic = GetOperatorLogic(leftSide);
                _oper = @operator;
                _rightSideLogic = GetOperatorLogic(rightSide);
            }

            public JToken Evaluate(JObject jObject)
            {
                var left = (JValue)_leftSideLogic.Evaluate(jObject);
                var right = (JValue)_rightSideLogic.Evaluate(jObject);
                switch (_oper)
                {
                    case "=":
                        var match = left.Equals(right);
                        return new JValue(match);
                    case ">":
                        var match2 = left.CompareTo(right)>0;
                        return new JValue(match2);
                    case "<":
                        var match3 = left.CompareTo(right)<0;
                        return new JValue(match3);
                }
                throw new NotImplementedException();
            }
        }

        private class JQueryFilterMethodLogic : IJObjectTokenExpression
        {
            private readonly string _method;
            private readonly string _argument;
            private IJObjectTokenExpression _leftSideLogic;
            private readonly string _oper;
            private IJObjectTokenExpression _rightSideLogic;

            public JQueryFilterMethodLogic(string method, string argument)
            {
                _method = method;
                _argument = argument;
                _rightSideLogic = GetOperatorLogic(_argument);
            }

            public JToken Evaluate(JObject jObject)
            {
                throw new NotImplementedException();
            }
        }

        private class JQueryFilterArrayLogic : IJObjectTokenExpression
        {
            private readonly string _method;
            private readonly string _argument;
            private IJObjectTokenExpression _leftSideLogic;
            private readonly string _oper;
            private IJObjectTokenExpression _rightSideLogic;

            public JQueryFilterArrayLogic(string method, string argument)
            {
                _method = method;
                _argument = argument;
                _rightSideLogic = GetOperatorLogic(_argument);
            }

            public JToken Evaluate(JObject jObject)
            {
                throw new NotImplementedException();
            }
        }

        public JToken Evaluate(JObject jObject)
        {
            return BaseLogic.Evaluate(jObject);
        }
    }
}