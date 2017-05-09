using Microsoft.VisualStudio.TestTools.UnitTesting;
using SIvanov.ExcelGenerator;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace SIvanov.ExcelGenerator.Tests
{
    [TestClass()]
    public class ExcelGeneratorTests
    {
        [TestMethod()]
        public void ExcelGenerator_Generate()
        {
            string configuration =
@"<SIvanov.ExcelGenerator.Config excelTemplate='C:\Проекты\Жестков\Acron.Reports\SIvanov.ExcelGenerator\Test.xlsx'>
  <Worksheets>
    <Worksheet name='Лист1'>
      <!--
      Описание таблицы Excel
      Атрибуты:
      - name - имя таблицы на листе
      - source - исходная таблица данных
      - start - координаты ячейки начала таблицы (заголовок первой колонки)
      -->
      <Table name='Таблица1' source='Table1' start='A1' showColumnHeaders='true' >
        <ColumnMapping>
          <!--
          Проекция столбца данных
          Атрибуты:
          - header - заголовок колонки в таблице
          - source - имя столбца в исходной таблице данных
          - dataType - тип данных для размещени в ячейки Excel - см. Type.GetType(String)
          -->
          <Column header='ID' source='@id' dataType='System.Int32' />
          <Column header='Наименование' source='@name' dataType='System.String' />
          <Column header='Кол-во' source='@count' dataType='System.Int32' />
        </ColumnMapping>
      </Table>
    </Worksheet>
  </Worksheets>
</SIvanov.ExcelGenerator.Config>";

            string data =
@"<Data>
  <Table1>
    <Record id='1' name='Наименование 1' count='100' date='2017-04-30T22:10:38'/>
    <Record id='2' name='Наименование 2' count='200' date='2017-05-01'/>
  </Table1 >
</Data>";

            var excelStream = (new ExcelGenerator()).Generate(configuration, data);
            if (excelStream != null)
            {
                string fileName = "test.xlsx";
                using (var fs = new FileStream(fileName, FileMode.Create))
                {
                    excelStream.CopyTo(fs);
                }
            }
        }

        [TestMethod()]
        public void ExcelGenerator_GetDataFromString()
        {
            object result;

            result = ExcelGenerator.GetDataFromString("test", typeof(string));
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(string), result.GetType());
            Assert.AreEqual("test", result);

            result = ExcelGenerator.GetDataFromString("1", typeof(int));
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(int), result.GetType());
            Assert.AreEqual(1, result);

            result = ExcelGenerator.GetDataFromString("1.5", typeof(double));
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(double), result.GetType());
            Assert.AreEqual(1.5, result);

            result = ExcelGenerator.GetDataFromString("1.5", typeof(decimal));
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(decimal), result.GetType());
            Assert.AreEqual(1.5m, result);

            result = ExcelGenerator.GetDataFromString("2017-04-30", typeof(DateTime));
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(DateTime), result.GetType());
            Assert.AreEqual(new DateTime(2017, 04, 30), result);

            result = ExcelGenerator.GetDataFromString("2017-04-30T20:53:15", typeof(DateTime));
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(DateTime), result.GetType());
            Assert.AreEqual(new DateTime(2017, 04, 30, 20, 53, 15), result);
        }

        [TestMethod()]
        public void ExcelGenerator_LoadDataTable()
        {
            string data =
@"<Data>
  <Table1>
    <Record id='1' name='Наименование 1' count='100' date='2017-04-30T22:10:38'/>
    <Record id='2' name='Наименование 2' count='200' date='2017-05-01'/>
  </Table1 >
</Data>";
            XDocument xmlData = XDocument.Parse(data);
            ExcelGenerator.ColumnMapping[] columnMappings =
            {
                new ExcelGenerator.ColumnMapping("@id", "ID", "System.Int32"),
                new ExcelGenerator.ColumnMapping("@name", "Наименование", "System.String"),
                new ExcelGenerator.ColumnMapping("@count", "Кол-во", "System.Int32"),
                new ExcelGenerator.ColumnMapping("@date", "Дата/время", "System.DateTime"),
            };

            DataTable table = ExcelGenerator.LoadDataTable(xmlData, "Table1", columnMappings);
            Assert.IsNotNull(table);
        }
    }
}