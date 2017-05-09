using System;
using System.IO;
using System.Reflection;

namespace SIvanov.ExcelGenerator
{
    public class AddInGenerator : IExcelGenerator
    {
        public AddInGenerator(string assemblyName, string className)
        {
            this.assemblyName = assemblyName;
            this.className = className;
        }

        string assemblyName;
        string className;

        #region Реализация IExcelGenerator

        public Stream Generate(string data, string configuration)
        {
            var excelGeneratorHandle = Activator.CreateInstanceFrom(assemblyName, className);
            IExcelGenerator excelGenerator = excelGeneratorHandle.Unwrap() as IExcelGenerator;
            return excelGenerator.Generate(data, configuration);
        }

        #endregion Реализация IExcelGenerator
    }
}
