using ESI.NET;
using ESI.NET.Enumerations;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace esidatascrape
{
    public class CollectData
    {
        private IOptions<EsiConfig> RunningConfig;
        private IOptions<EsiConfig> TestingConfig;
        private EsiClient ProdClient;
        private EsiClient TestClient;

        public CollectData()
        {
            RunningConfig = Options.Create(new EsiConfig()
            {
                EsiUrl = "https://esi.evetech.net/latest/",
                DataSource = DataSource.Tranquility,
                ClientId = "f5a0a03c5af74f6884a278675ad3411f",
                SecretKey = "UhjDXz5POW05q18cBSrVgwdNEYzD1fkOlRkTQMb8",
                CallbackUrl = "",
                UserAgent = ""
            });

            TestingConfig = Options.Create(new EsiConfig()
            {
                EsiUrl = "https://esi.evetech.net/dev",
                DataSource = DataSource.Tranquility,
                ClientId = "f5a0a03c5af74f6884a278675ad3411f",
                SecretKey = "UhjDXz5POW05q18cBSrVgwdNEYzD1fkOlRkTQMb8",
                CallbackUrl = "",
                UserAgent = ""
            });

            EsiClient ProdClient = new EsiClient(RunningConfig);
            EsiClient TestClient = new EsiClient(TestingConfig);
        }
    }
}
