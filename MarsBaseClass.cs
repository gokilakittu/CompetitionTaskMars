using CompetitionTaskMars.Utility;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using NUnit.Framework;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports;

namespace CompetitionTaskMars
{
    /*
    get url
    page time out,
    implicit wait
    delete cookie
    maximize window*/

    [SetUpFixture]
    public class MarsBaseClass
    {
        public static IWebDriver driver;

        protected ExtentReports extent;
        protected ExtentTest test;

        public void InitializeReport()
        {
            var htmlReporter = new ExtentHtmlReporter(ConstantHelpers.extendReportsPath);
            extent = new ExtentReports();
            extent.AttachReporter(htmlReporter);
        }
        public static void Initialize()
        {
            driver = new ChromeDriver();
            TurnOnWait();
            driver.Manage().Window.Maximize();
            //ExtentReportLibHelper.CreateTest(TestContext.CurrentContext.Test.MethodName);
        }
       
        public static void NavigateUrl()
        {
            driver.Navigate().GoToUrl(BaseUrl);
        }

        public static string BaseUrl
        {
            get { return ConstantHelpers.url; }
        }

        //Implicit Wait
        public static void TurnOnWait()
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            Thread.Sleep(2000);
        }

        public static void PageRefresh()
        {
            driver.Navigate().Refresh();
        }

        //Close the browser
        public static void CleanUp()
        {
            driver.Close();
            //driver.Quit();
        }

        public static void NavigateToProfileEducation()
        {
            driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[1]/a[3]")).Click();
        }

        public static void NavigateToProfileCertification()
        {
            driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[1]/a[4]")).Click();

        }

        public void FinalizeReport()
        {
            extent.Flush();
        }
    }
}
