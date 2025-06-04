using EventAssociation.Infrastructure.SqlliteDmPersistence.Context;
using EventAssociation.Infrastructure.SqlliteDmPersistence.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace UnitTests.QueryTests
{
    public static class TestContextFactory
    {
        private static SqliteConnection? _keepAliveConnection;

        // Call this once per test class (or before each test) to open the in‐memory connection.
        public static void OpenInMemoryConnection()
        {

            if (_keepAliveConnection != null)
            {
                CloseInMemoryConnection();
            }

            _keepAliveConnection = new SqliteConnection("DataSource=:memory:");
            _keepAliveConnection.Open();
        }

        public static VeaDbContext CreateWriteContext()
        {
            var options = new DbContextOptionsBuilder<VeaDbContext>()
                .UseSqlite(_keepAliveConnection)
                .Options;

            var writeCtx = new VeaDbContext(options);
            writeCtx.Database.EnsureCreated();
            return writeCtx;
        }

        public static VeadatabaseProductionContext CreateReadContext()
        {
            var options = new DbContextOptionsBuilder<VeadatabaseProductionContext>()
                .UseSqlite(_keepAliveConnection)
                .Options;

            var readCtx = new VeadatabaseProductionContext(options);
            return readCtx;
        }

        // Call this when you’re done with all tests to close connection:
        public static void CloseInMemoryConnection()
        {
            _keepAliveConnection?.Close();
            _keepAliveConnection = null;
        }
    }
}
