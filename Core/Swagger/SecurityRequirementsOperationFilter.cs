using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Core.Swagger
{
    public class SecurityRequirementsOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Policy names map to scopes
            var attributes = context.MethodInfo.GetCustomAttributes(true);
            var hasAnonymouse = attributes.OfType<AllowAnonymousAttribute>().Any();
            if (hasAnonymouse)
                return;
            var authorizeAttributes = context.MethodInfo.GetCustomAttributes(true)
                                                        .OfType<AuthorizeAttribute>();
            if (!authorizeAttributes.Any())
                authorizeAttributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
                                                                      .OfType<AuthorizeAttribute>();
            var requiredScopes = authorizeAttributes.Select(attr => attr.Policy).Distinct();
            if (requiredScopes.Any())
            {
                operation.Responses.Add("Authorize", new OpenApiResponse { Description = string.Join(",", requiredScopes) });
                operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
                operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });
                var oAuthScheme = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                };
                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
                    {
                        [ oAuthScheme ] = requiredScopes.ToList()
                    }
                };
            }
        }
    }
}
