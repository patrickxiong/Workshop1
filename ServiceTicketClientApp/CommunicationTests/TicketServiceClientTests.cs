using Microsoft.VisualStudio.TestTools.UnitTesting;
using Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication.Tests
{
    [TestClass()]
    public class TicketServiceClientTests
    {
        [TestMethod()]
        public void ReadyTest()
        {
            var target=TicketServiceClient.Instance;

            target.Ready();

        }
    }
}