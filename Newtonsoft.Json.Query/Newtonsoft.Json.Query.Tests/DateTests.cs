using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Newtonsoft.Json.Query.Tests
{
    public class DateTests : JObjectTestBase
    {
        private JObject _jObject;

        [SetUp]
        public void Setup()
        {
            _jObject = JObject.Parse(@"{
                'LastName': 'Kerry', 
                'AuthorisedOn':'2012-03-19T07:22Z',
                'ApprovedOn':'2012-03-19T07:22Z',
                'SubmittedOn':'2012-04-19T07:22Z'}");
        }
        
        [Test]
        [TestCase(".SubmittedOn<=.AuthorisedOn", false)]
        [TestCase(".AuthorisedOn<=.SubmittedOn", true)]
        [TestCase(".AuthorisedOn<=.ApprovedOn", true)]
        [TestCase(".AuthorisedOn<='2012-01-19T07:22Z'", false)]
        [TestCase(".AuthorisedOn<='2012-04-19T07:22Z'", true)]
        public void DateTimeLessThanEqualToTests(string query, bool expectedOutcome) => TestIsMatch(_jObject, query, expectedOutcome);
        
        [Test]
        [TestCase(".SubmittedOn>=.AuthorisedOn", true)]
        [TestCase(".AuthorisedOn>=.SubmittedOn", false)]
        [TestCase(".AuthorisedOn>=.ApprovedOn", true)]
        [TestCase(".AuthorisedOn>='2012-01-19T07:22Z'", true)]
        [TestCase(".AuthorisedOn>='2012-04-19T07:22Z'", false)]
        public void DateTimeGreaterThanEqualToTests(string query, bool expectedOutcome) => TestIsMatch(_jObject, query, expectedOutcome);
        
        [Test]
        [TestCase(".SubmittedOn>.AuthorisedOn", true)]
        [TestCase(".AuthorisedOn>.SubmittedOn", false)]
        [TestCase(".AuthorisedOn>.ApprovedOn", false)]
        [TestCase(".AuthorisedOn>.LastName", false)] //should fail - make sure it is a date type
        [TestCase(".AuthorisedOn>'2012-01-19T07:22Z'", true)]
        [TestCase(".AuthorisedOn>'2012-04-19T07:22Z'", false)]
        public void DateTimeGreaterThanTests(string query, bool expectedOutcome) => TestIsMatch(_jObject, query, expectedOutcome);
        
        [Test]
        [TestCase(".AuthorisedOn<.SubmittedOn", true)]
        [TestCase(".SubmittedOn<.AuthorisedOn", false)]
        [TestCase(".AuthorisedOn<.LastName", true)] //todo: this should fail as above
        [TestCase(".AuthorisedOn<'2014-03-19T07:22Z'", true)]
        [TestCase(".AuthorisedOn<'2011-04-19T07:22Z'", false)]
        public void DateTimeLessThanTests(string query, bool expectedOutcome) => TestIsMatch(_jObject, query, expectedOutcome);
        
        [Test]
        [TestCase(".AuthorisedOn=.ApprovedOn", true)]
        [TestCase(".AuthorisedOn=.SubmittedOn", false)]
        [TestCase(".AuthorisedOn=.LastName", false)]
        [TestCase(".AuthorisedOn='2012-03-19T07:22Z'", true)]
        [TestCase(".AuthorisedOn='2012-04-19T07:22Z'", false)]
        public void DateTimeEqualsTests(string query, bool expectedOutcome) => TestIsMatch(_jObject, query, expectedOutcome);
    }
}