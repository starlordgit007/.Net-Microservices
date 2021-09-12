using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PlatformService.Data;
using PlatformService.Interface;
using PlatformService.SyncDataServices.Http;
using System;

namespace Microservice
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (_env.IsProduction())
            {
                Console.WriteLine("Using MSSQL DB");
                services.AddDbContext<AppDBContext>(opt =>
                    opt.UseSqlServer(Configuration.GetConnectionString("platformsConn")));
            }
            else
            {
                Console.WriteLine("Using InMem Db");
                services.AddDbContext<AppDBContext>(opt =>
                    opt.UseInMemoryDatabase("PlatformService"));
            }
            services.AddControllers();

            services.AddScoped<IPlatformRepo, PlatformRepo>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            Console.WriteLine($"{Configuration["CommandService"]}");
            PrepareDatabase.PreparePopulations(app, _env.IsProduction());
        }
    }
}