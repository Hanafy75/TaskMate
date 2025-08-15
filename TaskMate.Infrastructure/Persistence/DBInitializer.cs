using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TaskMate.Infrastructure.Persistence
{
    public class DBInitializer
    {
        private readonly ILogger<DBInitializer> _logger;

        public DBInitializer(ILogger<DBInitializer> logger)
        {
            _logger = logger;
        }

        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService<AppDbContext>();

            if ((await context.Database.GetPendingMigrationsAsync()).Count() > 0)
                await context.Database.MigrateAsync();
        }
    }
}
