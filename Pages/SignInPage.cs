using CompetitionTaskMars;
using OpenQA.Selenium;

namespace CompetionTaskMarsAutomation.Pages
{
    public class SignInPage:MarsBaseClass
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


    }
}
