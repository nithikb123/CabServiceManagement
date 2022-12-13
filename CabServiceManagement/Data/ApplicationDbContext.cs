using CabServiceManagement.Models;

namespace CabServiceManagement.Data
{
    public class ApplicationDbContext:IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) 
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Booking>()
                .HasOne(m => m.User)
                .WithMany(m => m.BookingCabs)
                .OnDelete(DeleteBehavior.NoAction);
        }
        public DbSet<ApplicationUser> User { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<DriverDetails> DriverDetail { get; set; }
    }
}
