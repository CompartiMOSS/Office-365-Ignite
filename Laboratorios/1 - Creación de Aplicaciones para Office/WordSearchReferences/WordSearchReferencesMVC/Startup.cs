using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WordSearchReferencesMVC.Startup))]
namespace WordSearchReferencesMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            
        }
    }
}
