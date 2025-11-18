using Invoice.Entites;
using Microsoft.EntityFrameworkCore;

namespace Invoice.DataAccess
{
    public class InvoiceDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connString = Environment.GetEnvironmentVariable("ConnectionString");
            Console.WriteLine($"PostgreSqlConnectionString: {connString}");
            optionsBuilder.UseNpgsql(connString);
        }
        public DbSet<InvoiceStatusLog> InvoiceStatusLogs { get; set; }
    }
}
