using System;
using System.Configuration;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace ExcelReportingServiceLib
{
    public class BasicAuthorizationManager : ServiceAuthorizationManager
    {
        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
            if ((authHeader != null) && (authHeader != string.Empty))
            {
                var svcCredentials = System.Text.ASCIIEncoding.ASCII
                    .GetString(Convert.FromBase64String(authHeader.Substring(6)))
                    .Split(':');
                var credentials = new
                {
                    Name = svcCredentials[0],
                    Password = svcCredentials[1]
                };

                string user = ConfigurationManager.AppSettings["user"];
                string password = ConfigurationManager.AppSettings["password"];
                return user == credentials.Name && password == credentials.Password;
            }
            else
            {
                //No authorization header was provided, so challenge the client to provide before proceeding:  
                WebOperationContext.Current.OutgoingResponse.Headers.Add("WWW-Authenticate: Basic realm=\"ExcelReportingService\"");
                //Throw an exception with the associated HTTP status code equivalent to HTTP status 401  
                throw new WebFaultException(HttpStatusCode.Unauthorized);
            }
        }
    }
}
