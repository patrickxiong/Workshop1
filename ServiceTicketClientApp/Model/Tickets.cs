using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Tickets
    {
        public int Ticket_ID { get; set; }  
        public string Contact_GUID { get; set; }
        public virtual Contacts Contact { get; set; }
        public string TicketTypeCode { get; set; }
        public virtual TicketTypes ticketType { get; set; }

    }
}
