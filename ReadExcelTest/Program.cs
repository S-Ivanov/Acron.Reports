using ClosedXML.Excel;
using System;

namespace ReadExcelTest
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length == 0)
                Console.WriteLine("Необходим один параметр: полный путь к Excel-файлу.");
            else
            {
                try
                {
                    LoadAndShowExcel(args[0]);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка: " + ex.Message);
                }
            }

            Console.WriteLine();
            Console.Write("Для выхода нажмите Enter ...");
            Console.ReadLine();
        }

        private static void LoadAndShowExcel(string fileName)
        {
            using (var workbook = new XLWorkbook(fileName))
            using (var ws = workbook.Worksheet(1))
            {
                Console.WriteLine("Лист: " + ws.Name);
                for (int i = 1; i <= 20; i++)
                {
                    Console.WriteLine("A{0}: {1}", i, ws.Cell(i, "A").Value);
                }
            }
        }
    }
}
