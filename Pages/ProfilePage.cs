using CompetitionTaskMars;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using static CompetionTaskMarsAutomation.Utility.JsonLibHelper;

namespace CompetionTaskMarsAutomation.Pages
{
    public class ProfilePage : MarsBaseClass
    {
        private List<EducationDataList> educationData;
        private List<CertificateDataList> certificateData;

        public void GetEducationData(List<EducationDataList> educationData)
        {
            foreach (EducationDataList educationDetails in educationData)
            {
                AddEducationData(educationDetails.country,
                                 educationDetails.university,
                                 educationDetails.title,
                                 educationDetails.degree,
                                 educationDetails.graduationYear);
            }
        }

        private void AddEducationData(string country, string university, string title, string degree, string graduationYear)
        {
            driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[4]/div/div[2]/div/table/thead/tr/th[6]/div")).Click();
            TurnOnWait();
            EnterEducation(country, university, title, degree, graduationYear);
            driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[4]/div/div[2]/div/div/div[3]/div/input[1]")).Click();
        }

        private void EnterEducation(string country, string university, string title, string degree, string graduationYear)
        {
            TurnOnWait();
            IWebElement educationInstituteName = driver.FindElement(By.Name("instituteName"));
            educationInstituteName.Clear();
            educationInstituteName.SendKeys(university);

            SelectElement selectEducationCountry = new SelectElement(driver.FindElement(By.Name("country")));
            selectEducationCountry.SelectByText(country);

            SelectElement educationTitle = new SelectElement(driver.FindElement(By.Name("title")));
            educationTitle.SelectByText(title);

            IWebElement educationDegree = driver.FindElement(By.Name("degree"));
            educationDegree.Clear();
            educationDegree.SendKeys(degree);

            SelectElement SelectGraduationYear = new SelectElement(driver.FindElement(By.Name("yearOfGraduation")));
            SelectGraduationYear.SelectByText(graduationYear);
        }

        public (String, String) ValidateEducationData(string country, string university, string title, string degree, string graduationYear)
        {
            TurnOnWait();
            String newEducationMessage = "";
            String newEducationStatus = "N";

            IWebElement educationTable = driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[4]/div/div[2]/div/table"));
            List<IWebElement> allEducationRow = new List<IWebElement>(educationTable.FindElements(By.TagName("tbody")));

            if (allEducationRow.Count > 0)
            {
                String eduDegree = degree;
                String eduTitle = title;
                foreach (var education in allEducationRow)
                {
                    var resultCols = education.FindElements(By.TagName("td"));

                    if ((resultCols[2].Text == eduTitle) && (resultCols[3].Text == eduDegree))
                    {
                        newEducationMessage = "Added to the table";
                        newEducationStatus = "Y";
                    }
                    else
                    {
                        newEducationMessage = "Education already exist in the list";
                    }
                }
            }
            return (InsertStatus: newEducationStatus, InsertMessage: newEducationMessage);
        }
        public void EnterEditEducation(string toBeEditDegree, string editCountry, string editUniversty, string editTitle, string editDegree, string editGradYear)
        {
            Console.WriteLine(EnterEditEducation);
            IWebElement editButton = driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[4]/div/div[2]/div/table/tbody[1]/tr/td[6]/span[1]"));
            editButton.Click();
            TurnOnWait();
            EnterEducation(editCountry, editUniversty, editTitle, editDegree, editGradYear);
            driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[4]/div/div[2]/div/table/tbody[1]/tr/td/div[3]/input[1]")).Click();
        }

        public (String, String) ValidateUpdatedEducation(string updatedCountry, string updatedUniversty, string updatedTitle, string updatedDegree, string updatedGradYear)
        {
            TurnOnWait();
            String updateEducationStatus = "N";
            String updateEducationMessage = "";
            IWebElement educationTable = driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[4]/div/div[2]/div/table"));
            List<IWebElement> allEducationRow = new List<IWebElement>(educationTable.FindElements(By.TagName("tbody")));

            foreach (var row in allEducationRow)
            {
                var resultCols = row.FindElements(By.TagName("td"));
                Console.WriteLine($"resultCols[2].Text{resultCols[2].Text} and resultCols[3].Text{resultCols[3].Text}");

                if ((resultCols[2].Text == updatedTitle) && (resultCols[3].Text == updatedDegree))
                {
                    updateEducationStatus = "Y";
                    updateEducationMessage = "Education Updated successfully";
                    break;
                }
                else
                {
                    updateEducationMessage = "Education Updation failed";
                }
            }
            return (UpdateStatus: updateEducationStatus, UpdateMessage: updateEducationMessage);
        }
        public void DeleteEducation(string deleteEducationTitle, string deleteEducationDegree)
        {
            IWebElement deleteEducationButton = driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[4]/div/div[2]/div/table/tbody[1]/tr/td[6]/span[2]/i"));
            deleteEducationButton.Click();
        }

