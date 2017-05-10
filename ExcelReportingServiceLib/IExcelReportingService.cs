using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ExcelReportingServiceLib
{
    /// <summary>
    /// Контракт службы генерации отчетов Excel
    /// </summary>
    [ServiceContract]
    public interface IExcelReportingService
    {
        ///// <summary>
        ///// Генерировать отчет Excel
        ///// </summary>
        ///// <param name="data">исходные данные для генерации</param>
        ///// <param name="configuration">
        ///// Строка конфигурации в виде пар "ключ = значение", разделенных символом ';' (точка с запятой)
        ///// Предопределенные ключи:
        ///// - модуль - эквивалентно параметру assemblyFile метода Activator.CreateInstanceFrom (String, String)
        ///// - класс - эквивалентно параметру typeName метода Activator.CreateInstanceFrom (String, String)
        ///// - формат - обозначение формата данных - необязательно
        ///// </param>
        ///// <returns>массив байтов, содержащий отчет Excel</returns>
        //[OperationContract]
        //Stream GetExcelReport(string data, string configuration);

        ///// <summary>
        ///// Генерировать отчет Excel
        ///// </summary>
        ///// <param name="data">исходные данные для генерации</param>
        ///// <param name="configuration">
        ///// Строка конфигурации в виде пар "ключ = значение", разделенных символом ';' (точка с запятой)
        ///// Предопределенные ключи:
        ///// - модуль - эквивалентно параметру assemblyFile метода Activator.CreateInstanceFrom (String, String)
        ///// - класс - эквивалентно параметру typeName метода Activator.CreateInstanceFrom (String, String)
        ///// - формат - обозначение формата данных - необязательно
        ///// </param>
        ///// <returns>массив байтов, содержащий отчет Excel и закодированный в bse64</returns>
        ///// <remarks>
        ///// Метод специально оптимизирован для работы Java-клиента, использующего для доступа к web-сервису фреймворк Axis2
        ///// <see cref="http://axis.apache.org/axis2/java/core/"/>
        ///// Причина оптимизации - указанный фреймворк некорректно работает с возвращемым байтовым массивом, в который автоматически представляется
        ///// метод <see cref="IExcelReportingService.GetExcelReport(string, string)"/>
        ///// Поскольку web-сервис все равно преобразует байтовый массив в base64, достаточно сделать такое преобразование самостоятельно
        ///// и возвращать строку; на клиенте эту строку нужно преобразовать обратно base64 -> byte[]
        ///// </remarks>
        //[OperationContract]
        //string GetExcelReportBase64(string data, string configuration);

        //[OperationContract]
        //string Echo(string data);

        [WebGet(UriTemplate = "excel?id={id}", BodyStyle = WebMessageBodyStyle.Bare)]
        //[WebGet(UriTemplate = "c", BodyStyle = WebMessageBodyStyle.Bare)]
        [OperationContract]
        Stream GetHtml(int id);
        //string GetHtml();

    }
}
