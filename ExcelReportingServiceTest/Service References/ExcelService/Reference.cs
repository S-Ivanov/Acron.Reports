﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ExcelReportingServiceTest.ExcelService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ExcelService.IExcelReportingService")]
    public interface IExcelReportingService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IExcelReportingService/GetExcelReport", ReplyAction="http://tempuri.org/IExcelReportingService/GetExcelReportResponse")]
        System.IO.Stream GetExcelReport(string data, string configuration);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IExcelReportingServiceChannel : ExcelReportingServiceTest.ExcelService.IExcelReportingService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ExcelReportingServiceClient : System.ServiceModel.ClientBase<ExcelReportingServiceTest.ExcelService.IExcelReportingService>, ExcelReportingServiceTest.ExcelService.IExcelReportingService {
        
        public ExcelReportingServiceClient() {
        }
        
        public ExcelReportingServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ExcelReportingServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ExcelReportingServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ExcelReportingServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.IO.Stream GetExcelReport(string data, string configuration) {
            return base.Channel.GetExcelReport(data, configuration);
        }
    }
}