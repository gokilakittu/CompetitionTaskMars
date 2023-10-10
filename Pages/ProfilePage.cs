using CompetitionTaskMars;
using CompetitionTaskMars.Utility;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Wordprocessing;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Org.BouncyCastle.Ocsp;
using Org.BouncyCastle.Tls;
using static CompetionTaskMarsAutomation.Utility.JsonLibHelper;

namespace CompetionTaskMarsAutomation.Pages
{
    public class ProfilePage : MarsBaseClass
    {

        public (String, String) AddEachEducationData(string country, string university, string title, string degree, string graduationYear)
        {
            String InsertEducationStatus = "N";
            String InsertEducationMessage = "";
            int i = 0;
            String eduDegree = degree;
            String eduTitle = title;

            IWebElement educationTable = driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[4]/div/div[2]/div/table"));
            List<IWebElement> allEducationRow = new List<IWebElement>(educationTable.FindElements(By.TagName("tbody")));

            if (allEducationRow.Count > 0)
            {
                List<string> EducationTitleTableList = new List<string>();
                List<string> EducationDegreeTableList = new List<string>();

                foreach (var education in allEducationRow)
                {
                    var resultData = education.FindElements(By.TagName("td"));
                    EducationTitleTableList.Add(resultData[2].Text);
                    EducationDegreeTableList.Add(resultData[3].Text);
                }
                var TitleDegreeTableList = EducationTitleTableList.Zip(EducationDegreeTableList, (T, D) => new { Title = T, Degree = D });
               
                foreach (var TitleDegree in TitleDegreeTableList)
                {
                    if ((TitleDegree.Title == eduTitle) && (TitleDegree.Degree == eduDegree))
                    {
                        InsertEducationMessage = $"Education({eduTitle}-{eduDegree}) intended to be added is already in the list";
                        break;
                    }
                    else {
                        foreach (var education in allEducationRow)
                        {
                            List<IWebElement> tableEducationRow = new List<IWebElement>(education.FindElements(By.TagName("td")));
                            var resultRows = education.FindElements(By.TagName("td"));
                            TurnOnWait();
                            var resultCols = education.FindElements(By.TagName("td"));

                            if ((TitleDegree.Title == eduTitle) && (TitleDegree.Degree == eduDegree))
                            {
                                InsertEducationMessage = $"Education({eduTitle}-{eduDegree})  intended to be added is already in the list";
                                break;
                            }
                            else
                            {
                                TurnOnWait();
                                driver.FindElement(By.XPath("/html/body/div[1]/div/section[2]/div/div/div/div[3]/form/div[4]/div/div[2]/div/table/thead/tr/th[6]/div")).Click();
                                                            
                                EnterEducation(country, university, title, degree, graduationYear);
                                driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[4]/div/div[2]/div/div/div[3]/div/input[1]")).Click();
                                var newEducationStatus = ValidateEducationData(title, degree);
                                if (newEducationStatus.Item1 == "N")
                                {
                                    InsertEducationMessage = $"Adding {title}-{degree} education is not done";
                                }
                                else
                                {
                                    InsertEducationMessage = $"Adding {title}-{degree} education is done";
                                    InsertEducationStatus = "Y";
                                }
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                TurnOnWait();
                driver.FindElement(By.XPath("/html/body/div[1]/div/section[2]/div/div/div/div[3]/form/div[4]/div/div[2]/div/table/thead/tr/th[6]/div")).Click();
                EnterEducation(country, university, title, degree, graduationYear);
                driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[4]/div/div[2]/div/div/div[3]/div/input[1]")).Click();

                var newEducationStatus = ValidateEducationData(title, degree);
                if (newEducationStatus.Item1 == "N")
                {
                    InsertEducationMessage = $"Adding {title}-{degree} education is not done";
                }
                else
                {
                    InsertEducationMessage = $"Adding {title}-{degree} education is done";
                    InsertEducationStatus = "Y";
                }
            }
            return (InsertStatus: InsertEducationStatus, InsertEducationMessage: InsertEducationMessage);
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

        public (String, String) ValidateEducationData(string title, string degree)
        {
            TurnOnWait();
            String validateEducationMessage = "";
            String validateEducationStatus = "N";

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
                        validateEducationMessage = $"{eduTitle},{eduDegree} was added to the table";
                        validateEducationStatus = "Y";
                        break;
                    }
                    else
                    {
                        validateEducationMessage = $"{eduTitle},{eduDegree} education already exist in the list";
                    }
                }
            }
            else
            {
                validateEducationMessage = "ELSE THE TABLE COUNT IS LESS THAN O";
                validateEducationStatus = "N";

            }
            return (InsertStatus: validateEducationStatus, InsertMessage: validateEducationMessage);
        }
        public void EnterEditEducation(string toBeEditTitle, string toBeEditDegree, string editCountry, string editUniversty, string editTitle, string editDegree, string editGradYear)
        {
            String educationUpdationStatus = "";
            IWebElement educationTable = driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[4]/div/div[2]/div/table"));
            List<IWebElement> allEducationRow = new List<IWebElement>(educationTable.FindElements(By.TagName("tbody")));
            foreach (var row in allEducationRow)
            {
                var resultCols = row.FindElements(By.TagName("td"));

                if ((resultCols[2].Text == toBeEditTitle) && (resultCols[3].Text == toBeEditDegree))
                {
                    List<IWebElement> allIconsRow = new List<IWebElement>(row.FindElements(By.TagName("i")));
                    foreach (var icon in allIconsRow)
                    {
                        if (icon.GetAttribute("class") == "outline write icon")
                        {
                            icon.Click();
                            EnterEducation(editCountry, editUniversty, editTitle, editDegree, editGradYear);
                            driver.FindElement(By.CssSelector("input[class*='ui teal button']")).Click();
                            break;
                        }
                        else
                        {
                            educationUpdationStatus = "Undefined Error";
                        }
                    }
                }
                else
                {
                    educationUpdationStatus = "The education intented to update is not in the list";
                }
            }
        }

        public static bool CheckEducationIsPresent(string toBeEditTitle, string toBeEditDegree)
        {
            bool isEducationPresent = false;
            IWebElement educationTable = driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[4]/div/div[2]/div/table"));
            List<IWebElement> allEducationRow = new List<IWebElement>(educationTable.FindElements(By.TagName("tbody")));

            foreach (var row in allEducationRow)
            {
                var resultCols = row.FindElements(By.TagName("td"));

                if ((resultCols[2].Text == toBeEditTitle) && (resultCols[3].Text == toBeEditDegree))
                {
                    isEducationPresent = true;
                    break;
                }
                else
                {
                    isEducationPresent = false;
                }
            }
            return isEducationPresent;

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
            String educationDeleteStatus = "";
            IWebElement educationTable = driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[4]/div/div[2]/div/table"));
            List<IWebElement> allEducationRow = new List<IWebElement>(educationTable.FindElements(By.TagName("tbody")));
            foreach (var row in allEducationRow)
            {
                var resultCols = row.FindElements(By.TagName("td"));
                if ((resultCols[2].Text == deleteEducationTitle) && (resultCols[3].Text == deleteEducationDegree))
                {
                    List<IWebElement> allIconsRow = new List<IWebElement>(row.FindElements(By.TagName("i")));
                    foreach (var icon in allIconsRow)
                    {
                        if (icon.GetAttribute("class") == "remove icon")
                        {
                            TurnOnWait();
                            icon.Click();
                            educationDeleteStatus = "";
                        }
                        else
                        {
                            educationDeleteStatus = "Undefined Error";
                        }
                    }
                    break;
                }
                else
                {
                    educationDeleteStatus = "The education intented to delete is not in the list";
                }
            }
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
                    break;
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
            EnterCertificate(certificate, from, year);
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

        public (String, String) AddEachCertificateData(string certificate, string from, string year)
        {
            String InsertCertificateStatus = "N";
            String InsertCertificateMessage = "";
            
            IWebElement certificateTable = driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[5]/div[1]/div[2]/div/table"));
            List<IWebElement> allCertificateRow = new List<IWebElement>(certificateTable.FindElements(By.TagName("tbody")));

            if (allCertificateRow.Count > 0)
            {
                List<string> CertificateTableList = new List<string>();
                
                foreach (var certificateData in allCertificateRow)
                {
                    var resultData = certificateData.FindElements(By.TagName("td"));
                    CertificateTableList.Add(resultData[0].Text);
                }
                
                foreach (var singleCertificate in CertificateTableList)
                {
                    if (singleCertificate == certificate)
                    {
                        InsertCertificateMessage = $"Certificte({certificate}) intended to be added is already in the list";
                        break;
                    }
                    else
                    {
                        foreach (var certification in allCertificateRow)
                        {
                            List<IWebElement> tableCertificateRow = new List<IWebElement>(certification.FindElements(By.TagName("td")));
                            var resultRows = certification.FindElements(By.TagName("td"));

                            if (resultRows[0].Text == certificate)
                            {
                                InsertCertificateMessage = $"certificate ({certificate})  intended to be added is already in the list";
                                break;
                            }
                            else
                            {
                                TurnOnWait();
                                driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[5]/div[1]/div[2]/div/table/thead/tr/th[4]/div")).Click();
                                EnterCertificate(certificate, from, year);
                                driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[5]/div[1]/div[2]/div/div/div[3]/input[1]")).Click();
                                var newCertificateStatus = ValidateCertificateData(certificate,from, year);
                                if (newCertificateStatus.Item1 == "N")
                                {
                                    InsertCertificateMessage = $"Adding {certificate} certificate is not done";
                                }
                                else
                                {
                                    InsertCertificateMessage = $"Adding {certificate} certificate is done";
                                    InsertCertificateStatus = "Y";
                                }
                                break;
                            }
                        }
                    }
                }
            }
            else
            {

                TurnOnWait();
                driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[5]/div[1]/div[2]/div/table/thead/tr/th[4]/div")).Click();
                EnterCertificate(certificate, from, year);
                driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[5]/div[1]/div[2]/div/div/div[3]/input[1]")).Click();
                var newCertificateStatus = ValidateCertificateData(certificate, from, year);
                if (newCertificateStatus.Item1 == "N")
                {
                    InsertCertificateMessage = $"Adding {certificate} certificate is not done";
                }
                else
                {
                    InsertCertificateMessage = $"Adding {certificate} certificate is done";
                    InsertCertificateStatus = "Y";
                }
            }
            return (InsertCertificateStatus: InsertCertificateStatus, InsertCertificateMessage: InsertCertificateMessage);


        }

        public void GetCertificateData(List<CertificateData> certificateData)
        {
            foreach (CertificateData certificateDetails in certificateData)
            {
                AddCertificateData(certificateDetails.Certificate,
                                  certificateDetails.From,
                                  certificateDetails.Year);
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

                    if ((resultCols[0].Text == certificateName))
                    {
                        newCertificateMessage = "Added to the table";
                        newCertificateStatus = "Y";
                        break;
                    }
                    else
                    {
                        newCertificateMessage = "Certificate already exist in the list";
                        newCertificateStatus = "N";
                    }
                }
            }
            return (InsertStatus: newCertificateStatus, InsertMessage: newCertificateMessage);

        }

        public static bool CheckCertificateIsPresent(String certificatePresent)
        {
            bool isCertificatePresent = false;
            IWebElement certificateTable = driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[5]/div[1]/div[2]/div/table"));
            List<IWebElement> allCertificateRow = new List<IWebElement>(certificateTable.FindElements(By.TagName("tbody")));
            foreach (var row in allCertificateRow)
            {
                var resultCols = row.FindElements(By.TagName("td"));
                if (resultCols[0].Text == certificatePresent)
                {
                    isCertificatePresent = true;
                    break;
                }
                else
                {
                    isCertificatePresent = false;
                }
            }
            return isCertificatePresent;
        }
        public void EnterEditCertificate(string certificate, string newCertificate, string newFrom, string newYear)
        {
            String educationDeleteStatus = "";
            IWebElement certificateTable = driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[5]/div[1]/div[2]/div/table"));
            List<IWebElement> allCertificateRow = new List<IWebElement>(certificateTable.FindElements(By.TagName("tbody")));
            foreach (var row in allCertificateRow)
            {
                var resultCols = row.FindElements(By.TagName("td"));
                if ((resultCols[0].Text == certificate))
                {
                    List<IWebElement> allIconsRow = new List<IWebElement>(row.FindElements(By.TagName("i"))); foreach (var icon in allIconsRow)
                    {
                        if (icon.GetAttribute("class") == "outline write icon")
                        {
                            icon.Click();
                            EnterCertificate(newCertificate, newFrom, newYear);
                            driver.FindElement(By.CssSelector("input[class*='ui teal button']")).Click();
                            educationDeleteStatus = "";
                            break;
                        }
                        else
                        {
                            educationDeleteStatus = "Undefined Error";
                        }
                    }
                }
                else
                {
                    educationDeleteStatus = "The education intented to edit is not in the list";
                }
            }
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

                if ((resultCols[0].Text == newcertificate))
                {
                    newCertificateStatus = "Y";
                    newCertificateMessage = "Certificate Updated successfully"; break;
                }
                else
                {
                    newCertificateStatus = "N";
                    newCertificateMessage = "Certificate Updation failed.Certificate name intended to update was not present in the list.";
                }
            }
            return (InsertStatus: newCertificateStatus, InsertMessage: newCertificateMessage);
        }

        public void DeleteCertificate(string deleteCertificate)
        {
            String certificateDeleteStatus = "";
            IWebElement certificateTable = driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[5]/div[1]/div[2]/div/table"));
            List<IWebElement> allEducationRow = new List<IWebElement>(certificateTable.FindElements(By.TagName("tbody")));
            foreach (var row in allEducationRow)
            {
                var resultCols = row.FindElements(By.TagName("td"));
                if (resultCols[0].Text == deleteCertificate)
                {
                    List<IWebElement> allIconsRow = new List<IWebElement>(row.FindElements(By.TagName("i")));
                    foreach (var icon in allIconsRow)
                    {
                        if (icon.GetAttribute("class") == "remove icon")
                        {
                            TurnOnWait();
                            icon.Click();
                            certificateDeleteStatus = "";
                        }
                        else
                        {
                            certificateDeleteStatus = "Undefined Error";
                        }
                    }
                    break;
                }
                else
                {
                    certificateDeleteStatus = "The certificate intented to delete is not in the list";
                }
            }
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

                if (resultCols[2].Text == deleteCertificate)
                {
                    deletionCertificateStatus = "N";
                    deletionCertificateMessage = "Certificate deleted unsuccessfully";

                    break;
                }
                else
                {
                    deletionCertificateStatus = "Y";
                    deletionCertificateMessage = "Certificate was deletion successfully";
                }
            }
            Console.WriteLine(deletionCertificateMessage);
            return (DeletionStatus: deletionCertificateStatus, DeletionMessage: deletionCertificateMessage);
        }
    }
}
