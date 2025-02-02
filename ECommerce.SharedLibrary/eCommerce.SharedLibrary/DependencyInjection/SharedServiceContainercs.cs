﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCommerce.SharedLibrary.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace eCommerce.SharedLibrary.DependencyInjection
{
    public static class SharedServiceContainercs
    {
        public static IServiceCollection AddSharedService<TContext>
            (this IServiceCollection services, IConfiguration config, string filename) where TContext : DbContext
        {
            services.AddDbContext<TContext>(options => options.UseSqlServer(
                config
                .GetConnectionString("eCommerceConnection"),sqlServerOptions =>
                sqlServerOptions.EnableRetryOnFailure()));

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel
                .Information()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.File(path: $"{filename}-.text",
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {message:Lj}{NewLine}{Exception}",
                rollingInterval: RollingInterval.Day)
                .CreateLogger();

            JWTAuthenticationScheme.AddAuthenticationScheme(services, config);
            return services;
        }

        public static IApplicationBuilder UseSharedPolicies(this IApplicationBuilder app) 
        {
            app.UseMiddleware<GlobalException>();

            app.UseMiddleware<ListenToOnlyApiGateway>();
            return app;
        }

    }
}
