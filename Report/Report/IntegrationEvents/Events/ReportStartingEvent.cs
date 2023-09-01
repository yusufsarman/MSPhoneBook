using EventBus.Base.Events;

namespace ReportApi.IntegrationEvents.Events
{
    public class ReportStartingEvent : IntegrationEvent
    {
        public ReportStartingEvent() { }

        public ReportStartingEvent(Guid reportId)
        {
            ReportId = reportId;
        }

        public Guid ReportId { get; set; }
    }
}
