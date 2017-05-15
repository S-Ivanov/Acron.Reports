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
			System.out.println("Загрузка Excel-отчетов с использованием WEB-сервиса .Net");
			System.out.println();
			
			System.out.print("Введите id отчета: ");
			BufferedReader reader = new BufferedReader(new InputStreamReader(System.in));
			String id = reader.readLine();
			
			// сгенерировать Excel-отчет с использованием сервиса
        	String excelReportStr = getExcelReportFromService(id);
        	
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

	/**
	 * Загрузить Excel-отчет с использованием сервиса
	 * @param id - идентификатор отчета
	 * @return - бинарное представление сгенерированного отчета, закодированное в base64
	 * @throws RemoteException
	 */
	private static String getExcelReportFromService(String id) throws RemoteException
	{
		// вызвать сервис по адресу, зафиксированному во время генерации прокси
       	//return (new IExcelReportingServiceProxy()).getReport(id, null, null);
		
		// вызвать сервис по указанному адресу
       	return (new IExcelReportingServiceProxy("http://localhost:2585/ExcelReportingService/")).getReport(id, null, null);
	}
}
