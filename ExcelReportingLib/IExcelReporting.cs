using System.Collections.Generic;
using System.IO;

namespace ExcelReportingLib
{
    public interface IExcelReporting
    {
        /// <summary>
        /// Подготовить генератор к работе
        /// </summary>
        /// <param name="data">исходные данные для генерации</param>
        /// <param name="configuration">
        /// Словарь конфигурации
        /// Предопределенные ключи:
        /// - модуль - эквивалентно параметру assemblyFile метода Activator.CreateInstanceFrom (String, String)
        /// - класс - эквивалентно параметру typeName метода Activator.CreateInstanceFrom (String, String)
        /// - формат - обозначение формата данных - необязательно
        /// </param>
        void Prepare(string data, IDictionary<string, string> configuration);

        Stream GetExcelReport();
    }
}
