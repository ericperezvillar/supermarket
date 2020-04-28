using Domain.Persistence.Repositories;
using Domain.Repositories;
using Domain.Services;
using Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;
using System;
using System.Text;

namespace Extensions
{
    public static class MiddlewareExtensions
    {
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
        {
            services.AddSwaggerDocument(config =>
            {
                config.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "Supermarket API";
                    document.Info.Description = "A simple ASP.NET Core web API 2";
                    document.Info.TermsOfService = "https://example.com/terms";
                    document.Info.Contact = new NSwag.OpenApiContact
                    {
                        Name = "Eric Perez Villar",
                        Email = string.Empty,
                        Url = "https://www.linkedin.com/in/eric-perez-villar-87aa6746/",
                    };
                    document.Info.License = new NSwag.OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = "https://example.com/license"
                    };
                };
                
                // This Example is when you have a login page to redirect

                //config.AddSecurity("bearer", Enumerable.Empty<string>(), new OpenApiSecurityScheme
                //{
                //    Type = OpenApiSecuritySchemeType.OAuth2,
                //    Description = "My Authentication",
                //    Flow = OpenApiOAuth2Flow.Implicit,
                //    Flows = new OpenApiOAuthFlows()
                //    {
                //        Implicit = new OpenApiOAuthFlow()
                //        {
                //            Scopes = new Dictionary<string, string>
                //            {
                //                {"api1", "My API"}
                //            },
                //            TokenUrl = "https://localhost:44350/api/users/authenticate",
                //            AuthorizationUrl = "https://localhost:44350/api/users/authenticate",

                //        },
                //    }
                //});

                config.DocumentProcessors.Add(
                    new SecurityDefinitionAppender("JWT",
                    new OpenApiSecurityScheme
                    {
                        Type = OpenApiSecuritySchemeType.ApiKey,
                        Name = "Authorization",
                        In = OpenApiSecurityApiKeyLocation.Header,
                        Description = "Type into the textbox: Bearer {your JWT token}."
                    }));
                config.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));


             });

            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors();

            // configure strongly typed settings objects
            var appSettingsSection = configuration.GetSection("JwtToken");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = appSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = appSettings.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            return services;
        }

        public static IServiceCollection AddScopeCustom(this IServiceCollection services)
        {

            //services.AddDbContext<AppDbContext>(options => {
            //    options.UseInMemoryDatabase("Supermarket.API-in-memory");
            //});

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }

        public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder app)
        {
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Supermarket swagger");
            });
            app.UseOpenApi();
            
            return app;
        }
    }
}
