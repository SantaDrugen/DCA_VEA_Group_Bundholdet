using EfcQueries.Models;
using EventAssociation.Infrastructure.SqlliteDmPersistence.Context;
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
