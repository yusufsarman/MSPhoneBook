using EventBus.Base.Events;

namespace ReportApi.IntegrationEvents.Events
{
    public class ReportStartedIntegrationEvent : IntegrationEvent
    {
        public ReportStartedIntegrationEvent() { }

        public ReportStartedIntegrationEvent(Guid reportId)
        {
            ReportId = reportId;
        }

        public Guid ReportId { get; set; }
    }
}
