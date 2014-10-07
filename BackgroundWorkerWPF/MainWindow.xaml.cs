using System.Windows;

namespace BackgroundWorkerWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            // Initialize component
            InitializeComponent();

            // Add MainWindowViewModel as the DataContext
            MainWindowViewModel viewModel = new MainWindowViewModel();
            base.DataContext = viewModel;

        }
    }
}
