using ExcelReportingServiceLib;
using System;
using System.ServiceModel;

namespace ExcelReportingServiceHost
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost hostobj = new ServiceHost(typeof(ExcelReportingService));
            hostobj.Open();
            Console.WriteLine("Service Started");
            Console.Read();
        }
    }
}
