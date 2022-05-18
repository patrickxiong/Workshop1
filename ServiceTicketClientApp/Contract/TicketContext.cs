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
                .HasKey(t=>t.Ticket_ID)
                .ToTable("Tickets");

            modelBuilder.Entity<Contacts>()
                .HasKey(c => c.Contact_GUID)
                .ToTable("Contacts");

            modelBuilder.Entity<TicketTypes>()
                .HasKey(t => t.TicketTypeCode)
                .ToTable("TicketTypes");

            modelBuilder.Entity<Departments>()
                .HasKey(d => d.Department_ID)
                .ToTable("Departments");

            modelBuilder.Entity<Outcomes>()
                .HasKey(o=>o.OutcomeCode)
                .ToTable("Outcomes");
            //            modelBuilder.Entity<Teacher>()
            //                .MapToStoredProcedures();
        }

        public DbSet<Tickets> Tickets { get; set; }
        public DbSet<Outcomes> Outcomes { get; set; }
    }
}