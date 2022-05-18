﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication
{
    public class Parser
    {
        public static TicketMessage GetTicket(string message)
        {
            TicketMessage ticket = null;

            if (message.Take(2).Equals("AC"))
            {
                ticket = new TicketMessage();
            }

            var fields = message.Split('\\');
            ticket.CampaignName = fields.First(m => m.Take(2).Equals("CN")).Substring(2);

            var dataFields = fields.First(f => f.Take(2).Equals("DT")).Substring(2).Split('|');
            foreach (var df in dataFields)
            {
                var dataItems = df.Split('~');
                if (dataItems.Length >= 3 && dataItems[0].Equals("Ticket_ID"))
                {
                    ticket.TicketId = dataItems[2];
                }

                if (dataItems.Length >= 3 && dataItems[0].Equals("TicketTypeCode"))
                {
                    ticket.TicketType = dataItems[2];
                }

                if (dataItems.Length >= 3 && dataItems[0].Equals("Contact_GUID"))
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
            return fields.First(f => f.Take(2).Equals("PG"))[2] == 1;
        }

        public static string GetLoginCommand(string user)
        {
            return $"AL\\AN{user}\\CNTickets\\ADuser1\\AE1234\\NU\\TDdefault";
        }

        public static bool LoginSuccessful(string message)
        {
            return (message.Take(2).Equals("LI")) ;
        }

        public static string GetReadyCommand(string user)
        {
            return $"AA\\AN{user}\\CNTickets";
        }

        public static bool IsReadySuccessful(string message)
        {
            return (message.Take(2).Equals("NA"));
        }

        public static bool IsUserRecongnizedReady(string message)
        {
            return (message.Take(2).Equals("AR"));
        }

        public static string GetTransactionCompleteCommand(string user, int outcome)
        {
            return $"TC\\AN{user}\\CNTickets\\AO{outcome}\\BT36892";
        }

        public static bool TransactionCompleted(string message)
        {
            return (message.Take(2).Equals("CE"));
        }

        public static string GetBreakRequestCommand(string user)
        {
            return $"AU\\AN{user}\\CNTickets\\PR0\\TDdefault";
        }

        public static bool BreakGranted(string message)
        {
            return (message.Take(2).Equals("AF"));
        }
    }
}