﻿namespace Contract
{
    using Model;
    using System;
    using System.Collections.Generic;

    public interface IUnitOfWork
    {
        IRepository<Tickets> Tickets { get; }
        IRepository<Outcomes> Outcomes { get; }
        void Commit();
    }
}
