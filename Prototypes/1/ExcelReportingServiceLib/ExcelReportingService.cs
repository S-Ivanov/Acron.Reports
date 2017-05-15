using System;
using System.Configuration;
using System.IO;
using System.ServiceModel;

namespace ExcelReportingServiceLib
{
    public class ExcelReportingService : IExcelReportingService
    {
        public string GetReport(string id, string configuration, string data)
        {
            string fileName = ConfigurationManager.AppSettings["excel-" + id];
            if (string.IsNullOrEmpty(fileName))
                throw new FaultException("Report id is wrong");
            else
            {
                try
                {
                    return Convert.ToBase64String(File.ReadAllBytes(fileName));
                }
                catch
                {
                    throw new FaultException("Error loading file");
                }
            }
        }
    }
}
