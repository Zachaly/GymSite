using GymSite.Database;
using Microsoft.EntityFrameworkCore;
using System;

namespace GymSite.Tests.Unit.Repository
{
    public abstract class DatabaseTest
    {
        protected ApplicationDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            return new ApplicationDbContext(options);
        }
    }

    public static class DbExtensions
    {
        public static Task AddContent<T>(this ApplicationDbContext context, List<T> content) where T : class
        {
            context.AddRangeAsync(content.ToList<object>());
            return context.SaveChangesAsync();
        }
    }
}
