using CompetitionTaskMars.Utility;
using MarsQA_1.SpecflowPages.Pages;
using NUnit.Framework;
using static CompetionTaskMarsAutomation.Utility.JsonLibHelper;
using CompetionTaskMarsAutomation.Utility;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using System.Linq;
using CompetitionTaskMars.Pages;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;

namespace CompetitionTaskMars.Test
{

    [TestFixture]
    //[Parallelizable]
    public class CertificationTest : MarsBaseClass
    {
        private LoginPage LoginPageObj;
        private ProfileCertificatePage ProfileCertificatePageObj;

        // Constructor for setup
        public CertificationTest()
        {
            InitializeReport();
            Initialize();
            NavigateUrl();

            // Initialize instances of the classes you want to test
            LoginPageObj = new LoginPage(MarsBaseClass.driver);
            ProfileCertificatePageObj = new ProfileCertificatePage(MarsBaseClass.driver);
            LoginPageObj.LoginSteps();
        }

        [SetUp]
        public void BeforeTest()
        {
            // Create a new test in the report
            test = extent.CreateTest(TestContext.CurrentContext.Test.Name);
            test.Info($"The test name :{TestContext.CurrentContext.Test.Name}");
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
        //[Ignore("Ignore AddCertificate With DuplicateData")]
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
                        //Assert.Inconclusive("Test is marked as inconclusive.");
                        //throw new InconclusiveException($"This test is inconclusive because of {newCertificateStatus.returnMessage}.");
                        Assert.AreEqual(item.ExpectedCertificateResult, newCertificateStatus.returnStatus);
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
                        test.Info($"{editCertificateStatus.returnMessage}");
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
            //extent.Flush();
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var stackTrace = string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace)
                    ? ""
                    : string.Format("<pre>{0}</pre>", TestContext.CurrentContext.Result.StackTrace);
            Status logstatus;

            switch (status)
            {
                case TestStatus.Failed:
                    logstatus = Status.Fail;
                    break;
                case TestStatus.Inconclusive:
                    logstatus = Status.Warning;
                    break;
                case TestStatus.Skipped:
                    logstatus = Status.Skip;
                    break;
                default:
                    logstatus = Status.Pass;
                    break;
            }

            test.Log(logstatus, "Test ended with " + logstatus + stackTrace);
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
