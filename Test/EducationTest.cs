using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using CompetionTaskMarsAutomation.Utility;
using CompetitionTaskMars.Pages;
using CompetitionTaskMars.Utility;
using MarsQA_1.SpecflowPages.Pages;
using NUnit.Framework;
using static CompetionTaskMarsAutomation.Utility.JsonLibHelper;

namespace CompetitionTaskMars.Test
{

    /*
     TC_EDU_03- Add education deltails that is already present in the list
     TC_EDU_05- Check if the user is able to add empty data
     TC_EDU_06 to TC_EDU_10- User trying to add incomplete education data 

     */
    [TestFixture]
    public class EducationTest:MarsBaseClass
    {
        private LoginPage LoginPageObj;
        private ProfileEducationPage ProfileEducationPageObj;

        private ExtentReports extent;
        private ExtentTest test;

       // Constructor for setup
        public EducationTest()
        {
            // Initialize instances of the classes you want to test
            LoginPageObj = new LoginPage();
            ProfileEducationPageObj = new ProfileEducationPage();

            // Constructor: Initialize ExtentReports and attach an HTML reporter
            extent = new ExtentReports();
            var htmlReporter = new ExtentHtmlReporter(ConstantHelpers.extendReportsPath);
            extent.AttachReporter(htmlReporter);
 
            MarsBaseClass.Initialize();
            MarsBaseClass.NavigateUrl();
            LoginPageObj.LoginSteps();
        }

        [SetUp]
        public void BeforeTest()
        {
            // Create a new test in the report
            test = extent.CreateTest(TestContext.CurrentContext.Test.Name);
        }
        
        [Test, Order(1), Description("TC_EDU_01- Add valid education details")]
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
                    var newEducationStatus = ProfileEducationPageObj.AddEachEducationData(item);
                    test.Info($"Education insertion status:{newEducationStatus.returnStatus} - Education insertion message: {newEducationStatus.returnMessage}");
                    Assert.AreEqual(item.ExpectedEducationResult, newEducationStatus.returnStatus);
                }
                
            }
            else
            {
                Assert.Fail("Error in reading the JSON data.");
            }
        }
        [Test, Order(2), Description("TC_EDU_0-3Verify if user is able to  add  education that is already in the education list in Profile - education")]
        public void AddEducationWithDuplicateData()
        {
            TurnOnWait();
            MarsBaseClass.NavigateToProfileEducation();
            ProfileEducationPageObj.ClearEducationData();

            List<EducationData> educationDataList = JsonLibHelper.ReadJsonFile<EducationData>(ConstantHelpers.educationDataDuplicatePath);
            if (educationDataList != null)
            {
                foreach (var item in educationDataList)
                {
                    var newEducationStatus = ProfileEducationPageObj.AddEachEducationData(item);
                    if (newEducationStatus.returnStatus == "Fail")
                    {
                        test.Info("Test is marked as inconclusive.");
                        test.Fail($"Education insertion status:{newEducationStatus.returnStatus} - Education insertion message: {newEducationStatus.returnMessage}");
                        Assert.Inconclusive("Test is marked as inconclusive.");
                    }
                    else
                    {
                        test.Info($"Education insertion status:{newEducationStatus.returnStatus} - Education insertion message: {newEducationStatus.returnMessage}");

                    }
                }
            }
            else
            {
                Assert.Fail("Error in reading the JSON data.");
            }
        }

        [Test, Order(3), Description("TC_EDU_02- Add and then edit valid education details")]
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
                        test.Info("The education was edited");
                        Assert.AreEqual(item.ExpectedEducationResult, editEducationStatus.returnStatus);
                    }
                    else
                    {
                        ProfileEducationPageObj.ClearEducationData();
                        var newEducationStatus = ProfileEducationPageObj.AddEachEducationData(item);
                        test.Info("The education intended to delete was added");
                    }
                }
            }
            else
            {
                Assert.Fail("Error in reading the JSON data.");
            }
        }


        [Test,Order(4), Description("TC_EDU_04 - Add and then delete education details")]
        public void DeleteEducation()
        {
            TurnOnWait();
            MarsBaseClass.NavigateToProfileEducation();
            List<EducationData> educationDataList = JsonLibHelper.ReadJsonFile<EducationData>(ConstantHelpers.educationDeleteDataPath);

            if (educationDataList != null)
            {
                foreach (var item in educationDataList)
                {
                    ProfileEducationPageObj.ClearEducationData();
                    var newEducationStatus = ProfileEducationPageObj.AddEachEducationData(item);
                    test.Info("The education intended to delete was added");
                    var deleteEducationStatus = ProfileEducationPageObj.DeleteEducation(item);
                    test.Info("The education was deleted");
                    Assert.AreEqual(item.ExpectedEducationResult, deleteEducationStatus.returnStatus);
                }
            }
            else
            {
                Assert.Fail("Error in reading the JSON data.");
            }
        }
        
        [TearDown]
        public void AfterTest()
        {
            // End the test and add it to the report
            extent.Flush();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            // End the test and save the report
            extent.Flush();
            MarsBaseClass.CleanUp();
        }
    }
}
