using System.IO;

namespace SIvanov.ExcelGenerator
{
    public interface IExcelGenerator
    {
        Stream Generate(string data, string configuration);
    }
}
