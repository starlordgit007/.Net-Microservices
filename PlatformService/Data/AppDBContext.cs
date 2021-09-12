using Microsoft.EntityFrameworkCore;
using PlatformService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformService.Data
{
    public class AppDBContext : DbContext
    {
        private readonly DbContextOptions _dbContextOptions;

        public AppDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
            _dbContextOptions = dbContextOptions;
        }

        public DbSet<Platform> Platforms { get; set; }
    }
}