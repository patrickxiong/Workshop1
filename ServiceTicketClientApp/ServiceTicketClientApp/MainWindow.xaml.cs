using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Contract.Tests;
using Model;
using ServiceTicketClientApp.ViewModels;

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

        public List<TicketTypes> TicketTypes { get; set; }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            TicketTypes = UnitOfWorkTests.GetTicketType().ToList();

            var ticket = UnitOfWorkTests.GetTicketByID();
            this.DataContext = new TicketViewModel
            {
                TicketTypeCode = ticket.TicketTypeCode,
                Ticket_ID = ticket.Ticket_ID,
                TicketType = TicketTypes.FirstOrDefault(t=>t.TicketTypeCode == ticket.TicketTypeCode)
            };
        }
    }
}
