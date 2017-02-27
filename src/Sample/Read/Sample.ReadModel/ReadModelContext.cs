using Microsoft.EntityFrameworkCore;
using Sample.ReadModel.DataModel;

namespace Sample.ReadModel
{
    internal class ReadModelContext : DbContext
    {
        public ReadModelContext(DbContextOptions<ReadModelContext> options) : base(options)
        {
        }

        public DbSet<AccountDetails> AccountDetails { get; set; }
        public DbSet<SystemTag> SystemTags { get; set; }
    }
}