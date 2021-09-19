using AutoMapper;
using CommandService.Data;
using CommandService.DTO;
using CommandService.Interface;
using CommandService.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace CommandService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IMapper _autoMapper;

        public EventProcessor(IServiceScopeFactory serviceScopeFactory, IMapper autoMapper)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _autoMapper = autoMapper;
        }

        public void ProcessEvent(string message)
        {
            EventType eventType = DetermineEvent(message);

            //todo: move to different method
            switch (eventType)
            {
                case EventType.PlatformPublished:
                    addPlatform(message);
                    break;

                default:
                    break;
            }
        }

        private EventType DetermineEvent(string notificationMessage)
        {
            Console.WriteLine("Determining Event");

            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);
            switch (eventType.Event)
            {
                case "PlatformPublished":
                    Console.WriteLine("Platform Publish Event Detected!");
                    return EventType.PlatformPublished;

                default:
                    Console.WriteLine("Could Not Determine Event Type!");
                    return EventType.Undetermined;
            }
        }

        //Move to other class
        private void addPlatform(string platformPublishMessage)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var repository = scope.ServiceProvider.GetRequiredService<ICommandRepo>();
                var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(platformPublishMessage);
                try
                {
                    var plat = _autoMapper.Map<Platform>(platformPublishedDto);
                    if (repository.ExternalPlatformExists(plat.ExternalID))
                    {
                        Console.WriteLine("Platform Already Exists!");
                        return;
                    }
                    repository.CreatePlatform(plat);
                    repository.SaveChanges();
                    Console.WriteLine("Platform Added!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Could Not Add Platform To DB : {ex.Message}");
                }
            }
        }
    }

    internal enum EventType
    {
        PlatformPublished,
        Undetermined
    }
}