using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Query.Exceptions;

namespace Newtonsoft.Json.Query.TokenExpressions
{
    internal class JObjectOperationTokenExpression : IJObjectTokenExpression
    {
        private readonly IJObjectTokenExpression _leftSideLogic;
        private readonly string _leftSide;
        private readonly string _operation;
        private readonly string _rightSide;
        private readonly IJObjectTokenExpression _rightSideLogic;

        public JObjectOperationTokenExpression(string leftSide, string operation, string rightSide)
        {
            _leftSideLogic = JObjectTokenExpressionBuilder.GetOperatorLogic(leftSide);
            _leftSide = leftSide;
            _operation = operation;
            _rightSide = rightSide;
            _rightSideLogic = JObjectTokenExpressionBuilder.GetOperatorLogic(rightSide);
        }

        public JToken Evaluate(JObject jObject, StringComparison stringComparison = StringComparison.CurrentCulture)
        {
            var left = (JValue)_leftSideLogic.Evaluate(jObject);
            var right = (JValue)_rightSideLogic.Evaluate(jObject);
            var cultureInfo = stringComparison switch
            {
                StringComparison.InvariantCulture => CultureInfo.InvariantCulture,
                StringComparison.InvariantCultureIgnoreCase => CultureInfo.InvariantCulture,
                _ => CultureInfo.CurrentCulture
            };
            var compareOptions = stringComparison switch
            {
                StringComparison.CurrentCultureIgnoreCase => CompareOptions.IgnoreCase,
                StringComparison.InvariantCultureIgnoreCase => CompareOptions.IgnoreCase,
                StringComparison.Ordinal => CompareOptions.Ordinal,
                StringComparison.OrdinalIgnoreCase => CompareOptions.OrdinalIgnoreCase,
                _ => CompareOptions.None
            };

            switch (_operation)
            {
                case "&":
                    if (left.Type != JTokenType.Boolean && right.Type != JTokenType.Boolean)
                        throw new InvalidQueryException("Boolean values expected", $"{_leftSide}{_operation}^{_rightSide}");
                    return new JValue((bool)left && (bool)right);
                case "|":
                    if (left.Type != JTokenType.Boolean && right.Type != JTokenType.Boolean)
                        throw new InvalidQueryException("Boolean values expected", $"{_leftSide}{_operation}^{_rightSide}");
                    return new JValue((bool)left || (bool)right);
                case "=":
                    if (left.Type == JTokenType.String || left.Type != right.Type)
                    {
                        var leftValue = left.Value?.ToString() ?? string.Empty;
                        var rightValue = right.Value?.ToString() ?? string.Empty;
                        
                        return new JValue(leftValue.Equals(rightValue, stringComparison));
                    }
                    return new JValue(left.Equals(right));
                case "!=":
                    if (left.Type == JTokenType.String || left.Type != right.Type)
                    {
                        var leftValue = left.Value?.ToString() ?? string.Empty;
                        var rightValue = right.Value?.ToString() ?? string.Empty;
                        
                        return new JValue(!leftValue.Equals(rightValue, stringComparison));
                    }
                    return new JValue(!left.Equals(right));
                case ">":
                    if (left.Type == JTokenType.String || left.Type != right.Type)
                    {
                        var leftValue = left.Value?.ToString() ?? string.Empty;
                        var rightValue = right.Value?.ToString() ?? string.Empty;

                        var comparison = string.Compare(leftValue, rightValue, cultureInfo, compareOptions);
                        return new JValue(comparison > 0);
                    }

                    return new JValue(left.CompareTo(right) > 0);
                case ">=":
                    if (left.Type == JTokenType.String || left.Type != right.Type)
                    {
                        var leftValue = left.Value?.ToString() ?? string.Empty;
                        var rightValue = right.Value?.ToString() ?? string.Empty;

                        var comparison = string.Compare(leftValue, rightValue, cultureInfo, compareOptions);
                        return new JValue(comparison >= 0);
                    }

                    return new JValue(left.CompareTo(right) >= 0);
                case "<":

                    if (left.Type == JTokenType.String || left.Type != right.Type)
                    {
                        var leftValue = left.Value?.ToString() ?? string.Empty;
                        var rightValue = right.Value?.ToString() ?? string.Empty;

                        var comparison = string.Compare(leftValue, rightValue, cultureInfo, compareOptions);
                        return new JValue(comparison < 0);
                    }

                    return new JValue(left.CompareTo(right) < 0);
                case "<=":

                        if (left.Type == JTokenType.String || left.Type != right.Type)
                        {
                            var leftValue = left.Value?.ToString() ?? string.Empty;
                            var rightValue = right.Value?.ToString() ?? string.Empty;

                            var comparison = string.Compare(leftValue, rightValue, cultureInfo, compareOptions);
                            return new JValue(comparison <= 0);
                        }

                    return new JValue(left.CompareTo(right) <= 0);
                case "^=": //starts with
                {
                    var leftValue = left.Value?.ToString() ?? string.Empty;
                    var rightValue = right.Value?.ToString() ?? string.Empty;

                    var startsWith = (leftValue).StartsWith(rightValue, stringComparison);
                    return new JValue(startsWith);
                }
                case "$=": //ends with
                {
                    var leftValue = left.Value?.ToString() ?? string.Empty;
                    var rightValue = right.Value?.ToString() ?? string.Empty;
                    var endsWith = (leftValue).EndsWith(rightValue, stringComparison);
                    return new JValue(endsWith);
                }
                case "*=": //contains
                {
                    var leftValue = left.Value?.ToString() ?? string.Empty;
                    var rightValue = right.Value?.ToString() ?? string.Empty;
                    var startsWith = leftValue.Contains(rightValue, stringComparison);
                    return new JValue(startsWith);
                }
            }

            throw new NotImplementedException(_operation);
        }
    }
}