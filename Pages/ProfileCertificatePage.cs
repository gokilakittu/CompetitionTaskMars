using CompetionTaskMarsAutomation.Utility;
using DocumentFormat.OpenXml.Bibliography;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Org.BouncyCastle.Tls;
using static CompetionTaskMarsAutomation.Utility.JsonLibHelper;

namespace CompetitionTaskMars.Pages
{
    public class ProfileCertificatePage : MarsBaseClass
    {
        public object ReturnStatus { get; private set; }
        public object ReturnMessage { get; private set; }
        public IWebDriver driver { get; }

        public ProfileCertificatePage(IWebDriver driver)
        {
            this.driver = driver;
        }

        IWebElement certificateTable => driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[5]/div[1]/div[2]/div/table"));
        IWebElement deleteCertBtn => driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[5]/div[1]/div[2]/div/table/tbody/tr/td[4]/span[2]/i"));
        IWebElement addNewCertBtn => driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[5]/div[1]/div[2]/div/table/thead/tr/th[4]/div"));
        IWebElement addCertItemBtn => driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[5]/div[1]/div[2]/div/div/div[3]/input[1]"));
        IWebElement certNameTxt => driver.FindElement(By.Name("certificationName"));
        IWebElement certFromTxt => driver.FindElement(By.Name("certificationFrom"));
        SelectElement certYearSel => new SelectElement(driver.FindElement(By.Name("certificationYear")));
        IWebElement editCertBtn => driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[5]/div[1]/div[2]/div/table/tbody[1]/tr/td[4]/span[1]/i"));
        IWebElement updateCertBtn => driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[5]/div[1]/div[2]/div/table/tbody/tr/td/div/span/input[1]"));
        IWebElement certSuccessMessage => driver.FindElement(By.ClassName("ns-type-success"));
        IWebElement certErrorMessage => driver.FindElement(By.ClassName("ns-type-error"));

        public void ClearCertificateData()
        {

            List<IWebElement> allCertificateRow = new List<IWebElement>(certificateTable.FindElements(By.TagName("tbody")));

            for (int i = 1; i <= allCertificateRow.Count(); i++)
            {
                deleteCertBtn.Click();
                TurnOnWait();
            }
        }
        public bool IsDataVisibleInTableRow(string visibleCertificate)
        {

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
            if (IsDataVisibleInTableRow(item.Certificate) == true)
            {
                ReturnStatus = "Fail";
                ReturnMessage = $"The certification {item.Certificate} which was intented to added was already present in the list";
            }
            else
            {
                addNewCertBtn.Click();
                EnterCertificate(item);
                addCertItemBtn.Click();

                if (certSuccessMessage.Displayed)
                {
                    ReturnStatus = "Pass";
                    ReturnMessage = $"The certification {item.Certificate} was added to the list successfully.";
                }
                else if (certErrorMessage.Displayed)
                {
                    ReturnStatus = "Fail";
                    ReturnMessage = $"The certification {item.Certificate} was not added to the list.";
                }
                else
                {
                    ReturnStatus = "Fail";
                    ReturnMessage = $"The certification {item.Certificate} was not added to the list.";
                }
            }
            Thread.Sleep(8000);
            return new ReturnObjContainer(ReturnStatus, ReturnMessage);
        }

        public void EnterCertificate(JsonLibHelper.CertificateData item)
        {
            TurnOnWait();
            TurnOnWait();
            certNameTxt.Clear();
            certNameTxt.SendKeys(item.Certificate);

            certFromTxt.Clear();
            certFromTxt.SendKeys(item.From);

            certYearSel.SelectByText(item.Year);
        }

        public JsonLibHelper.ReturnObjContainer EnterEditCertificate(JsonLibHelper.CertificateData item)
        {
            editCertBtn.Click();
            EnterCertificate(item);
            updateCertBtn.Click();

            if (certSuccessMessage.Displayed)
            {
                ReturnStatus = "Pass";
                ReturnMessage = $"The certification {item.Certificate} was updated to the list successfully.";
            }
            else if (certErrorMessage.Displayed)
            {
                ReturnStatus = "Fail";
                ReturnMessage = $"The certification {item.Certificate} was not in the list to the list.";
            }
            else
            {
                ReturnStatus = "Fail";
                ReturnMessage = $"There was error in editing {item.Certificate}.";
            }
            Thread.Sleep(8000);
            return new ReturnObjContainer(ReturnStatus, ReturnMessage);
        }

        public JsonLibHelper.ReturnObjContainer DeleteCertificate(JsonLibHelper.CertificateData item)
        {
            deleteCertBtn.Click();

            if (certSuccessMessage.Displayed)
            {
                ReturnStatus = "Pass";
                ReturnMessage = $"The certification {item.Certificate}  was deleted successfully.";
            }
            else if (certErrorMessage.Displayed)
            {
                ReturnStatus = "Fail";
                ReturnMessage = $"The certification {item.Certificate} was not in the list to the list.";
            }
            else
            {
                ReturnStatus = "Fail";
                ReturnMessage = $"There was error in deleting {item.Certificate}.";
            }
            Thread.Sleep(8000);
            return new ReturnObjContainer(ReturnStatus, ReturnMessage);
        }
    }
}
