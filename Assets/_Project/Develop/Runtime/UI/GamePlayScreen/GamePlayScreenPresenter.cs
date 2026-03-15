using Assets._Project.Develop.Runtime.GamePlay.Features;
using Assets._Project.Develop.Runtime.UI.CommonView;
using System.Collections.Generic;

namespace Assets._Project.Develop.Runtime.UI.GamePlayScreen
{
    public class GamePlayScreenPresenter : IPresenter
    {
        private ProjectPresentersFactory _projectPresentersFactory;
        private GamePlayScreenView _gamePlayScreenView;
        private UserInputHandlerService _userInputHandlerService;

        private List<IPresenter> _childPresenters = new();

        public GamePlayScreenPresenter(
             ProjectPresentersFactory projectPresentersFactory, 
            GamePlayScreenView gamePlayScreenView, 
            UserInputHandlerService userInputHandlerService)
        {
            _projectPresentersFactory = projectPresentersFactory;
            _gamePlayScreenView = gamePlayScreenView;
            _userInputHandlerService = userInputHandlerService;
        }

        public void Dispose()
        {
            foreach (var presenter in _childPresenters)
                presenter.Dispose();

            _childPresenters.Clear();
        }

        public void Initialize()
        {
            CreateTextExpectedPresenter();
            CreateTextPlayerInputPresenter();
            CreateInformationTextText();

            foreach (var presenter in _childPresenters)
                presenter.Initialize();
        }

        private void CreateTextExpectedPresenter()
        {
            TextFieldPresenter textFieldPresenter =
            _projectPresentersFactory.CreateTextFieldPresenter(_userInputHandlerService.ExpectedChars, _gamePlayScreenView.ExpectedText);

            _childPresenters.Add(textFieldPresenter);
        }

        private void CreateTextPlayerInputPresenter()
        {
            TextFieldPresenter textFieldPresenter =
            _projectPresentersFactory.CreateTextFieldPresenter(_userInputHandlerService.InputPlayer, _gamePlayScreenView.PlayerInputText);

            _childPresenters.Add(textFieldPresenter);
        }

        private void CreateInformationTextText()
        {
            TextFieldPresenter textFieldPresenter =
            _projectPresentersFactory.CreateTextFieldPresenter(_userInputHandlerService.InformationTexts, _gamePlayScreenView.InformationText);

            _childPresenters.Add(textFieldPresenter);
        }
    }
}
