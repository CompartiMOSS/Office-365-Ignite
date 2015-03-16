using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Espacios de nombres necesarios
using System.Net;
using SPCOSM = Microsoft.SharePoint.Client;
using System.Security;
using System.Configuration;
using System.IO;
using System.Web.Script.Serialization;

//
//Referencias
//
//https://jamessdixon.wordpress.com/2012/11/20/consuming-json-from-a-c-project/ -> Newtonsoft’s Json.NET framework
//http://www.webthingsconsidered.com/2013/08/09/adventures-in-json-parsing-with-c/
//http://www.drowningintechnicaldebt.com/ShawnWeisfeld/archive/2010/08/22/using-c-4.0-and-dynamic-to-parse-json.aspx


namespace CO365.SPO.CSOM.Demos
{
    class Program
    {
        static void Main(string[] args)
        {
            REST_Query();
            Console.ReadLine();
        }

        /// <summary>
        /// Método para hacer una consulta REST.
        /// </summary>
        static void REST_Query()
        {
            try
            {
                //
                //SharePoint Online Credentials
                //
                string sSPOUser =
                    ConfigurationManager.AppSettings["SPOUser"];
                string sPassword =
                    ConfigurationManager.AppSettings["SPOPassword"];
                SecureString ssPassword = new SecureString();
                foreach (char c in sPassword.ToCharArray())
                    ssPassword.AppendChar(c);

                //REST URL
                string sRESTUrl =
                    "https://itechcs.sharepoint.com/sites/CO365DeveloperSite/_api/web/lists";

                //Petición Web
                HttpWebRequest sphwrPeticion =
                    (HttpWebRequest)WebRequest.Create(sRESTUrl);
                sphwrPeticion.Credentials =
                    new SPCOSM.SharePointOnlineCredentials(sSPOUser,ssPassword);
                sphwrPeticion.Headers.Add(
                    "X-FORMS_BASED_AUTH_ACCEPTED", "f");
                sphwrPeticion.Accept = "application/json;odata=verbose";            
                HttpWebResponse sphwrRespuesta =
                    (HttpWebResponse)sphwrPeticion.GetResponse();
                StreamReader srReader =
                    new StreamReader(sphwrRespuesta.GetResponseStream());
                string sData= srReader.ReadToEnd();

                JavaScriptSerializer jssSerializer = 
                    new JavaScriptSerializer();
                jssSerializer.RegisterConverters(
                    new JavaScriptConverter[] { new DynamicJsonConverter() });

                dynamic dResults = 
                    jssSerializer.Deserialize(
                        sData, 
                        typeof(object)) as dynamic;
                Console.WriteLine("******************Listas & Bibliotecas del Sitio*********************");
                foreach (var dResult in dResults.d.results)
                {
                    Console.WriteLine("Lista:{0} -  Descripción:{1} - Plantilla:{2}",
                        dResult["Title"],
                        dResult["Description"],
                        dResult["BaseTemplate"]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", 
                    ex.Message);
            }
        }
    }
}
