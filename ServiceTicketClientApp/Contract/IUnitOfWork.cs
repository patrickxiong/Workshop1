namespace Contract
{
    using Model;
    using System;
    using System.Collections.Generic;

    public interface IUnitOfWork
    {
        IRepository<Tickets> Tickets { get; }
        //IRepository<Teacher> Teachers { get; }
        void Commit();
    }
}
