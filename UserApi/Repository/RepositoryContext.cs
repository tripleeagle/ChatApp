using System;
using Microsoft.EntityFrameworkCore;
using Polly;
using UserApi.Models;

namespace UserApi.Repository
{
    public class RepositoryContext: DbContext
    {
        public DbSet<User> User { get; set; }

        public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().HasKey(m => m.Email);
            builder.Entity<User>().ToTable("User");
            base.OnModelCreating(builder);
        }

        public void MigrateDB()
        {
             Policy
                .Handle<Exception>()
                .WaitAndRetry(10, r => TimeSpan.FromSeconds(10))
                .Execute(() => Database.Migrate());
        }
    }
}