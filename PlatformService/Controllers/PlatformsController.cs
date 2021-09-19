using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.AsyncDataServices;
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
        private readonly IMessageBusClient _messageClient;

        public PlatformsController(
            IPlatformRepo platformRepo,
            IMapper autoMapper,
            ICommandDataClient commandClient,
            IMessageBusClient messageClient)
        {
            _platformRepo = platformRepo;
            _autoMapper = autoMapper;
            _commandClient = commandClient;
            _messageClient = messageClient;
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
            //Send Async Message
            try
            {
                await _commandClient.SendPlatformToCommand(platformReadDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could Not Send Data To Command Service Sync Reason : {ex.Message}");
            }

            //Send Async Message
            try
            {
                PlatformPublishDto publishDto = _autoMapper.Map<PlatformPublishDto>(platformReadDto);
                publishDto.Event = "PlatformPublished";
                _messageClient.PublishNewPlatform(publishDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could Not Send Data To Command Service ASync Reason : {ex.Message}");
            }

            return CreatedAtRoute(nameof(GetPlatform), new { Id = platformReadDto.Id }, platformReadDto);
        }
    }
}