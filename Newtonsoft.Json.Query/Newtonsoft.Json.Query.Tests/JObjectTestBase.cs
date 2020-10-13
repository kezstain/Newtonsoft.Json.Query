using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Newtonsoft.Json.Query.Tests
{
    public class JObjectTestBase
    {
        internal void TestIsMatch(JObject jObject, string query, bool expectedOutcome)
        {
            var isMatch = jObject.IsMatch(query);
            Assert.AreEqual(expectedOutcome, isMatch);
        }
    }
}