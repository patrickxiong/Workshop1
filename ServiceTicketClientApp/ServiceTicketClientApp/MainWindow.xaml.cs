using System.Windows;
using ServiceTicketClientApp.ViewModels;


namespace ServiceTicketClientApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private UnitOfWorkTests2 UnitOfWorkTests = new UnitOfWorkTests2();
        public LoginWindow LoginWindowPage;

        public MainWindow()
        {
            InitializeComponent();
            //this.Loaded += this.MainWindow_Loaded;
        }

        //public List<TicketTypes> TicketTypes { get; set; }

        //private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        //{
        //    TicketTypes = UnitOfWorkTests.GetTicketType().ToList();

        //    var ticket = UnitOfWorkTests.GetTicketByID();
        //    this.DataContext = new TicketViewModel
        //    {
        //        TicketTypeCode = ticket.TicketTypeCode,
        //        Ticket_ID = ticket.Ticket_ID,
        //        TicketType = TicketTypes.FirstOrDefault(t=>t.TicketTypeCode == ticket.TicketTypeCode)
        //    };
        //}
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            LoginWindowPage?.Show();
        }
    }
}
