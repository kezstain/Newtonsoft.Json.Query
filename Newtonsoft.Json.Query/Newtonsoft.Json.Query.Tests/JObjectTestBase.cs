using System;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Newtonsoft.Json.Query.Tests
{
    public class JObjectTestBase
    {
        internal void TestIsMatch(JObject jObject, string query, bool expectedOutcome, StringComparison stringComparison = StringComparison.CurrentCulture)
        {
            var isMatch = jObject.IsMatch(query, stringComparison);
            Assert.AreEqual(expectedOutcome, isMatch);
        }
    }
}