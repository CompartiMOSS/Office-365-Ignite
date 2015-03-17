using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using WordSearchReferencesMVC.Controllers;

namespace WordSearchReferencesMVC.Models
{
    public interface IFilesRepository
    {
        Task<List<ResultItem>> SearchFiles(string Keyword);
    }

    public class FilesRepository : IFilesRepository
    {
        public string SharePointServiceRoot = ConfigurationManager.AppSettings["ida:SiteUrl"];
        public string SharePointResourceId = ConfigurationManager.AppSettings["ida:Resource"];
        public string Tenant = ConfigurationManager.AppSettings["ida:Tenant"];
        public async Task<List<ResultItem>> SearchFiles(string Keyword)
        {

            string requestUri = string.Concat("/_api/search/query?querytext='", Keyword, "'");

            //return new List<ResultItem>() {
            //    new ResultItem() { Title ="Fichero1", Url= "http://google.com", Created="01/01/2015", CreatedBy="Mario" },
            //    new ResultItem() { Title ="Fichero2", Url= "http://google.com", Created="01/01/2015", CreatedBy="Mario" }
            //};

            using (var client = CreateHttpClient(SharePointServiceRoot, GetAccessToken()))
            {
                HttpResponseMessage response = await client.GetAsync(requestUri);
                if (response.IsSuccessStatusCode)
                {
                    //var result = response.Content.ReadAsAsync<EventsResult>().Result;
                    var content = response.Content.ReadAsStringAsync();
                    return new List<ResultItem>()
                    {
                        new ResultItem() { Title ="Fichero1", Url= "http://google.com", Created="01/01/2015", CreatedBy="Mario" },
                        new ResultItem() { Title ="Fichero2", Url= "http://google.com", Created="01/01/2015", CreatedBy="Mario" }
                    };
                }

                return new List<ResultItem>();
            }
        }

        private HttpClient CreateHttpClient(string ServiceUrl, string accessToken)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(ServiceUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            return client;
        }

        private string GetAccessToken()
        {
            return OAuthController.GetAccessTokenFromCacheOrRefreshToken(this.Tenant, this.SharePointResourceId);
        }
    }
}