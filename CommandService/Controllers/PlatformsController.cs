using AutoMapper;
using CommandService.DTO;
using CommandService.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CommandService.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly ICommandRepo _repository;
        private readonly IMapper _autoMapper;

        public PlatformsController(ICommandRepo repository, IMapper autoMapper)
        {
            _repository = repository;
            _autoMapper = autoMapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("Getting Platforms From Command Service !");
            var platformItems = _repository.GetAllPlatforms();
            return Ok(_autoMapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
        }

        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            Console.WriteLine("---> Inbound Post At Commands Service");
            return Ok("Inbound Test Of Platform Controller At Commands Service");
        }
    }
}