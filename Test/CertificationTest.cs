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
    public class CertificationTest:MarsBaseClass
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

            //ExtentReportLibHelper.CreateTest(TestContext.CurrentContext.Test.MethodName);
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

        [Test, Order(1)]
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
                    
                    Assert.IsTrue(ProfileCertificatePageObj.IsDataVisibleInTableRow(item.Certificate));
                }
            }
            else
            {
                Assert.Fail("Error in reading the JSON data.");
            }
        }
        
        [Test, Order(2)]
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
                         //Console.WriteLine($"editCertificateStatus--{editCertificateStatus.returnStatus}");
                        //Console.WriteLine($"item.ExpectedCertificateResult--{item.ExpectedCertificateResult}");
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
        
       [Test, Order(3)]
       public void DeleteCertification()
       {
           TurnOnWait();
           MarsBaseClass.NavigateToProfileCertification();
           List<CertificateData> certificateDataList = JsonLibHelper.ReadJsonFile<CertificateData>(ConstantHelpers.certificateDataPath);

           if (certificateDataList != null)
           {
               var last = certificateDataList.Last();

               foreach (var item in certificateDataList)
               {
                   if (item.Equals(last))
                   {
                       var deleteEducationStatus = ProfileCertificatePageObj.DeleteEducation(item);
                       //Assert.AreEqual(item.ExpectedResult, editEducationStatus, $"Education data for {item.Title}-{item.Degree} was updated successfully!");
                   }
                   else
                   {
                       ProfileCertificatePageObj.ClearCertificateData();
                       var newEducationStatus = ProfileCertificatePageObj.AddEachCertificateData(item);
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
        public void AfterTest()
        {
            // Add test status (Pass/Fail) to the report
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var stackTrace = TestContext.CurrentContext.Result.StackTrace;
            var errorMessage = TestContext.CurrentContext.Result.Message;

            if (status == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                test.Fail($"Test failed: {errorMessage}");
                test.Fail(stackTrace);
            }
            else if (status == NUnit.Framework.Interfaces.TestStatus.Passed)
            {
                test.Pass("Test passed");
            }
            else if (status == NUnit.Framework.Interfaces.TestStatus.Skipped)
            {
                test.Skip("Test skipped");
            }

            // End the test and save the report
            extent.Flush();
            MarsBaseClass.CleanUp();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            // Close the ExtentReports instance
            extent.Flush();
            extent.RemoveTest(test);
            //extent.RemoveTest(ExtentTestManager.GetTest());
        }
    }
}
