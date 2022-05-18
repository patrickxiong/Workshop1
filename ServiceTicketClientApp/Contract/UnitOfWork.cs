namespace Contract
{
    using Model;

    public class UnitOfWork : IUnitOfWork
    {

        private TicketContext _dbContext;
        private BaseRepository<Tickets> _tickets;
        private BaseRepository<Outcomes> _outcomes;
        private BaseRepository<Result> _results;
        private BaseRepository<TicketTypes> _ticketTypes;

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
        public IRepository<Result> Results
        {
            get
            {
                return _results ??
                    (_results = new BaseRepository<Result>(_dbContext));
            }
        }

        public IRepository<TicketTypes> TicketTypes
        {
            get
            {
                return _ticketTypes ?? (_ticketTypes = new BaseRepository<TicketTypes>(_dbContext));
            }
        }

        public void Commit()
        {
            _dbContext.SaveChanges();
        }
    }
}
