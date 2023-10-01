using CompetitionTaskMars;
using OpenQA.Selenium;

namespace MarsQA_1.SpecflowPages.Pages
{
    public class LoginPage: MarsBaseClass
    {
        //ExcelLibHelper excelObj = new ExcelLibHelper();
        public void LoginSteps()
        {
            NavigateUrl();
            /*string jsonResponse = excelObj.ReadExcel();
            dynamic responseObj = JsonConvert.DeserializeObject(jsonResponse);
            string uName = responseObj[0].UserName;
            string uPassword = responseObj[0].Password;*/
            string uName = "testdata@gmail.com";
            string uPassword = "123123";
            driver.FindElement(By.XPath("//A[@class='item'][text()='Sign In']")).Click();
            driver.FindElement(By.XPath("(//INPUT[@type='text'])[2]")).SendKeys(uName);
            driver.FindElement(By.XPath("//INPUT[@type='password']")).SendKeys(uPassword);
            driver.FindElement(By.XPath("//BUTTON[@class='fluid ui teal button'][text()='Login']")).Click();
        }
        public bool ConfirmUser()
        {
            bool ValidateAvailability = false;
            TurnOnWait();
            IWebElement currentUser = driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/div[1]/div[2]/div/span"));
            if (currentUser.Text != null)
            {
                ValidateAvailability = true;
            }
            return ValidateAvailability;
        }
    }
}