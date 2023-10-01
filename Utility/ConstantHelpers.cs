using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompetitionTaskMars.Utility
{
    public class ConstantHelpers
    {
        //Base Url
        public static string url = "http://localhost:5000";

        //ScreenshotPath
        public static string screenShotPath = "";

        //loginExcelSheetPath
        public static string loginExcelSheetPath = @"C:\\Visual_studio_project\\CompetitionTaskMarsAutomation\\TestData\\LoginExcelData.xlsx";

        //loginExcelSheetPath
        public static string educationDataPath = @"C:\\Visual_studio_project\\CompetitionTaskMarsAutomation\\TestData\\EducationData.json";
        
        public static string educationEditDataPath = @"C:\\Visual_studio_project\\CompetitionTaskMarsAutomation\\TestData\\EducationEditData.json";

        //loginExcelSheetPath
        public static string certificateDataPath = @"C:\\Visual_studio_project\\CompetitionTaskMarsAutomation\\TestData\\CertificateData.json";
        public static string certificateEditDataPath = @"C:\\Visual_studio_project\\CompetitionTaskMarsAutomation\\TestData\\CertificateEditData.json";
        
        //ExtentReportsPath
        public static string extendReportsPath = @"C:\\Visual_studio_project\\CompetitionTaskMarsAutomation\\TestReport\\";

        //Current User Name
        public static string currentUser = "";
    }
}
