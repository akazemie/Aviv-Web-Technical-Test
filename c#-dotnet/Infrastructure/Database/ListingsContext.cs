using listingapi.Infrastructure.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace listingapi.Infrastructure.Database
{
    public class ListingsContext : DbContext
    {
        public ListingsContext(DbContextOptions<ListingsContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public virtual DbSet<Listing> Listings { get; set; }
        public virtual DbSet<ListingPriceHistory> ListingPriceHistories { get; set; }
    }
}
