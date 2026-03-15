using System;
using System.Collections.Generic;
using Assets._Project.Develop.Runtime.Configs;
using Assets._Project.Develop.Runtime.Meta.Features.Statistics;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.Utilities.ConfigsManagment;
using Assets._Project.Develop.Runtime.Utilities.DataManagment.Data;

namespace Assets._Project.Develop.Runtime.Utilities.DataManagment.DataProvider
{
    public class PlayerDataProvider : DataProvider<PlayerData>
    {
        private readonly ConfigsProviderService _configsProviderService;

        public PlayerDataProvider(ISaveLoadService saveLoadService, ConfigsProviderService configsProviderService) : base(saveLoadService)
        {
            _configsProviderService = configsProviderService;
        }

        protected override PlayerData GetOriginData()
        {
            return new PlayerData()
            {
                WalletData = InitializeWalletData(),
                StatsData = InitializeStatsData()
            };
        }

        private Dictionary<CurrencyTypes, int> InitializeWalletData()
        {
            Dictionary<CurrencyTypes, int> walletData = new();
          
            StartWalletConfig walletConfig = _configsProviderService.GetConfig<StartWalletConfig>();

            foreach (CurrencyTypes currencyTypes in Enum.GetValues(typeof(CurrencyTypes)))
                walletData[currencyTypes] = walletConfig.GetValueFor(currencyTypes);

            return walletData;
        }

        private Dictionary<StatisticsItemTypes, int> InitializeStatsData()
        {
            Dictionary<StatisticsItemTypes, int> statsData = new();

            StatisticsConfig statisticsConfig = _configsProviderService.GetConfig<StatisticsConfig>();

            foreach (StatisticsItemTypes statItem in Enum.GetValues(typeof(StatisticsItemTypes)))
                statsData[statItem] = statisticsConfig.GetValueFor(statItem);

            return statsData;
        } 
    }
}
