using PlatformService.Interface;
using PlatformService.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PlatformService.Data
{
    public class PlatformRepo : IPlatformRepo
    {
        private readonly AppDBContext _dbContext;

        public PlatformRepo(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Create(Platform platform)
        {
            if (platform == null)
                throw new ArgumentNullException(nameof(platform));

            _dbContext.Add(platform);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _dbContext.Platforms.ToList();
        }

        public Platform GetPlatform(int id)
        {
            return _dbContext.Platforms.FirstOrDefault(platform => platform.Id == id);
        }

        public bool SaveChanges()
        {
            return _dbContext.SaveChanges() > 0;
        }
    }
}