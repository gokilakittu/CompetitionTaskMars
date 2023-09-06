using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using DocumentFormat.OpenXml.Bibliography;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using System.Reflection;

namespace CompetitionTaskMars.Utility
{
    public class ExtentReportLibHelper:MarsBaseClass
    {
        //Reporter.log("",true);

        private static ExtentReports extentReports;
        private static ExtentTest extentTest;

        private static ExtentReports StartReporting()
        {
            
            if (extentReports == null)
            {
                //Directory.CreateDirectory(ConstantHelpers.extendReportsPath);
                extentReports = new ExtentReports();
                var htmlReporter = new ExtentHtmlReporter(ConstantHelpers.extendReportsPath+"MarsReport.html");
                extentReports.AttachReporter(htmlReporter);
            }
            return extentReports;
        }
        public static void CreateTest(string testName)
        {
            extentTest = StartReporting().CreateTest(testName);
        }
        public static void EndReporting()
        {
           StartReporting().Flush();
        }
        
        public static void LogInfo(string logInfo)
        {
            extentTest.Info(logInfo);
        }

        public static void LogPass(string passLogInfo)
        {
            extentTest.Pass(passLogInfo);
        }
            
        public static void LogFail(string failLogInfo)
        {
            extentTest.Fail(failLogInfo);
        }
        
        public static void LogScreenShot(string info,string screenShot) 
        {
            extentTest.Info(info, MediaEntityBuilder.CreateScreenCaptureFromBase64String(screenShot).Build());
        }
        public static void EndTest()
        {
            var testStatus = TestContext.CurrentContext.Result.Outcome.Status;
            var testMessage = TestContext.CurrentContext.Result.Message;
            switch (testStatus)
            {
                case TestStatus.Failed:
                    ExtentReportLibHelper.LogFail($"The test has Failed. Message:{testMessage}");
                    break;
                case TestStatus.Skipped:
                    ExtentReportLibHelper.LogInfo($"The test has skipped. Message:{testMessage}");
                    break;
                default:
                    ExtentReportLibHelper.LogPass($"The test has Passed. Message:{testMessage}");
                    break;
            }
            //ExtentReportLibHelper.LogInfo("End Reporting.");
            ExtentReportLibHelper.LogScreenShot("End Reporting.","Screenshot:" +ExtentReportLibHelper.GetScreenshot());
        }
        public static string GetScreenshot()
        {
            var file = ((ITakesScreenshot)driver).GetScreenshot();
            var img = file.AsBase64EncodedString;
            return img;
        }
    }
}