        public (String, String) ValidateEducationDeletion(string deleteEducationTitle, string deleteEducationDegree)
        {
            String deletionEducationStatus = "N";
            String deletionEducationMessage = "";
            IWebElement educationTable = driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[4]/div/div[2]/div/table"));
            List<IWebElement> allEducationRow = new List<IWebElement>(educationTable.FindElements(By.TagName("tbody")));

            foreach (var row in allEducationRow)
            {
                var resultCols = row.FindElements(By.TagName("td"));
                
                if ((resultCols[2].Text != deleteEducationTitle) && (resultCols[3].Text != deleteEducationDegree))
                {
                    deletionEducationStatus = "Y";
                    deletionEducationMessage = "Education deleted successfully";
                }
                else
                {
                    deletionEducationMessage = "Education deletion unsuccessfully";
                }
            }
            return (UpdateStatus: deletionEducationStatus, UpdateMessage: deletionEducationMessage);
        }

        private void AddCertificateData(string certificate, string from, string year)
        {
            driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[5]/div[1]/div[2]/div/table/thead/tr/th[4]/div")).Click();
            TurnOnWait();
            EnterCertificate(certificate,from,year);
            driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[5]/div[1]/div[2]/div/div/div[3]/input[1]")).Click();
        }

        private void EnterCertificate(string certificate, string from, string year)
        {
            TurnOnWait();
            IWebElement certificationName = driver.FindElement(By.Name("certificationName"));
            certificationName.Clear();
            certificationName.SendKeys(certificate);

            IWebElement certificationFrom = driver.FindElement(By.Name("certificationFrom"));
            certificationFrom.Clear();
            certificationFrom.SendKeys(from);

            SelectElement selectCertificationYear = new SelectElement(driver.FindElement(By.Name("certificationYear")));
            selectCertificationYear.SelectByText(year);
        }

        public void GetCertificateData(List<CertificateDataList> certificateData)
        {
            foreach (CertificateDataList certificateDetails in certificateData)
            {
                AddCertificateData(certificateDetails.certificate,
                                  certificateDetails.from,
                                  certificateDetails.year);
            }
        }

        public (String, String) ValidateCertificateData(string certificate, string from, string year)
        {
            TurnOnWait();
            String newCertificateMessage = "";
            String newCertificateStatus = "N";

            IWebElement certificateTable = driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[5]/div[1]/div[2]/div/table"));
            List<IWebElement> allCertificateRow = new List<IWebElement>(certificateTable.FindElements(By.TagName("tbody")));

            if (allCertificateRow.Count > 0)
            {
                String certificateName = certificate;
                foreach (var row in allCertificateRow)
                {
                    var resultCols = row.FindElements(By.TagName("td"));

                    if ((resultCols[0].Text == certificateName) )
                    {
                        newCertificateMessage = "Added to the table";
                        newCertificateStatus = "Y";
                    }
                    else
                    {
                        newCertificateMessage = "Certificate already exist in the list";
                    }
                }
            }
            return (InsertStatus: newCertificateStatus, InsertMessage: newCertificateMessage);

        }

        public void EnterEditCertificate(string certificate, string newCertificate, string newFrom, string newYear)
        {
            IWebElement editButton = driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[5]/div[1]/div[2]/div/table/tbody[1]/tr/td[4]/span[1]/i"));
            editButton.Click();
            TurnOnWait();
            EnterCertificate(newCertificate, newFrom, newYear);
            driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[5]/div[1]/div[2]/div/table/tbody[1]/tr/td/div/span/input[1]")).Click();
        }

        public (String, String) ValidateUpdatedCertificate(string certificate, string newcertificate)
        {
            TurnOnWait();
            String newCertificateMessage = "";
            String newCertificateStatus = "N";

            IWebElement certificateTable = driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[5]/div[1]/div[2]/div/table"));
            List<IWebElement> allCertificateRow = new List<IWebElement>(certificateTable.FindElements(By.TagName("tbody")));

            foreach (var row in allCertificateRow)
            {
                var resultCols = row.FindElements(By.TagName("td"));
                Console.WriteLine($"resultCols[2].Text{resultCols[2].Text} and resultCols[3].Text{resultCols[3].Text}");

                if ((resultCols[0].Text == newcertificate))
                {
                    newCertificateStatus = "Y";
                    newCertificateMessage = "Certificate Updated successfully";
                    break;
                }
                else
                {
                    newCertificateMessage = "Certificate Updation failed";
                }
            }
            return (InsertStatus: newCertificateStatus, InsertMessage: newCertificateMessage);
        }

        public void DeleteCertificate(string deleteCertificate)
        {
            IWebElement deleteEducationButton = driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[5]/div[1]/div[2]/div/table/tbody[1]/tr/td[4]/span[2]/i"));
            deleteEducationButton.Click();
        }

        public (String, String) ValidateCertificateDeletion(string deleteCertificate)
        {
            TurnOnWait();
            String deletionCertificateMessage = "";
            String deletionCertificateStatus = "N";
            IWebElement certificateTable = driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[5]/div[1]/div[2]/div/table"));
            List<IWebElement> allCertificateRow = new List<IWebElement>(certificateTable.FindElements(By.TagName("tbody")));

            foreach (var row in allCertificateRow)
            {
                var resultCols = row.FindElements(By.TagName("td"));

                if (resultCols[2].Text != deleteCertificate)
                {
                    deletionCertificateStatus = "Y";
                    deletionCertificateMessage = "Education deleted successfully";
                }
                else
                {
                    deletionCertificateMessage = "Education deletion unsuccessfully";
                }
            }
            return (DeletionStatus: deletionCertificateStatus, DeletionMessage: deletionCertificateMessage);
        }
    }

} 
