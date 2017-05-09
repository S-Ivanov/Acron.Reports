using ExcelReportingLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ExcelReportingServiceLib
{
    public class ExcelReportingService : IExcelReportingService
    {
        public string Echo(string data)
        {
            return data;
        }

        public Stream GetExcelReport(string data, string configuration)
        {
            try
            {
                IDictionary<string, string> configurationDict = GetConfiguration(configuration);
                string assemblyName = configurationDict["модуль"];
                string className = configurationDict["класс"];

                var excelGeneratorHandle = Activator.CreateInstanceFrom(assemblyName, className);
                IExcelReporting excelGenerator = excelGeneratorHandle.Unwrap() as IExcelReporting;
                excelGenerator.Prepare(data, configurationDict);
                return excelGenerator.GetExcelReport();
            }
            catch (Exception e)
            {
                throw new FaultException(e.Message);
            }
        }

        public string GetExcelReportBase64(string data, string configuration)
        {
            Stream stream = GetExcelReport(data, configuration);
            byte[] bytes;
            if (stream is MemoryStream)
                bytes = (stream as MemoryStream).ToArray();
            else
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    bytes = ms.ToArray();
                }
            }
            return Convert.ToBase64String(bytes);
        }

        public static IDictionary<string, string> GetConfiguration(string configuration)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(configuration))
            {
                string[] pairs = configuration.Split(';');
                foreach (var pair in pairs)
                {
                    string[] pairParts = pair.Split('=');
                    if (pairParts.Length == 2)
                    {
                        pairParts[0] = pairParts[0].Trim();
                        pairParts[1] = pairParts[1].Trim();
                        if (!string.IsNullOrEmpty(pairParts[0]) && !string.IsNullOrEmpty(pairParts[1]))
                            result.Add(pairParts[0], pairParts[1]);
                    }
                }
            }
            return result;
        }
    }
}
