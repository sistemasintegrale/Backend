using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace SGE.API.Extensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            var openApi = new OpenApiInfo
            {
                Title = "SIDE API",
                Version = "v1",
                Description = "Backend App 2023",
                //TermsOfService = new Uri(""),
                Contact = new OpenApiContact
                {
                    Name = "SIDE S.A.C.",
                    Email = "rogola06@hotmail.com",
                    //Url = new Uri(""),
                },
                License = new OpenApiLicense
                {
                    Name = "Use under LICX",
                    //Url = new Uri(""),
                }
            };

            services.AddSwaggerGen(x =>
            {
                openApi.Version = "v1";
                x.SwaggerDoc("v1", openApi);
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "JWT Bearer Token",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                x.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                x.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { securityScheme, new string[]{ } }
                });
            });
            return services;
        }
    }
}
