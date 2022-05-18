using Model;
using System.Data.Entity;

namespace Contract
{
    public class TicketContext : DbContext
    {
        public TicketContext() : base("Workshop")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Adds configurations for Student from separate class
            //modelBuilder.Configurations.Add(new StudentConfigurations());

            modelBuilder.Entity<Tickets>()
                .ToTable("Tickets");

//            modelBuilder.Entity<Teacher>()
//                .MapToStoredProcedures();
        }

        public DbSet<Tickets> Tickets { get; set; }
    }
}