using System;

namespace Newtonsoft.Json.Query.Exceptions
{
    internal class InvalidQueryException : Exception
    {
        public InvalidQueryException(string message, string query) : base($"Error occured: {message}\r\n{query}")
        {

        }
    }
}