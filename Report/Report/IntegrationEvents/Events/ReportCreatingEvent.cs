using EventBus.Base.Events;
using ReportApi.Model.ValidateObjects;

namespace ReportApi.IntegrationEvents.Events
{
    public class ReportCreatingEvent : IntegrationEvent
    {
        public Guid ReportId { get; set; }
        public IList<EventReportDetailDto> ReportDetails { get; set; }

        public class EventReportDetailDto
        {
            public string Location { get; set; }
            public int EmailCount { get; set; }
            public int PhoneCount { get; set; }
        }
    }
}
