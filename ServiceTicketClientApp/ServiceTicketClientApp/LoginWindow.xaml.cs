using System.Windows;
using ServiceTicketClientApp.ViewModels;

namespace ServiceTicketClientApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();

            this.DataContext = new LoginWindowViewModel(this);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //this.Hide();
            //var dd = new MainWindow();
            //dd.LoginWindowPage = this;
            //dd.DataContext = new MainWindowViewModel();
            //dd.ShowDialog();
        }
    }
}
