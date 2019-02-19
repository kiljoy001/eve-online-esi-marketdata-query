using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;

namespace esidatascrape
{
    public class KeyVaultHelper
    {
        private const string key = "https://esidatascrapevault.vault.azure.net/keys/esikey/7b3ebcee506f41bfb1e0f673705359c7";
        private const string ClientId = "https://esidatascrapevault.vault.azure.net/secrets/ClientId/34e758849f6f417482ee547f62348787";
        private const string SecretKey = "https://esidatascrapevault.vault.azure.net/secrets/ClientId/34e758849f6f417482ee547f62348787";
        private const string Cert = "https://esidatascrapevault.vault.azure.net/certificates/esicert/02eea644ef8140a48b237c1fda5bc1a9";
        private const string certPfxThumbprint = "A51E38CBA92CF8301532462422FC035E949FAD6F";
        private static ClientAssertionCertificate AssertionCert {get; set;}

        private async Task<string> GetAccessToken(string authority, string resource, string scope)
        {
            var context = new AuthenticationContext(authority, TokenCache.DefaultShared);
            var result = await context.AcquireTokenAsync(resource, AssertionCert);
            return result.AccessToken;
        }

        public string GetKeyVaultValue(string keyname)
        {
            GetAppCert();
            var keyvaultclient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(GetAccessToken));
            var task = new TaskFactory(
                CancellationToken.None,
                TaskCreationOptions.None,
                TaskContinuationOptions.None,
                TaskScheduler.Default
                ).StartNew<Task<SecretBundle>>(() => keyvaultclient.GetSecretAsync(keyname)).Unwrap().GetAwaiter().GetResult();
            return task.Value;
        }

        private void GetAppCert()
        {
            var pfxcert = getCertByThumbPrint(certPfxThumbprint);
            AssertionCert = new ClientAssertionCertificate(ClientId, pfxcert);
        }

        private static X509Certificate2 getCertByThumbPrint(string thumbprint)
        {
            X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            try
            {
                store.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection collection = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);
                if(collection == null || collection.Count == 0)
                {
                    return null;
                }
                return collection[0];
                
            }
            finally
            {
                store.Close();
            }
        }
    }
}
