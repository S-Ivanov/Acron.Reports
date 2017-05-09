/**
 * IExcelReportingService.java
 *
 * This file was auto-generated from WSDL
 * by the Apache Axis 1.4 Apr 22, 2006 (06:55:48 PDT) WSDL2Java emitter.
 */

package org.tempuri;

public interface IExcelReportingService extends java.rmi.Remote {
    public byte[] getExcelReport(java.lang.String data, java.lang.String configuration) throws java.rmi.RemoteException;
    public java.lang.String getExcelReportBase64(java.lang.String data, java.lang.String configuration) throws java.rmi.RemoteException;
    public java.lang.String echo(java.lang.String data) throws java.rmi.RemoteException;
}
