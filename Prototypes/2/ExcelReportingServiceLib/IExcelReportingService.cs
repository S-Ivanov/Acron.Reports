using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace ExcelReportingServiceLib
{
    [ServiceContract]
    public interface IExcelReportingService
    {
        [OperationContract]
        [WebGet(UriTemplate = "/GetReport/id={id}", BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetReport(string id);
    }
}
