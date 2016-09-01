using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            // Define PM server parameters
            var workspace = "myWorkspace";
            var protocol = "https";
            var hostname = "my.processmaker.server";
            var url = protocol + "://" + hostname;
            
            // Fix SSL certificate issues - may or not be required depending on type of certificate (if using HTTPS) and local configuration
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(AllwaysGoodCertificate);
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            
            // Perform initial request to get an OAuth2 token
            var client = new RestClient(url);
            var request = new RestRequest("{workspace}/oauth2/token", Method.POST);
            request.AddUrlSegment("workspace", workspace);
            request.AddParameter("grant_type", "password");
            request.AddParameter("scope", "*");
            request.AddParameter("client_id", "DZNXYIEDOQRAWFJGSZYAXSSHOCAEFWBI");
            request.AddParameter("client_secret", "85978646557c6ea9fb75744071747093");
            request.AddParameter("username", "my_pm_username");
            request.AddParameter("password", "my_pm_password");
            IRestResponse response = client.Execute(request);
            // Parse response to get the token as a string
            var content = response.Content;
            dynamic auth = JObject.Parse(response.Content);
            var aToken = Convert.ToString(auth.access_token);

            // Perform additional request using the token as part of the headers
            request = new RestRequest("api/1.0/{workspace}/users",Method.GET);
            request.AddUrlSegment("workspace", workspace);
            request.AddHeader("Authorization", "Bearer " + aToken);
            response = client.Execute(request);
            JArray users = JArray.Parse(response.Content);
            // Loop over all response objects, print usernames
            for (int i = 0; i < users.Count; i++)
            {
                dynamic user = users[i];
                var username = user.usr_username;
                Console.WriteLine(username);
            }
            
        }
        private static bool AllwaysGoodCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors policyErrors)
        {
            return true;
        }
    }
}
