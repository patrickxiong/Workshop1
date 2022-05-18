using System.Windows;
using Contract.Tests;

namespace ServiceTicketClientApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private UnitOfWorkTests2 UnitOfWorkTests = new UnitOfWorkTests2();

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += this.MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var ticket = UnitOfWorkTests.GetTicketByID();
            this.DataContext = ticket;
        }
    }
}
