using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using System;

namespace Voltaire
{
    public class DataBase : DbContext
    {
        public DbSet<Models.Guild> Guilds { get; set; }
        public DbSet<Models.BannedIdentifier> BannedIdentifiers { get; set; }

        public DataBase(DbContextOptions<DataBase> options) : base(options) {}

    }

    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DataBase>
    {
        public DataBase CreateDbContext(string[] args)
        {
            IConfiguration configuration = LoadConfig.Instance.config;

            var optionsBuilder = new DbContextOptionsBuilder<Voltaire.DataBase>();
            optionsBuilder.UseSqlServer($@"{configuration.GetConnectionString("sql")}");

            return new DataBase(optionsBuilder.Options);
        }
    }

}
