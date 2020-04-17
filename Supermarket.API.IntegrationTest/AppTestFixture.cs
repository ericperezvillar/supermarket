using Domain.Persistence.Contexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;

namespace Supermarket.API.IntegrationTest
{
    //public class AppTestFixture<TStartup> : IDisposable where TStartup : class
    public class AppTestFixture : WebApplicationFactory<Startup>
    {
        //protected AppDbContext DbContext { get; }

        //private readonly TestServer _server;
        //private readonly IServiceProvider _services;
        //private readonly IDbContextTransaction _transaction;
        //protected readonly HttpClient Client;
        //protected T GetService<T>() => (T)_services.GetService(typeof(T));

        //public AppTestFixture()
        //{
        //    var builder = WebHost.CreateDefaultBuilder()
        //    .UseStartup<Startup>();

        //    // construct the test server and client we'll use to
        //    // send requests
        //    _server = new TestServer(builder);
        //    _server.PreserveExecutionContext = true;

        //    Client = _server.CreateClient();
        //    _services = _server.Host.Services;
        //    // resolve a DbContext instance from the container
        //    // and begin a transaction on the context.
        //    DbContext = GetService<AppDbContext>();
        //    _transaction = DbContext.Database.BeginTransaction();

        //    //var options = new DbContextOptionsBuilder<AppDbContext>()
        //    //.UseSqlServer(Configuration.GetConnectionString("dbLog"))
        //    //.Options;

        //    //DbContext = new AppDbContext(options);

        //    //Transaction = DbContext.Database.BeginTransaction();
        //}

        //override methods here as needed
        protected override IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                       .ConfigureWebHostDefaults(c =>
                       {
                           c.UseStartup<Startup>();
                       });
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseInMemoryDatabase("Supermarket.API-in-memory");
                });
            });
        }
    }
    //public void Dispose()
    //    {
    //        if (_transaction != null)
    //        {
    //            _transaction.Rollback();
    //            _transaction.Dispose();
    //        }
    //    }

    //}
}
