import java.awt.Desktop;
import java.io.BufferedReader;
import java.io.File;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.URL;
import java.net.URLConnection;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.util.Base64;

public class TestClient {

	public static void main(String[] args) {
		
		try {
			System.out.println("Генерация Excel-отчетов с использованием WEB-сервиса .Net (базовая авторизация)");
			System.out.println();
			BufferedReader reader = new BufferedReader(new InputStreamReader(System.in));
			
			System.out.print("Введите идентификатор отчета: ");
			String id = reader.readLine();
			
			String serviceUri = "http://localhost:2585/ExcelReportingService/GetReport/id=" + id;
			
			System.out.print("Введите имя пользователя: ");
			String user = reader.readLine();
			System.out.print("Введите пароль: ");
			String password = reader.readLine();
			
			String authString = user + ":" + password;
			String authStringEnc = Base64.getEncoder().encodeToString(authString.getBytes());

			URL url = new URL(serviceUri);
			URLConnection urlConnection = url.openConnection();
			urlConnection.setRequestProperty("Authorization", "Basic " + authStringEnc);
			InputStream inputStream = urlConnection.getInputStream();
			
    		System.out.print("Имя файла-результата: ");
        	String fileName = reader.readLine();

        	Files.copy(inputStream, Paths.get(fileName));
			
			System.out.println("Файл записан.");
			
			Desktop.getDesktop().open(new File(fileName));
			
		} catch (Exception e) {
			e.printStackTrace();
		}
	}

}
