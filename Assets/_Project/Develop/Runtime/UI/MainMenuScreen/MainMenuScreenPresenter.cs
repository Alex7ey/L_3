using Assets._Project.Develop.Runtime.UI.Popups;
using System.Collections.Generic;


namespace Assets._Project.Develop.Runtime.UI
{
    public class MainMenuScreenPresenter : IPresenter
    {
        private ProjectPresentersFactory _projectPresentersFactory;
        private MainMenuScreenView _mainMenuScreenView;
        private PopupService _popupService;

        private List<IPresenter> _childPresenters = new();

        public MainMenuScreenPresenter(ProjectPresentersFactory projectPresentersFactory, MainMenuScreenView mainMenuScreenView, PopupService popupService)
        {
            _projectPresentersFactory = projectPresentersFactory;
            _mainMenuScreenView = mainMenuScreenView;
            _popupService = popupService;
        }

        public void Initialize()
        {
            CreateWallet();
            CreateStats();

            _mainMenuScreenView.RestartStatisticsButtonClicked += OnOpenLevelsMenuButtonClicked;

            foreach (var presenter in _childPresenters)
                presenter.Initialize();
        }

        public void Dispose()
        {
            _mainMenuScreenView.RestartStatisticsButtonClicked -= OnOpenLevelsMenuButtonClicked;

            foreach (var presenter in _childPresenters)
                presenter.Dispose();

            _childPresenters.Clear();
        }

        private void CreateWallet()
        {
            WalletPresenter walletPresenter = _projectPresentersFactory.CreateWalletPresenter(_mainMenuScreenView.WalletView);

            _childPresenters.Add(walletPresenter);
        }

        private void CreateStats()
        {
            StatisticsPresenter statsPresenter = _projectPresentersFactory.CreateStatisticsPresenter(_mainMenuScreenView.StatisticsView);

            _childPresenters.Add(statsPresenter);
        }

        private void OnOpenLevelsMenuButtonClicked()
        {
            _popupService.OpenResetStatisticsButton();
        }
    }
}
