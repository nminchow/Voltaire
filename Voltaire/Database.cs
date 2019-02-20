using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using Voltaire.Models;

namespace Voltaire
{
    public class DataBase : DbContext
    {
        public DbSet<Models.Guild> Guilds { get; set; }
        public DbSet<Models.BannedIdentifier> BannedIdentifiers { get; set; }

        private string _connection;

        public DataBase(string connection) : base()
        {
            _connection = connection;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer($@"{_connection}");
        }
    }

    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DataBase>
    {
        public DataBase CreateDbContext(string[] args)
        {
            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            var configuration = builder.Build();
            return new DataBase(configuration.GetConnectionString("sql"));
        }
    }

}
