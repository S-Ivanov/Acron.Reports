using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ExcelReportingLib
{
    public static class ExcelReportingCommon
    {
        public static class ConfigurationKeys
        {
            public const string Assembly = "модуль";
            public const string TypeName = "класс";
            public const string Format = "формат";
        }

        public static class Formats
        {
            public const string XML = "XML";
            public const string CSV = "CSV";
        }

        public static List<List<string[]>> ParseCSV(string csv, char separator = ';')
        {
            return ParseCSV(csv, new char[] { separator });
        }

        public static List<List<string[]>> ParseCSV(string csv, params char[] separators)
        {
            List<List<string[]>> result = new List<List<string[]>>();

            StringReader sr = new StringReader(csv);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                line = line.Trim();
                if (string.IsNullOrEmpty(line))
                {
                    if (result.Count == 0 || result.Last().Count > 0)
                        result.Add(new List<string[]>());
                }
                else if (line.StartsWith("//"))
                    continue;
                else
                {
                    if (result.Count == 0)
                        result.Add(new List<string[]>());

                    string[] ss = line.Split(separators);
                    for (int i = 0; i < ss.Length; i++)
                        ss[i] = ss[i].Trim();

                    result.Last().Add(ss);
                }
            }
            if (result.Count > 0 && result.Last().Count == 0)
                result.RemoveAt(result.Count - 1);

            return result;
        }

        public static IDictionary<string, string> GetConfiguration(string configuration)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(configuration))
            {
                string[] pairs = configuration.Split(';');
                foreach (var pair in pairs)
                {
                    string[] pairParts = pair.Split('=');
                    if (pairParts.Length == 2)
                    {
                        pairParts[0] = pairParts[0].Trim();
                        pairParts[1] = pairParts[1].Trim();
                        if (!string.IsNullOrEmpty(pairParts[0]) && !string.IsNullOrEmpty(pairParts[1]))
                            result.Add(pairParts[0], pairParts[1]);
                    }
                }
            }
            return result;
        }
    }
}
