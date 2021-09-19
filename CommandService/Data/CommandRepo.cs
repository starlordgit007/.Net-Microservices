using CommandService.Interface;
using CommandService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommandService.Data
{
    public class CommandRepo : ICommandRepo
    {
        private readonly AppDbContext _context;

        public CommandRepo(AppDbContext dbContext)
        {
            _context = dbContext;
        }

        public void CreateCommand(Command command, int platformID)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));
            //if (!PlatformExists(platformID))
            //    throw new InvalidOperationException("Platform Does Not Exist!");
            command.PlatformId = platformID;
            _context.Commands.Add(command);
        }

        public void CreatePlatform(Platform platform)
        {
            if (platform == null)
                throw new ArgumentNullException(nameof(platform));

            _context.Platforms.Add(platform);
        }

        public bool ExternalPlatformExists(int externalPlatformId)
        {
            return _context.Platforms.Any(p => p.ExternalID == externalPlatformId);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _context.Platforms.ToList();
        }

        public Command GetCommand(int platformID, int commandID)
        {
            return _context.Commands.FirstOrDefault(c => c.Id == commandID && c.PlatformId == platformID);
        }

        public IEnumerable<Command> GetCommandsForPlatform(int platformID)
        {
            return _context.Commands.Where(c => c.PlatformId == platformID);
        }

        public bool PlatformExists(int platformID)
        {
            return _context.Platforms.Any(p => p.Id == platformID);
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }
    }
}