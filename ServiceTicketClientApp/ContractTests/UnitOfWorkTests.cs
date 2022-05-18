using Microsoft.VisualStudio.TestTools.UnitTesting;
using Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Tests
{
    [TestClass()]
    public class UnitOfWorkTests
    {
        [TestMethod()]
        public void UnitOfWorkTest()
        {
            var target = new UnitOfWork(new TicketContext());

            var t=target.Tickets.GetByID(777);

        }
        [TestMethod()]
        public void GetOutComesTest()
        {
            var target = new UnitOfWork(new TicketContext());

            var o = target.Outcomes.Get(null, null);

        }

        [TestMethod()]
        public void CommitTest()
        {
            Assert.Fail();
        }
    }
}