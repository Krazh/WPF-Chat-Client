using System.Windows;
using SecureTcpWpfClient.ViewModel;

namespace SecureTcpWpfClient.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.DataContext = new MainViewModel();
            InitializeComponent();
            messageBox.Focus();
        }

        public void OnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            messageBox.Focus();
        }
    }
}
