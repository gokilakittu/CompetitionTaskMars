using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports;
using CompetionTaskMarsAutomation.Pages;
using CompetitionTaskMars.Utility;
using MarsQA_1.SpecflowPages.Pages;
using Newtonsoft.Json;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

using static CompetionTaskMarsAutomation.Utility.JsonLibHelper;
using Org.BouncyCastle.Tls;

namespace CompetitionTaskMars.Test
{
    [TestFixture]
    //[Parallelizable]
    public class CertificationTest: MarsBaseClass
    {
        private List<CertificateDataList> certificateData;
        
        LoginPage LoginPageObj = new LoginPage();
        SignInPage SignInPageObj = new SignInPage();
        ProfilePage ProfilePageObj = new ProfilePage();

        [SetUp]
        public void Setup()
        {
            ExtentReportLibHelper.CreateTest(TestContext.CurrentContext.Test.MethodName);
            MarsBaseClass.Initialize();
            MarsBaseClass.NavigateUrl();
            SignInPageObj.LoginSteps();
        } 

       [Test, Order(1)]
        public void AddCertification()
       {
            MarsBaseClass.NavigateToProfileCertification();
            string json = File.ReadAllText(ConstantHelpers.certificateDataPath);
            certificateData = JsonConvert.DeserializeObject<List<CertificateDataList>>(json);
            
            foreach (CertificateDataList certificate in certificateData)
            {
                var newCertificateStatus = ProfilePageObj.AddEachCertificateData(
                                 certificate.certificate,
                                 certificate.from,
                                 certificate.year);
                if (newCertificateStatus.Item1 == "N")
                {
                    ExtentReportLibHelper.LogFail(newCertificateStatus.Item2);
                    //Assert.Fail(newCertificateStatus.Item2);
                }
                else
                {
                    ExtentReportLibHelper.LogPass(newCertificateStatus.Item2);
                    //Assert.Pass(newCertificateStatus.Item2);
                }
            }
        }

        [Test, Order(2)]
        public void EditCertification()
        {
            MarsBaseClass.NavigateToProfileCertification();
            string certificate = "certificate1";
            string newCertificate = "ISTQB-Agile";
            string newFrom = "ISTQB";
            string newYear = "2021";

            bool certificatePresentStatus = ProfilePage.CheckCertificateIsPresent(certificate);
            if (certificatePresentStatus == true)
            {
                ExtentReportLibHelper.LogInfo($"{certificate} certificate is present in the list.");

                ProfilePageObj.EnterEditCertificate(certificate, newCertificate, newFrom, newYear);
                var updateCertificateStatus = ProfilePageObj.ValidateUpdatedCertificate(certificate, newCertificate);

                if (updateCertificateStatus.Item1 == "N")
                {
                    //Assert.Fail(updateCertificateStatus.Item2);
                    ExtentReportLibHelper.LogFail(updateCertificateStatus.Item2);
                }
                else
                {
                    //Assert.Pass(updateCertificateStatus.Item2);
                    ExtentReportLibHelper.LogInfo(updateCertificateStatus.Item2);
                }
            }
            else
            {
                //Assert.Fail("Certificate intented to edit is not in the list.");
                ExtentReportLibHelper.LogInfo("Certificate intented to edit is not in the list.");
            }
        }
        [Test, Order(3)]
        public void DeleteCertification()
        {
            string deleteCertificate = "certificate2";
            MarsBaseClass.NavigateToProfileCertification();
            bool certificatePresentStatus = ProfilePage.CheckCertificateIsPresent(deleteCertificate);
            if (certificatePresentStatus == true)
            {
                ExtentReportLibHelper.LogInfo($"{deleteCertificate} certificate is present in the list.");
                ProfilePageObj.DeleteCertificate(deleteCertificate);
                var deleteCertificationStatus = ProfilePageObj.ValidateCertificateDeletion(deleteCertificate);
                if (deleteCertificationStatus.Item1 == "N")
                {
                    //Assert.Fail(deleteCertificationStatus.Item2);
                    ExtentReportLibHelper.LogFail(deleteCertificationStatus.Item2);
                }
                else
                {
                   // Assert.Pass(deleteCertificationStatus.Item2);
                    ExtentReportLibHelper.LogInfo(deleteCertificationStatus.Item2);
                }
            }
            else
            {
                //Assert.Fail("Certificate intented to delete is not in the list.");
                ExtentReportLibHelper.LogFail("Certificate intented to delete is not in the list.");
               }
        }

        [TearDown]
        public void TearDown()
        {
            ExtentReportLibHelper.EndTest();
            ExtentReportLibHelper.EndReporting();
            MarsBaseClass.CleanUp();

        }
    }
}
