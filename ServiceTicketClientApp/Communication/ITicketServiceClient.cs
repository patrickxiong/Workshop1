using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication
{
    public interface ITicketServiceClient
    {
        event NewTicketEvent NewTicketEventHandler;
        event TicketServiceBreakEvent TicketServiceBreakEventHandler; 
        event TicketServiceTransactionCompleteEvent TicketServiceTransactionCompleteEventHandler;

        void Connect(NetworkConfiguration config);
        void Disconnect();
        void Ready();
        
        void CompleteTransaction(int outcomeCode);
        void RequestBreak();

    }
}
