using UnityEngine;
using System.Collections;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilities.SceneManagment;
using Assets._Project.Develop.Runtime.Utilities.DataManagment.DataProvider;
using Assets._Project.Develop.Runtime.Meta.Features.Statistics;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.Meta.Features.Combinations;
using Assets._Project.Develop.Runtime.GamePlay.Features;
using Assets._Project.Develop.Runtime.Utilities.ConfigsManagment;
using Assets._Project.Develop.Runtime.Configs;

namespace Assets._Project.Develop.Runtime.GamePlay.Infrastructure
{
    public class GameProcess
    {
        private ICombination _combination;
        private RewardsAndCostsConfig _rewardsAndCostsConfig;

        private WalletService _walletService;
        private GameplayInputArgs _gameplayInputArgs;
        private PlayerDataProvider _playerDataProvider;
        private CombinationFactory _combinationFactory;
        private ICoroutinesPerformer _coroutinesPerformer;
        private SceneSwitcherService _sceneSwitcherService;
        private ConfigsProviderService _configsProviderService;
        private PlayerStatisticsService _playerStatisticsService;
        private UserInputHandlerService _userInputHandlerService;

        public GameProcess(GameplayInputArgs gameplayInputArgs,
            SceneSwitcherService sceneSwitcherService,
            ICoroutinesPerformer coroutinesPerformer,
            CombinationFactory combinationFactory,
            WalletService walletService,
            PlayerDataProvider playerDataProvider,
            PlayerStatisticsService playerStatisticsService,
            ConfigsProviderService configsProviderService,
            UserInputHandlerService userInputHandlerService)
        {
            _gameplayInputArgs = gameplayInputArgs;
            _sceneSwitcherService = sceneSwitcherService;
            _coroutinesPerformer = coroutinesPerformer;
            _combinationFactory = combinationFactory;
            _walletService = walletService;
            _playerDataProvider = playerDataProvider;
            _playerStatisticsService = playerStatisticsService;
            _configsProviderService = configsProviderService;
            _userInputHandlerService = userInputHandlerService;
        }

        public void Initialize()
        {
            _combination = _combinationFactory.CreateCombination(_gameplayInputArgs.GameMode);

            _userInputHandlerService.SetString(_combination.Value);

            _rewardsAndCostsConfig = _configsProviderService.GetConfig<RewardsAndCostsConfig>();

            _userInputHandlerService.WinGame += WinGameProcess;
            _userInputHandlerService.LossGame += DefeatGameProcess;
        }

        private void WinGameProcess()
        {
            _walletService.Add(_rewardsAndCostsConfig.Currency, _rewardsAndCostsConfig.WinReward);
            _playerStatisticsService.Add(StatisticsItemTypes.Win);

            _coroutinesPerformer.StartPerform(Reset());
        } 

        private void DefeatGameProcess()
        {
            if (_walletService.Enough(_rewardsAndCostsConfig.Currency, _rewardsAndCostsConfig.DefeatCost))
                _walletService.Spend(_rewardsAndCostsConfig.Currency, _rewardsAndCostsConfig.DefeatCost);

            _playerStatisticsService.Add(StatisticsItemTypes.Loss);

            _coroutinesPerformer.StartPerform(Reset());
        }

        private IEnumerator Reset()
        {
            _userInputHandlerService.WinGame -= WinGameProcess;
            _userInputHandlerService.LossGame -= DefeatGameProcess;

            _userInputHandlerService.Dispose();

            yield return _playerDataProvider.Save();
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

            _coroutinesPerformer.StartPerform(_sceneSwitcherService.ProcessSwitchTo(Scenes.MainMenu, _gameplayInputArgs));
        }
    }
}