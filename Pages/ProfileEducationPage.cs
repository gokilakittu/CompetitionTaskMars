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
    public class ProfileEducationPage : MarsBaseClass
    {
        public object ReturnStatus { get; private set; }
        public object ReturnMessage { get; private set; }
        public object IsItEmpty { get; private set; }
        public IWebDriver driver { get; }

        public ProfileEducationPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        IWebElement educationTable => driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[4]/div/div[2]/div/table"));
        IWebElement addNewEduBtn => driver.FindElement(By.XPath("/html/body/div[1]/div/section[2]/div/div/div/div[3]/form/div[4]/div/div[2]/div/table/thead/tr/th[6]/div"));
        IWebElement addEduItemBtn => driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[4]/div/div[2]/div/div/div[3]/div/input[1]"));
        IWebElement educationInstituteName => driver.FindElement(By.Name("instituteName"));
        SelectElement selectEducationCountry => new SelectElement(driver.FindElement(By.Name("country")));
        SelectElement educationTitle => new SelectElement(driver.FindElement(By.Name("title")));
        IWebElement educationDegree => driver.FindElement(By.Name("degree"));
        SelectElement SelectGraduationYear => new SelectElement(driver.FindElement(By.Name("yearOfGraduation")));
        IWebElement editEduBtn => driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[4]/div/div[2]/div/table/tbody[1]/tr/td[6]/span[1]/i"));
        IWebElement updateEduBtn => driver.FindElement(By.CssSelector("input[class*='ui teal button']"));
        IWebElement deleteEduBtn => driver.FindElement(By.XPath("//*[@id=\"account-profile-section\"]/div/section[2]/div/div/div/div[3]/form/div[4]/div/div[2]/div/table/tbody/tr/td[6]/span[2]/i"));
        IWebElement eduSuccessMessage => driver.FindElement(By.ClassName("ns-type-success"));
        IWebElement eduErrorMessage => driver.FindElement(By.ClassName("ns-type-error"));

        public void ClearEducationData()
        {
            List<IWebElement> allEducationRow = new List<IWebElement>(educationTable.FindElements(By.TagName("tbody")));

            for (int i = 1; i <= allEducationRow.Count(); i++)
            {
                deleteEduBtn.Click();
                TurnOnWait();
            }
        }

        public JsonLibHelper.ReturnObjContainer AddEachEducationData(JsonLibHelper.EducationData item)
        {
            var isEducationTableEmptyStatus = isEducationTableEmpty();
            int isEducationTableEmptyResult = true.CompareTo(isEducationTableEmptyStatus.isItEmpty);

            if (isEducationTableEmptyResult == 0)
            {
                addNewEduBtn.Click();
                TurnOnWait();
                EnterEducation(item);
                addEduItemBtn.Click();
                if (eduSuccessMessage.Displayed)
                {
                    ReturnStatus = "Pass";
                    ReturnMessage = $"The first data({item.Title}-{item.Degree})  from the education JSON file has been added to the list";
                }
                else if (eduErrorMessage.Displayed)
                {
                    ReturnStatus = "Fail";
                    ReturnMessage = $"There was some error in adding the education.";
                }
            }
            else
            {
                List<IWebElement> allEducationRow = new List<IWebElement>(educationTable.FindElements(By.TagName("tbody")));
                foreach (var education in allEducationRow)
                {
                    List<IWebElement> tableEducationRow = new List<IWebElement>(education.FindElements(By.TagName("td")));
                    TurnOnWait();
                    var resultCols = education.FindElements(By.TagName("td"));

                    if ((resultCols[2].Text == item.Title) && (resultCols[3].Text == item.Degree))
                    {
                        addNewEduBtn.Click();
                        EnterEducation(item);
                        addEduItemBtn.Click();
                        ReturnStatus = "Fail";
                        ReturnMessage = $"Education({item.Title}-{item.Degree}) intended to be added is already in the list";
                        break;
                    }
                    else
                    {
                        addNewEduBtn.Click();
                        EnterEducation(item);
                        addEduItemBtn.Click();
                        if (eduSuccessMessage.Displayed)
                        {
                            ReturnStatus = "Pass";
                            ReturnMessage = $"Education({item.Title}-{item.Degree})  was added to the education list";
                        }
                        else if (eduErrorMessage.Displayed)
                        {
                            ReturnStatus = "Fail";
                            ReturnMessage = $"There was some error in adding the education.";
                        }

                    }
                }
            }
            Thread.Sleep(8000);
            return new ReturnObjContainer(ReturnStatus, ReturnMessage);
        }

        public void EnterEducation(JsonLibHelper.EducationData item)
        {
            TurnOnWait();
            educationInstituteName.Clear();
            educationInstituteName.SendKeys(item.University);
            selectEducationCountry.SelectByText(item.Country);
            educationTitle.SelectByText(item.Title);
            educationDegree.Clear();
            educationDegree.SendKeys(item.Degree);
            SelectGraduationYear.SelectByText(item.GraduationYear);
        }

        public JsonLibHelper.ReturnObjContainer EnterEditEducation(JsonLibHelper.EducationData item)
        {
            editEduBtn.Click();
            EnterEducation(item);
            updateEduBtn.Click();

            if (eduSuccessMessage.Displayed)
            {
                ReturnStatus = "Pass";
                ReturnMessage = $"The education {item.Title}-{item.Degree} was updated to the list successfully.";
            }
            else if (eduErrorMessage.Displayed)
            {
                ReturnStatus = "Fail";
                ReturnMessage = $"The education {item.Title}-{item.Degree} was not in the list.";
            }
            else
            {
                ReturnStatus = "Fail";
                ReturnMessage = $"There was error in editing {item.Title}-{item.Degree}.";
            }

            Thread.Sleep(8000);
            return new ReturnObjContainer(ReturnStatus, ReturnMessage);
        }

        public JsonLibHelper.ReturnObjContainer DeleteEducation(JsonLibHelper.EducationData item)
        {
            deleteEduBtn.Click();

            if (eduSuccessMessage.Displayed)
            {
                ReturnStatus = "Pass";
                ReturnMessage = $"The education {item.Title}-{item.Degree} was deleted successfully.";
            }
            else if (eduErrorMessage.Displayed)
            {
                ReturnStatus = "Fail";
                ReturnMessage = $"The education {item.Title}-{item.Degree} was not in the list.";
            }
            else
            {
                ReturnStatus = "Fail";
                ReturnMessage = $"There was error in deleting {item.Title}-{item.Degree}.";
            }

            Thread.Sleep(8000);
            return new ReturnObjContainer(ReturnStatus, ReturnMessage);
        }

        public JsonLibHelper.ReturnObjContainer isEducationTableEmpty()
        {
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
