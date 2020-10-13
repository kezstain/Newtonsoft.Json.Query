using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Newtonsoft.Json.Query.Tests
{
    public class JObjectIsMatchTests : JObjectTestBase
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


        //[Test]
        //[TestCase(
        //    @".Name='abcdef' & .ApprovedBy*='Joe' & 
        //        (.Age>18 | (.Approved & .ApprovedBy != null)) & 
        //        .Hobbies.Sports['Football' | 'Golf'] & 
        //        .Scores[score=>score.PointsEarned=100] & Sum(.Scores[.PointsEarned])<=200",
        //    true)]
        //public void WishListTest(string query, bool expectedOutcome) => TestIsMatch(_jObject, query, expectedOutcome);

    }
}