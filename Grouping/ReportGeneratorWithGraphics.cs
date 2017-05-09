using DocumentFormat.OpenXml.Drawing.Charts;
using SpreadsheetLight;
using SpreadsheetLight.Charts;
using System.IO;
using System.Linq;

namespace Grouping
{
    /// <summary>
    /// Класс для генерации отчета с диаграммой
    /// </summary>
    public class ReportGeneratorWithGraphics : ReportGenerator
    {
        public ReportGeneratorWithGraphics() 
            : base()
        {
        }

        /// <summary>
        /// Генерировать отчет
        /// </summary>
        /// <returns></returns>
        public override Stream GetExcelReport()
        {
            MemoryStream stream = new MemoryStream();
            // добавить лист с диаграммой к сгенерированному базовому отчету
            var baseStream = base.GetExcelReport();
            stream = AddGraphics(baseStream, ReportData);
            stream.Position = 0;
            return stream;
        }

        /// <summary>
        /// Добавить диаграмму
        /// </summary>
        /// <param name="stream">сгенерированный базовый отчет</param>
        /// <param name="reportData">данные отчета</param>
        /// <returns></returns>
        MemoryStream AddGraphics(Stream stream, ReportData reportData)
        {
            MemoryStream result = new MemoryStream();
            using (SLDocument sl = new SLDocument(stream))
            {
                sl.AddWorksheet("Диаграмма");

                var range = AddData(sl, reportData);

                SLChart chart = sl.CreateChart(range.StartRowIndex, range.StartColumnIndex, range.EndRowIndex, range.EndColumnIndex, new SLCreateChartOptions { RowsAsDataSeries = true });
                chart.SetChartType(SLColumnChartType.ClusteredColumn3D);
                chart.SetChartPosition(1, 5, 26, 20);
                chart.Title.Text = "Динамика поставок";
                chart.ShowChartTitle(false);
                chart.ShowChartLegend(LegendPositionValues.Right, false);

                sl.InsertChart(chart);

                // сделать активным первый лист книги
                sl.SelectWorksheet(sl.GetWorksheetNames()[0]);

                // сохранение результатов
                sl.SaveAs(result);
            }
            return result;
        }

        /// <summary>
        /// Добавить на лист исходные данные для построения диаграммы
        /// </summary>
        /// <param name="sl">книга Excel, активный лист - Диаграмма</param>
        /// <param name="reportData">данные отчета</param>
        /// <returns>область исходных данных для диаграммы</returns>
        SLCellPointRange AddData(SLDocument sl, ReportData reportData)
        {
            var saleGroups = from Data in reportData.Data
                             from Region in Data.Region
                             from Company in Region.Company
                             from Sale in Company.Sale
                             let record = new
                             {
                                 productId = Sale.productId,
                                 sum = Sale.sum,
                                 month = Sale.date.Month
                             }
                             group record by record.productId into newGroup1
                             from newGroup2 in 
                             (from record in newGroup1
                              group record by record.month)
                             group newGroup2 by newGroup1.Key into mg
                             orderby mg.Key
                             select mg;

            int row = 2;
            int col = 2;
            foreach (var g in saleGroups)
            {
                sl.SetCellValue(row, 1, GetProductName(reportData, g.Key));
                col = 2;
                foreach (var g2 in g)
                {
                    if (row == 2)
                        sl.SetCellValue(row - 1, col, GetMonthName(g2.Key));
                    sl.SetCellValue(row, col, g2.Sum(x => x.sum));
                    col++;
                }
                row++;
            }

            SLStyle style = sl.CreateStyle();
            style.FormatCode = "#,##0";
            sl.SetCellStyle(2, 2, row - 1, col - 1, style);

            sl.AutoFitColumn(2, col - 1);

            return new SLCellPointRange(1, 1, row - 1, col - 1);
        }
    }
}
