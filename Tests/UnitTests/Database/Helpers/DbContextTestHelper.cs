using Microsoft.EntityFrameworkCore;

namespace UnitTests.Database.Helpers
{
    public static class DbContextTestHelper
    {
        public static async Task SaveAndClearAsync(DbContext context)
        {
            await context.SaveChangesAsync();
            context.ChangeTracker.Clear();
        }
    }
}
