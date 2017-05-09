using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace SIvanov.ExcelGenerator
{
    public class ExcelGenerator : IExcelGenerator
    {
        #region IExcelGenerator

        public Stream Generate(string data, string configuration)
        {
            XDocument xmlConfig = XDocument.Parse(configuration);
            XDocument xmlData = XDocument.Parse(data);

            var result = new MemoryStream();
            using (var workbook = new XLWorkbook(xmlConfig.Root.Attribute("excelTemplate").Value))
            {
                foreach (var sheetNode in xmlConfig.Root.Element("Worksheets").Elements())
                {
                    using (var sheet = workbook.Worksheet(sheetNode.Attribute("name").Value))
                    {
                        // Сохранение данных в виде таблиц
                        SaveSmartTables(sheet, sheetNode, xmlData);
                        SaveTables(sheet, sheetNode, xmlData);
                    }
                }

                workbook.SaveAs(result);
                result.Position = 0;
            }
            return result;
        }

        #endregion IExcelGenerator

        /// <summary>
        /// Сохранение данных в виде "умной" таблицы
        /// </summary>
        /// <param name="sheet">лист книги</param>
        /// <param name="sheetNode">описатель страницы</param>
        /// <param name="xmlData">данные</param>
        /// <remarks>
        /// вставка таблиц https://github.com/ClosedXML/ClosedXML/wiki/Inserting-Tables
        /// </remarks>
        private void SaveSmartTables(IXLWorksheet sheet, XElement sheetNode, XDocument xmlData)
        {
            foreach (var tableNode in sheetNode.Elements("SmartTable"))
            {
                DataTable dataTable = LoadDataTable(
                    xmlData,
                    tableNode.Attribute("source").Value,
                    tableNode.Element("ColumnMapping").Elements("Column").Select(x => 
                        new ColumnMapping(x.Attribute("source").Value, x.Attribute("header").Value, x.Attribute("dataType").Value)));

                sheet.Cell(tableNode.Attribute("start").Value).InsertTable(dataTable, tableNode.Attribute("name").Value);
            }
        }

        /// <summary>
        /// Сохранение данных в виде таблицы
        /// </summary>
        /// <param name="sheet">лист книги</param>
        /// <param name="sheetNode">описатель страницы</param>
        /// <param name="xmlData">данные</param>
        /// <remarks>
        /// загрузка данных https://github.com/ClosedXML/ClosedXML/wiki/Copying-IEnumerable-Collections
        /// </remarks>
        private void SaveTables(IXLWorksheet sheet, XElement sheetNode, XDocument xmlData)
        {
            foreach (var tableNode in sheetNode.Elements("Table"))
            {
                DataTable dataTable = LoadDataTable(
                    xmlData,
                    tableNode.Attribute("source").Value,
                    tableNode.Element("ColumnMapping").Elements("Column").Select(x =>
                        new ColumnMapping(x.Attribute("source").Value, x.Attribute("header").Value, x.Attribute("dataType").Value)));

                var start = tableNode.Attribute("start").Value;
                bool showColumnHeaders = (bool)GetDataFromString(tableNode.Attribute("showColumnHeaders").Value, typeof(bool));
                SaveTable(sheet, dataTable, start, showColumnHeaders);
            }
        }

        public static void SaveTable(IXLWorksheet sheet, DataTable dataTable, string start, bool showColumnHeaders)
        {
            if (showColumnHeaders)
            {
                int colStart = XLHelper.GetColumnNumberFromAddress(start);

                var rowPos = 0;
                while (start[rowPos] > '9')
                    rowPos++;
                int rowStart = int.Parse(start.Substring(rowPos));

                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    sheet.Cell(rowStart, colStart + i).Value = dataTable.Columns[i].ColumnName;
                }

                sheet.Cell(rowStart + 1, colStart).InsertData(dataTable.AsEnumerable());
            }
            else
                sheet.Cell(start).InsertData(dataTable.AsEnumerable());
        }

        public static DataTable LoadDataTable(XDocument xmlData, string sourceTableName, IEnumerable<ColumnMapping> columnMappings)
        {
            DataTable table = new DataTable();
            foreach (var columnMapping in columnMappings)
            {
                table.Columns.Add(columnMapping.Header, Type.GetType(columnMapping.DataType));
            }

            table.BeginLoadData();
            object[] rowData = new object[table.Columns.Count];
            foreach (var record in xmlData.Root.Element(sourceTableName).Elements())
            {
                var columnIndex = 0;
                foreach (var columnMapping in columnMappings)
                {
                    if (columnMapping.IsAttribute)
                    {
                        var dataAttribute = record.Attribute(columnMapping.Source);
                        if (dataAttribute != null && !string.IsNullOrEmpty(dataAttribute.Value))
                            rowData[columnIndex] = GetDataFromString(dataAttribute.Value, table.Columns[columnIndex].DataType);
                    }
                    else
                    {
                        var dataElement = record.Element(columnMapping.Source);
                        if (dataElement != null && !string.IsNullOrEmpty(dataElement.Value))
                            rowData[columnIndex] = GetDataFromString(dataElement.Value, table.Columns[columnIndex].DataType);
                    }
                    columnIndex++;
                }
                if (rowData.Any(x => x != null))
                {
                    table.Rows.Add(rowData);
                    Array.Clear(rowData, 0, rowData.Length);
                }
            }
            table.EndLoadData();

            return table;
        }

        public static object GetDataFromString(string s, Type type)
        {
            if (type == typeof(bool))
                return XmlConvert.ToBoolean(s);
            else if (type == typeof(DateTime))
                return XmlConvert.ToDateTime(s, XmlDateTimeSerializationMode.Utc);
            else if (type == typeof(float) || type == typeof(double) || type == typeof(decimal))
                return Convert.ChangeType(s, type, NumberFormatInfo.InvariantInfo);
            else
                return Convert.ChangeType(s, type);
        }

        public class ColumnMapping
        {
            // TODO: задавать хранение в узле в явном виде, по умолчанию - в атрибутах
            public ColumnMapping(string source, string header, string dataType)
            {
                IsAttribute = source[0] == '@';
                Source = IsAttribute ? source.Substring(1) : source;
                Header = header;
                DataType = dataType;
            }

            public string Header { get; private set; }

            public string Source { get; private set; }

            public bool IsAttribute { get; private set; }

            public string DataType { get; private set; }
        }
    }
}
