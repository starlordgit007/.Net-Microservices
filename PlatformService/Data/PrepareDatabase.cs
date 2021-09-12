using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PlatformService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformService.Data
{
    public static class PrepareDatabase
    {
        public static void PreparePopulations(IApplicationBuilder app, bool isProd = false)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDBContext>(), isProd);
            }
        }

        private static void SeedData(AppDBContext context, bool isProd = false)
        {
            if (isProd)
            {
                Console.WriteLine("Attempting To Apply Migrations...");
                try
                {
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Could Not Run Migrations : {ex.Message}");
                }
            }

            if (!context.Platforms.Any())
            {
                context.Platforms.AddRange(
                            new Platform() { Name = "dotnet", Publisher = "Microsoft", Cost = "Free" },
                            new Platform() { Name = "SQLExpress", Publisher = "Microsoft", Cost = "Free" },
                            new Platform() { Name = "Kubernets", Publisher = "Cloud Native Computing Foundations", Cost = "Free" }
                        );
                context.SaveChanges();
            }
        }
    }
}