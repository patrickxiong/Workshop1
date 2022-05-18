using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Result
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Result_ID { get; set; }
        public int Ticket_ID { get; set; }
        public string TicketTypeCode { get; set; }
        public string Contact_GUID { get; set; }
        public int OutcomeCode { get; set; }
        public string InsertingUserID { get; set; }
        public string InsertingCompaign { get; set; }
        public DateTime DateTimeStamp { get; set; }
    }
}
