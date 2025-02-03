using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCommerce.SharedLibrary.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductApi.Application.Interface;
using ProductApi.Infrstructure.Data;
using ProductApi.Infrstructure.Repository;

namespace ProductApi.Infrstructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructreService(this IServiceCollection services, IConfiguration config)
        {

            SharedServiceContainercs.AddSharedService<ProductDbContext>(services, config, config["MySeriLog:FileName"]!);

            services.AddScoped<IProduct, ProductRepository>();
            return services;
        }

        public static IApplicationBuilder UseInfrastructurePolicy(this IApplicationBuilder app)
        {
            SharedServiceContainercs.UseSharedPolicies (app);
            return app;
        }
    }
}
