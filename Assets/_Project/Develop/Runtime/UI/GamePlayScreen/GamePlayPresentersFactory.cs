using Assets._Project.Develop.Runtime.GamePlay.Features;
using Assets._Project.Develop.Runtime.Infrastructure.DI;

namespace Assets._Project.Develop.Runtime.UI.GamePlayScreen
{
    public class GamePlayPresentersFactory
    {
        private DIContainer _container;

        public GamePlayPresentersFactory(DIContainer container)
        {
            _container = container;
        }

        public GamePlayScreenPresenter CreateGamePlayScreenPresenter(GamePlayScreenView view)
        {
            return new GamePlayScreenPresenter(_container.Resolve<ProjectPresentersFactory>(), view, _container.Resolve<UserInputHandlerService>());
        }
    }
}
