using CompetionTaskMarsAutomation.Pages;
using CompetitionTaskMars.Pages;
using CompetitionTaskMars.Utility;
using DocumentFormat.OpenXml.Wordprocessing;
using MarsQA_1.SpecflowPages.Pages;
using Newtonsoft.Json;
using NUnit.Framework;
using System.ComponentModel;
using static CompetionTaskMarsAutomation.Utility.JsonLibHelper;
using DescriptionAttribute = NUnit.Framework.DescriptionAttribute;

namespace CompetitionTaskMars.Test
{
    /*
    //[TestFixture]
    //[Parallelizable]
    public class EducationTest_old:MarsBaseClass
    {
        //private List<EducationDataList> educationData;

        LoginPage LoginPageObj = new LoginPage();
        ProfilePage ProfilePageObj = new ProfilePage();
        ProfileEducationPage ProfileEducationPageObj = new ProfileEducationPage();

        //[SetUp]
        public void Setup()
        {
            ExtentReportLibHelper.CreateTest(TestContext.CurrentContext.Test.MethodName);
            MarsBaseClass.Initialize();
            MarsBaseClass.NavigateUrl();
            LoginPageObj.LoginSteps();
        }

        //[Test, Order(1), Description("TC_EDU_01 - Verify if user is able to enter valid credential in Profile-education and save the data")]
        public void AddEducation()
        {
            MarsBaseClass.NavigateToProfileEducation();
            string json = File.ReadAllText(ConstantHelpers.educationDataPath);
            educationData = JsonConvert.DeserializeObject<List<EducationDataList>>(json);
            foreach (EducationDataList educationDetails in educationData)
            {
                var newEducationStatus = ProfilePageObj.AddEachEducationData(educationDetails.country,
                                 educationDetails.university,
                                 educationDetails.title,
                                 educationDetails.degree,
                                 educationDetails.graduationYear);

                if (newEducationStatus.Item1 == "N")
                {
                    ExtentReportLibHelper.LogFail(newEducationStatus.Item2);
                    //Assert.Fail(newEducationStatus.Item2);
                }
                else
                {
                    ExtentReportLibHelper.LogPass(newEducationStatus.Item2);
                    //Assert.Fail(newEducationStatus.Item2);
                }
            }
        }
        
        //[Test, Order(2), Description("TC_EDU_02 - Verify if user is able to edit valid credential in Profile-education and update the data")]

        public void EditEducation()
        {
            MarsBaseClass.NavigateToProfileEducation();
            string toBeEditTitle = "B.Tech";
            string toBeEditDegree = "IT";
            string editCountry = "India";
            string editUniversty = "Madras Institute of Technologies";
            string editTitle = "B.Tech";
            string editDegree = "Information Technology";
            string editGradYear = "2004";
            
            bool educationPresentStatus = ProfilePage.CheckEducationIsPresent(toBeEditTitle, toBeEditDegree);
            if (educationPresentStatus == true)
            {
                ExtentReportLibHelper.LogInfo($"{toBeEditTitle} and {toBeEditDegree} education is present in the list.");
                ProfilePageObj.EnterEditEducation(toBeEditTitle, toBeEditDegree, editCountry, editUniversty, editTitle, editDegree, editGradYear);
                var updateEducationStatus = ProfilePageObj.ValidateUpdatedEducation(editCountry, editUniversty, editTitle, editDegree, editGradYear);
                if (updateEducationStatus.Item1 == "N")
                {
                    //Assert.Fail(updateEducationStatus.Item2);
                    ExtentReportLibHelper.LogFail(updateEducationStatus.Item2);
                }
                else
                {
                    //Assert.Pass(updateEducationStatus.Item2);
                    ExtentReportLibHelper.LogPass(updateEducationStatus.Item2);
                }
            }
            else
            {
                //Assert.Fail("Education intented to edit is not in the list.");
                ExtentReportLibHelper.LogFail("Education intented to edit is not in the list.");
            }
        }
        
        //[Test,Order(3), Description("TC_EDU_04 - Verify if user is able to  delete  education from Profile - education")]
        public void DeleteEducation()
        {
            string deleteEducationTitle= "B.Sc";
            string deleteEducationDegree = "Analytics";
            MarsBaseClass.NavigateToProfileEducation();
            //ProfileEducationPage.ClearEducationData();

            bool educationPresentStatus = ProfilePage.CheckEducationIsPresent(deleteEducationTitle, deleteEducationDegree);
            if (educationPresentStatus == true)
            {
                ExtentReportLibHelper.LogInfo($"{deleteEducationTitle} and {deleteEducationDegree} education is present in the list.");

                ProfilePageObj.DeleteEducation(deleteEducationTitle, deleteEducationDegree);

                var deleteEducationStatus = ProfilePageObj.ValidateEducationDeletion(deleteEducationTitle, deleteEducationDegree);
                if (deleteEducationStatus.Item1 == "N")
                {
                    //Assert.Fail(deleteEducationStatus.Item2);
                    ExtentReportLibHelper.LogFail(deleteEducationStatus.Item2);
                }
                else
                {
                    //Assert.Pass(deleteEducationStatus.Item2);
                    ExtentReportLibHelper.LogPass(deleteEducationStatus.Item2);
                }
            }
            else
            {
                //Assert.Fail("Education intented to delete is not in the list.");
                ExtentReportLibHelper.LogFail("Education intented to delete is not in the list.");
            }
        }

        //[TearDown]
        public void TearDown()
        {
            ExtentReportLibHelper.EndTest();
            ExtentReportLibHelper.EndReporting();
            MarsBaseClass.CleanUp();
        }
    }*/
}
