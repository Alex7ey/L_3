using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
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
        private bool _isRunning;
        private int _currentCharIndex;
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


        public GameProcess(GameplayInputArgs gameplayInputArgs,
            SceneSwitcherService sceneSwitcherService,
            ICoroutinesPerformer coroutinesPerformer,
            CombinationFactory combinationFactory,
            WalletService walletService,
            PlayerDataProvider playerDataProvider,
            PlayerStatisticsService playerStatisticsService,
            ConfigsProviderService configsProviderService)
        {
            _gameplayInputArgs = gameplayInputArgs;
            _sceneSwitcherService = sceneSwitcherService;
            _coroutinesPerformer = coroutinesPerformer;
            _combinationFactory = combinationFactory;
            _walletService = walletService;
            _playerDataProvider = playerDataProvider;
            _playerStatisticsService = playerStatisticsService;
            _configsProviderService = configsProviderService;
        }

        public void Initialize()
        {
            Keyboard.current.onTextInput += ProcessInput;

            _combination = _combinationFactory.CreateCombination(_gameplayInputArgs.GameMode);
            _rewardsAndCostsConfig = _configsProviderService.GetConfig<RewardsAndCostsConfig>();
            _isRunning = true;

            ShowGameInfo();
        }

        public void Run() => _isRunning = true;

        public void ProcessInput(char inputChar)
        {
            if (_isRunning == false)
                return;

            if (IsCorrectChar(inputChar))
            {
                _currentCharIndex++;

                if (_currentCharIndex >= _combination.Value.Length)
                    _coroutinesPerformer.StartPerform(WinGameProcess());

                return;
            }

            _coroutinesPerformer.StartPerform(DefeatGameProcess());
        }

        public bool IsCorrectChar(char inputChar)
        {
            if (inputChar.ToString().ToUpper() == _combination.Value[_currentCharIndex].ToString().ToUpper())
                return true;

            return false;
        }

        private IEnumerator WinGameProcess()
        {
            Debug.Log("Ты победил!\nНажмите Space чтобы продолжить");
 
            _walletService.Add(_rewardsAndCostsConfig.Currency, _rewardsAndCostsConfig.WinReward);
            _playerStatisticsService.AddWin();

            yield return Reset();
        }

        private IEnumerator DefeatGameProcess()
        {
            Debug.Log("Ты проиграл!\nНажмите Space чтобы продолжить");

            if (_walletService.Enough(_rewardsAndCostsConfig.Currency, _rewardsAndCostsConfig.DefeatCost))
                _walletService.Spend(_rewardsAndCostsConfig.Currency, _rewardsAndCostsConfig.DefeatCost);

            _playerStatisticsService.AddLoss();

            yield return Reset();
        }

        private IEnumerator Reset()
        {
            _isRunning = false;
            _currentCharIndex = 0;

            Keyboard.current.onTextInput -= ProcessInput;

            yield return _playerDataProvider.Save();
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

            _coroutinesPerformer.StartPerform(_sceneSwitcherService.ProcessSwitchTo(Scenes.MainMenu, _gameplayInputArgs));
        }

        private void ShowGameInfo() => Debug.Log($"Для победы введите без ошибок {_combination.Value}");
    }
}