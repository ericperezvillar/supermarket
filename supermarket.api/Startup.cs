using System;
using System.Diagnostics;
using System.Text;
using AutoMapper;
using Controllers.Config;
using Domain.Persistence.Contexts;
using Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;

namespace Supermarket.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                // Adds a custom error response factory when ModelState is invalid
                options.InvalidModelStateResponseFactory = InvalidModelStateResponseFactory.ProduceErrorResponse;
            });

            services.AddJwtAuthentication(Configuration);

            services.AddDbContext<AppDbContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("dbSupermarket"));
            });

            InitialiseLogger();

            services.AddScopeCustom();
            services.AddAutoMapper(typeof(Startup));

            services.AddLogging((builder) =>
            {
                builder.AddSerilog(dispose: true);
            });

            services.AddCustomSwagger();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            Serilog.Debugging.SelfLog.Enable(msg =>
            {
                Debug.Print(msg);
                Debugger.Break();
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(option => option
              .AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //loggerFactory.AddProvider("Logs/myapp-{Date}.txt");
            app.UseCustomSwagger();
            
        }

        private void InitialiseLogger()
        {
            Log.Logger = new LoggerConfiguration()
             .MinimumLevel.Information()
             .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
             .Enrich.FromLogContext()
             .WriteTo.MSSqlServer(connectionString: Configuration.GetConnectionString("dbLog"),
                    tableName: "Log")
             .CreateLogger();

            //var configuration = new ConfigurationBuilder()
            //                .AddJsonFile("appsettings.json")
            //                .Build();

            //Log.Logger = new LoggerConfiguration()
            //    .ReadFrom.Configuration(configuration)
            //    .CreateLogger();
        }
    }
}
