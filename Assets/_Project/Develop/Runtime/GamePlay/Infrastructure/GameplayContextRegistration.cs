using Assets._Project.Develop.Runtime.Configs;
using Assets._Project.Develop.Runtime.GamePlay.Features;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Meta.Features.Statistics;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.UI;
using Assets._Project.Develop.Runtime.UI.GamePlayScreen;
using Assets._Project.Develop.Runtime.UI.UIRoot;
using Assets._Project.Develop.Runtime.Utilities.AssetsLoader;
using Assets._Project.Develop.Runtime.Utilities.ConfigsManagment;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilities.DataManagment.DataProvider;
using Assets._Project.Develop.Runtime.Utilities.SceneManagment;
using UnityEngine;

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
            container.RegisterAsSingle(CreateUserInputHandlerService);
            container.RegisterAsSingle(CreateGamePlayPresentersFactory);

            container.RegisterAsSingle(CreateGamePlayUIRoot).NonLazy();
            container.RegisterAsSingle(CreateGamePlayScreenPresenter).NonLazy();
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
                container.Resolve<ConfigsProviderService>(),
                container.Resolve<UserInputHandlerService>()
                );
        }

        private static GamePlayUIRoot CreateGamePlayUIRoot(DIContainer container)
        {
            ResourcesAssetsLoader resourcesAssetsLoader = container.Resolve<ResourcesAssetsLoader>();

            GamePlayUIRoot gamePlayUIRootPrefab = resourcesAssetsLoader
                .Load<GamePlayUIRoot>("UI/GamePlay/GamePlayUIRoot");

            return Object.Instantiate(gamePlayUIRootPrefab);
        }

        private static GamePlayScreenPresenter CreateGamePlayScreenPresenter(DIContainer container)
        {
            GamePlayUIRoot uiRoot = container.Resolve<GamePlayUIRoot>();

            GamePlayScreenView view = container
                .Resolve<ViewsFactory>()
                .Create<GamePlayScreenView>(ViewIDs.GamePlayScreen, uiRoot.HUDLayer);

            GamePlayScreenPresenter presenter = container
                .Resolve<GamePlayPresentersFactory>()
                .CreateGamePlayScreenPresenter(view);

            return presenter;
        }

        private static GamePlayPresentersFactory CreateGamePlayPresentersFactory(DIContainer container) => new(container);

        private static UserInputHandlerService CreateUserInputHandlerService(DIContainer container) => new();
    }
}
