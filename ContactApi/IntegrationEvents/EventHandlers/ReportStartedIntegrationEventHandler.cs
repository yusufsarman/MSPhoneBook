using ContactApi.Enums;
using ContactApi.Infrastructure.Interfaces;
using ContactApi.IntegrationEvents.Events;
using EventBus.Base.Abstraction;
using Newtonsoft.Json;

namespace ContactApi.IntegrationEvents.EventHandlers
{
    public class ReportStartedIntegrationEventHandler : IIntegrationEventHandler<ReportStartedIntegrationEvent>
    {
        private readonly IContactDetailService _detailService;
        private readonly ILogger<ReportStartedIntegrationEvent> _logger;
        private readonly IEventBus _eventBus;

        public ReportStartedIntegrationEventHandler(IContactDetailService detailService, IEventBus eventBus, ILogger<ReportStartedIntegrationEvent> logger)
        {
            _detailService = detailService;
            _logger = logger;
            _eventBus = eventBus;

        }

        public async Task Handle(ReportStartedIntegrationEvent @event)
        {
            _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at ContactService.API - ({@IntegrationEvent})", @event.ReportId, @event);

            var eventModel = new ReportCreatedIntegrationEvent(@event.ReportId);

            var contactInfos = await _detailService.GetListAsync();
            if (contactInfos == null || !contactInfos.Any())
            {
                _eventBus.Publish(eventModel);
                return;
            }

            var locations = contactInfos.Where(x => x.ContactDetailType == ContactDetailTypeEnum.Location);
            if (locations == null || !locations.Any())
            {
                _eventBus.Publish(eventModel);
                return;
            }

            var distinctLocations = locations.Select(x => x.Content).Distinct();
            foreach (var location in distinctLocations)
            {
                var contacts = locations
                  .Where(x => x.Content == location)
                  .Select(x => x.ContactId)
                  .Distinct();

                var phoneNumbers = contactInfos
                  .Where(x => x.ContactDetailType == ContactDetailTypeEnum.Phone && contacts.Contains(x.ContactId))
                  .Select(x => x.Content)
                  .Distinct()
                  .Count();

                eventModel.AddDetail(location, contacts.Count(), phoneNumbers);
            }

            try
            {
                _eventBus.Publish(eventModel);
                Console.WriteLine(JsonConvert.SerializeObject(eventModel));
            }
            catch (Exception ex)
            {
                _logger.LogError("----- Handling integration event: {IntegrationEventId} at ContactApi - ({@IntegrationEvent}) - Error : {@error}", @event.ReportId, @event,ex.Message);

            }
        }

    }
}
