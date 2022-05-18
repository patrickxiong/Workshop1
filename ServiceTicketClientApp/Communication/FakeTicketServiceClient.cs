using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication
{
    public class FakeTicketServiceClient : ITicketServiceClient
    {
        public event NewTicketEvent NewTicketEventHandler;
        public event TicketServiceBreakEvent TicketServiceBreakEventHandler;
        public event TicketServiceTransactionCompleteEvent TicketServiceTransactionCompleteEventHandler;

        public void CompleteTransaction(int outcomeCode)
        {
            
        }

        public void Connect(NetworkConfiguration config)
        {
            
        }

        public void Disconnect()
        {
            
        }

        public TicketMessage GetTicketMessage()
        {
            return new TicketMessage { TicketId = "777", CampaignName = "Test", TicketType = "NEW", UserId = "User1" };
        }

        public void Ready()
        {
        }

        public void RequestBreak()
        {
        }
    }
}
