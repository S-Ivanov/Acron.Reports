using System.ServiceModel;

namespace ExcelReportingServiceLib
{
    /// <summary>
    /// Контракт службы генерации отчетов Excel
    /// </summary>
    [ServiceContract]
    public interface IExcelReportingService
    {
        /// <summary>
        /// Генерировать отчет Excel
        /// </summary>
        /// <param name="id">идентификатор отчета</param>
        /// <param name="configuration">конфигурация отчета</param>
        /// <param name="data">исходные данные для генерации</param>
        /// <returns>массив байтов, содержащий отчет Excel и закодированный в base64</returns>
        /// <remarks>
        /// Метод специально оптимизирован для работы Java-клиента, использующего для доступа к web-сервису фреймворк Axis2
        /// <see cref="http://axis.apache.org/axis2/java/core/"/>
        /// Причина оптимизации - указанный фреймворк некорректно работает с возвращемым байтовым массивом
        /// Поскольку web-сервис все равно преобразует байтовый массив в base64, достаточно сделать такое преобразование самостоятельно
        /// и возвращать строку; на клиенте эту строку нужно преобразовать обратно base64 -> byte[]
        /// </remarks>
        [OperationContract]
        string GetReport(string id, string configuration, string data);
    }
}
