using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Query.Exceptions;
using NUnit.Framework;

namespace Newtonsoft.Json.Query.Tests
{
    public class NumberTests : JObjectTestBase
    {
        private JObject _jObject;

        [SetUp]
        public void Setup()
        {
            _jObject = JObject.Parse(@"{
                'LowValue': 1, 
                'MediumValue': 2,
                'MediumValue2': 2,
                'HighValue': 3,
                'NotAValue': 'NaN'}");
        }

        [Test]
        [TestCase(".MediumValue<=.LowValue", false)]
        [TestCase(".MediumValue<=.MediumValue2", true)]
        [TestCase(".MediumValue<=.HighValue", true)]
        [TestCase(".MediumValue<=1", false)]
        [TestCase(".MediumValue<=2", true)]
        [TestCase(".MediumValue<=3", true)]
        public void IntLessThanEqualToTests(string query, bool expectedOutcome) => TestIsMatch(_jObject, query, expectedOutcome);

        [Test]
        [TestCase(".MediumValue>=.LowValue", true)]
        [TestCase(".MediumValue>=.MediumValue2", true)]
        [TestCase(".MediumValue>=.HighValue", false)]
        [TestCase(".MediumValue>=1", true)]
        [TestCase(".MediumValue>=2", true)]
        [TestCase(".MediumValue>=3", false)]
        public void IntGreaterThanEqualToTests(string query, bool expectedOutcome) => TestIsMatch(_jObject, query, expectedOutcome);

        [Test]
        [TestCase(".MediumValue>.LowValue", true)]
        [TestCase(".MediumValue>.MediumValue2", false)]
        [TestCase(".MediumValue>.HighValue", false)]
        [TestCase(".MediumValue>1", true)]
        [TestCase(".MediumValue>2", false)]
        [TestCase(".MediumValue>3", false)]
        //[TestCase(".MediumValue>'NaN'", false)] //TODO: Does this need safecheck
        //[TestCase(".MediumValue>.NotAValue", false)] //TODO: Does this need safecheck
        public void IntGreaterThanTests(string query, bool expectedOutcome) => TestIsMatch(_jObject, query, expectedOutcome);

        [Test]
        [TestCase(".MediumValue<.LowValue", false)]
        [TestCase(".MediumValue<.MediumValue2", false)]
        [TestCase(".MediumValue<.HighValue", true)]
        [TestCase(".MediumValue<1", false)]
        [TestCase(".MediumValue<2", false)]
        [TestCase(".MediumValue<3", true)]
        //[TestCase(".Age>'NaN'", false, TestName = "IntEqualsTests > 05 IsMatch (.int>str) expect false")]
        public void IntLessThanTests(string query, bool expectedOutcome) => TestIsMatch(_jObject, query, expectedOutcome);

        [Test]
        [TestCase(".MediumValue=.LowValue", false)]
        [TestCase(".MediumValue=.MediumValue2", true)]
        [TestCase(".MediumValue=.HighValue", false)]
        [TestCase(".MediumValue=1", false)]
        [TestCase(".MediumValue=2", true)]
        [TestCase(".MediumValue=3", false)]
        public void IntEqualsTests(string query, bool expectedOutcome) => TestIsMatch(_jObject, query, expectedOutcome);

        [Test]
        [TestCase(".MediumValue!=.LowValue", true)]
        [TestCase(".MediumValue!=.MediumValue2", false)]
        [TestCase(".MediumValue!=.HighValue", true)]
        [TestCase(".MediumValue!=1", true)]
        [TestCase(".MediumValue!=2", false)]
        [TestCase(".MediumValue!=3", true)]
        [TestCase(".MediumValue!=33", true)]
        [TestCase(".MediumValue!=333", true)]
        public void IntNotEqualsTests(string query, bool expectedOutcome) => TestIsMatch(_jObject, query, expectedOutcome);

    }
}