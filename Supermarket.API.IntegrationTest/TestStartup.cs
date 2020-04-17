using Domain.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Supermarket.API.IntegrationTest
{
    public class TestStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddSingleton<DbContext, AppDbContext>();
            // ...
            // ...
        }
    }
}
