using ClosedXML.Excel;
using ExcelReportingLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Grouping
{
    /// <summary>
    /// Класс для генерации отчета
    /// </summary>
    public class ReportGenerator : IExcelReporting
    {
        public ReportGenerator()
        {
        }

        /// <summary>
        /// Русская культура для форматирования
        /// </summary>
        static CultureInfo ruCultureInfo = new CultureInfo("ru-RU");

        #region Реализация IExcelReporting

        public void Prepare(string data, IDictionary<string, string> configuration)
        {
            if (configuration == null ||
                !configuration.ContainsKey(ExcelReportingCommon.ConfigurationKeys.Format) ||
                configuration[ExcelReportingCommon.ConfigurationKeys.Format] == ExcelReportingCommon.Formats.XML)
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(ReportData));
                this.ReportData = xmlSerializer.Deserialize(new StringReader(data)) as ReportData;
            }
            else if (configuration[ExcelReportingCommon.ConfigurationKeys.Format] == ExcelReportingCommon.Formats.CSV)
                this.ReportData = LoadCSV(data);
            else
                throw new ArgumentException("Unknown configurtion format", "configuration");
        }

        /// <summary>
        /// Генерировать отчет
        /// </summary>
        /// <returns></returns>
        public virtual Stream GetExcelReport()
        {
            MemoryStream stream = new MemoryStream();
            using (var wb = new XLWorkbook())
            {
                AddHierarchy(wb, ReportData);
                var table = AddTable(wb, ReportData);
                AddPivotTable(wb, table);
                AddProperties(wb);

                wb.SaveAs(stream);
            }
            stream.Position = 0;

            return stream;
        }

        #endregion Реализация IExcelReporting

        public static ReportData LoadCSV(string csvData)
        {
            var csv = ExcelReportingCommon.ParseCSV(csvData);

            // * Поставки
            // * Страны
            // * Регионы
            // * Компании
            // * Продукция
            if (csv.Count != 5)
                throw new ArgumentException();

            var countries = csv[1].ToDictionary(ss => ss[0], ss => ss[1]);
            var regions = csv[2].ToDictionary(ss => ss[0], ss => ss[1]);

            ReportData result = new ReportData();
            result.References = new ReportDataReferences();
            result.References.Companies.AddRange(csv[3].Select(x => new ReportDataReferencesCompany { id = byte.Parse(x[0]), name = x[1] }));
            result.References.Products.AddRange(csv[4].Select(x => new ReportDataReferencesProduct { id = byte.Parse(x[0]), name = x[1] }));

            foreach (var gCountry in csv[0].GroupBy(x => x[0]))
            {
                ReportDataCountry country = new ReportDataCountry { name = countries[gCountry.Key] };
                result.Data.Add(country);
                foreach (var gRegion in gCountry.GroupBy(x => x[1]))
                {
                    ReportDataCountryRegion region = new ReportDataCountryRegion { name = regions[gRegion.Key] };
                    country.Region.Add(region);
                    foreach (var gCompany in gRegion.GroupBy(x => x[2]))
                    {
                        ReportDataCountryRegionCompany company = new ReportDataCountryRegionCompany { companyId = byte.Parse(gCompany.Key) };
                        region.Company.Add(company);
                        company.Sale.AddRange(gCompany.Select(x =>
                            new ReportDataCountryRegionCompanySale
                            {
                                //countryId;regionId;companyId;productId;contract;date;sum
                                productId = byte.Parse(x[3]),
                                contract = x[4],
                                date = DateTime.ParseExact(x[5], "yyyy-MM-dd", ruCultureInfo),
                                sum = uint.Parse(x[6])
                            }));
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Данные отчета
        /// </summary>
        public ReportData ReportData { get; private set; }

        /// <summary>
        /// Получить имя месяца
        /// </summary>
        /// <param name="month">номер месяца (начинается с 1)</param>
        /// <returns></returns>
        public static string GetMonthName(int month)
        {
            return ruCultureInfo.DateTimeFormat.MonthNames[month - 1]; 
        }

        /// <summary>
        /// Нормализовать наименование региона
        /// </summary>
        /// <param name="regionName"></param>
        /// <returns></returns>
        static string NormalizeRegionName(string regionName)
        {
            return string.IsNullOrEmpty(regionName) ? "(без региона)" : regionName;
        }

        /// <summary>
        /// Получить наименование компании
        /// </summary>
        /// <param name="reportData">данные отчета</param>
        /// <param name="companyId">идентификатор компании</param>
        /// <returns></returns>
        public static string GetCompanyName(ReportData reportData, int companyId)
        {
            var company = reportData.References.Companies.FirstOrDefault(c => c.id == companyId);
            return company == null ? companyId.ToString() : company.name;
        }

        /// <summary>
        /// Получить наименование продукции
        /// </summary>
        /// <param name="reportData">данные отчета</param>
        /// <param name="productId">идентификатор продукции</param>
        /// <returns></returns>
        public static string GetProductName(ReportData reportData, int productId)
        {
            var product = reportData.References.Products.FirstOrDefault(p => p.id == productId);
            return product == null ? productId.ToString() : product.name;
        }

        /// <summary>
        /// Добавить свойства книги
        /// </summary>
        /// <param name="wb">книга Excel</param>
        private void AddProperties(XLWorkbook wb)
        {
            wb.Properties.Author = "С.Иванов";
            wb.Properties.Title = "Отчет о поставках за II квартал 2016 года";
            wb.Properties.Subject = "Отчет о поставках";
            wb.Properties.Category = "Отчеты";
            wb.Properties.Comments = "Отчет автоматически генерируется из учетной системы";
            wb.Properties.Company = "ПАО \"Дорогобуж\"";
        }

        /// <summary>
        /// Добавить лист со сводной таблицей
        /// </summary>
        /// <param name="wb">книга для добавления листа</param>
        /// <param name="table">данные для сводной таблицы</param>
        private void AddPivotTable(XLWorkbook wb, IXLTable table)
        {
            using (var ws = wb.Worksheets.Add("Сводная таблица"))
            {
                // После добавления фильтров таблицы pt.ReportFilters.Add сгенерированный файл при открытии выдавал ошибку
                // Решение: https://closedxml.codeplex.com/discussions/640419
                //var pt = ws.PivotTables.AddNew("Сводная_таблица", ws.Cell(1, 1), table);
                var pt = ws.PivotTables.AddNew("Сводная_таблица", ws.Cell(4, 1), table);

                pt.ReportFilters.Add("Страна");
                pt.ReportFilters.Add("Регион");
                pt.ReportFilters.Add("Продукция");

                pt.RowLabels.Add("Компания");

                pt.ColumnLabels.Add("Месяц");

                pt.Values.Add("Сумма").NumberFormat.NumberFormatId = 3;

                // TODO: автоподбор ширины колонок в сводной таблице не работает
                //ws.Columns().AdjustToContents();
                ws.Column(1).Width = 50;
                ws.Columns(2, 5).Width = 15;
            }
        }

        /// <summary>
        /// Добавить лист с умной таблицей
        /// </summary>
        /// <param name="wb">книга для добавления листа</param>
        /// <param name="reportData">данные</param>
        /// <returns>умная таблица</returns>
        private IXLTable AddTable(XLWorkbook wb, ReportData reportData)
        {
            DataTable dataTable = GetDataTable(reportData);
            using (var ws = wb.Worksheets.Add("Таблица данных"))
            {
                var tableWithData = ws.Cell(1, 1).InsertTable(dataTable, "Таблица", true);

                ws.Column("D").Hide();
                ws.Column("G").Style.NumberFormat.NumberFormatId = 3;

                ws.Columns().AdjustToContents();

                var protection = ws.Protect("123");
                protection.AutoFilter = true;

                return tableWithData;
            }
        }

        /// <summary>
        /// Добавить лист с группировками
        /// </summary>
        /// <param name="wb">книга для добавления листа</param>
        /// <param name="reportData">данные</param>
        private static void AddHierarchy(XLWorkbook wb, ReportData reportData)
        {
            using (var ws = wb.Worksheets.Add("Структура поставок"))
            {
                ws.Cell("A1").Value = "Итого сумма поставок";
                ws.Cell("A1").Style.Font.Bold = ws.Cell("C1").Style.Font.Bold = true;
                ws.Cell("A1").RichText.Substring(0, 5).SetFontColor(XLColor.Red);

                ws.Cell("A2").Value = "в том числе:";
                //ws.Column("C").Style.NumberFormat.Format = "### ### ##0";
                ws.Column("C").Style.NumberFormat.NumberFormatId = 3;

                ws.Outline.SummaryVLocation = XLOutlineSummaryVLocation.Top;

                int row = 3;
                List<int> countryGroupRows = new List<int>();
                foreach (var country in reportData.Data)
                {
                    int rowCountry = row;
                    row++;

                    List<int> regionGroupRows = new List<int>();
                    foreach (var region in country.Region)
                    {
                        int rowRegion = row;
                        row++;

                        List<int> companyGroupRows = new List<int>();
                        foreach (var company in region.Company)
                        {
                            int rowCompany = row;
                            row++;

                            var companyName = GetCompanyName(reportData, company.companyId);

                            List<int> productGroupRows = new List<int>();
                            var productGroups = company.Sale.GroupBy(sale => sale.productId);
                            foreach (var productGroup in productGroups)
                            {
                                int rowProduct = row;
                                row++;

                                var productName = GetProductName(reportData, productGroup.Key);

                                List<int> monthGroupRows = new List<int>();
                                var monthGroups = productGroup.GroupBy(sale => sale.date.Month);
                                foreach (var monthGroup in monthGroups)
                                {
                                    int rowMonth = row;
                                    row++;

                                    foreach (var sale in monthGroup.OrderBy(s => s.date))
                                    {
                                        ws.Cell(row, "A").Value = sale.contract;
                                        ws.Cell(row, "B").Value = sale.date;
                                        ws.Cell(row, "C").Value = sale.sum;

                                        row++;
                                    }
                                    ws.Cell(rowMonth, "A").Value = string.Format("Итого за {0}:", GetMonthName(monthGroup.Key));
                                    ws.Cell(rowMonth, "A").Style.Alignment.Indent = 4;
                                    ws.Cell(rowMonth, "C").FormulaA1 = string.Format("=SUM(C{0}:C{1})", rowMonth + 1, row - 1);
                                    monthGroupRows.Add(rowMonth);

                                    ws.Rows(rowMonth + 1, row - 1).Group();
                                    ws.Rows(rowMonth + 1, row - 1).Collapse();
                                }

                                ws.Cell(rowProduct, "A").Value = productName;
                                ws.Cell(rowProduct, "A").Style.Alignment.Indent = 3;
                                ws.Cell(rowProduct, "C").FormulaA1 = "=" + string.Join("+", monthGroupRows.Select(r => "C" + r.ToString()));
                                productGroupRows.Add(rowProduct);

                                ws.Rows(rowProduct + 1, row - 1).Group();
                                ws.Rows(rowProduct + 1, row - 1).Collapse();
                            }
                            ws.Cell(rowCompany, "A").Value = companyName;
                            ws.Cell(rowCompany, "A").Style.Alignment.Indent = 2;
                            ws.Cell(rowCompany, "C").FormulaA1 = "=" + string.Join("+", productGroupRows.Select(r => "C" + r.ToString()));
                            companyGroupRows.Add(rowCompany);

                            ws.Rows(rowCompany + 1, row - 1).Group();
                            ws.Rows(rowCompany + 1, row - 1).Collapse();
                        }
                        ws.Cell(rowRegion, "A").Value = NormalizeRegionName(region.name);
                        ws.Cell(rowRegion, "A").Style.Alignment.Indent = 1;
                        ws.Cell(rowRegion, "C").FormulaA1 = "=" + string.Join("+", companyGroupRows.Select(r => "C" + r.ToString()));
                        regionGroupRows.Add(rowRegion);

                        ws.Rows(rowRegion + 1, row - 1).Group();
                    }
                    ws.Cell(rowCountry, "A").Value = country.name;
                    ws.Cell(rowCountry, "C").FormulaA1 = "=" + string.Join("+", regionGroupRows.Select(r => "C" + r.ToString()));
                    countryGroupRows.Add(rowCountry);

                    ws.Rows(rowCountry + 1, row - 1).Group();
                }
                ws.Cell("C1").FormulaA1 = "=" + string.Join("+", countryGroupRows.Select(r => "C" + r.ToString()));

                ws.Column("C").AddConditionalFormat().DataBar(XLColor.Aqua).LowestValue().HighestValue();

                ws.Columns().AdjustToContents();
            }
        }

        /// <summary>
        /// Получить денормализованную таблицу данных
        /// </summary>
        /// <param name="reportData">данные отчета</param>
        /// <returns></returns>
        public static DataTable GetDataTable(ReportData reportData)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Страна", typeof(string));
            table.Columns.Add("Регион", typeof(string));
            table.Columns.Add("Компания", typeof(string));
            table.Columns.Add("ID продукции", typeof(int));
            table.Columns.Add("Продукция", typeof(string));
            table.Columns.Add("Месяц", typeof(string));
            table.Columns.Add("Сумма", typeof(int));

            table.BeginLoadData();
            foreach (var country in reportData.Data)
            {
                foreach (var region in country.Region)
                {
                    var regionName = NormalizeRegionName(region.name);
                    foreach (var company in region.Company)
                    {
                        var companyName = GetCompanyName(reportData, company.companyId);

                        var saleGroups = from sale in company.Sale
                                         let record = new
                                         {
                                             productId = sale.productId,
                                             sum = sale.sum,
                                             month = sale.date.Month
                                         }
                                         group record by record.productId into newGroup1
                                         from newGroup2 in
                                            (from record in newGroup1
                                             group record by record.month)
                                         group newGroup2 by newGroup1.Key into mg
                                         orderby mg.Key
                                         select mg;

                        foreach (var g in saleGroups)
                        {
                            var productName = GetProductName(reportData, g.Key);
                            foreach (var g2 in g)
                            {
                                table.Rows.Add(
                                    country.name,
                                    regionName,
                                    companyName,
                                    g.Key,
                                    productName,
                                    GetMonthName(g2.Key),
                                    g2.Sum(x => x.sum));
                            }
                        }
                    }
                }
            }
            table.EndLoadData();

            return table;
        }
    }
}
