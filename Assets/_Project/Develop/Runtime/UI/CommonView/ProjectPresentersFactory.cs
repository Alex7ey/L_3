using Assets._Project.Develop.Runtime.Configs;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Meta.Features.Statistics;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.UI.CommonView;
using Assets._Project.Develop.Runtime.UI.Popups;
using Assets._Project.Develop.Runtime.UI.UIRoot;
using Assets._Project.Develop.Runtime.Utilities.ConfigsManagment;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilities.DataManagment.DataProvider;
using Assets._Project.Develop.Runtime.Utilities.Reactive;

namespace Assets._Project.Develop.Runtime.UI
{
    public class ProjectPresentersFactory
    {
        private readonly DIContainer _container;

        public ProjectPresentersFactory(DIContainer container) => _container = container;

        public WalletPresenter CreateWalletPresenter(IconTextListView iconTextListView)
        {
            return new WalletPresenter(iconTextListView, _container.Resolve<ViewsFactory>(), _container.Resolve<WalletService>(), this);
        }

        public StatisticsPresenter CreateStatisticsPresenter(IconTextListView iconTextListView)
        {
            return new StatisticsPresenter(iconTextListView, _container.Resolve<ViewsFactory>(), _container.Resolve<PlayerStatisticsService>(), this);
        }

        public CurrencyPresenter CreateCurrencyPresenter(IReadOnlyVariable<int> currency, CurrencyTypes currencyType, IconTextView view)
        {
            return new CurrencyPresenter(currency, currencyType, _container.Resolve<ConfigsProviderService>().GetConfig<CurrencyIconsConfig>(), view);
        }

        public StatisticsItemPresenter CreateStatisticsItemPresenter(IReadOnlyVariable<int> value, StatisticsItemTypes statisticsType, IconTextView view)
        {
            return new StatisticsItemPresenter(value, statisticsType, _container.Resolve<ConfigsProviderService>().GetConfig<StatisticsIconsConfig>(), view);
        }

        public ResetStatisticsPopupPresenter CreateResetStatisticsButtonPresenter(ResetStatisticsPopupView view)
        {
            return new ResetStatisticsPopupPresenter(
                _container.Resolve<WalletService>(), 
                _container.Resolve<PlayerStatisticsService>(), 
                _container.Resolve<ConfigsProviderService>().GetConfig<RewardsAndCostsConfig>(),               
                _container.Resolve <PlayerDataProvider>(),
                _container.Resolve<ICoroutinesPerformer>(),
                view,
                _container.Resolve<ConfigsProviderService>().GetConfig<CurrencyIconsConfig>())
            ;
        }
    }
}
