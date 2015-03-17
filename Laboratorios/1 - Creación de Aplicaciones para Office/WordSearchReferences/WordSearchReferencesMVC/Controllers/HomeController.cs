using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WordSearchReferencesMVC.Models;

namespace WordSearchReferencesMVC.Controllers
{
    public class HomeController : Controller
    {
        static readonly string ServiceResourceId = ConfigurationManager.AppSettings["ida:resource"];
        static readonly Uri ServiceEndpointUri = new Uri(ConfigurationManager.AppSettings["ida:SiteURL"] + "/_api/");
        private IFilesRepository _repository;

        public HomeController(IFilesRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult> Index(string authType)
        {
            string redirectUri = this.Request.Url.GetLeftPart(UriPartial.Authority).ToString() + "/Home/App";
            string authorizationUrl = OAuthController.GetAuthorizationUrl(ServiceResourceId, new Uri(redirectUri));
            //string authorizationUrl = this.Request.Url.GetLeftPart(UriPartial.Authority).ToString() + "/Home/App";
            return new RedirectResult(authorizationUrl);
        }

        //public async Task<ActionResult> App()
        //{
        //    return View(new IndexViewModel() { Results = new List<Models.FileResult>()});
        //}

        public ActionResult App(IndexViewModel indexModel)
        {
            if(indexModel.Results == null)
                indexModel.Results = new List<Models.FileResult>();
            return View(indexModel);
        }

        [HttpPost]
        public ActionResult SearchForm(IndexViewModel indexModel)
        {
            var resultItem = _repository.SearchFiles(indexModel.Keyword).Result;
            IndexViewModel resultViewModel = new IndexViewModel() { Results = new List<Models.FileResult>() };

            foreach (var item in resultItem)
            {
                resultViewModel.Results.Add(new Models.FileResult()
                {
                    Title = item.Title,
                    Created = item.Created,
                    CreatedBy = item.CreatedBy,
                    Url = item.Url
                });
            }

            return View ("App", resultViewModel);
        }

    }
}