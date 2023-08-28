using CompetionTaskMarsAutomation.Pages;
using CompetitionTaskMars.Utility;
using MarsQA_1.SpecflowPages.Pages;
using Newtonsoft.Json;
using NUnit.Framework;
using static CompetionTaskMarsAutomation.Utility.JsonLibHelper;

namespace CompetitionTaskMars.Test
{
    [TestFixture]
    public class CertificationTest: MarsBaseClass
    {
        private List<CertificateDataList> certificateData;

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
        public void AddCertification()
        {
            MarsBaseClass.NavigateToProfileCertification();
            string json = File.ReadAllText(ConstantHelpers.certificateDataPath);
            certificateData = JsonConvert.DeserializeObject<List<CertificateDataList>>(json);
            ProfilePageObj.GetCertificateData(certificateData);
            
            foreach (CertificateDataList certificate in certificateData)
            {
                var newCertificateStatus = ProfilePageObj.ValidateCertificateData(
                                 certificate.certificate,
                                 certificate.from,
                                 certificate.year);
                if (newCertificateStatus.Item1 == "N")
                {
                    Assert.Fail(newCertificateStatus.Item2);
                }
            }

        }
        [Test, Order(2)]
        public void EditCertification()
        {
            MarsBaseClass.NavigateToProfileCertification();
            string certificate = "ISTQB-Foundation";
            string newCertificate = "ISTQB-Agile";
            string newFrom = "ISTQB";
            string newYear = "2021";

            ProfilePageObj.EnterEditCertificate(certificate, newCertificate, newFrom, newYear);

            var updateCertificateStatus = ProfilePageObj.ValidateUpdatedCertificate(certificate, newCertificate);
            if (updateCertificateStatus.Item1 == "N")
            {
                Assert.Fail(updateCertificateStatus.Item2);
            }
        }
        [Test, Order(3)]
        public void DeleteCertification()
        {
            string deleteCertificate = "ISTQB-Agile";
            MarsBaseClass.NavigateToProfileCertification();
            ProfilePageObj.DeleteCertificate(deleteCertificate);

            var deletionStatus = ProfilePageObj.ValidateCertificateDeletion(deleteCertificate);
            if (deletionStatus.Item1 == "N")
            {
                Assert.Fail(deletionStatus.Item2);
            }
        }

        [TearDown]
        public void TearDown()
        {
            MarsBaseClass.CleanUp();
        }
    }
}
