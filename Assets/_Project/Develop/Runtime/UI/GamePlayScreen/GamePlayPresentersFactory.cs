using Assets._Project.Develop.Runtime.GamePlay.Features;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.UI.TextOutput;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using TMPro;

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
            return new GamePlayScreenPresenter(this, view, _container.Resolve<UserInputHandlerService>());
        }

        public TextFieldPresenter CreateTextFieldPresenter(IReadOnlyVariable<string> text, TextMeshProUGUI view) => new TextFieldPresenter(text, view);    
    }
}
