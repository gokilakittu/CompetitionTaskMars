using CompetionTaskMarsAutomation.Pages;
using CompetitionTaskMars.Utility;
using MarsQA_1.SpecflowPages.Pages;
using Newtonsoft.Json;
using NUnit.Framework;
using static CompetionTaskMarsAutomation.Utility.JsonLibHelper;

namespace CompetitionTaskMars.Test
{
    
    [TestFixture]
    //[Parallelizable]
    public class EducationTest:MarsBaseClass
    {
        private List<EducationDataList> educationData;

        LoginPage LoginPageObj = new LoginPage();
        SignInPage SignInPageObj = new SignInPage();
        ProfilePage ProfilePageObj = new ProfilePage();

        [SetUp]
        public void Setup()
        {
            MarsBaseClass.Initialize();
            MarsBaseClass.NavigateUrl();
            SignInPageObj.LoginSteps();
        }

        [Test, Order(1)]
        public void AddEducation()
        {
            MarsBaseClass.NavigateToProfileEducation();
            string json = File.ReadAllText(ConstantHelpers.educationDataPath);
            educationData = JsonConvert.DeserializeObject<List<EducationDataList>>(json);
            ProfilePageObj.GetEducationData(educationData);
            
            foreach (EducationDataList education in educationData)
            {
                var newEducationStatus = ProfilePageObj.ValidateEducationData(education.country,
                                 education.university,
                                 education.title,
                                 education.degree,
                                 education.graduationYear);
                if (newEducationStatus.Item1 == "N")
                {
                    Assert.Fail(newEducationStatus.Item2);
                }
            }
        }

        [Test, Order(2)]
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

            ProfilePageObj.EnterEditEducation(toBeEditTitle,toBeEditDegree, editCountry, editUniversty, editTitle, editDegree, editGradYear);
            var updateEducationStatus = ProfilePageObj.ValidateUpdatedEducation(editCountry, editUniversty, editTitle, editDegree, editGradYear);
            if (updateEducationStatus.Item1 == "N")
            {
                Assert.Fail(updateEducationStatus.Item2);
            }
        }
        [Test,Order(3)]
        public void DeleteEducation()
        {
            string deleteEducationTitle= "B.Sc";
            string deleteEducationDegree = "Analytics";
            MarsBaseClass.NavigateToProfileEducation();
            ProfilePageObj.DeleteEducation(deleteEducationTitle, deleteEducationDegree);
            
            var deleteEducationStatus = ProfilePageObj.ValidateEducationDeletion(deleteEducationTitle, deleteEducationDegree);
            if (deleteEducationStatus.Item1 == "N")
            {
                Assert.Fail(deleteEducationStatus.Item2);
            }
        }

        [TearDown]
        public void TearDown()
        {
            MarsBaseClass.CleanUp();
        }
    }
}