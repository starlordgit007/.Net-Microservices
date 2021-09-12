using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.DTO;
using PlatformService.Interface;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _platformRepo;
        private readonly IMapper _autoMapper;
        private readonly ICommandDataClient _commandClient;

        public PlatformsController(
            IPlatformRepo platformRepo,
            IMapper autoMapper,
            ICommandDataClient commandClient)
        {
            _platformRepo = platformRepo;
            _autoMapper = autoMapper;
            _commandClient = commandClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetAllPlatforms()
        {
            return Ok(_autoMapper.Map<IEnumerable<PlatformReadDto>>(_platformRepo.GetAllPlatforms()));
        }

        [HttpGet("{id}", Name = "GetPlatform")]
        public ActionResult<PlatformReadDto> GetPlatform(int id)
        {
            var platform = _platformRepo.GetPlatform(id);
            if (platform == null)
            {
                return NotFound();
            }
            return Ok(_autoMapper.Map<PlatformReadDto>(platform));
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platformDto)
        {
            if (platformDto == null)
                return BadRequest();
            var platformModel = _autoMapper.Map<Platform>(platformDto);
            _platformRepo.Create(platformModel);
            _platformRepo.SaveChanges();
            var platformReadDto = _autoMapper.Map<PlatformReadDto>(platformModel);
            try
            {
                await _commandClient.SendPlatformToCommand(platformReadDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could Not Send Data To Command Service Reason : {ex.Message}");
            }
            return CreatedAtRoute(nameof(GetPlatform), new { Id = platformReadDto.Id }, platformReadDto);
        }
    }
}