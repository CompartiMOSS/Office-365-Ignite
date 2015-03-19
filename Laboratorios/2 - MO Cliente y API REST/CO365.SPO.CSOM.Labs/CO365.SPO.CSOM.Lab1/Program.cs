using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
//Directivas using necesarias
using SPCSOM = Microsoft.SharePoint.Client;
using System.Security;
using System.Configuration;

namespace CO365.SPO.CSOM.Lab1
{
    class Program
    {
        /// <summary>
        /// Método Principal de la Aplicación de Consola
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
           
            //ConsultarContactosCSOM();
            //OperacionesCRUD("C");
            OperacionesCRUD("A");
            OperacionesCRUD("B");
            Console.ReadLine();
        }

        /// <summary>
        /// Método estático para realizar una consulta a la Lista
        /// </summary>
        static void ConsultarContactosCSOM()
        {
            try
            {
                string sSiteUrl = "https://itechcs.sharepoint.com/sites/CO365DeveloperSite/";
                using (SPCSOM.ClientContext spoCtx = new SPCSOM.ClientContext(sSiteUrl))
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
                    spoCtx.Credentials =
                        new SPCSOM.SharePointOnlineCredentials(
                            sSPOUser, ssPassword);

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
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
            }
        }
        /// <summary>
        /// Método para realizar operaciones de tipo CRUD
        /// </summary>
        /// <param name="sTipoDeOperacion"></param>
       static void OperacionesCRUD(string sTipoDeOperacion)
       {
           try
           {
               string sSiteUrl = "https://itechcs.sharepoint.com/sites/CO365DeveloperSite/";
               using (SPCSOM.ClientContext spoCtx = new SPCSOM.ClientContext(sSiteUrl))
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
                    spoCtx.Credentials =
                        new SPCSOM.SharePointOnlineCredentials(
                            sSPOUser, ssPassword);

                    SPCSOM.List spoList =
                        spoCtx.Web.Lists.GetByTitle("Contactos Curso Office 365");
                    spoCtx.Load(spoList);
                    spoCtx.ExecuteQuery();
                    if (spoList != null && spoList.ItemCount > 0)
                    {
                        switch (sTipoDeOperacion)
                        {
                            //Crear
                            case "C":
                                Console.WriteLine("****Creando Elemento en la lista****");
                                SPCSOM.ListItemCreationInformation liciContacto =
                                    new SPCSOM.ListItemCreationInformation();
                                SPCSOM.ListItem liElemento = 
                                    spoList.AddItem(liciContacto);
                                liElemento["FirstName"] = "Adrián";
                                liElemento["Title"] = "Díaz";
                                liElemento.Update();
                                spoCtx.ExecuteQuery();  
                                ConsultarContactosCSOM();
                                break;
                            //Crear
                            case "A":
                                Console.WriteLine("****Actualizando Elemento en la lista****");
                                SPCSOM.ListItem liElementoAActualizar =
                                    spoList.GetItemById(4);
                                liElementoAActualizar["FirstName"] = "Adrián";
                                liElementoAActualizar["Title"] = "Díaz Cervera";
                                liElementoAActualizar.Update();
                                spoCtx.ExecuteQuery();                                  
                                ConsultarContactosCSOM();
                                break;
                            //Crear
                            case "B":
                                Console.WriteLine("****Borrando Elemento en la lista****");
                                SPCSOM.ListItem liElementoABorrar =
                                    spoList.GetItemById(4);
                                liElementoABorrar.DeleteObject();
                                spoCtx.ExecuteQuery();                                                                  
                                ConsultarContactosCSOM();
                                break;
                            default:
                                break;
                        }
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
