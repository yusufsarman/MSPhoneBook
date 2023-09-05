using EventBus.Base.Events;

namespace ContactApi.IntegrationEvents.Events
{
    public class ReportStartedIntegrationEvent : IntegrationEvent
    {
        public Guid ReportId { get; set; }
    }
}
