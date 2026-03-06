using Assets._Project.Develop.Runtime.Configs;
using Assets._Project.Develop.Runtime.GamePlay.Features;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Meta.Features.Statistics;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.Utilities.ConfigsManagment;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilities.DataManagment.DataProvider;
using Assets._Project.Develop.Runtime.Utilities.SceneManagment;

namespace Assets._Project.Develop.Runtime.GamePlay.Infrastructure
{
    public class GameplayContextRegistration
    {
        private static GameplayInputArgs _gameplayInputArgs;

        public static void Process(DIContainer container, GameplayInputArgs gameplayInputArgs)
        {
            _gameplayInputArgs = gameplayInputArgs;

            container.RegisterAsSingle(CreateCombinationFactory);
            container.RegisterAsSingle(CreateGameProcess);
        }

        private static CombinationFactory CreateCombinationFactory(DIContainer container)
            => new CombinationFactory(container.Resolve<ConfigsProviderService>().GetConfig<CombinationConfig>());

        private static GameProcess CreateGameProcess(DIContainer container)
        {
            return new GameProcess(
                _gameplayInputArgs,
                container.Resolve<SceneSwitcherService>(),
                container.Resolve<ICoroutinesPerformer>(),
                container.Resolve<CombinationFactory>(),
                container.Resolve<WalletService>(),
                container.Resolve<PlayerDataProvider>(),
                container.Resolve<PlayerStatisticsService>(),
                container.Resolve<ConfigsProviderService>()
                );
        }
    }
}
