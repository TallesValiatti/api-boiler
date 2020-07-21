using Api.Web.Configuration.HealthCheck;
using HealthChecks.UI.Client;
using HealthChecks.UI.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Net.NetworkInformation;

namespace Api.Web.Configuration
{
    public static class HealthCheckSetup
    {
        // Register the Swagger generator, defining 1 or more Swagger documents
        public static void AddHealthCheck(this IServiceCollection services, string connectionString, IConfiguration config)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            var failureMicroServiceUrl = config.GetSection("Settings:MicroServices:FailureMicroservice:Url").Value;

            services.AddHealthChecks()
                .AddSqlServer(connectionString, name: "BaseSql")
                .AddCheck("Failure-MicroService", new PingHealthCheck(failureMicroServiceUrl, 100))
                .AddCheck("ping2", new PingHealthCheck("www.bing.com", 100));

            services.AddHealthChecksUI(setupSettings: setup =>
            {
                setup.AddHealthCheckEndpoint("Basic healthcheck", "https://localhost:44323/health");
            })
            .AddSqlServerStorage(connectionString);
        }

        public static void UseHealthCheck(this IApplicationBuilder app)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));

            app.UseHealthChecks("/health", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseHealthChecksUI(opt => opt.UIPath = "/Health-UI");
        }
    }
}
