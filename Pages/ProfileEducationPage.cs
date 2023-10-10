using AventStack.ExtentReports;
using CompetionTaskMarsAutomation.Utility;
using CompetitionTaskMars.Utility;
using DocumentFormat.OpenXml.Math;
using NPOI.SS.Formula.Functions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CompetionTaskMarsAutomation.Utility.JsonLibHelper;

namespace CompetitionTaskMars.Pages
{
    public class ProfileEducationPage: MarsBaseClass
    {
        public object ReturnStatus { get; private set; }
        public object ReturnMessage { get; private set; }
        public object IsItEmpty { get; private set; }
        
        public ProfileEducationPage()
        {

        }

        public void ClearEducationData()
        {
            IWebElement educationTable = driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[4]/div/div[2]/div/table"));
            List<IWebElement> allEducationRow = new List<IWebElement>(educationTable.FindElements(By.TagName("tbody")));
           
            for (int i=1; i<= allEducationRow.Count(); i++)
            {
                driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[4]/div/div[2]/div/table/tbody[1]/tr/td[6]/span[2]/i")).Click();
                TurnOnWait();
            }
        }

        public JsonLibHelper.ReturnObjContainer AddEachEducationData(JsonLibHelper.EducationData item)
        {
            var isEducationTableEmptyStatus = isEducationTableEmpty();
            int isEducationTableEmptyResult = true.CompareTo(isEducationTableEmptyStatus.isItEmpty);

            if (isEducationTableEmptyResult == 0)
            {
                driver.FindElement(By.XPath("/html/body/div[1]/div/section[2]/div/div/div/div[3]/form/div[4]/div/div[2]/div/table/thead/tr/th[6]/div")).Click();
                TurnOnWait();
                EnterEducation(item);
                driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[4]/div/div[2]/div/div/div[3]/div/input[1]")).Click();
                ReturnStatus = "Pass";
                ReturnMessage = $"The first data({item.Title}-{item.Degree})  from the education JSON file has been added to the list";
            }
            else
            {
                IWebElement educationTable = driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[4]/div/div[2]/div/table"));
                List<IWebElement> allEducationRow = new List<IWebElement>(educationTable.FindElements(By.TagName("tbody")));
                foreach (var education in allEducationRow)
                {
                    List<IWebElement> tableEducationRow = new List<IWebElement>(education.FindElements(By.TagName("td")));
                    TurnOnWait();
                    var resultCols = education.FindElements(By.TagName("td"));

                    if ((resultCols[2].Text == item.Title) && (resultCols[3].Text == item.Degree))
                    {
                        driver.FindElement(By.XPath("/html/body/div[1]/div/section[2]/div/div/div/div[3]/form/div[4]/div/div[2]/div/table/thead/tr/th[6]/div")).Click();
                        EnterEducation(item);
                        driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[4]/div/div[2]/div/div/div[3]/div/input[1]")).Click();
                        ReturnStatus = "Fail";
                        ReturnMessage = $"Education({item.Title}-{item.Degree}) intended to be added is already in the list";
                        break;
                    }
                    else
                    {
                        driver.FindElement(By.XPath("/html/body/div[1]/div/section[2]/div/div/div/div[3]/form/div[4]/div/div[2]/div/table/thead/tr/th[6]/div")).Click();
                        EnterEducation(item);
                        driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[4]/div/div[2]/div/div/div[3]/div/input[1]")).Click();
                        ReturnStatus = "Pass";
                        ReturnMessage = $"Education({item.Title}-{item.Degree})  was added to the education list";
                    }
                }
            }
            return new ReturnObjContainer(ReturnStatus, ReturnMessage);
        }

        private static void EnterEducation(JsonLibHelper.EducationData item)
        {
            TurnOnWait();
            IWebElement educationInstituteName = driver.FindElement(By.Name("instituteName"));
            educationInstituteName.Clear();
            educationInstituteName.SendKeys(item.University);

            SelectElement selectEducationCountry = new SelectElement(driver.FindElement(By.Name("country")));
            selectEducationCountry.SelectByText(item.Country);

            SelectElement educationTitle = new SelectElement(driver.FindElement(By.Name("title")));
            educationTitle.SelectByText(item.Title);

            IWebElement educationDegree = driver.FindElement(By.Name("degree"));
            educationDegree.Clear();
            educationDegree.SendKeys(item.Degree);

            SelectElement SelectGraduationYear = new SelectElement(driver.FindElement(By.Name("yearOfGraduation")));
            SelectGraduationYear.SelectByText(item.GraduationYear);
        }

        public JsonLibHelper.ReturnObjContainer EnterEditEducation(JsonLibHelper.EducationData item)
        {
            driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[4]/div/div[2]/div/table/tbody[1]/tr/td[6]/span[1]/i")).Click();
            EnterEducation(item);
            driver.FindElement(By.CssSelector("input[class*='ui teal button']")).Click();
            //return "Pass";
            ReturnStatus = "Pass";
            ReturnMessage = $"The education {item.Title}-{item.Degree}  was deleted successfully.";

            return new ReturnObjContainer(ReturnStatus, ReturnMessage);
        }

        public JsonLibHelper.ReturnObjContainer DeleteEducation(JsonLibHelper.EducationData item)
        {
            driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[4]/div/div[2]/div/table/tbody/tr/td[6]/span[2]/i")).Click();
            //return "Pass";
            ReturnStatus = "Pass";
            ReturnMessage = $"The education {item.Title}-{item.Degree}  was deleted successfully.";

            return new ReturnObjContainer(ReturnStatus, ReturnMessage);
        }

        public JsonLibHelper.ReturnObjContainer isEducationTableEmpty()
        {
            IWebElement educationTable = driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[4]/div/div[2]/div/table"));
            List<IWebElement> allEducationRow = new List<IWebElement>(educationTable.FindElements(By.TagName("tbody")));
            if (allEducationRow.Count() > 0)
            {
                IsItEmpty = false;
            }
            else 
            {
                IsItEmpty = true;
            }
            return new ReturnObjContainer(IsItEmpty);
        }

        public JsonLibHelper.ReturnObjContainer IsEducationVisibleInTable(EducationData item)
        {
            IWebElement educationTable = driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[4]/div/div[2]/div/table"));
            List<IWebElement> allEducationRow = new List<IWebElement>(educationTable.FindElements(By.TagName("tbody")));
            if (allEducationRow.Count() > 0)
            {
                IsItEmpty = false;
            }
            else
            {
                IsItEmpty = true;
            }
            return new ReturnObjContainer(IsItEmpty);
        }
    }
}
