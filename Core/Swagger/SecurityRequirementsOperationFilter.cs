using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Core.Swagger
{
    public class SecurityRequirementsOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // MethodInfo can be null in .NET 9 (minimal APIs, metadata routing)
            var methodInfo = context.MethodInfo;
            if (methodInfo == null)
                return;

            var attributes = methodInfo.GetCustomAttributes(true);

            if (attributes.OfType<AllowAnonymousAttribute>().Any())
                return;

            var authorizeAttributes = attributes.OfType<AuthorizeAttribute>();

            if (!authorizeAttributes.Any())
            {
                authorizeAttributes = methodInfo.DeclaringType?
                    .GetCustomAttributes(true)
                    .OfType<AuthorizeAttribute>()
                    ?? Enumerable.Empty<AuthorizeAttribute>();
            }

            if (!authorizeAttributes.Any())
                return;

            // Add standard auth responses
            operation.Responses.TryAdd("401", new OpenApiResponse { Description = "Unauthorized" });
            operation.Responses.TryAdd("403", new OpenApiResponse { Description = "Forbidden" });

            // Add Bearer security requirement
            var bearerScheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            };

            operation.Security ??= new List<OpenApiSecurityRequirement>();

            operation.Security.Add(new OpenApiSecurityRequirement
            {
                [bearerScheme] = Array.Empty<string>()
            });
        }
    }
}
