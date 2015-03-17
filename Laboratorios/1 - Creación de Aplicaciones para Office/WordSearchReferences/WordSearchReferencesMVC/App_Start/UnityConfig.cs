using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Unity.Mvc5;
using WordSearchReferencesMVC.Models;

namespace WordSearchReferencesMVC.App_Start
{
    public class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<IFilesRepository, FilesRepository>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}