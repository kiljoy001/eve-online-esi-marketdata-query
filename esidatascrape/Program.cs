using System;

namespace esidatascrape
{
    class Program
    {
        static void Main(string[] args)
        {
            KeyVaultHelper helper = new KeyVaultHelper();
            string clientid = helper.GetKeyVaultValue("ClientId");
            string secretkey = helper.GetKeyVaultValue("SecretKey");
            string esikey = helper.GetKeyVaultValue("esikey");
            Console.WriteLine($"{clientid}\n{secretkey}\n{esikey}");
        }
    }
}
