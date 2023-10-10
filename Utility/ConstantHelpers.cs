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
        
        public static string projectRelativePath = @"..\..\..\";
        public static string projectPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, projectRelativePath);
        public static string baseProjectPath = Path.GetFullPath(projectPath);
     
        //ScreenshotPath
        public static string screenShotPath = "";

        //loginExcelSheetPath
        public static string loginExcelSheetPath = @"C:\\Visual_studio_project\\CompetitionTaskMarsAutomation\\TestData\\LoginExcelData.xlsx";

        //Education path
        public static string educationDataPath = baseProjectPath + @"TestData\EducationData.json";
        public static string educationDataDuplicatePath = baseProjectPath + @"TestData\EducationDataWithDuplicateData.json";
        public static string educationEditDataPath = baseProjectPath + @"\TestData\EducationEditData.json";
        public static string educationDeleteDataPath = baseProjectPath + @"\TestData\EducationDeleteData.json";

        
        //Certrificates path
        public static string certificateDataPath = baseProjectPath + @"\TestData\CertificateData.json";
        public static string certificateDataDuplicatePath = baseProjectPath + @"\TestData\CertificateDataWithDuplicateData.json";
        public static string certificateEditDataPath = baseProjectPath + @"\TestData\CertificateEditData.json";
        public static string certificateDeleteDataPath = baseProjectPath + @"\TestData\CertificateDeleteData.json";
        
        //ExtentReportsPath
        public static string extendReportsPath = baseProjectPath + @"TestReport\\";

        public static string sampleDataPath = baseProjectPath + @"\TestData\sample.json";

        //Current User Name
        public static string currentUser = "";
    }
}
