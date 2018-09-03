using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Voltaire
{
    public sealed class LoadConfig
    {

        private static readonly Lazy<LoadConfig> lazy =
            new Lazy<LoadConfig>(() => new LoadConfig());

        public static LoadConfig Instance { get { return lazy.Value; } }

        public IConfiguration config;

        private LoadConfig()
        {
            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            config = builder.Build();
        }
    }
}
