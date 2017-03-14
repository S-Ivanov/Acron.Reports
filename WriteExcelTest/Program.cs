using ClosedXML.Excel;
using System;

namespace WriteExcelTest
{
    class Program
    {
        static void Main()
        {
            Console.Write("Введите имя файла: ");
            string fileName = Console.ReadLine();

            try
            {
                CreateExcelFile(fileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }

            Console.WriteLine();
            Console.Write("Для выхода нажмите Enter ...");
            Console.ReadLine();
        }

        private static void CreateExcelFile(string fileName)
        {
            Console.Write("Введите имя листа: ");
            string sheetName = Console.ReadLine();

            Console.WriteLine("Вводите данные (Enter - конец ввода):");

            using (var workbook = new XLWorkbook())
            using (var ws = workbook.Worksheets.Add(sheetName))
            {
                for (int i = 1; i <= 20; i++)
                {
                    string s = Console.ReadLine();
                    if (string.IsNullOrEmpty(s))
                        break;

                    ws.Cell(i, "A").Value = s;
                    ws.Cell(i, "A").Style.Border.OutsideBorder = XLBorderStyleValues.Thick;
                }

                workbook.SaveAs(fileName + ".xlsx");
            }
        }
    }
}
