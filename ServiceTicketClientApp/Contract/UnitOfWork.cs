﻿namespace Contract
{
    using Model;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class UnitOfWork : IUnitOfWork
    {

        private TicketContext _dbContext;
        private BaseRepository<Tickets> _tickets;
        private BaseRepository<Outcomes> _outcomes;

        public UnitOfWork(TicketContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IRepository<Tickets> Tickets
        {
            get
            {
                return _tickets ??
                    (_tickets = new BaseRepository<Tickets>(_dbContext));
            }
        }

        public IRepository<Outcomes> Outcomes
        {
            get
            {
                return _outcomes ??
                    (_outcomes = new BaseRepository<Outcomes>(_dbContext));
            }
        }
        public void Commit()
        {
            _dbContext.SaveChanges();
        }
    }
}
