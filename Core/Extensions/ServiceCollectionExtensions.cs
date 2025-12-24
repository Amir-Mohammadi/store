using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Core.Data;
using Core.Swagger;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Core.Extensions
{
    public static class ServiceCollectionExtensions
    {

        public static void AddContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
              options.UseSqlServer(configuration.GetConnectionString("SqlServer")));
        }
        public static IServiceCollection AddSwaggerGen(this IServiceCollection services, string apiVersion)
        {
            return services.AddSwaggerGen(c =>
            {
                var assemblyName = Assembly.GetEntryAssembly().GetName().Name;
                c.SwaggerDoc(apiVersion, new OpenApiInfo { Title = assemblyName, Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.SchemaFilter<EnumSchemaFilter>();
                c.OperationFilter<SecurityRequirementsOperationFilter>();
            });
        }

    }
}
