using Microsoft.VisualStudio.TestTools.UnitTesting;
using Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Communication.Tests
{
    [TestClass()]
    public class ServiceClientTests
    {
        [TestMethod()]
        public void ConnectTest()
        {
            var target = ServiceClient.Instance;

            target.Connect(new NetworkConfiguration { Server= "172.25.12.79", Port=6502, User= "user1001" });
            Thread.Sleep(10000);
            target.Ready();

            Thread.Sleep(10000);

            target.RequestBreak();

            Console.ReadLine();
        }
    }
}