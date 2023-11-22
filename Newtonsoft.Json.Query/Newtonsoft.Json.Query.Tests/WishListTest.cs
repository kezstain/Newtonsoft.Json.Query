using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Newtonsoft.Json.Query.Tests
{
    public class WishlistTests : JObjectTestBase
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
                'ApprovedByNull':null, 
                'Hobbies':{
                    'Sports':['Football']
                }, 
                'Scores':[
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

        /*todo:
        Add arithmetic routines
        Add array functions
        Work out array functions - should they be any or return matches 
        .Hobbies.Sports['Football' | 'Golf'] to support & as well
         */

        [Test]
        //[TestCase(
        //    @".FirstName='Paul' & .ApprovedBy^='Joe' & 
        //        (.Age>18 | (.Approved & .ApprovedBy != null))",
        //    true)]
        //[TestCase(
        //    @".FirstName='Paul' & .ApprovedBy^='Joe' & 
        //        (.Age>18 | (.Approved & .ApprovedByNull = null))",
        //    true)]
        //[TestCase(
        //    @".FirstName='Paul' & .ApprovedBy^='Joe' & 
        //        (.Age>18 | (.Approved & .ApprovedByNull = null)) & 
        //        .Hobbies.Sports[..='Football' | ..='Golf']",
        //    true)]
        [TestCase(
            @".FirstName='Paul' & .ApprovedBy^='Joe' & 
                (.Age>18 | (.Approved & .ApprovedByNull = null)) & 
                Any(.Hobbies.Sports[..='Football' | ..='Golf'])",
            true)]
        //[TestCase(
        //    @".FirstName='Paul' & .ApprovedBy^='Joe' & 
        //        (.Age>18 | (.Approved & .ApprovedByNull = null)) & 
        //        .Hobbies.Sports[..^='G' | ..$='l']",
        //    true)]
        //[TestCase(
        //    @".Name='abcdef' & .ApprovedBy^='Joe' & 
        //        (.Age>18 | (.Approved & .ApprovedBy != null)) & 
        //        .Hobbies.Sports[..='Football' | ..='Golf'] & 
        //        Any(.Scores[..PointsEarned=100]) & Sum(.Scores[..PointsEarned])<=200",
        //    true)]
        public void WishListTest(string query, bool expectedOutcome) => TestIsMatch(_jObject, query, expectedOutcome);

    }
}