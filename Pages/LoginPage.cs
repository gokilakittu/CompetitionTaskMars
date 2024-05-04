using CompetionTaskMarsAutomation.Utility;
using CompetitionTaskMars;
using Newtonsoft.Json;
using OpenQA.Selenium;

namespace MarsQA_1.SpecflowPages.Pages
{
    public class LoginPage : MarsBaseClass
    {
        private readonly IWebDriver driver;

        public LoginPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        IWebElement signinLink => driver.FindElement(By.XPath("//A[@class='item'][text()='Sign In']"));
        IWebElement emailTxt => driver.FindElement(By.XPath("(//INPUT[@type='text'])[2]"));
        IWebElement passwordTxt => driver.FindElement(By.XPath("//INPUT[@type='password']"));
        IWebElement loginBtn => driver.FindElement(By.XPath("//BUTTON[@class='fluid ui teal button'][text()='Login']"));
        IWebElement currentUser => driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/div[1]/div[2]/div/span"));


        ExcelLibHelper excelObj = new ExcelLibHelper();
        public void LoginSteps()
        {
            string jsonResponse = excelObj.ReadExcel();
            dynamic responseObj = JsonConvert.DeserializeObject(jsonResponse);
            string usernameExcel = responseObj[0].UserName;
            string passExcel = responseObj[0].Password;

            signinLink.Click();
            emailTxt.SendKeys(usernameExcel);
            passwordTxt.SendKeys(passExcel);
            loginBtn.Submit();
        }
        public bool ConfirmUser()
        {
            bool ValidateAvailability = false;
            TurnOnWait();
            if (currentUser.Text != null)
            {
                ValidateAvailability = true;
            }
            return ValidateAvailability;
        }
    }
}
