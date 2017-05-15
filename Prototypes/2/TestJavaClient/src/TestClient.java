import java.awt.Desktop;
import java.io.IOException;
import java.net.URI;
import java.net.URISyntaxException;

public class TestClient {

	public static void main(String[] args) throws IOException, URISyntaxException {
		
		Desktop.getDesktop().browse(new URI("http://localhost:2585/ExcelReportingService/GetReport/id=1"));

	}

}
