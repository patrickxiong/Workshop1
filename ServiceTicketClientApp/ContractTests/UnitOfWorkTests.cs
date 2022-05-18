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

            var t = target.Tickets.GetByID(777);

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
            var target = new UnitOfWork(new TicketContext());

            var r = new Model.Result
            {
                Ticket_ID = 777,
                Contact_GUID = "DA3BFC7DFC3748448819AE46505D97F1,",
                InsertingCompaign = "test compagin",
                InsertingUserID = "user1",
                OutcomeCode = 102,
                TicketTypeCode = "NEW",
                DateTimeStamp = DateTime.Now
            };

            target.Results.Insert(r);
            target.Commit();
        }
    }
}