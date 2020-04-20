using Domain.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace Supermarket.API.IntegrationTest
{
    public class AppTestFixture : WebApplicationFactory<Startup>
    {
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
                //services.AddDbContext<AppDbContext>(options =>
                //{
                //    options.UseInMemoryDatabase("Supermarket.API-in-memory");
                //});
                //services.AddScoped<ICategoryRepository, CategoryRepositoryTest>();
            });

        }
    }
}
