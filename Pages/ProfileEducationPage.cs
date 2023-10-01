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

namespace CompetitionTaskMars.Pages
{
    public class ProfileEducationPage: MarsBaseClass
    {
        public ProfileEducationPage()
        {

        }

        public void ClearEducationData()
        {
            IWebElement educationTable = driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[4]/div/div[2]/div/table"));
            List<IWebElement> allEducationRow = new List<IWebElement>(educationTable.FindElements(By.TagName("tbody")));
           
            for (int i=1; i<= allEducationRow.Count(); i++)
            {
                //Console.WriteLine($"click count--{i}");
                driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[4]/div/div[2]/div/table/tbody[1]/tr/td[6]/span[2]/i")).Click();
                TurnOnWait();
            }
        }

        public static object AddEachEducationData(JsonLibHelper.EducationData item)
        {
            //Console.WriteLine($"Country: {item.Country}, Title: {item.Title}");
            driver.FindElement(By.XPath("/html/body/div[1]/div/section[2]/div/div/div/div[3]/form/div[4]/div/div[2]/div/table/thead/tr/th[6]/div")).Click();
            EnterEducation(item);
            driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[4]/div/div[2]/div/div/div[3]/div/input[1]")).Click();

            return "Pass";
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

        public object EnterEditEducation(JsonLibHelper.EducationData item)
        {
            driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[4]/div/div[2]/div/table/tbody[1]/tr/td[6]/span[1]/i")).Click();
            EnterEducation(item);
            driver.FindElement(By.CssSelector("input[class*='ui teal button']")).Click();
          
            return "Pass";
        }

        public object DeleteEducation(JsonLibHelper.EducationData item)
        {
            driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[4]/div/div[2]/div/table/tbody/tr/td[6]/span[2]/i")).Click();
            return "Pass";
        }

    }
}
