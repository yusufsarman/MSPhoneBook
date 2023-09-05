﻿using EventBus.Base.Abstraction;
using EventBus.Base;
using EventBus.Factory;
using RabbitMQ.Client;
using ReportApi.IntegrationEvents.Events;
using ReportApi.IntegrationEvents.Handlers;

namespace ReportApi.IoC
{
    public static class RabbitMQRegistration
    {
        public static void ConfigureRabbitMQ(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton(sp =>
            {
                EventBusConfig config = new()
                {
                    ConnectionRetryCount = 5,
                    EventNameSuffix = builder.Configuration.GetValue<string>("RabbitMQConfig:EventNameSuffix"),
                    SubscriberClientAppName = builder.Configuration.GetValue<string>("RabbitMQConfig:SubscriberClientAppName"),
                    Connection = new ConnectionFactory()
                    {
                        HostName = builder.Configuration.GetValue<string>("RabbitMQConfig:HostName"),
                        UserName = builder.Configuration.GetValue<string>("RabbitMQConfig:UserName"),
                        Password = builder.Configuration.GetValue<string>("RabbitMQConfig:Password"),
                        Port = builder.Configuration.GetValue<int>("RabbitMQConfig:Port"),
                        VirtualHost = "/"
                    },
                    EventBusType = EventBusType.RabbitMQ,

                };

                return EventBusFactory.Create(config, sp);
            });

            builder.Services.AddTransient<ReportCreatedIntegrationEventHandler>();

        }

        public static void ConfigureEventBusForSubscription(this IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

            eventBus.Subscribe<ReportCreatedIntegrationEvent, ReportCreatedIntegrationEventHandler>();
        }
    }
}
