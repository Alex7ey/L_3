using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Meta.Features.Combinations;
using Assets._Project.Develop.Runtime.Meta.Features.Statistics;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.Utilities.ConfigsManagment;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilities.DataManagment.DataProvider;

namespace Assets._Project.Develop.Runtime.Meta.Infrastructure
{
    public class MainMenuContextRegistration
    {
        public static void Process(DIContainer container)
        {
            container.RegisterAsSingle(CreateCombinationSelectorService);
            container.RegisterAsSingle(CreateMetaProcess);
        }

        private static CombinationSelector CreateCombinationSelectorService(DIContainer container) => new CombinationSelector();

        private static MetaProcess CreateMetaProcess(DIContainer container)
        {
            return new MetaProcess(container.Resolve<WalletService>(), container.Resolve<PlayerStatisticsService>(), new CombinationSelector(), container.Resolve<ICoroutinesPerformer>(), container.Resolve<PlayerDataProvider>(), container.Resolve<ConfigsProviderService>());
        }
    }
}
