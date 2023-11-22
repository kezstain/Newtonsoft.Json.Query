using System;
using System.Globalization;
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
                'FirstNameInitial': 'P',
                'FirstNameMid': 'au',
                'FirstNameEnd': 'l', 
                'Age': 22}");
        }
        
        //"^=", //starts with
        //"$=", //ends with
        //"*=" //contains

        [Test]
        [TestCase(".FirstName*=.FirstNameInitial", true)] 
        [TestCase(".FirstName*=.FirstNameMid", true)]
        [TestCase(".FirstName*=.FirstNameEnd", true)] //matches
        [TestCase(".FirstName*='P'", true)] 
        [TestCase(".FirstName*='au'", true)]
        [TestCase(".FirstName*='l'", true)]
        [TestCase(".FirstName*='z'", false)]
        public void StringContainsTests(string query, bool expectedOutcome) => TestIsMatch(_jObject, query, expectedOutcome);
        
        [Test]
        [TestCase(".FirstName*='A'", StringComparison.InvariantCultureIgnoreCase, true)]
        [TestCase(".FirstName*='A'", StringComparison.InvariantCulture, false)]
        public void StringContainsCultureTests(string query, StringComparison stringComparison, bool expectedOutcome) => TestIsMatch(_jObject, query, expectedOutcome, stringComparison);

        [Test]
        [TestCase(".FirstName$=.FirstNameInitial", false)] 
        [TestCase(".FirstName$=.FirstNameMid", false)]
        [TestCase(".FirstName$=.FirstNameEnd", true)] //matches
        [TestCase(".FirstName$='P'", false)] 
        [TestCase(".FirstName$='au'", false)]
        [TestCase(".FirstName$='l'", true)]
        public void StringEndsWithTests(string query, bool expectedOutcome) => TestIsMatch(_jObject, query, expectedOutcome);
        
        [Test]
        [TestCase(".FirstName$='L'", StringComparison.InvariantCultureIgnoreCase, true)]
        [TestCase(".FirstName$='L'", StringComparison.InvariantCulture, false)]
        public void StringEndsWithCultureTests(string query, StringComparison stringComparison, bool expectedOutcome) => TestIsMatch(_jObject, query, expectedOutcome, stringComparison);

        [Test]
        [TestCase(".FirstName^=.FirstNameInitial", true)] 
        [TestCase(".FirstName^=.FirstNameMid", false)]
        [TestCase(".FirstName^=.FirstNameEnd", false)] //matches
        [TestCase(".FirstName^='P'", true)] 
        [TestCase(".FirstName^='p'", false)] 
        [TestCase(".FirstName^='au'", false)]
        [TestCase(".FirstName^='l'", false)]
        public void StringStartsWithTests(string query, bool expectedOutcome) => TestIsMatch(_jObject, query, expectedOutcome);
        
        [Test]
        [TestCase(".FirstName^='p'", StringComparison.InvariantCultureIgnoreCase, true)]
        [TestCase(".FirstName^='p'", StringComparison.InvariantCulture, false)]
        public void StringStartsWithCultureTests(string query, StringComparison stringComparison, bool expectedOutcome) => TestIsMatch(_jObject, query, expectedOutcome, stringComparison);

        [Test]
        [TestCase(".FirstName<=.LastName", false)] 
        [TestCase(".LastName<=.FirstName", true)]
        [TestCase(".FirstName<=.PreferredName", true)] //matches
        [TestCase(".FirstName>12", true)]
        [TestCase(".FirstName<='Adam'", false)]
        [TestCase(".FirstName<='Zelda'", true)]
        [TestCase(".FirstName<='Paul'", true)]
        [TestCase(".FirstName>'adam'", true)] //case insensitive
        [TestCase(".FirstName>'zelda'", false)] //case insensitive
        public void StringLessThanEqualToTests(string query, bool expectedOutcome) => TestIsMatch(_jObject, query, expectedOutcome);

        [Test]
        [TestCase(".FirstName>=.LastName", true)] 
        [TestCase(".LastName>=.FirstName", false)]
        [TestCase(".FirstName>=.PreferredName", true)] //matches
        [TestCase(".FirstName>12", true)] //fails
        [TestCase(".FirstName>='Adam'", true)]
        [TestCase(".FirstName>='Zelda'", false)]
        [TestCase(".FirstName>='Paul'", true)]
        [TestCase(".FirstName>'adam'", true)] //case insensitive
        [TestCase(".FirstName>'zelda'", false)] //case insensitive
        public void StringGreaterThanEqualToTests(string query, bool expectedOutcome) => TestIsMatch(_jObject, query, expectedOutcome);

        [Test]
        [TestCase(".FirstName>.LastName", true)] 
        [TestCase(".LastName>.FirstName", false)]
        [TestCase(".LastName>.PreferredName", false)] //matches
        [TestCase(".FirstName>12", true)] //fails
        [TestCase(".FirstName>'Adam'", true)]
        [TestCase(".FirstName>'Zelda'", false)]
        [TestCase(".FirstName>'Paul'", false)]
        [TestCase(".FirstName>'adam'", true)] //case insensitive
        [TestCase(".FirstName>'zelda'", false)] //case insensitive
        public void StringGreaterThanTests(string query, bool expectedOutcome) => TestIsMatch(_jObject, query, expectedOutcome);
        
        [Test]
        [TestCase(".LastName<.FirstName", true)]
        [TestCase(".FirstName<.LastName", false)]
        [TestCase(".LastName<12", false)] //fails
        [TestCase(".FirstName<'Paul'", false)]
        [TestCase(".FirstName<'Zelda'", true)]
        [TestCase(".FirstName<'zelda'", true)] //case insensitive
        [TestCase(".FirstName<'Adam'", false)]
        [TestCase(".FirstName<'adam'", false)] //case insensitive
        public void StringLessThanTests(string query, bool expectedOutcome) => TestIsMatch(_jObject, query, expectedOutcome);
        
        //[Test] //not supported
        //[TestCase(".FirstName<'zelda'", StringComparison.InvariantCultureIgnoreCase, true)]
        //[TestCase(".FirstName<'zelda'", StringComparison.InvariantCulture, false)]
        //public void StringLessThanCultureTests(string query, StringComparison stringComparison, bool expectedOutcome) => TestIsMatch(_jObject, query, expectedOutcome, stringComparison);
        
        [Test]
        [TestCase(".FirstName=.PreferredName", true)]
        [TestCase(".FirstName=.LastName", false)]
        [TestCase(".Age=.LastName", false)]
        [TestCase(".FirstName='Paul'", true)]
        [TestCase(".FirstName='Kerry'", false)]
        public void StringEqualsTests(string query, bool expectedOutcome) => TestIsMatch(_jObject, query, expectedOutcome);
        
        [Test]
        [TestCase(".FirstName='paul'", StringComparison.InvariantCultureIgnoreCase, true)]
        [TestCase(".FirstName='paul'", StringComparison.InvariantCulture, false)]
        public void StringEqualsCultureTests(string query, StringComparison stringComparison, bool expectedOutcome) => TestIsMatch(_jObject, query, expectedOutcome, stringComparison);

        [Test]
        [TestCase(".FirstName!=.PreferredName", false)]
        [TestCase(".FirstName!=.LastName", true)]
        [TestCase(".Age!=.LastName", true)]
        [TestCase(".FirstName!='Paul'", false)]
        [TestCase(".FirstName!='Kerry'", true)]
        public void StringNotEqualsTests(string query, bool expectedOutcome) => TestIsMatch(_jObject, query, expectedOutcome);
        
        [Test]
        [TestCase(".FirstName!='paul'", StringComparison.InvariantCultureIgnoreCase, false)]
        [TestCase(".FirstName!='paul'", StringComparison.InvariantCulture, true)]
        public void StringNotEqualsCultureTests(string query, StringComparison stringComparison, bool expectedOutcome) => TestIsMatch(_jObject, query, expectedOutcome, stringComparison);

        
        [Test]
        [TestCase("(.FirstName!=(((.PreferredName))))", false)]
        [TestCase("(.FirstName!=.LastName)", true)]
        [TestCase("(.Age!=.LastName)", true)]
        [TestCase("(.FirstName!='Paul')", false)]
        [TestCase("(.FirstName!='Kerry')", true)]
        public void BracketTests(string query, bool expectedOutcome) => TestIsMatch(_jObject, query, expectedOutcome);
        
    }
}