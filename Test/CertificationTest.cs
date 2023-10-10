using CompetitionTaskMars.Utility;
using MarsQA_1.SpecflowPages.Pages;
using NUnit.Framework;
using static CompetionTaskMarsAutomation.Utility.JsonLibHelper;
using CompetionTaskMarsAutomation.Utility;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using System.Linq;

namespace CompetitionTaskMars.Test
{

    [TestFixture]
    //[Parallelizable]
    public class CertificationTest : MarsBaseClass
    {
        private LoginPage LoginPageObj;
        private ProfileCertificatePage ProfileCertificatePageObj;

        private ExtentReports extent;
        private ExtentTest test;

        // Constructor for setup
        public CertificationTest()
        {
            // Initialize instances of the classes you want to test
            LoginPageObj = new LoginPage();
            ProfileCertificatePageObj = new ProfileCertificatePage();

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

        [Test, Order(1), Description("TC_CER_01- Add valid Certificate details")]
        public void AddCertification()
        {
            TurnOnWait();
            MarsBaseClass.NavigateToProfileCertification();
            ProfileCertificatePageObj.ClearCertificateData();

            List<CertificateData> certificateDataList = JsonLibHelper.ReadJsonFile<CertificateData>(ConstantHelpers.certificateDataPath);
            if (certificateDataList != null)
            {
                foreach (var item in certificateDataList)
                {
                    var newCertificateStatus = ProfileCertificatePageObj.AddEachCertificateData(item);
                    test.Info($"Certification insertion status:{newCertificateStatus.returnStatus} - Certification insertion message: {newCertificateStatus.returnMessage}");
                    Assert.AreEqual(item.ExpectedCertificateResult, newCertificateStatus.returnStatus);
                }
            }
            else
            {
                Assert.Fail("Error in reading the JSON data.");
            }
        }
        [Test, Order(2), Description("TC_CER_0-3 Verify if user is able to  add  certificate that is already in the certificate list in Profile - certificate")]
        public void AddCertificateWithDuplicateData()
        {
            TurnOnWait();
            MarsBaseClass.NavigateToProfileCertification();
            ProfileCertificatePageObj.ClearCertificateData();

            List<CertificateData> certificateDataList = JsonLibHelper.ReadJsonFile<CertificateData>(ConstantHelpers.certificateDataDuplicatePath);
            if (certificateDataList != null)
            {
                foreach (var item in certificateDataList)
                {
                    var newCertificateStatus = ProfileCertificatePageObj.AddEachCertificateData(item);
                    if (newCertificateStatus.returnStatus == "Fail")
                    {
                        test.Info("Test is marked as inconclusive.");
                        test.Fail($"Certification insertion status:{newCertificateStatus.returnStatus} - Certification insertion message: {newCertificateStatus.returnMessage}");
                        Assert.Inconclusive("Test is marked as inconclusive.");
                        Assert.Fail($"{newCertificateStatus.returnMessage}");
                    }
                    else
                    {
                        test.Info($"Certification insertion status:{newCertificateStatus.returnStatus} - Certification insertion message: {newCertificateStatus.returnMessage}");
                        Assert.AreEqual(item.ExpectedCertificateResult, newCertificateStatus.returnStatus);
                    }
                }
            }
            else
            {
                Assert.Fail("Error in reading the JSON data.");
            }
        }

        [Test, Order(3), Description("TC_CER_02- Add and then edit valid certificate details")]
        public void EditCertification()
        {
            TurnOnWait();
            MarsBaseClass.NavigateToProfileCertification();
            List<CertificateData> certificateDataList = JsonLibHelper.ReadJsonFile<CertificateData>(ConstantHelpers.certificateEditDataPath);

            if (certificateDataList != null)
            {
                var last = certificateDataList.Last();

                foreach (var item in certificateDataList)
                {
                    TurnOnWait();
                    if (item.Equals(last))
                    {
                        var editCertificateStatus = ProfileCertificatePageObj.EnterEditCertificate(item);
                        test.Info("The certificate was edited");
                       Assert.AreEqual(item.ExpectedCertificateResult, editCertificateStatus.returnStatus);
                    }
                    else
                    {
                        ProfileCertificatePageObj.ClearCertificateData();
                        var newCertificateStatus = ProfileCertificatePageObj.AddEachCertificateData(item);
                        test.Info("The certificate intended to edit was added");
                    }
                }
            }
            else
            {
                Assert.Fail("Error in reading the JSON data.");
            }
        }
        
       [Test, Order(4), Description("TC_CER_04 - Add and then delete certificate details")]
       public void DeleteCertification()
       {
           TurnOnWait();
           MarsBaseClass.NavigateToProfileCertification();
           List<CertificateData> certificateDataList = JsonLibHelper.ReadJsonFile<CertificateData>(ConstantHelpers.certificateDeleteDataPath);

           if (certificateDataList != null)
           {
                foreach (var item in certificateDataList)
                {
                    ProfileCertificatePageObj.ClearCertificateData();
                    var newCertificateStatus = ProfileCertificatePageObj.AddEachCertificateData(item);
                    test.Info("The certificate intended to delete was added");
                    var deleteCertificateStatus = ProfileCertificatePageObj.DeleteCertificate(item);
                    test.Info("The certificate was deleted");
                    Assert.AreEqual(item.ExpectedCertificateResult, deleteCertificateStatus.returnStatus);
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
