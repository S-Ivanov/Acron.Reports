import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.rmi.RemoteException;
import java.util.Base64;

import org.tempuri.IExcelReportingServiceProxy;

public class TestClient {

	public static void main(String[] args) throws IOException 
	{
		System.out.println("��������� Excel-������� � �������������� WEB-������� .Net");
		System.out.println();
		
		System.out.println("��������� ������� �������� ������:");
		System.out.println("1 - XML-������");
		System.out.println("2 - CSV (Comma-Separated Values)");
		System.out.println("3 - ��������� ������ �������");
		System.out.println("0 - �����");
		System.out.print("��� �����: ");
		BufferedReader reader = new BufferedReader(new InputStreamReader(System.in));
		int mode = Integer.parseInt(reader.readLine());
		if (mode == 1 || mode == 2)
		{
			// ��������� �������� ������
			String data = mode == 1 ? loadXML() : loadCSV();
			
			if (data != null && !data.isEmpty())
			{
		        try { 
		        	// ������������ ������ ������������
					String configuration = 
							//"������ = C:\\�������\\Excel\\Acron.Reports\\Grouping\\bin\\Debug\\Grouping.dll; ����� = Grouping.ReportGeneratorWithGraphics; ������ = " +
							"������ = C:\\�������\\Excel\\��������\\Grouping\\bin\\Grouping.dll; ����� = Grouping.ReportGeneratorWithGraphics; ������ = " +
							(mode == 1 ? "XML" : "CSV");
					
					// ������������� Excel-����� � �������������� �������
		        	String excelReportStr = getExcelReportFromService(data, configuration);
		        	
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
		}
		else if (mode == 3)
		{
	        try { 
				// �������������� ������ �������
	        	getExcelReportFromService(null, null);
	        	
	        } catch (Exception e) {  
	            e.printStackTrace();  
	        }  
		}
	}

	/**
	 * ������������ Excel-����� � �������������� �������
	 * @param data - �������� ������ ��� ���������
	 * @param configuration - ������������ ��� ������ �������
	 * @return - �������� ������������� ���������������� ������, �������������� � base64
	 * @throws RemoteException
	 */
	private static String getExcelReportFromService(String data, String configuration) throws RemoteException
	{
		// ������� ������ �� ������, ���������������� �� ����� ��������� ������
       	//return (new IExcelReportingServiceProxy()).getExcelReportBase64(data, configuration);
		
		// ������� ������ �� ���������� ������
       	return (new IExcelReportingServiceProxy("http://localhost:8733/Design_Time_Addresses/ExcelReportingServiceLib/ExcelReportingService/"))
       			.getExcelReportBase64(data, configuration);
	}
	
	/**
	 * ��������� CSV-���� � ��������� �������
	 * @return
	 * @throws IOException
	 */
	private static String loadCSV() throws IOException {
		return loadText("��� ��������� CSV-�����: ");
	}

	/**
	 * ��������� XML-���� � ��������� �������
	 * @return
	 * @throws IOException
	 */
	private static String loadXML() throws IOException {
		// Byte order mark screws up file reading in Java
		// http://stackoverflow.com/questions/1835430/byte-order-mark-screws-up-file-reading-in-java
		return loadText("��� ��������� XML-�����: ");
	}

	/**
	 * ��������� ��������� ���� (� ������� Windows-1251)
	 * @param message - ������ ����� �����
	 * @return
	 * @throws IOException
	 */
	private static String loadText(String message) throws IOException {
		System.out.print(message);
		BufferedReader reader = new BufferedReader(new InputStreamReader(System.in));
		String fileName = reader.readLine();
		return new String(Files.readAllBytes(Paths.get(fileName)));
	}
}
