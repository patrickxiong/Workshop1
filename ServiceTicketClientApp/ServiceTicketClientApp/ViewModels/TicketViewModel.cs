using System.Collections.Generic;
using System.ComponentModel;
using Model;

namespace ServiceTicketClientApp.ViewModels
{
    public class TicketViewModel : BaseViewModel
    {
        public int Ticket_ID { get; set; }

        public string Contact_GUID { get; set; }

        public virtual Contacts Contact { get; set; }

        public string TicketTypeCode { get; set; }

        public TicketTypes TicketType { get; set; }

        public List<TicketTypes> TicketTypes { get; set; }
    }

    public class BaseViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
