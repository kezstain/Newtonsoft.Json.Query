using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Newtonsoft.Json.Query.Tests
{
    public class BooleanTests : JObjectTestBase
    {
        private JObject _jObject;

        [SetUp]
        public void Setup()
        {
            _jObject = JObject.Parse(@"{
                'LastName': 'Kerry',  
                'Authorised':true, 
                'Approved':true,
                'Complete':false,
                'ApprovedBy':'Joe Smith'}");
        }
        
        [Test]
        [Category("Boolean Equals Tests")]
        [TestCase(".Authorised", true)]
        [TestCase(".Complete", false)]
        [TestCase(".Authorised=.Approved", true)]
        [TestCase(".Authorised=.Complete", false)]
        [TestCase(".Authorised=.LastName", false)]
        [TestCase(".Authorised=true", true)]
        [TestCase(".Authorised=false", false)]
        public void BooleanEqualsTests(string query, bool expectedOutcome) => TestIsMatch(_jObject, query, expectedOutcome);
        
    }
}