using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Espacios de nombres necesarios
using SPCSOM=Microsoft.SharePoint.Client;
using System.Security;
using System.Configuration;

namespace CO365.SPO.CSOM.Lab2
{
    class Program
    {
        static void Main(string[] args)
        {

            Uri siteUri = 
                new Uri("https://itechcs.sharepoint.com/sites/CO365DeveloperSite");
            string realm = 
                TokenHelper.GetRealmFromTargetUrl(siteUri);
            string accessToken = 
                TokenHelper.GetAppOnlyAccessToken(
                    TokenHelper.SharePointPrincipal,
                    siteUri.Authority, realm).AccessToken;

            using (var spoCtx = TokenHelper.GetClientContextWithAccessToken(siteUri.ToString(), accessToken))
            {
                ApplyTheme(spoCtx);
                Console.WriteLine("El tema ha sido actualizado...");
                //ConsultarContactosCSOM(spoCtx);
                //Console.ReadLine();
            }

            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="relativeUrl"></param>
        /// <returns></returns>
        private static string URLCombine(string baseUrl, string relativeUrl)
        {
            if (baseUrl.Length == 0)
                return relativeUrl;
            if (relativeUrl.Length == 0)
                return baseUrl;
            return string.Format("{0}/{1}",
                    baseUrl.TrimEnd(new char[] { '/', '\\' }),
                    relativeUrl.TrimStart(new char[] { '/', '\\' }));
        }

        /// <summary>
        /// Método para aplicar el Tema al Sitio
        /// </summary>
        /// <param name="spoCtx"></param>
        private static void ApplyTheme(SPCSOM.ClientContext spoCtx)
        {
            try
            {
                SPCSOM.Web wWeb = spoCtx.Web;
                spoCtx.Load(wWeb);
                spoCtx.ExecuteQuery();

                //Aplicación del tema Sketch
                wWeb.ApplyTheme(
                        URLCombine(wWeb.ServerRelativeUrl, 
                            "/_catalogs/theme/15/palette007.spcolor"),
                        URLCombine(wWeb.ServerRelativeUrl, 
                            "/_catalogs/theme/15/fontscheme002.spfont"),
                        URLCombine(wWeb.ServerRelativeUrl, 
                            "/_layouts/15/images/image_bg007.jpg"),
                        false);
                spoCtx.ExecuteQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
            }
        }

        /// <summary>
        /// Método que consulta la lista de Contactos - Sin credenciales
        /// </summary>
        static void ConsultarContactosCSOM(SPCSOM.ClientContext spoCtx)
        {
            try
            {
                SPCSOM.List spoList =
                    spoCtx.Web.Lists.GetByTitle("Contactos Curso Office 365");
                spoCtx.Load(spoList);
                spoCtx.ExecuteQuery();
                if (spoList != null && spoList.ItemCount > 0)
                {
                    SPCSOM.CamlQuery spocqConsulta =
                        new SPCSOM.CamlQuery();
                    spocqConsulta.ViewXml =
                        @"<View>  
                                <Query> 
                                    <OrderBy>
                                        <FieldRef Name='FirstName' />
                                    </OrderBy> 
                                </Query> 
                                <ViewFields>
                                    <FieldRef Name='LinkTitle' />
                                    <FieldRef Name='FirstName' />
                                </ViewFields> 
                            </View>";
                    SPCSOM.ListItemCollection spoliItemCollection =
                        spoList.GetItems(spocqConsulta);
                    spoCtx.Load(spoliItemCollection);
                    spoCtx.ExecuteQuery();

                    //Procesado de los datos
                    foreach (SPCSOM.ListItem spoliItem in spoliItemCollection)
                    {
                        Console.WriteLine("Nombre: {0} - Apellido: {1}",
                            spoliItem["FirstName"].ToString(),
                            spoliItem["Title"].ToString());
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
            }
        }
    }
}
