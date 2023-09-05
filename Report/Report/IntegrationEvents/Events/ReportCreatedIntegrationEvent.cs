using EventBus.Base.Events;
using ReportApi.Model.ValidateObjects;

namespace ReportApi.IntegrationEvents.Events
{
    public class ReportCreatedIntegrationEvent : IntegrationEvent
    {
        public Guid ReportId { get; set; }
        public IList<ReportDetailDto> Details { get; set; }

        public class ReportDetailDto
        {
            public string Location { get; set; }
            public int ContactCount { get; set; }
            public int PhoneNumberCount { get; set; }
        }
    }
}
