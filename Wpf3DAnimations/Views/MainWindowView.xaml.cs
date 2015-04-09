namespace Wpf3DAnimations.Views
{
    public partial class MainWindowView
    {
        public MainWindowView()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel(Dispatcher);
        }
    }
}
