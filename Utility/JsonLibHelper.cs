namespace CompetionTaskMarsAutomation.Utility
{
    public class JsonLibHelper
    {
        public class EducationDataList
        {
            public string country { get; set; }
            public string university { get; set; }
            public string title { get; set; }
            public string degree { get; set; }
            public string graduationYear { get; set; }
        }
       public class CertificateDataList
       {
            public string certificate { get; set; }
            public string from { get; set; }
            public string year { get; set; }
        }
        public class educationTableData
        {
            public string educationTableTitle { get; set; }
            public string educationTableDegree { get; set; }
        }
        public class certificateTableData
        {
            public string educationTableTitle { get; set; }
        }

    }
}
