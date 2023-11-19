using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Newtonsoft.Json.Query.Tests
{
    public class AndOrTests : JObjectTestBase
    {
        private JObject _jObject;

        [SetUp]
        public void Setup()
        {
            _jObject = JObject.Parse(@"{
                'FirstName': 'Paul', 
                'LastName': 'Kerry', 
                'PreferredName': 'Paul', 
                'Age': 22}");
        }
        
        //"^=", //starts with
        //"$=", //ends with
        //"*=" //contains

        [Test]
        [TestCase(".FirstName=.PreferredName&.LastName='Kerry'", true)] //TODO: test and support escape characters and escape strings with & etc in
        [TestCase(".FirstName=.PreferredName&.Age=22", true)]
        public void AndEqualsTests(string query, bool expectedOutcome) => TestIsMatch(_jObject, query, expectedOutcome);
        
        [Test]
        [TestCase(".FirstName=.PreferredName|.LastName='Kerry'", true)] //TODO: test and support escape characters and escape strings with & etc in
        [TestCase(".FirstName=.LastName|.LastName='Kerry'", true)] //TODO: test and support escape characters and escape strings with & etc in
        [TestCase(".FirstName=.PreferredName|.LastName='Bernard'", true)] //TODO: test and support escape characters and escape strings with & etc in
        [TestCase(".FirstName=.PreferredName|.Age=22", true)]
        public void OrEqualsTests(string query, bool expectedOutcome) => TestIsMatch(_jObject, query, expectedOutcome);
        
        [Test]
        [TestCase("(.FirstName=.LastName&.Age=22)|.LastName='Kerry'", true)]
        [TestCase(".FirstName=.LastName&(.Age=22|.LastName='Kerry')", false)]
        public void BracketAndOrEqualsTests(string query, bool expectedOutcome) => TestIsMatch(_jObject, query, expectedOutcome);
        
    }
}