namespace Wpf3DAnimations
{
    using System.Windows;
    using Wpf3DAnimations.Views;

    public partial class App
    {
        private MainWindowViewModel _mainViewModel;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainView = new MainWindowView();
            _mainViewModel = new MainWindowViewModel(mainView.Dispatcher);
            mainView.DataContext = _mainViewModel;
            mainView.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _mainViewModel.Dispose();
            base.OnExit(e);
        }
    }
}
