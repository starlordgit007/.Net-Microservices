using AutoMapper;
using CommandService.DTO;
using CommandService.Interface;
using CommandService.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;

namespace CommandService.Controllers
{
    [Route("api/c/platforms/{platformId}/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandRepo _repository;
        private readonly IMapper _autoMapper;

        public CommandsController(ICommandRepo repository, IMapper autoMapper)
        {
            _repository = repository;
            _autoMapper = autoMapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId)
        {
            Console.WriteLine($"--> Getting Commands For Platform {platformId}");
            if (!_repository.PlatformExists(platformId))
                return NotFound();
            var commands = _repository.GetCommandsForPlatform(platformId);
            return Ok(_autoMapper.Map<IEnumerable<CommandReadDto>>(commands));
        }

        [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
        public ActionResult<CommandReadDto> GetCommandForPlatform(int platformId, int commandId)
        {
            Console.WriteLine($"--> Getting commands for Platform : {platformId} And Command : {commandId}");
            if (!_repository.PlatformExists(platformId))
                return NotFound();
            var commands = _repository.GetCommand(platformId, commandId);
            if (commands == null)
                return NotFound();
            return Ok(_autoMapper.Map<CommandReadDto>(commands));
        }

        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommandForPlatform(int platformId, CommandCreateDto createDto)
        {
            Console.WriteLine($"--> Creating commands for Platform : {platformId}");
            if (!_repository.PlatformExists(platformId))
                return NotFound();
            if (createDto == null)
                return BadRequest();

            var createCommand = _autoMapper.Map<Command>(createDto);
            _repository.CreateCommand(createCommand, platformId);
            _repository.SaveChanges();

            var commandReadDto = _autoMapper.Map<CommandReadDto>(createCommand);

            return CreatedAtRoute(nameof(GetCommandForPlatform), new { platformId = platformId, CommandId = commandReadDto.Id }, commandReadDto);
        }
    }
}