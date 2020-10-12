using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Newtonsoft.Json.Query.Tests
{
    public class Tests
    {
        private JObject _jObject;

        [SetUp]
        public void Setup()
        {
            _jObject = JObject.Parse(@"{
                'FirstName': 'Paul', 
                'LastName': 'Kerry', 
                'PreferredName': 'Paul', 
                'Age': 22, 
                'BrotherAge': 22, 
                'BabyAge': 1, 
                'SisterAge': 25, 
                'Authorised':true, 
                'AuthorisedOn':'2012-03-19T07:22Z',
                'Approved':true,
                'ApprovedOn':'2012-03-19T07:22Z',
                'SubmittedOn':'2012-04-19T07:22Z',
                'Complete':false,
                'ApprovedBy':'Joe Smith', 
                Hobbies:{
                    Sports:['Football']
                }, 
                Scores:[
                    {
                        PointsEarned:100
                    },
                    {
                        PointsEarned:50
                    },  
                    {
                        PointsEarned:25
                    }
                ]}");
        }

        [Test]
        [Category("Int Greater Than Tests")]
        [TestCase(".Age>.BabyAge", true, TestName = "IntGreaterThanTests > 01 IsMatch (.int>.int) expect true")]
        [TestCase(".Age>.SisterAge", false, TestName = "IntGreaterThanTests > 02 IsMatch (.int>.int) expect false")]
        [TestCase(".Age>21", true, TestName = "IntGreaterThanTests > 03 IsMatch (.int>int) expect true")]
        [TestCase(".Age>22", false, TestName = "IntGreaterThanTests > 04 IsMatch (.int>int) expect false")]
        [TestCase(".Age>23", false, TestName = "IntGreaterThanTests > 05 IsMatch (.int>int) expect false")]
        //[TestCase(".Age>'NaN'", false, TestName = "IntEqualsTests > 05 IsMatch (.int>str) expect false")]??Support
        public void IntGreaterThanTests(string query, bool expectedOutcome) => TestIsMatch(query, expectedOutcome);

        [Test]
        [Category("Int Less Than Tests")]
        [TestCase(".BabyAge<.Age", true, TestName = "IntLessThanTests > 01 IsMatch (.int<.int) expect true")]
        [TestCase(".Age<.BabyAge", false, TestName = "IntLessThanTests > 02 IsMatch (.int<.int) expect false")]
        [TestCase(".Age<23", true, TestName = "IntLessThanTests > 03 IsMatch (.int<int) expect true")]
        [TestCase(".Age<22", false, TestName = "IntLessThanTests > 04 IsMatch (.int<int) expect false")]
        [TestCase(".Age<21", false, TestName = "IntLessThanTests > 05 IsMatch (.int<int) expect false")]
        //[TestCase(".Age>'NaN'", false, TestName = "IntEqualsTests > 05 IsMatch (.int>str) expect false")]
        public void IntLessThanTests(string query, bool expectedOutcome) => TestIsMatch(query, expectedOutcome);

        [Test]
        [Category("Int Equals Tests")]
        [TestCase(".Age=.BrotherAge", true, TestName = "IntEqualsTests > 01 IsMatch (.int=.int) expect true")]
        [TestCase(".Age=.SisterAge", false, TestName = "IntEqualsTests > 02 IsMatch (.int=.int) expect false")]
        [TestCase(".Age=22", true, TestName = "IntEqualsTests > 03 IsMatch (.int=int) expect true")]
        [TestCase(".Age=23", false, TestName = "IntEqualsTests > 04 IsMatch (.int=int) expect false")]
        [TestCase(".Age='NaN'", false, TestName = "IntEqualsTests > 05 IsMatch (.int=str) expect false")]
        public void IntEqualsTests(string query, bool expectedOutcome) => TestIsMatch(query, expectedOutcome);

        
        [Test]
        [Category("String Greater Than Tests")]
        [TestCase(".FirstName>.LastName", true, TestName = "StringGreaterThanTests > 01 IsMatch(.str>.str) expect true")]
        [TestCase(".LastName>.FirstName", false, TestName = "StringGreaterThanTests > 02 IsMatch(.str>.str) expect false")]
        //[TestCase(".FirstName>12", false, TestName = "StringGreaterThanTests > 03 IsMatch(.str>.int) expect false")] //fails
        [TestCase(".FirstName>'Adam'", true, TestName = "StringGreaterThanTests > 04 IsMatch(.str>str) expect true")]
        [TestCase(".FirstName>'Zelda'", false, TestName = "StringGreaterThanTests > 05 IsMatch(.str>str) expect true")]
        //[TestCase(".FirstName>'adam'", true, TestName = "StringGreaterThanTests > 04 IsMatch(.str>str) expect true")] //case insensitive
        //[TestCase(".FirstName>'zelda'", false, TestName = "StringGreaterThanTests > 05 IsMatch(.str>str) expect true")] //case insensitive
        public void StringGreaterThanTests(string query, bool expectedOutcome) => TestIsMatch(query, expectedOutcome);
        
        [Test]
        [Category("String Less Than Tests")]
        [TestCase(".LastName<.FirstName", true, TestName = "StringLessThanTests > 01 IsMatch(.str<.str) expect true")]
        [TestCase(".FirstName<.LastName", false, TestName = "StringLessThanTests > 02 IsMatch(.str<.str) expect false")]
        //[TestCase(".LastName<12", false, TestName = "StringLessThanTests > 03 IsMatch(.int<.str) expect false")] //fails
        [TestCase(".FirstName<'Zelda'", true, TestName = "StringLessThanTests > 04 IsMatch(.str<str) expect true")]
        //[TestCase(".FirstName<'zelda'", true, TestName = "StringLessThanTests > 05 IsMatch(.str<str) expect true")] //case insensitive
        [TestCase(".FirstName<'Adam'", false, TestName = "StringLessThanTests > 06 IsMatch(.str<str) expect true")]
        //[TestCase(".FirstName<'adam'", false, TestName = "StringLessThanTests > 07 IsMatch(.str<str) expect true")] //case insensitive
        public void StringLessThanTests(string query, bool expectedOutcome) => TestIsMatch(query, expectedOutcome);
        
        [Test]
        [Category("String Equals Tests")]
        [TestCase(".FirstName=.PreferredName", true, TestName = "StringEqualsTests > 01 IsMatch(.str=.str) expect true")]
        [TestCase(".FirstName=.LastName", false, TestName = "StringEqualsTests > 02 IsMatch(.str=.str) expect false")]
        [TestCase(".Age=.LastName", false, TestName = "StringEqualsTests > 03 IsMatch(.int=.str) expect false")]
        [TestCase(".FirstName='Paul'", true, TestName = "StringEqualsTests > 04 IsMatch(.str=str) expect true")]
        [TestCase(".FirstName='Kerry'", false, TestName = "StringEqualsTests > 05 IsMatch(.str=str) expect true")]
        public void StringEqualsTests(string query, bool expectedOutcome) => TestIsMatch(query, expectedOutcome);
        
        [Test]
        [Category("Boolean Equals Tests")]
        [TestCase(".Authorised=.Approved", true, TestName = "BooleanEqualsTests > 01 IsMatch(.bool=.bool) expect true")]
        [TestCase(".Authorised=.Complete", false, TestName = "BooleanEqualsTests > 02 IsMatch(.bool=.bool) expect false")]
        [TestCase(".Authorised=.LastName", false, TestName = "BooleanEqualsTests > 03 IsMatch(.bool=.str) expect false")]
        [TestCase(".Authorised=true", true, TestName = "BooleanEqualsTests > 04 IsMatch(.bool=bool) expect true")]
        [TestCase(".Authorised=false", false, TestName = "BooleanEqualsTests > 05 IsMatch(.bool=bool) expect true")]
        public void BooleanEqualsTests(string query, bool expectedOutcome) => TestIsMatch(query, expectedOutcome);
        
        [Test]
        [Category("DateTime Equals Tests")]
        [TestCase(".AuthorisedOn=.ApprovedOn", true, TestName = "DateTimeEqualsTests > 01 IsMatch(.dt=.dt) expect true")]
        [TestCase(".AuthorisedOn=.SubmittedOn", false, TestName = "DateTimeEqualsTests > 02 IsMatch(.dt=.dt) expect false")]
        [TestCase(".AuthorisedOn=.LastName", false, TestName = "DateTimeEqualsTests > 03 IsMatch(.dt=.str) expect false")]
        [TestCase(".AuthorisedOn='2012-03-19T07:22Z'", true, TestName = "DateTimeEqualsTests > 04 IsMatch(.dt=dt) expect true")]
        [TestCase(".AuthorisedOn='2012-04-19T07:22Z'", false, TestName = "DateTimeEqualsTests > 05 IsMatch(.dt=dt) expect true")]
        public void DateTimeEqualsTests(string query, bool expectedOutcome) => TestIsMatch(query, expectedOutcome);

        private void TestIsMatch(string query, bool expectedOutcome)
        {
            var isMatch = _jObject.IsMatch(query);
            Assert.AreEqual(expectedOutcome, isMatch);
        }

        [Test]
        public void TestParse()
        {
            var query = @".Name='abcdef' & .ApprovedBy*='Joe' & (.Age>18 | (.Approved & .ApprovedBy != null)) & .Hobbies.Sports['Football' | 'Golf'] & .Scores[score=>score.PointsEarned=100] & Sum(.Scores[.PointsEarned])<=200";
            
            //var JObjectFilter = JObjectTokenExpressionBuilder.GetOperatorLogic(query);
        }
    }
}