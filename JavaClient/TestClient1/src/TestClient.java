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
		System.out.println("Генерация Excel-отчетов с использованием WEB-сервиса .Net");
		System.out.println();
		
		System.out.println("Возможные форматы исходных данных:");
		System.out.println("1 - XML-строка");
		System.out.println("2 - CSV (Comma-Separated Values)");
		System.out.println("3 - обработка ошибки сервиса");
		System.out.println("0 - выход");
		System.out.print("Ваш выбор: ");
		BufferedReader reader = new BufferedReader(new InputStreamReader(System.in));
		int mode = Integer.parseInt(reader.readLine());
		if (mode == 1 || mode == 2)
		{
			// загрузить исходные данные
			String data = mode == 1 ? loadXML() : loadCSV();
			
			if (data != null && !data.isEmpty())
			{
		        try { 
		        	// сформировать строку конфигурации
					String configuration = 
							//"модуль = C:\\Проекты\\Excel\\Acron.Reports\\Grouping\\bin\\Debug\\Grouping.dll; класс = Grouping.ReportGeneratorWithGraphics; формат = " +
							"модуль = C:\\Проекты\\Excel\\Прототип\\Grouping\\bin\\Grouping.dll; класс = Grouping.ReportGeneratorWithGraphics; формат = " +
							(mode == 1 ? "XML" : "CSV");
					
					// сгенерировать Excel-отчет с использованием сервиса
		        	String excelReportStr = getExcelReportFromService(data, configuration);
		        	
		        	// запросить имя файла для сохранения результата
		    		System.out.print("Имя файла-результата: ");
		        	String fileName = reader.readLine();
		        	
		        	// сохранить сгенерированный Excel-отчет
		            Files.write(Paths.get(fileName), Base64.getDecoder().decode(excelReportStr));
		            
					System.out.println("Файл записан.");
					
		        } catch (Exception e) {  
		            e.printStackTrace();  
		        }  
			}
		}
		else if (mode == 3)
		{
	        try { 
				// спровоцировать ошибку сервиса
	        	getExcelReportFromService(null, null);
	        	
	        } catch (Exception e) {  
	            e.printStackTrace();  
	        }  
		}
	}

	/**
	 * Генерировать Excel-отчет с использованием сервиса
	 * @param data - исходные данные для генерации
	 * @param configuration - конфигурация для работы сервиса
	 * @return - бинарное представление сгенерированного отчета, закодированное в base64
	 * @throws RemoteException
	 */
	private static String getExcelReportFromService(String data, String configuration) throws RemoteException
	{
		// вызвать сервис по адресу, зафиксированному во время генерации прокси
       	//return (new IExcelReportingServiceProxy()).getExcelReportBase64(data, configuration);
		
		// вызвать сервис по указанному адресу
       	return (new IExcelReportingServiceProxy("http://localhost:8733/Design_Time_Addresses/ExcelReportingServiceLib/ExcelReportingService/"))
       			.getExcelReportBase64(data, configuration);
	}
	
	/**
	 * Загрузить CSV-файл с исходными данными
	 * @return
	 * @throws IOException
	 */
	private static String loadCSV() throws IOException {
		return loadText("Имя исходного CSV-файла: ");
	}

	/**
	 * Загрузить XML-файл с исходными данными
	 * @return
	 * @throws IOException
	 */
	private static String loadXML() throws IOException {
		// Byte order mark screws up file reading in Java
		// http://stackoverflow.com/questions/1835430/byte-order-mark-screws-up-file-reading-in-java
		return loadText("Имя исходного XML-файла: ");
	}

	/**
	 * Загрузить текстовый файл (в формате Windows-1251)
	 * @param message - запрос имени файла
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
