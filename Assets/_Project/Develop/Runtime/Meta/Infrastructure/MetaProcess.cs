using System;
using UnityEngine;
using System.Collections;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.Meta.Features.Statistics;
using Assets._Project.Develop.Runtime.Meta.Features.Combinations;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilities.DataManagment.DataProvider;
using Assets._Project.Develop.Runtime.Utilities.ConfigsManagment;
using Assets._Project.Develop.Runtime.Configs;

namespace Assets._Project.Develop.Runtime.Meta.Infrastructure
{
    public class MetaProcess
    {
        private WalletService _walletService;
        private PlayerDataProvider _playerDataProvider;
        private CombinationSelector _combinationSelector;
        private ICoroutinesPerformer _coroutinesPerformer;
        private PlayerStatisticsService _playerStatisticService;
        private ConfigsProviderService _configProviderService;

        private bool _isRunning;
        private Action<TypesGameModes> _onModeSelected;
        private RewardsAndCostsConfig _rewardsAndCostsConfig;

        public MetaProcess(WalletService walletService,
            PlayerStatisticsService playerStatisticsService,
            CombinationSelector combinationSelector,
            ICoroutinesPerformer coroutinesPerformer,
            PlayerDataProvider dataProvider,
            ConfigsProviderService configProviderService)
        {
            _walletService = walletService;
            _playerStatisticService = playerStatisticsService;
            _combinationSelector = combinationSelector;
            _coroutinesPerformer = coroutinesPerformer;
            _playerDataProvider = dataProvider;
            _configProviderService = configProviderService;
        }

        public void Initialize() => _rewardsAndCostsConfig = _configProviderService.GetConfig<RewardsAndCostsConfig>();

        public void Run(Action<TypesGameModes> onModeSelected)
        {
            _isRunning = true;
            _onModeSelected = onModeSelected;
            _coroutinesPerformer.StartPerform(Update());
        }

        private IEnumerator Update()
        {
            while (_isRunning)
            {
                HandleInput();
                yield return null;
            }
        }

        private void HandleInput()
        {
            SelectModeOnKeyPress(_onModeSelected);

            if (Input.GetKeyDown(KeyCode.R))
                ResetDataOnKeyPress();

            if (Input.GetKeyDown(KeyCode.Tab))
                ShowStatsOnKeyPress();
        }

        private void SelectModeOnKeyPress(Action<TypesGameModes> callback)
        {
            if (_combinationSelector.TryGetSelectedModeType(out TypesGameModes mode))
            {
                _isRunning = false;
                callback?.Invoke(mode);
            }
        }

        private void ResetDataOnKeyPress()
        {
            if (_walletService.Enough(_rewardsAndCostsConfig.Currency, _rewardsAndCostsConfig.ResetCost) == false)
            {
                Debug.Log("Недостаточно средств!");
                return;
            }

            _walletService.Spend(_rewardsAndCostsConfig.Currency, _rewardsAndCostsConfig.ResetCost);

            _playerStatisticService.Reset();

            ShowStatsOnKeyPress();

            _coroutinesPerformer.StartPerform(_playerDataProvider.Save());
        }

        private void ShowStatsOnKeyPress()
        {
            Debug.Log(
                $"Побед - {_playerStatisticService.GetWinCount()}, " +
                $"Поражений - {_playerStatisticService.GetLossCount()}, " +
                $"Золото - {_walletService.GetCurrency(CurrencyTypes.Gold).Value}"
                );
        }
    }
}
