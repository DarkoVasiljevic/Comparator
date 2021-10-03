using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace Comparator.IntegrationTests
{
    public class TestFactory : WebApplicationFactory<Startup>
    {
        public IConfiguration Configuration { get; private set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {

            builder.ConfigureAppConfiguration(config =>
            {
                Configuration = new ConfigurationBuilder().Build();

                config.AddConfiguration(Configuration);
            });
        }
    }
}
