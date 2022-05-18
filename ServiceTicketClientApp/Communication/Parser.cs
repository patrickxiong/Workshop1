using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication
{
    public class Parser
    {
        public static int AENumber = 7687342;

        public static TicketMessage GetTicket(string message)
        {
            TicketMessage ticket = null;

            if (message.Substring(0, 2).Trim().Equals("AC"))

            {
                ticket = new TicketMessage();
            }

            var fields = message.Split('\\');

            ticket.CampaignName = fields.First(m => m.Substring(0, 2).Trim().Equals("CN")).Substring(2);

            var dataFields = fields.First(f => f.Substring(0, 2).Trim().Equals("DT")).Substring(2).Split('|');

            foreach (var df in dataFields)
            {
                var dataItems = df.Split('~');
                if (dataItems.Length >= 3 && dataItems[0].Trim().Equals("Ticket_ID"))
                {
                    ticket.TicketId = dataItems[2];
                }

                if (dataItems.Length >= 3 && dataItems[0].Trim().Equals("TicketTypeCode"))
                {
                    ticket.TicketType = dataItems[2];
                }

                if (dataItems.Length >= 3 && dataItems[0].Trim().Equals("Contact_GUID"))
                {
                    ticket.UserId = dataItems[2];
                }
            }

            return ticket;
        }

        public static string GetValidateUserCommand(string user)
        {
            return $"UA\\AN{user}\\TDdefault";
        }

        public static bool UserExists(string message)
        {
            var fields = message.Split('\\');

            return fields.First(f => f.Substring(0, 2).Trim().Equals("PG"))[2] == '1';

        }

        public static string GetLoginCommand(string user)
        {
            return $"AL\\AN{user}\\CNTickets\\AD{user}\\AE{AENumber++}\\NU";
        }

        public static bool LoginSuccessful(string message)
        {
            return (message.Substring(0, 2).Trim().Equals("LI")) ;

        }

        public static string GetReadyCommand(string user)
        {
            return $"AA\\AN{user}\\CNTickets";
        }

        public static bool IsReadySuccessful(string message)
        {

            return (message.Substring(0, 2).Trim().Equals("NA"));

        }

        public static bool IsUserRecongnizedReady(string message)
        {

            return (message.Substring(0, 2).Trim().Equals("AR"));

        }

        public static string GetTransactionCompleteCommand(string user, int outcome)
        {
            return $"TC\\AN{user}\\CNTickets\\AO{outcome}\\BT36892";
        }

        public static bool TransactionCompleted(string message)
        {

            return (message.Substring(0, 2).Trim().Equals("CE"));

        }

        public static string GetBreakRequestCommand(string user)
        {
            return $"AU\\AN{user}\\CNTickets\\PR0\\TDdefault";
        }

        public static bool BreakGranted(string message)
        {

            return (message.Substring(0, 2).Trim().Equals("AF"));

        }
    }
}