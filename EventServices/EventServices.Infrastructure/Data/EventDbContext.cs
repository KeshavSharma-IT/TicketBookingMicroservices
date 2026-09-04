using Microsoft.EntityFrameworkCore;
using EventServices.Domain.Entities;


namespace EventServices.Infrastructure.Data
{
    public class EventDbContext : DbContext
    {
        public EventDbContext(DbContextOptions<EventDbContext> options):base(options)
        {            
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Venue> Venues { get; set; }
        public DbSet<Screen> Screen {  get; set; }
        public DbSet<Show> Shows { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // screen belongs to venue (1-to-many)
            modelBuilder.Entity<Screen>()
                .HasOne(s => s.venue)
                .WithMany(v => v.Screens)
                .HasForeignKey(s => s.VenueId)
                .OnDelete(DeleteBehavior.Cascade);

            // Show belongs to Screen (1-to-many)

            modelBuilder.Entity<Show>()
                .HasOne(s => s.Screen)
                .WithMany()
                .HasForeignKey(s => s.ScreenId)
                .OnDelete(DeleteBehavior.Restrict);

            // Show belongs to Event (1-to-many)
            modelBuilder.Entity<Show>()
                .HasOne(s => s.Event)
                .WithMany()
                .HasForeignKey(s => s.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            
            modelBuilder.Entity<Show>()
                .Property(s => s.Price)
                .HasColumnType("decimal(18,2)"); // Configures 18 total digits, with 2 after the decimal

            // Composite Unique Index for Venue (Name + City + Address)
            modelBuilder.Entity<Venue>()
                .HasIndex(v => new { v.Name, v.City, v.Address })
                .IsUnique();

            // Composite Unique Index for Screen (VenueId + Name)
            modelBuilder.Entity<Screen>()
                .HasIndex(s => new { s.VenueId, s.Name })
                .IsUnique();
        }
    }
}
