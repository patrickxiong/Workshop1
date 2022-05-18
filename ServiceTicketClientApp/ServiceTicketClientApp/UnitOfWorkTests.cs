using System;
using System.Collections.Generic;
using Model;

namespace Contract.Tests
{
    public class UnitOfWorkTests2
    {
        public Tickets GetTicketByID()
        {
            var target = new UnitOfWork(new TicketContext());

            var t = target.Tickets.GetByID(777);
            return t;
        }

        public IEnumerable<TicketTypes> GetTicketType()
        {
            var target = new UnitOfWork(new  TicketContext());

            var t = target.TicketTypes.Get();
            return t;
        }

        public void GetOutComesTest()
        {
            var target = new UnitOfWork(new TicketContext());

            var o = target.Outcomes.Get(null, null);

        }

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