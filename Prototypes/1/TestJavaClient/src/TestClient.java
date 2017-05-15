import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.rmi.RemoteException;
import java.util.Base64;

import org.tempuri.IExcelReportingServiceProxy;

public class TestClient {

	public static void main(String[] args) {
		
		try {
			System.out.println("�������� Excel-������� � �������������� WEB-������� .Net");
			System.out.println();
			
			System.out.print("������� id ������: ");
			BufferedReader reader = new BufferedReader(new InputStreamReader(System.in));
			String id = reader.readLine();
			
			// ������������� Excel-����� � �������������� �������
        	String excelReportStr = getExcelReportFromService(id);
        	
        	// ��������� ��� ����� ��� ���������� ����������
    		System.out.print("��� �����-����������: ");
        	String fileName = reader.readLine();
        	
        	// ��������� ��������������� Excel-�����
            Files.write(Paths.get(fileName), Base64.getDecoder().decode(excelReportStr));
            
			System.out.println("���� �������.");

		} catch (Exception e) {  
            e.printStackTrace();  
        }  
	}

	/**
	 * ��������� Excel-����� � �������������� �������
	 * @param id - ������������� ������
	 * @return - �������� ������������� ���������������� ������, �������������� � base64
	 * @throws RemoteException
	 */
	private static String getExcelReportFromService(String id) throws RemoteException
	{
		// ������� ������ �� ������, ���������������� �� ����� ��������� ������
       	//return (new IExcelReportingServiceProxy()).getReport(id, null, null);
		
		// ������� ������ �� ���������� ������
       	return (new IExcelReportingServiceProxy("http://localhost:2585/ExcelReportingService/")).getReport(id, null, null);
	}
}
