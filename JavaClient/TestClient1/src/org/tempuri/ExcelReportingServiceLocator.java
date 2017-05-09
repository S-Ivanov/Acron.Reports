/**
 * ExcelReportingServiceLocator.java
 *
 * This file was auto-generated from WSDL
 * by the Apache Axis 1.4 Apr 22, 2006 (06:55:48 PDT) WSDL2Java emitter.
 */

package org.tempuri;

public class ExcelReportingServiceLocator extends org.apache.axis.client.Service implements org.tempuri.ExcelReportingService {

    public ExcelReportingServiceLocator() {
    }


    public ExcelReportingServiceLocator(org.apache.axis.EngineConfiguration config) {
        super(config);
    }

    public ExcelReportingServiceLocator(java.lang.String wsdlLoc, javax.xml.namespace.QName sName) throws javax.xml.rpc.ServiceException {
        super(wsdlLoc, sName);
    }

    // Use to get a proxy class for BasicHttpBinding_IExcelReportingService
    private java.lang.String BasicHttpBinding_IExcelReportingService_address = "http://localhost:8733/Design_Time_Addresses/ExcelReportingServiceLib/ExcelReportingService/";

    public java.lang.String getBasicHttpBinding_IExcelReportingServiceAddress() {
        return BasicHttpBinding_IExcelReportingService_address;
    }

    // The WSDD service name defaults to the port name.
    private java.lang.String BasicHttpBinding_IExcelReportingServiceWSDDServiceName = "BasicHttpBinding_IExcelReportingService";

    public java.lang.String getBasicHttpBinding_IExcelReportingServiceWSDDServiceName() {
        return BasicHttpBinding_IExcelReportingServiceWSDDServiceName;
    }

    public void setBasicHttpBinding_IExcelReportingServiceWSDDServiceName(java.lang.String name) {
        BasicHttpBinding_IExcelReportingServiceWSDDServiceName = name;
    }

    public org.tempuri.IExcelReportingService getBasicHttpBinding_IExcelReportingService() throws javax.xml.rpc.ServiceException {
       java.net.URL endpoint;
        try {
            endpoint = new java.net.URL(BasicHttpBinding_IExcelReportingService_address);
        }
        catch (java.net.MalformedURLException e) {
            throw new javax.xml.rpc.ServiceException(e);
        }
        return getBasicHttpBinding_IExcelReportingService(endpoint);
    }

    public org.tempuri.IExcelReportingService getBasicHttpBinding_IExcelReportingService(java.net.URL portAddress) throws javax.xml.rpc.ServiceException {
        try {
            org.tempuri.BasicHttpBinding_IExcelReportingServiceStub _stub = new org.tempuri.BasicHttpBinding_IExcelReportingServiceStub(portAddress, this);
            _stub.setPortName(getBasicHttpBinding_IExcelReportingServiceWSDDServiceName());
            return _stub;
        }
        catch (org.apache.axis.AxisFault e) {
            return null;
        }
    }

    public void setBasicHttpBinding_IExcelReportingServiceEndpointAddress(java.lang.String address) {
        BasicHttpBinding_IExcelReportingService_address = address;
    }

    /**
     * For the given interface, get the stub implementation.
     * If this service has no port for the given interface,
     * then ServiceException is thrown.
     */
    public java.rmi.Remote getPort(Class serviceEndpointInterface) throws javax.xml.rpc.ServiceException {
        try {
            if (org.tempuri.IExcelReportingService.class.isAssignableFrom(serviceEndpointInterface)) {
                org.tempuri.BasicHttpBinding_IExcelReportingServiceStub _stub = new org.tempuri.BasicHttpBinding_IExcelReportingServiceStub(new java.net.URL(BasicHttpBinding_IExcelReportingService_address), this);
                _stub.setPortName(getBasicHttpBinding_IExcelReportingServiceWSDDServiceName());
                return _stub;
            }
        }
        catch (java.lang.Throwable t) {
            throw new javax.xml.rpc.ServiceException(t);
        }
        throw new javax.xml.rpc.ServiceException("There is no stub implementation for the interface:  " + (serviceEndpointInterface == null ? "null" : serviceEndpointInterface.getName()));
    }

    /**
     * For the given interface, get the stub implementation.
     * If this service has no port for the given interface,
     * then ServiceException is thrown.
     */
    public java.rmi.Remote getPort(javax.xml.namespace.QName portName, Class serviceEndpointInterface) throws javax.xml.rpc.ServiceException {
        if (portName == null) {
            return getPort(serviceEndpointInterface);
        }
        java.lang.String inputPortName = portName.getLocalPart();
        if ("BasicHttpBinding_IExcelReportingService".equals(inputPortName)) {
            return getBasicHttpBinding_IExcelReportingService();
        }
        else  {
            java.rmi.Remote _stub = getPort(serviceEndpointInterface);
            ((org.apache.axis.client.Stub) _stub).setPortName(portName);
            return _stub;
        }
    }

    public javax.xml.namespace.QName getServiceName() {
        return new javax.xml.namespace.QName("http://tempuri.org/", "ExcelReportingService");
    }

    private java.util.HashSet ports = null;

    public java.util.Iterator getPorts() {
        if (ports == null) {
            ports = new java.util.HashSet();
            ports.add(new javax.xml.namespace.QName("http://tempuri.org/", "BasicHttpBinding_IExcelReportingService"));
        }
        return ports.iterator();
    }

    /**
    * Set the endpoint address for the specified port name.
    */
    public void setEndpointAddress(java.lang.String portName, java.lang.String address) throws javax.xml.rpc.ServiceException {
        
if ("BasicHttpBinding_IExcelReportingService".equals(portName)) {
            setBasicHttpBinding_IExcelReportingServiceEndpointAddress(address);
        }
        else 
{ // Unknown Port Name
            throw new javax.xml.rpc.ServiceException(" Cannot set Endpoint Address for Unknown Port" + portName);
        }
    }

    /**
    * Set the endpoint address for the specified port name.
    */
    public void setEndpointAddress(javax.xml.namespace.QName portName, java.lang.String address) throws javax.xml.rpc.ServiceException {
        setEndpointAddress(portName.getLocalPart(), address);
    }

}
