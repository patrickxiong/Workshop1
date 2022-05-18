using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication
{
    public interface IServiceClient
    {
        void Connect(NetworkConfiguration config);
        void Disconnect();
        void Ready();
        void CompleteTransaction(int OutcomeCode);
        void RequestBreak();

    }
}
