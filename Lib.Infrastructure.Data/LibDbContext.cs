using Lib.Domain.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Lib.Infrastructure.Data
{
    public class LibDbContext : DbContext
    {
        public LibDbContext(DbContextOptions<LibDbContext> options)
            : base(options)
        {

        }

        public LibDbContext()            
        {
            Database.EnsureCreated();
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<History> Histories { get; set; }
        public DbSet<Reader> Readers { get; set; }
        public DbSet<ReaderProfile> ReaderProfiles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.\\sqlexpress;Database=LibDbContext;Trusted_Connection=True;");

        }
    }
}
