using Newtonsoft.Json;

namespace CompetionTaskMarsAutomation.Utility
{
    public class JsonLibHelper
    {
        public static List<T> ReadJsonFile<T>(string filePath)
        {
            List<T> dataList = new List<T>();

            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);

                try
                {
                    dataList = JsonConvert.DeserializeObject<List<T>>(jsonData);
                }
                catch (JsonException ex)
                {
                    // Handle deserialization errors if needed
                    Console.WriteLine("Error deserializing JSON: " + ex.Message);
                }
            }
            else
            {
                Console.WriteLine("File not found: " + filePath);
            }

            return dataList;
        }

        public class EducationData
        {
            public string Country { get; set; }
            public string University { get; set; }
            public string Title { get; set; }
            public string Degree { get; set; }
            public string GraduationYear { get; set; }
            public string ExpectedEducationResult { get; set; }
        }

        public class CertificateData
        {
            public string Certificate { get; set; }
            public string From { get; set; }
            public string Year { get; set; }
            public string ExpectedCertificateResult { get; set; }
        }

        public class ReturnObjContainer
        {
            public object returnStatus;
            public object returnMessage;

            public ReturnObjContainer(object returnStatus, object returnMessage)
            {
                this.returnStatus = returnStatus;
                this.returnMessage = returnMessage;
            }
        }
    }
}
