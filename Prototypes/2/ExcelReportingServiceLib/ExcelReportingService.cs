using System.Configuration;
using System.IO;
using System.Net;
using System.ServiceModel.Web;

namespace ExcelReportingServiceLib
{
    public class ExcelReportingService : IExcelReportingService
    {
        public Stream GetReport(string id)
        {
            string fileName = ConfigurationManager.AppSettings["excel-" + id];
            if (string.IsNullOrEmpty(fileName))
                throw new WebFaultException(HttpStatusCode.Gone);
            else
            {
                try
                {
                    byte[] resultBytes = File.ReadAllBytes(fileName);
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    WebOperationContext.Current.OutgoingResponse.Headers.Add("Content-disposition", "inline; filename=excel.xlsx");
                    return new MemoryStream(resultBytes);
                }
                catch
                {
                    throw new WebFaultException(HttpStatusCode.NotFound);
                }
            }
        }
    }
}
