using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication
{
    public interface ITicketServiceClient
    {
        void Connect(NetworkConfiguration config);
        void Disconnect();
        void Ready();
        TicketMessage GetTicketMessage();
        void CompleteTransaction();
        void RequestBreak();

    }
}
