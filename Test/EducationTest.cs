using CompetionTaskMarsAutomation.Utility;
using CompetitionTaskMars.Pages;
using CompetitionTaskMars.Utility;
using MarsQA_1.SpecflowPages.Pages;
using NUnit.Framework;
using static CompetionTaskMarsAutomation.Utility.JsonLibHelper;

namespace CompetitionTaskMars.Test
{
    [TestFixture]
    public class EducationTest:MarsBaseClass
    {
        private LoginPage LoginPageObj;
        private ProfileEducationPage ProfileEducationPageObj;
        
        // Constructor for setup
        public EducationTest()
        {
            // Initialize instances of the classes you want to test
            LoginPageObj = new LoginPage();
            ProfileEducationPageObj = new ProfileEducationPage();
            
            //ExtentReportLibHelper.CreateTest(TestContext.CurrentContext.Test.MethodName);
            MarsBaseClass.Initialize();
            MarsBaseClass.NavigateUrl();
            LoginPageObj.LoginSteps();
        }

        [Test, Order(1)]
        public void AddEducation()
        {
            TurnOnWait();
            MarsBaseClass.NavigateToProfileEducation();
            ProfileEducationPageObj.ClearEducationData();

            List<EducationData> educationDataList = JsonLibHelper.ReadJsonFile<EducationData>(ConstantHelpers.educationDataPath);

            if (educationDataList != null)
            {
                foreach (var item in educationDataList)
                {
                    //Console.WriteLine($"Country: {item.Country}, Title: {item.Title}");
                   
                    var newEducationStatus = ProfileEducationPage.AddEachEducationData(item);

                    Assert.AreEqual(item.ExpectedEducationResult, newEducationStatus, $"Education data for {item.Title}-{item.Degree} was added successfully!");
                    
                }
                //Assert.Inconclusive("Test is marked as inconclusive.");
            }
            else
            {
                Assert.Fail("Error in reading the JSON data.");
            }
        }

        [Test, Order(2)]
        public void EditEducation()
        {
            TurnOnWait();
            MarsBaseClass.NavigateToProfileEducation();
            List<EducationData> educationDataList = JsonLibHelper.ReadJsonFile<EducationData>(ConstantHelpers.educationEditDataPath);

            if (educationDataList != null)
            {
                var last = educationDataList.Last();

                foreach (var item in educationDataList)
                {
                    if (item.Equals(last))
                    {
                        var editEducationStatus = ProfileEducationPageObj.EnterEditEducation(item);
                        //Assert.AreEqual(item.ExpectedResult, editEducationStatus, $"Education data for {item.Title}-{item.Degree} was updated successfully!");
                    }
                    else
                    {
                        ProfileEducationPageObj.ClearEducationData();
                        var newEducationStatus = ProfileEducationPage.AddEachEducationData(item);
                        //Assert.AreEqual(item.ExpectedResult, newEducationStatus, $"Education data for {item.Title}-{item.Degree} was updated successfully!");
                    }
                }
            }
            else
            {
                Assert.Fail("Error in reading the JSON data.");
            }
        }


        [Test,Order(3)]
        public void DeleteEducation()
        {
            TurnOnWait();
            MarsBaseClass.NavigateToProfileEducation();
            List<EducationData> educationDataList = JsonLibHelper.ReadJsonFile<EducationData>(ConstantHelpers.educationEditDataPath);

            if (educationDataList != null)
            {
                var last = educationDataList.Last();

                foreach (var item in educationDataList)
                {
                    if (item.Equals(last))
                    {
                        var deleteEducationStatus = ProfileEducationPageObj.DeleteEducation(item);
                        //Assert.AreEqual(item.ExpectedResult, editEducationStatus, $"Education data for {item.Title}-{item.Degree} was updated successfully!");
                    }
                    else
                    {
                        ProfileEducationPageObj.ClearEducationData();
                        var newEducationStatus = ProfileEducationPage.AddEachEducationData(item);
                        //Assert.AreEqual(item.ExpectedResult, newEducationStatus, $"Education data for {item.Title}-{item.Degree} was updated successfully!");
                    }
                }
            }
            else
            {
                Assert.Fail("Error in reading the JSON data.");
            }
        }
        
        [TearDown]
        public void TearDown()
        {
            //ExtentReportLibHelper.EndTest();
           // ExtentReportLibHelper.EndReporting();
            MarsBaseClass.CleanUp();
        }
    }
}
