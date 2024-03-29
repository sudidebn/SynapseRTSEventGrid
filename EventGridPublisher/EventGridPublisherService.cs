﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Extensions.Configuration;
using SynapseRealTimeSync.Models;

namespace SynapseRealTimeSync.EventGridPublisher
{
    public class EventGridPublisherService
    {
        private readonly string topicEndpoint;
        private readonly string topicKey;
        private readonly string topicHostname;
        readonly TopicCredentials topicCredentials;
        readonly EventGridClient client;
        

        public EventGridPublisherService(IConfiguration configuration)
        {
            topicEndpoint = configuration["topic-endpoint"];
            topicKey = configuration["topic-key"];
            this.topicHostname = new Uri(topicEndpoint).Host;
            this.topicCredentials = new TopicCredentials(topicKey);
            this.client = new EventGridClient(topicCredentials);
        }

        public async Task<bool> EventGridPublisher(EventDetails eventDetails )
        {
            await client.PublishEventsAsync(topicHostname, GetEventsList(eventDetails));
            return true;
        }

        private static IList<EventGridEvent> GetEventsList(EventDetails eventDetails)
        {
            var eventsList = new List<EventGridEvent>();
            {
                for (var i = 0; i < 1; i++)
                {
                    eventsList.Add(new EventGridEvent()
                    {
                        Id = Guid.NewGuid().ToString(),
                        EventType = eventDetails.EventType,
                        Data = eventDetails.Data,
                        EventTime = eventDetails.EventTime,
                        Subject = eventDetails.Subject,
                        DataVersion = eventDetails.Version
                    });
                }
                return eventsList;
            }
        }
    }
}







