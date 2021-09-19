using CommandService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommandService.Interface
{
    public interface ICommandRepo
    {
        bool SaveChanges();

        //Command Related Stuff
        IEnumerable<Command> GetCommandsForPlatform(int platformID);

        Command GetCommand(int platformID, int commandID);

        void CreateCommand(Command command, int platformID);

        //Platform Related Stuff
        IEnumerable<Platform> GetAllPlatforms();

        void CreatePlatform(Platform platform);

        bool PlatformExists(int platformID);

        bool ExternalPlatformExists(int externalPlatformId);
    }
}