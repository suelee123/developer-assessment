using DataExporter.Model;
using Microsoft.EntityFrameworkCore;


namespace DataExporter
{
    public class ExporterDbContext : DbContext
    {
        public DbSet<Policy> Policies { get; set; }     //Policies entity
        public DbSet<Note> Notes { get; set; }          //Notes entity 


        public ExporterDbContext(DbContextOptions<ExporterDbContext> options) : base(options)
        { 
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseInMemoryDatabase("ExporterDb");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Policy>().HasData(new Policy() { Id = 1, PolicyNumber = "HSCX1001", Premium = 200, StartDate = new DateTime(2024, 4, 1) },
                new Policy() { Id = 2, PolicyNumber = "HSCX1002", Premium = 153, StartDate = new DateTime(2024, 4, 5) },
                new Policy() { Id = 3, PolicyNumber = "HSCX1003", Premium = 220, StartDate = new DateTime(2024, 3, 10) },
                new Policy() { Id = 4, PolicyNumber = "HSCX1004", Premium = 200, StartDate = new DateTime(2024, 5, 1) },
                new Policy() { Id = 5, PolicyNumber = "HSCX1005", Premium = 100, StartDate = new DateTime(2024, 4, 1) });

            // Configure relationship 1-to-many Policy → Notes
            modelBuilder.Entity<Policy>()
                .HasMany(p => p.Notes)
                .WithOne(n => n.Policy)
                .HasForeignKey(n => n.PolicyId)
                .OnDelete(DeleteBehavior.Cascade);


            // Seed Notes
            modelBuilder.Entity<Note>().HasData(
                new Note { Id = 1, Text = "Initial quotation completed.", PolicyId = 1 },
                new Note { Id = 2, Text = "Customer requested policy update.", PolicyId = 1 },
                new Note { Id = 3, Text = "Premium adjustment discussed.", PolicyId = 2 },
                new Note { Id = 4, Text = "Awaiting documents from client.", PolicyId = 3 }
                );


            base.OnModelCreating(modelBuilder);
        }
    }
}
