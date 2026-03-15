using Assets._Project.Develop.Runtime.Configs;
using Assets._Project.Develop.Runtime.Meta.Features.Statistics;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.UI.UIRoot;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilities.DataManagment.DataProvider;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.UI.Popups
{
    public class ResetStatisticsPopupPresenter : PopupPresenterBase, IPresenter
    {
        private WalletService _walletService;
        private PlayerStatisticsService _playerStatisticsService;
        private RewardsAndCostsConfig _rewardAndCostsConfig;
        private CurrencyIconsConfig _currencyIconsConfig;
        private PlayerDataProvider _playerDataProvider;
        private ResetStatisticsPopupView _view;
  
        protected override PopupViewBase PopupView => _view;

        public ResetStatisticsPopupPresenter(
            WalletService walletService,
            PlayerStatisticsService playerStatisticsService,
            RewardsAndCostsConfig rewardAndCostsConfig,
            PlayerDataProvider playerDataProvider,
            ICoroutinesPerformer coroutinesPerformer,
            ResetStatisticsPopupView view,
            CurrencyIconsConfig currencyIconsConfig) : base(coroutinesPerformer)
        {
            _walletService = walletService;
            _playerStatisticsService = playerStatisticsService;
            _rewardAndCostsConfig = rewardAndCostsConfig;
            _playerDataProvider = playerDataProvider;
            _view = view;
            _currencyIconsConfig = currencyIconsConfig;
        }

        public override void Initialize()
        {
            _view.SetIconTextView(_rewardAndCostsConfig.ResetCost, _currencyIconsConfig.GetSpriteFor(_rewardAndCostsConfig.Currency));

            _view.Button.onClick.AddListener(OnClick);
        }

        public override void Dispose()
        {
            base.Dispose();

            _view.Button.onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            if (_walletService.Enough(_rewardAndCostsConfig.Currency, _rewardAndCostsConfig.ResetCost))
            {
                _walletService.Spend(_rewardAndCostsConfig.Currency, _rewardAndCostsConfig.ResetCost);
                _playerStatisticsService.Reset();
                _playerDataProvider.Save();

                // not save!!! xz
            }
        }    
    }
}
