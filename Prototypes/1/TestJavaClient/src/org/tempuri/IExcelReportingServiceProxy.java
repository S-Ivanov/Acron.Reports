package org.tempuri;

public class IExcelReportingServiceProxy implements org.tempuri.IExcelReportingService {
  private String _endpoint = null;
  private org.tempuri.IExcelReportingService iExcelReportingService = null;
  
  public IExcelReportingServiceProxy() {
    _initIExcelReportingServiceProxy();
  }
  
  public IExcelReportingServiceProxy(String endpoint) {
    _endpoint = endpoint;
    _initIExcelReportingServiceProxy();
  }
  
  private void _initIExcelReportingServiceProxy() {
    try {
      iExcelReportingService = (new org.tempuri.ExcelReportingServiceLocator()).getBasicHttpBinding_IExcelReportingService();
      if (iExcelReportingService != null) {
        if (_endpoint != null)
          ((javax.xml.rpc.Stub)iExcelReportingService)._setProperty("javax.xml.rpc.service.endpoint.address", _endpoint);
        else
          _endpoint = (String)((javax.xml.rpc.Stub)iExcelReportingService)._getProperty("javax.xml.rpc.service.endpoint.address");
      }
      
    }
    catch (javax.xml.rpc.ServiceException serviceException) {}
  }
  
  public String getEndpoint() {
    return _endpoint;
  }
  
  public void setEndpoint(String endpoint) {
    _endpoint = endpoint;
    if (iExcelReportingService != null)
      ((javax.xml.rpc.Stub)iExcelReportingService)._setProperty("javax.xml.rpc.service.endpoint.address", _endpoint);
    
  }
  
  public org.tempuri.IExcelReportingService getIExcelReportingService() {
    if (iExcelReportingService == null)
      _initIExcelReportingServiceProxy();
    return iExcelReportingService;
  }
  
  public java.lang.String getReport(java.lang.String id, java.lang.String configuration, java.lang.String data) throws java.rmi.RemoteException{
    if (iExcelReportingService == null)
      _initIExcelReportingServiceProxy();
    return iExcelReportingService.getReport(id, configuration, data);
  }
  
  
}