using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExcelReportingLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExcelReportingLib.Tests
{
    [TestClass()]
    public class ExcelReportingCommonTests
    {
        [TestMethod()]
        public void ExcelReportingCommon_ParseCSV()
        {
            string csvData =
@"

// * Страны
//countryId;name
1;Россия
2;Беларусь

// * Регионы
//regionId;name
0;
1;Москва
2;Московская область

// * Компании
";
            var csv = ExcelReportingCommon.ParseCSV(csvData);
            Assert.IsNotNull(csv);
        }
    }
}