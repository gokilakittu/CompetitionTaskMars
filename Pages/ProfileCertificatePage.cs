using CompetionTaskMarsAutomation.Utility;
using DocumentFormat.OpenXml.Bibliography;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Org.BouncyCastle.Tls;
using static CompetionTaskMarsAutomation.Utility.JsonLibHelper;

namespace CompetitionTaskMars.Test
{
    public class ProfileCertificatePage : MarsBaseClass
    {
        public object ReturnStatus { get; private set; }
        public object ReturnMessage { get; private set; }

        
        public ProfileCertificatePage()
        {
        }

        public void ClearCertificateData()
        {
            IWebElement certificateTable = driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[5]/div[1]/div[2]/div/table"));

            List<IWebElement> allCertificateRow = new List<IWebElement>(certificateTable.FindElements(By.TagName("tbody")));

            for (int i = 1; i <= allCertificateRow.Count(); i++)
            {
                driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[5]/div[1]/div[2]/div/table/tbody/tr/td[4]/span[2]/i")).Click();
                TurnOnWait();
            }
        }
        public bool IsDataVisibleInTableRow(string visibleCertificate)
        {
            IWebElement certificateTable = driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[5]/div[1]/div[2]/div/table"));

            IReadOnlyCollection<IWebElement> tableRows = certificateTable.FindElements(By.TagName("tbody"));

            foreach (IWebElement row in tableRows)
            {
                IEnumerable<IWebElement> tableData = row.FindElements(By.TagName("td"));

                foreach (IWebElement data in tableData)
                {
                    if (data.Text.Contains(visibleCertificate))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public JsonLibHelper.ReturnObjContainer AddEachCertificateData(JsonLibHelper.CertificateData item)
        {
            if (IsDataVisibleInTableRow(item.Certificate) == true) {
                ReturnStatus = "Fail";
                ReturnMessage = $"The certification {item.Certificate} which was intented to added was already present in the list";
                //Assert.Fail($"The certification {item.Certificate} intended to add is already in the list");
            }
            else {
                driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[5]/div[1]/div[2]/div/table/thead/tr/th[4]/div")).Click();
                EnterCertificate(item);
                driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[5]/div[1]/div[2]/div/div/div[3]/input[1]")).Click();
                ReturnStatus = "Pass";
                ReturnMessage = $"The certification {item.Certificate}  was added to the list successfully.";
            }
            return new ReturnObjContainer(ReturnStatus, ReturnMessage);
        }

        private static void EnterCertificate(JsonLibHelper.CertificateData item)
        {
            TurnOnWait();
            IWebElement certificationName = driver.FindElement(By.Name("certificationName"));
            certificationName.Clear();
            certificationName.SendKeys(item.Certificate);

            IWebElement certificationFrom = driver.FindElement(By.Name("certificationFrom"));
            certificationFrom.Clear();
            certificationFrom.SendKeys(item.From);

            SelectElement selectCertificationYear = new SelectElement(driver.FindElement(By.Name("certificationYear")));
            selectCertificationYear.SelectByText(item.Year);
        }

        public JsonLibHelper.ReturnObjContainer EnterEditCertificate(JsonLibHelper.CertificateData item)
        {

            driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[5]/div[1]/div[2]/div/table/tbody[1]/tr/td[4]/span[1]/i")).Click();
            EnterCertificate(item);
            driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[5]/div[1]/div[2]/div/table/tbody/tr/td/div/span/input[1]")).Click();
            ReturnStatus = "Pass";
            ReturnMessage = "";
            return new ReturnObjContainer(ReturnStatus, ReturnMessage);
        }

        public JsonLibHelper.ReturnObjContainer DeleteCertificate(JsonLibHelper.CertificateData item)
        {
            driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[5]/div[1]/div[2]/div/table/tbody/tr/td[4]/span[2]/i")).Click();
            ReturnStatus = "Pass";
            ReturnMessage = $"The certification {item.Certificate}  was deleted successfully.";
            return new ReturnObjContainer(ReturnStatus, ReturnMessage);
            
        }
    }


    /*
     Kanjula code

    Assert.IsTrue(IsDataVisibleInTableRow(certificationsTable, certification), "Certification is not added successfully");


    private bool IsDataVisibleInTableRow(IWebElement table, string qualification)
    {
        IReadOnlyCollection<IWebElement> tableRows = table.FindElements(By.TagName("tbody"));

        foreach (IWebElement row in tableRows)
        {
            IEnumerable<IWebElement> tableData = row.FindElements(By.TagName("td"));

            foreach (IWebElement data in tableData)
            {
                if (data.Text.Contains(qualification))
                {
                    return true;
                }
            }
        }
        return false;
    }
    */
}
