using Grouping;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GenerateCSV
{
    class Program
    {
        static void Main(string[] args)
        {
            ReportGenerator reportGenerator = new ReportGenerator();
            reportGenerator.Prepare(SourceData, null);

            // страны
            Dictionary<string, int> countries = new Dictionary<string, int>();
            int countryId = 1;
            foreach (var country in reportGenerator.ReportData.Data)
            {
                if (!countries.ContainsKey(country.name))
                {
                    countries.Add(country.name, countryId);
                    countryId++;
                }
            }

            // регионы
            Dictionary<string, int> regions = new Dictionary<string, int>();
            regions.Add("", 0);
            int regionId = 1;
            foreach (var country in reportGenerator.ReportData.Data)
                foreach (var region in country.Region)
                {
                    if (!regions.ContainsKey(region.name))
                    {
                        regions.Add(region.name, regionId);
                        regionId++;
                    }
                }

            var xx = from country in reportGenerator.ReportData.Data
                     from region in country.Region
                     from company in region.Company
                     from sale in company.Sale
                     select new
                     {
                         countryId = countries[country.name],
                         regionId = regions[region.name],
                         companyId = company.companyId,
                         sale = sale
                     };

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("// * Поставки");
            sb.AppendLine("//countryId;regionId;companyId;productId;contract;date;sum");
            foreach (var x in xx)
            {
                sb.AppendLine(string.Format("{0};{1};{2};{3};{4};{5};{6}", 
                    x.countryId, x.regionId, x.companyId, x.sale.productId, x.sale.contract, x.sale.date.ToString("yyyy-MM-dd"), x.sale.sum));
            }
            sb.AppendLine();

            sb.AppendLine("// * Страны");
            sb.AppendLine("//countryId;name");
            foreach (var kvp in countries)
            {
                sb.AppendLine(string.Format("{0};{1}", kvp.Value, kvp.Key));
            }
            sb.AppendLine();

            sb.AppendLine("// * Регионы");
            sb.AppendLine("//regionId;name");
            foreach (var kvp in regions)
            {
                sb.AppendLine(string.Format("{0};{1}", kvp.Value, kvp.Key));
            }
            sb.AppendLine();

            sb.AppendLine("// * Компании");
            sb.AppendLine("//companyId;name");
            foreach (var company in reportGenerator.ReportData.References.Companies)
            {
                sb.AppendLine(string.Format("{0};{1}", company.id, company.name));
            }
            sb.AppendLine();

            sb.AppendLine("// * Продукция");
            sb.AppendLine("//productId;name");
            foreach (var product in reportGenerator.ReportData.References.Products)
            {
                sb.AppendLine(string.Format("{0};{1}", product.id, product.name));
            }
            sb.AppendLine();

            File.WriteAllText("test.txt", sb.ToString());
        }

        #region Тестовые данные

        public static string SourceData =
@"<ReportData>
  <Data>
    <Country name='Россия'>
      <Region name='Москва'>
        <Company companyId='1'>
          <Sale productId='1' contract='112/04-2016' date='2016-04-11' sum='2053673' />
          <Sale productId='1' contract='118/04-2016' date='2016-04-15' sum='3106752' />
          <Sale productId='1' contract='125/04-2016' date='2016-04-21' sum='2837659' />
          <Sale productId='1' contract='134/05-2016' date='2016-05-12' sum='1921061' />
          <Sale productId='1' contract='152/05-2016' date='2016-05-25' sum='2361842' />
          <Sale productId='1' contract='176/06-2016' date='2016-06-07' sum='2227749' />
          <Sale productId='1' contract='183/06-2016' date='2016-06-15' sum='2872523' />
          <Sale productId='1' contract='187/06-2016' date='2016-06-21' sum='1867594' />
          <Sale productId='1' contract='195/06-2016' date='2016-06-23' sum='2148918' />
          <Sale productId='2' contract='112/04-2016' date='2016-04-11' sum='1753967' />
          <Sale productId='2' contract='118/04-2016' date='2016-04-15' sum='1716989' />
          <Sale productId='2' contract='125/04-2016' date='2016-04-21' sum='780789' />
          <Sale productId='2' contract='134/05-2016' date='2016-05-12' sum='2543985' />
          <Sale productId='2' contract='152/05-2016' date='2016-05-25' sum='1621460' />
          <Sale productId='2' contract='176/06-2016' date='2016-06-07' sum='2593240' />
          <Sale productId='2' contract='183/06-2016' date='2016-06-15' sum='2188057' />
          <Sale productId='2' contract='187/06-2016' date='2016-06-21' sum='2788386' />
          <Sale productId='2' contract='195/06-2016' date='2016-06-23' sum='885713' />
          <Sale productId='3' contract='112/04-2016' date='2016-04-11' sum='2748916' />
          <Sale productId='3' contract='118/04-2016' date='2016-04-15' sum='2760774' />
          <Sale productId='3' contract='125/04-2016' date='2016-04-21' sum='2889846' />
          <Sale productId='3' contract='134/05-2016' date='2016-05-12' sum='1765589' />
          <Sale productId='3' contract='152/05-2016' date='2016-05-25' sum='1057069' />
          <Sale productId='3' contract='176/06-2016' date='2016-06-07' sum='1331740' />
          <Sale productId='3' contract='183/06-2016' date='2016-06-15' sum='1011695' />
          <Sale productId='3' contract='187/06-2016' date='2016-06-21' sum='2651733' />
          <Sale productId='3' contract='195/06-2016' date='2016-06-23' sum='1319748' />
          <Sale productId='4' contract='112/04-2016' date='2016-04-11' sum='2014093' />
          <Sale productId='4' contract='118/04-2016' date='2016-04-15' sum='1599826' />
          <Sale productId='4' contract='125/04-2016' date='2016-04-21' sum='925468' />
          <Sale productId='4' contract='134/05-2016' date='2016-05-12' sum='1583080' />
          <Sale productId='4' contract='152/05-2016' date='2016-05-25' sum='893773' />
          <Sale productId='4' contract='176/06-2016' date='2016-06-07' sum='808443' />
          <Sale productId='4' contract='183/06-2016' date='2016-06-15' sum='1181323' />
          <Sale productId='4' contract='187/06-2016' date='2016-06-21' sum='980606' />
          <Sale productId='4' contract='195/06-2016' date='2016-06-23' sum='2272204' />
          <Sale productId='5' contract='112/04-2016' date='2016-04-11' sum='2168941' />
          <Sale productId='5' contract='118/04-2016' date='2016-04-15' sum='2724430' />
          <Sale productId='5' contract='125/04-2016' date='2016-04-21' sum='2949499' />
          <Sale productId='5' contract='134/05-2016' date='2016-05-12' sum='2137182' />
          <Sale productId='5' contract='152/05-2016' date='2016-05-25' sum='922517' />
          <Sale productId='5' contract='176/06-2016' date='2016-06-07' sum='1313856' />
          <Sale productId='5' contract='183/06-2016' date='2016-06-15' sum='1031763' />
          <Sale productId='5' contract='187/06-2016' date='2016-06-21' sum='1142561' />
          <Sale productId='5' contract='195/06-2016' date='2016-06-23' sum='2615643' />
          <Sale productId='6' contract='112/04-2016' date='2016-04-11' sum='3109682' />
          <Sale productId='6' contract='118/04-2016' date='2016-04-15' sum='2428481' />
          <Sale productId='6' contract='125/04-2016' date='2016-04-21' sum='2540173' />
          <Sale productId='6' contract='134/05-2016' date='2016-05-12' sum='2469369' />
          <Sale productId='6' contract='152/05-2016' date='2016-05-25' sum='965551' />
          <Sale productId='6' contract='176/06-2016' date='2016-06-07' sum='2851949' />
          <Sale productId='6' contract='183/06-2016' date='2016-06-15' sum='1510103' />
          <Sale productId='6' contract='187/06-2016' date='2016-06-21' sum='2894015' />
          <Sale productId='6' contract='195/06-2016' date='2016-06-23' sum='2165222' />
          <Sale productId='7' contract='112/04-2016' date='2016-04-11' sum='3015080' />
          <Sale productId='7' contract='118/04-2016' date='2016-04-15' sum='2483319' />
          <Sale productId='7' contract='125/04-2016' date='2016-04-21' sum='852697' />
          <Sale productId='7' contract='134/05-2016' date='2016-05-12' sum='865022' />
          <Sale productId='7' contract='152/05-2016' date='2016-05-25' sum='2754240' />
          <Sale productId='7' contract='176/06-2016' date='2016-06-07' sum='3101621' />
          <Sale productId='7' contract='183/06-2016' date='2016-06-15' sum='1650431' />
          <Sale productId='7' contract='187/06-2016' date='2016-06-21' sum='1467621' />
          <Sale productId='7' contract='195/06-2016' date='2016-06-23' sum='2076443' />
        </Company>
        <Company companyId='2'>
          <Sale productId='1' contract='112/04-2016' date='2016-04-11' sum='2425976' />
          <Sale productId='1' contract='118/04-2016' date='2016-04-15' sum='1589084' />
          <Sale productId='1' contract='125/04-2016' date='2016-04-21' sum='1263244' />
          <Sale productId='1' contract='134/05-2016' date='2016-05-12' sum='2328984' />
          <Sale productId='1' contract='152/05-2016' date='2016-05-25' sum='1410368' />
          <Sale productId='1' contract='176/06-2016' date='2016-06-07' sum='2621298' />
          <Sale productId='1' contract='183/06-2016' date='2016-06-15' sum='2342112' />
          <Sale productId='1' contract='187/06-2016' date='2016-06-21' sum='2843533' />
          <Sale productId='1' contract='195/06-2016' date='2016-06-23' sum='1541706' />
          <Sale productId='2' contract='112/04-2016' date='2016-04-11' sum='1518879' />
          <Sale productId='2' contract='118/04-2016' date='2016-04-15' sum='2720055' />
          <Sale productId='2' contract='125/04-2016' date='2016-04-21' sum='3031162' />
          <Sale productId='2' contract='134/05-2016' date='2016-05-12' sum='2552566' />
          <Sale productId='2' contract='152/05-2016' date='2016-05-25' sum='1606970' />
          <Sale productId='2' contract='176/06-2016' date='2016-06-07' sum='1631353' />
          <Sale productId='2' contract='183/06-2016' date='2016-06-15' sum='2276955' />
          <Sale productId='2' contract='187/06-2016' date='2016-06-21' sum='860753' />
          <Sale productId='2' contract='195/06-2016' date='2016-06-23' sum='3021327' />
          <Sale productId='3' contract='112/04-2016' date='2016-04-11' sum='2129394' />
          <Sale productId='3' contract='118/04-2016' date='2016-04-15' sum='2725147' />
          <Sale productId='3' contract='125/04-2016' date='2016-04-21' sum='2637665' />
          <Sale productId='3' contract='134/05-2016' date='2016-05-12' sum='2750875' />
          <Sale productId='3' contract='152/05-2016' date='2016-05-25' sum='2059383' />
          <Sale productId='3' contract='176/06-2016' date='2016-06-07' sum='2612799' />
          <Sale productId='3' contract='183/06-2016' date='2016-06-15' sum='1480096' />
          <Sale productId='3' contract='187/06-2016' date='2016-06-21' sum='2077080' />
          <Sale productId='3' contract='195/06-2016' date='2016-06-23' sum='1989324' />
          <Sale productId='4' contract='112/04-2016' date='2016-04-11' sum='985086' />
          <Sale productId='4' contract='118/04-2016' date='2016-04-15' sum='2450080' />
          <Sale productId='4' contract='125/04-2016' date='2016-04-21' sum='1261057' />
          <Sale productId='4' contract='134/05-2016' date='2016-05-12' sum='2745609' />
          <Sale productId='4' contract='152/05-2016' date='2016-05-25' sum='1695241' />
          <Sale productId='4' contract='176/06-2016' date='2016-06-07' sum='2300101' />
          <Sale productId='4' contract='183/06-2016' date='2016-06-15' sum='1056973' />
          <Sale productId='4' contract='187/06-2016' date='2016-06-21' sum='1359637' />
          <Sale productId='4' contract='195/06-2016' date='2016-06-23' sum='3044997' />
          <Sale productId='5' contract='112/04-2016' date='2016-04-11' sum='1403358' />
          <Sale productId='5' contract='118/04-2016' date='2016-04-15' sum='1217301' />
          <Sale productId='5' contract='125/04-2016' date='2016-04-21' sum='2478663' />
          <Sale productId='5' contract='134/05-2016' date='2016-05-12' sum='3108797' />
          <Sale productId='5' contract='152/05-2016' date='2016-05-25' sum='1738930' />
          <Sale productId='5' contract='176/06-2016' date='2016-06-07' sum='2233697' />
          <Sale productId='5' contract='183/06-2016' date='2016-06-15' sum='3132007' />
          <Sale productId='5' contract='187/06-2016' date='2016-06-21' sum='1114447' />
          <Sale productId='5' contract='195/06-2016' date='2016-06-23' sum='1399700' />
          <Sale productId='6' contract='112/04-2016' date='2016-04-11' sum='2934021' />
          <Sale productId='6' contract='118/04-2016' date='2016-04-15' sum='1602711' />
          <Sale productId='6' contract='125/04-2016' date='2016-04-21' sum='1952086' />
          <Sale productId='6' contract='134/05-2016' date='2016-05-12' sum='864550' />
          <Sale productId='6' contract='152/05-2016' date='2016-05-25' sum='1516078' />
          <Sale productId='6' contract='176/06-2016' date='2016-06-07' sum='3007415' />
          <Sale productId='6' contract='183/06-2016' date='2016-06-15' sum='2922475' />
          <Sale productId='6' contract='187/06-2016' date='2016-06-21' sum='1231082' />
          <Sale productId='6' contract='195/06-2016' date='2016-06-23' sum='783495' />
          <Sale productId='7' contract='112/04-2016' date='2016-04-11' sum='2370029' />
          <Sale productId='7' contract='118/04-2016' date='2016-04-15' sum='3097199' />
          <Sale productId='7' contract='125/04-2016' date='2016-04-21' sum='1981479' />
          <Sale productId='7' contract='134/05-2016' date='2016-05-12' sum='1199311' />
          <Sale productId='7' contract='152/05-2016' date='2016-05-25' sum='1074201' />
          <Sale productId='7' contract='176/06-2016' date='2016-06-07' sum='2917708' />
          <Sale productId='7' contract='183/06-2016' date='2016-06-15' sum='1753637' />
          <Sale productId='7' contract='187/06-2016' date='2016-06-21' sum='1274054' />
          <Sale productId='7' contract='195/06-2016' date='2016-06-23' sum='858988' />
        </Company>
      </Region>
      <Region name='Московская область'>
        <Company companyId='6'>
          <Sale productId='1' contract='112/04-2016' date='2016-04-11' sum='979175' />
          <Sale productId='1' contract='118/04-2016' date='2016-04-15' sum='2727445' />
          <Sale productId='1' contract='125/04-2016' date='2016-04-21' sum='2111339' />
          <Sale productId='1' contract='134/05-2016' date='2016-05-12' sum='2496766' />
          <Sale productId='1' contract='152/05-2016' date='2016-05-25' sum='2087924' />
          <Sale productId='1' contract='176/06-2016' date='2016-06-07' sum='2051064' />
          <Sale productId='1' contract='183/06-2016' date='2016-06-15' sum='1424413' />
          <Sale productId='1' contract='187/06-2016' date='2016-06-21' sum='2165075' />
          <Sale productId='1' contract='195/06-2016' date='2016-06-23' sum='2115735' />
          <Sale productId='2' contract='112/04-2016' date='2016-04-11' sum='2656866' />
          <Sale productId='2' contract='118/04-2016' date='2016-04-15' sum='1911494' />
          <Sale productId='2' contract='125/04-2016' date='2016-04-21' sum='2648952' />
          <Sale productId='2' contract='134/05-2016' date='2016-05-12' sum='986876' />
          <Sale productId='2' contract='152/05-2016' date='2016-05-25' sum='1920890' />
          <Sale productId='2' contract='176/06-2016' date='2016-06-07' sum='1583057' />
          <Sale productId='2' contract='183/06-2016' date='2016-06-15' sum='1159588' />
          <Sale productId='2' contract='187/06-2016' date='2016-06-21' sum='882197' />
          <Sale productId='2' contract='195/06-2016' date='2016-06-23' sum='1984855' />
          <Sale productId='3' contract='112/04-2016' date='2016-04-11' sum='966097' />
          <Sale productId='3' contract='118/04-2016' date='2016-04-15' sum='2046417' />
          <Sale productId='3' contract='125/04-2016' date='2016-04-21' sum='1292446' />
          <Sale productId='3' contract='134/05-2016' date='2016-05-12' sum='2019312' />
          <Sale productId='3' contract='152/05-2016' date='2016-05-25' sum='2425096' />
          <Sale productId='3' contract='176/06-2016' date='2016-06-07' sum='2050001' />
          <Sale productId='3' contract='183/06-2016' date='2016-06-15' sum='1376521' />
          <Sale productId='3' contract='187/06-2016' date='2016-06-21' sum='2157726' />
          <Sale productId='3' contract='195/06-2016' date='2016-06-23' sum='1571659' />
          <Sale productId='4' contract='112/04-2016' date='2016-04-11' sum='2872237' />
          <Sale productId='4' contract='118/04-2016' date='2016-04-15' sum='2323713' />
          <Sale productId='4' contract='125/04-2016' date='2016-04-21' sum='1811260' />
          <Sale productId='4' contract='134/05-2016' date='2016-05-12' sum='2918092' />
          <Sale productId='4' contract='152/05-2016' date='2016-05-25' sum='1195998' />
          <Sale productId='4' contract='176/06-2016' date='2016-06-07' sum='2389960' />
          <Sale productId='4' contract='183/06-2016' date='2016-06-15' sum='957490' />
          <Sale productId='4' contract='187/06-2016' date='2016-06-21' sum='2820919' />
          <Sale productId='4' contract='195/06-2016' date='2016-06-23' sum='2227425' />
          <Sale productId='5' contract='112/04-2016' date='2016-04-11' sum='931493' />
          <Sale productId='5' contract='118/04-2016' date='2016-04-15' sum='2175640' />
          <Sale productId='5' contract='125/04-2016' date='2016-04-21' sum='1304343' />
          <Sale productId='5' contract='134/05-2016' date='2016-05-12' sum='2590481' />
          <Sale productId='5' contract='152/05-2016' date='2016-05-25' sum='1831473' />
          <Sale productId='5' contract='176/06-2016' date='2016-06-07' sum='2037010' />
          <Sale productId='5' contract='183/06-2016' date='2016-06-15' sum='2251273' />
          <Sale productId='5' contract='187/06-2016' date='2016-06-21' sum='1816786' />
          <Sale productId='5' contract='195/06-2016' date='2016-06-23' sum='1378717' />
          <Sale productId='6' contract='112/04-2016' date='2016-04-11' sum='2371561' />
          <Sale productId='6' contract='118/04-2016' date='2016-04-15' sum='1805465' />
          <Sale productId='6' contract='125/04-2016' date='2016-04-21' sum='1566768' />
          <Sale productId='6' contract='134/05-2016' date='2016-05-12' sum='1261365' />
          <Sale productId='6' contract='152/05-2016' date='2016-05-25' sum='877627' />
          <Sale productId='6' contract='176/06-2016' date='2016-06-07' sum='2194004' />
          <Sale productId='6' contract='183/06-2016' date='2016-06-15' sum='1240598' />
          <Sale productId='6' contract='187/06-2016' date='2016-06-21' sum='1743204' />
          <Sale productId='6' contract='195/06-2016' date='2016-06-23' sum='1944673' />
          <Sale productId='7' contract='112/04-2016' date='2016-04-11' sum='955504' />
          <Sale productId='7' contract='118/04-2016' date='2016-04-15' sum='1588943' />
          <Sale productId='7' contract='125/04-2016' date='2016-04-21' sum='1972304' />
          <Sale productId='7' contract='134/05-2016' date='2016-05-12' sum='2779373' />
          <Sale productId='7' contract='152/05-2016' date='2016-05-25' sum='3027272' />
          <Sale productId='7' contract='176/06-2016' date='2016-06-07' sum='1960512' />
          <Sale productId='7' contract='183/06-2016' date='2016-06-15' sum='2836000' />
          <Sale productId='7' contract='187/06-2016' date='2016-06-21' sum='2087609' />
          <Sale productId='7' contract='195/06-2016' date='2016-06-23' sum='840079' />
        </Company>
      </Region>
    </Country>
    <Country name='Беларусь'>
      <Region name=''>
        <Company companyId='45'>
          <Sale productId='1' contract='112/04-2016' date='2016-04-11' sum='979375' />
          <Sale productId='1' contract='118/04-2016' date='2016-04-15' sum='272745' />
          <Sale productId='1' contract='125/04-2016' date='2016-04-21' sum='211339' />
          <Sale productId='1' contract='134/05-2016' date='2016-05-12' sum='249766' />
          <Sale productId='1' contract='152/05-2016' date='2016-05-25' sum='208724' />
          <Sale productId='1' contract='176/06-2016' date='2016-06-07' sum='205064' />
          <Sale productId='1' contract='183/06-2016' date='2016-06-15' sum='142413' />
          <Sale productId='1' contract='187/06-2016' date='2016-06-21' sum='216075' />
          <Sale productId='1' contract='195/06-2016' date='2016-06-23' sum='211535' />
          <Sale productId='2' contract='112/04-2016' date='2016-04-11' sum='265666' />
          <Sale productId='2' contract='118/04-2016' date='2016-04-15' sum='191494' />
          <Sale productId='2' contract='125/04-2016' date='2016-04-21' sum='264952' />
          <Sale productId='2' contract='134/05-2016' date='2016-05-12' sum='986876' />
          <Sale productId='2' contract='152/05-2016' date='2016-05-25' sum='192890' />
          <Sale productId='2' contract='176/06-2016' date='2016-06-07' sum='158057' />
          <Sale productId='2' contract='183/06-2016' date='2016-06-15' sum='115588' />
          <Sale productId='2' contract='187/06-2016' date='2016-06-21' sum='882197' />
          <Sale productId='2' contract='195/06-2016' date='2016-06-23' sum='198855' />
          <Sale productId='3' contract='112/04-2016' date='2016-04-11' sum='96697' />
          <Sale productId='3' contract='118/04-2016' date='2016-04-15' sum='204417' />
          <Sale productId='3' contract='125/04-2016' date='2016-04-21' sum='129446' />
          <Sale productId='3' contract='134/05-2016' date='2016-05-12' sum='201312' />
          <Sale productId='3' contract='152/05-2016' date='2016-05-25' sum='242096' />
          <Sale productId='3' contract='176/06-2016' date='2016-06-07' sum='205001' />
          <Sale productId='3' contract='183/06-2016' date='2016-06-15' sum='137521' />
          <Sale productId='3' contract='187/06-2016' date='2016-06-21' sum='215726' />
          <Sale productId='3' contract='195/06-2016' date='2016-06-23' sum='157659' />
          <Sale productId='4' contract='112/04-2016' date='2016-04-11' sum='287237' />
          <Sale productId='4' contract='118/04-2016' date='2016-04-15' sum='232713' />
          <Sale productId='4' contract='125/04-2016' date='2016-04-21' sum='181260' />
          <Sale productId='4' contract='134/05-2016' date='2016-05-12' sum='291092' />
          <Sale productId='4' contract='152/05-2016' date='2016-05-25' sum='119998' />
          <Sale productId='4' contract='176/06-2016' date='2016-06-07' sum='238960' />
          <Sale productId='4' contract='183/06-2016' date='2016-06-15' sum='95790' />
          <Sale productId='4' contract='187/06-2016' date='2016-06-21' sum='282919' />
          <Sale productId='4' contract='195/06-2016' date='2016-06-23' sum='222425' />
          <Sale productId='5' contract='112/04-2016' date='2016-04-11' sum='93193' />
          <Sale productId='5' contract='118/04-2016' date='2016-04-15' sum='217640' />
          <Sale productId='5' contract='125/04-2016' date='2016-04-21' sum='130343' />
          <Sale productId='5' contract='134/05-2016' date='2016-05-12' sum='259481' />
          <Sale productId='5' contract='152/05-2016' date='2016-05-25' sum='183473' />
          <Sale productId='5' contract='176/06-2016' date='2016-06-07' sum='203010' />
          <Sale productId='5' contract='183/06-2016' date='2016-06-15' sum='225273' />
          <Sale productId='5' contract='187/06-2016' date='2016-06-21' sum='181786' />
          <Sale productId='5' contract='195/06-2016' date='2016-06-23' sum='137717' />
          <Sale productId='6' contract='112/04-2016' date='2016-04-11' sum='237561' />
          <Sale productId='6' contract='118/04-2016' date='2016-04-15' sum='180465' />
          <Sale productId='6' contract='125/04-2016' date='2016-04-21' sum='156768' />
          <Sale productId='6' contract='134/05-2016' date='2016-05-12' sum='126365' />
          <Sale productId='6' contract='152/05-2016' date='2016-05-25' sum='87727' />
          <Sale productId='6' contract='176/06-2016' date='2016-06-07' sum='219004' />
          <Sale productId='6' contract='183/06-2016' date='2016-06-15' sum='124598' />
          <Sale productId='6' contract='187/06-2016' date='2016-06-21' sum='174204' />
          <Sale productId='6' contract='195/06-2016' date='2016-06-23' sum='194673' />
          <Sale productId='7' contract='112/04-2016' date='2016-04-11' sum='95504' />
          <Sale productId='7' contract='118/04-2016' date='2016-04-15' sum='158943' />
          <Sale productId='7' contract='125/04-2016' date='2016-04-21' sum='197304' />
          <Sale productId='7' contract='134/05-2016' date='2016-05-12' sum='277373' />
          <Sale productId='7' contract='152/05-2016' date='2016-05-25' sum='302272' />
          <Sale productId='7' contract='176/06-2016' date='2016-06-07' sum='196512' />
          <Sale productId='7' contract='183/06-2016' date='2016-06-15' sum='283000' />
          <Sale productId='7' contract='187/06-2016' date='2016-06-21' sum='208609' />
          <Sale productId='7' contract='195/06-2016' date='2016-06-23' sum='84079' />
        </Company>
      </Region>
    </Country>
  </Data>
  <References>
    <Companies>
      <Company id='1' name='ООО РДЭ' />
      <Company id='2' name='ЗАО СК ГРИФОН' />
      <Company id='3' name='ООО Энергодрайв' />
      <Company id='4' name='ЗАО НФ АК ПРАКТИК' />
      <Company id='5' name='Группа компаний Электромотор' />
      <Company id='6' name='ООО Электростиль' />
      <Company id='7' name='АО Электроагрегат' />
      <Company id='8' name='ООО ВЭТК ГК' />
      <Company id='9' name='ООО Росэлектро ТД' />
      <Company id='10' name='ООО АМКТ' />
      <Company id='11' name='ООО КВ-Индастри' />
      <Company id='12' name='ООО СЗЭМО Электродвигатель' />
      <Company id='13' name='ЗАО Энергопром' />
      <Company id='14' name='ООО ЛОНГРИ' />
      <Company id='15' name='ООО Электростиль Киров' />
      <Company id='16' name='ООО Электростиль Набережные Челны' />
      <Company id='17' name='ООО Электростиль Нижний Новгород' />
      <Company id='18' name='ООО Русэлпром-Урал' />
      <Company id='19' name='ООО Уралстройинвест' />
      <Company id='20' name='ЗАО КОНСАР' />
      <Company id='21' name='ОАО АЭРОМАШ' />
      <Company id='22' name='ООО Электростиль Уфа' />
      <Company id='23' name='ООО Электростиль Краснодар' />
      <Company id='24' name='ООО Насосэнергомаш' />
      <Company id='25' name='ООО Электростиль Ростов-на-Дону' />
      <Company id='26' name='ООО Энергоиндустрия ПО' />
      <Company id='27' name='Иркутскпромоборудование' />
      <Company id='28' name='ООО СИБТЭК' />
      <Company id='29' name='ООО Русэлпром-Кузбасс' />
      <Company id='30' name='ООО СГК ТД' />
      <Company id='31' name='ООО Электростиль Новосибирск' />
      <Company id='32' name='ООО Торговая компания ТЭС' />
      <Company id='33' name='ООО Энергоснабкомплект' />
      <Company id='34' name='ООО Электростиль Екатеринбург' />
      <Company id='35' name='ООО Сибирский тракт' />
      <Company id='36' name='ООО Дальэнергооборудование' />
      <Company id='37' name='ООО Электростиль Хабаровск' />
      <Company id='38' name='ТОО Energotechnic' />
      <Company id='39' name='ТОО НПФ АВИА' />
      <Company id='40' name='ООО Энергия' />
      <Company id='41' name='ООО РОСУКРЭЛПРОМ' />
      <Company id='42' name='ООО ЛБЮ-Тех' />
      <Company id='43' name='ООО ТД ЭНЕРГОПРОМ КР' />
      <Company id='44' name='ООО Русэлпром-Украина' />
      <Company id='45' name='ООО Техноэкссервис' />
      <Company id='46' name='SIA EnergoStar' />
      <Company id='47' name='Сиана Електрик ЕООД' />
    </Companies>
    <Products>
      <Product id='1' name='Низковольтные электродвигатели' />
      <Product id='2' name='Высоковольтные электродвигатели' />
      <Product id='3' name='Генераторы' />
      <Product id='4' name='Системы управления электромашинами' />
      <Product id='5' name='Трансформаторы и реакторы' />
      <Product id='6' name='Электропривод транспорта' />
      <Product id='7' name='Прочее' />
    </Products>
  </References>
</ReportData>";

        #endregion Тестовые данные
    }
}
