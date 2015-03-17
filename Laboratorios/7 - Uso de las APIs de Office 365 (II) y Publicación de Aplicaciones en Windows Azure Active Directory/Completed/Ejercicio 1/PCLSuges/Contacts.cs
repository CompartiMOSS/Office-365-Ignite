using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Office365.Discovery;
using Microsoft.Office365.OutlookServices;

namespace PCLSuges
{
    public class Contacts
    {
        
        public static string ClientId = "1535bdf7-af56-4103-83ea-256ced9cff99";
        public static Uri ReturnUri = new Uri("http://DotNetConference");        
        public const string CommonAuthority = "https://login.windows.net/Common";
        public const string DiscoveryResourceId = "https://api.office.com/discovery/";

       


        public static AuthenticationContext _authenticationContext { get; set; }
        private static async Task<string> GetTokenHelperAsync(AuthenticationContext context, string resourceId, IAuthorizationParameters parent)
        {
            string accessToken = null;
            //AuthenticationResult result = null;

            AuthenticationResult result = await context.AcquireTokenAsync(resourceId, ClientId, ReturnUri, parent);

            accessToken = result.AccessToken;


            return accessToken;
        }

        public static async Task<OutlookServicesClient> CreateOutlookClientAsync(string capability, IAuthorizationParameters parent)
        {
            try
            {
                //First, look for the authority used during the last authentication.
                //If that value is not populated, use CommonAuthority.
                string authority = null;

                authority = CommonAuthority;

                // Create an AuthenticationContext using this authority.
                _authenticationContext = new AuthenticationContext(authority);


                var discoveryClient = new DiscoveryClient(
                    async () => await GetTokenHelperAsync(_authenticationContext, DiscoveryResourceId, parent));


                var result = await discoveryClient.DiscoverCapabilityAsync("Contacts");
                var client = new OutlookServicesClient(
                    result.ServiceEndpointUri,
                    async () =>
                        await GetTokenHelperAsync(_authenticationContext, result.ServiceResourceId, parent));
                return client;
            }
            catch (Exception)
            {
                if (_authenticationContext != null && _authenticationContext.TokenCache != null)
                    _authenticationContext.TokenCache.Clear();
                return null;
            }
        }

        public static async Task<List<MyContacts>> GetContacts(IAuthorizationParameters parent)
        {
            var outlookClient = await CreateOutlookClientAsync("Contacts", parent);
            var contactList = new List<MyContacts>();

            var contactsResults = await outlookClient.Me.Contacts.ExecuteAsync();


            foreach (var contact in contactsResults.CurrentPage.OrderBy(c => c.Surname))
            {
                contactList.Add(new MyContacts
                {
                    Id = contact.Id,
                    GivenName = contact.GivenName,
                    Surname = contact.Surname,
                    DisplayName = contact.Surname + ", " + contact.GivenName,
                    CompanyName = contact.CompanyName,
                    EmailAddress1 = contact.EmailAddresses.FirstOrDefault().Address,
                    BusinessPhone1 = contact.BusinessPhones.FirstOrDefault(),
                    HomePhone1 = contact.HomePhones.FirstOrDefault()
                });
            }
            return contactList;
        }
    }
}
