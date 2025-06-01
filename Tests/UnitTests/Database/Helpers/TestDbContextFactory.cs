using EventAssociation.Infrastructure.SqlliteDmPersistence.Context;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace UnitTests.Database.Helpers
{
    public static class TestDbContextFactory
    {
        public static VeaDbContext CreateInMemoryContext()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<VeaDbContext>()
                .UseSqlite(connection)
                .Options;

            var context = new VeaDbContext(options);
            context.Database.EnsureCreated();

            return context;
        }
    }
}
