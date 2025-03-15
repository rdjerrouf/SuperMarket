using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SuperMarket.DataAccess.Data
{
    /// <summary>
    /// Factory for creating AppDbContext instances during design time (e.g., for migrations).
    /// </summary>
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            // Configure the database context to use SQLite
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlite("Data Source=market.db"); // SQLite database file

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}