using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Newtonsoft.Json.Query.Tests
{
    public class StringTests : JObjectTestBase
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
        
        //"*=", //starts with
        //"?=", //ends with
        //"~=" //contains

        [Test]
        [TestCase(".FirstName<=.LastName", false)] 
        [TestCase(".LastName<=.FirstName", true)]
        [TestCase(".FirstName<=.PreferredName", true)] //matches
        //[TestCase(".FirstName>12", false)] //fails
        [TestCase(".FirstName<='Adam'", false)]
        [TestCase(".FirstName<='Zelda'", true)]
        //[TestCase(".FirstName>'adam'", true)] //case insensitive
        //[TestCase(".FirstName>'zelda'", false)] //case insensitive
        public void StringLessThanEqualToTests(string query, bool expectedOutcome) => TestIsMatch(_jObject, query, expectedOutcome);

        [Test]
        [TestCase(".FirstName>=.LastName", true)] 
        [TestCase(".LastName>=.FirstName", false)]
        [TestCase(".FirstName>=.PreferredName", true)] //matches
        //[TestCase(".FirstName>12", false)] //fails
        [TestCase(".FirstName>='Adam'", true)]
        [TestCase(".FirstName>='Zelda'", false)]
        //[TestCase(".FirstName>'adam'", true)] //case insensitive
        //[TestCase(".FirstName>'zelda'", false)] //case insensitive
        public void StringGreaterThanEqualToTests(string query, bool expectedOutcome) => TestIsMatch(_jObject, query, expectedOutcome);

        [Test]
        [TestCase(".FirstName>.LastName", true)] 
        [TestCase(".LastName>.FirstName", false)]
        [TestCase(".LastName>.PreferredName", false)] //matches
        //[TestCase(".FirstName>12", false)] //fails
        [TestCase(".FirstName>'Adam'", true)]
        [TestCase(".FirstName>'Zelda'", false)]
        //[TestCase(".FirstName>'adam'", true)] //case insensitive
        //[TestCase(".FirstName>'zelda'", false)] //case insensitive
        public void StringGreaterThanTests(string query, bool expectedOutcome) => TestIsMatch(_jObject, query, expectedOutcome);
        
        [Test]
        [TestCase(".LastName<.FirstName", true)]
        [TestCase(".FirstName<.LastName", false)]
        //[TestCase(".LastName<12", false)] //fails
        [TestCase(".FirstName<'Zelda'", true)]
        //[TestCase(".FirstName<'zelda'", true)] //case insensitive
        [TestCase(".FirstName<'Adam'", false)]
        //[TestCase(".FirstName<'adam'", false)] //case insensitive
        public void StringLessThanTests(string query, bool expectedOutcome) => TestIsMatch(_jObject, query, expectedOutcome);
        
        [Test]
        [TestCase(".FirstName=.PreferredName", true)]
        [TestCase(".FirstName=.LastName", false)]
        [TestCase(".Age=.LastName", false)]
        [TestCase(".FirstName='Paul'", true)]
        [TestCase(".FirstName='Kerry'", false)]
        public void StringEqualsTests(string query, bool expectedOutcome) => TestIsMatch(_jObject, query, expectedOutcome);
        
        [Test]
        [TestCase(".FirstName!=.PreferredName", false)]
        [TestCase(".FirstName!=.LastName", true)]
        [TestCase(".Age!=.LastName", true)]
        [TestCase(".FirstName!='Paul'", false)]
        [TestCase(".FirstName!='Kerry'", true)]
        public void StringNotEqualsTests(string query, bool expectedOutcome) => TestIsMatch(_jObject, query, expectedOutcome);
        
    }
}