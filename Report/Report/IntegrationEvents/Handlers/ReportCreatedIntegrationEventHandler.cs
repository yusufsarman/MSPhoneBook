using EventBus.Base.Abstraction;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ReportApi.Entities;
using ReportApi.Infrastructure.Interfaces;
using ReportApi.IntegrationEvents.Events;
using ReportApi.Model.ValidateObjects;

namespace ReportApi.IntegrationEvents.Handlers
{
    public class ReportCreatedIntegrationEventHandler : IIntegrationEventHandler<ReportCreatedIntegrationEvent>
    {
        private readonly IReportDetailService _reportDetailService;
        private readonly IReportService _reportService;
        private readonly ILogger<ReportCreatedIntegrationEvent> _logger;

        public ReportCreatedIntegrationEventHandler(IReportDetailService reportDetailService,
            IReportService reportService,
            ILogger<ReportCreatedIntegrationEvent> logger)
        {
            _reportDetailService = reportDetailService;
            _reportService = reportService;
            _logger = logger;
        }
        public async Task Handle(ReportCreatedIntegrationEvent @event)
        {
            _logger.LogInformation("@ RabbitMQ Event Handling: {IntegrationEventId} at MSPhoneBook.ReportApi - ({@IntegrationEvent})", @event.ReportId, @event);

            var report = await _reportService.GetReportByIdAsync(@event.ReportId);
            if (report == null) return;
            try
            {
                await _reportService.ReportCompletedAsync(report.Id);
                _logger.LogInformation("@ Report Completed : | {IntegrationEventId} | Report Id : " + report.Id);
                Console.WriteLine(JsonConvert.SerializeObject(report));
            }
            catch (Exception ex)
            {
                _logger.LogInformation("@ Report Not Completed : | {IntegrationEventId} | Error is : " + ex.Message.ToString());
            }
            if (@event == null) return;
            var details = @event.Details
              .Select(x => new ReportDetailCreateDto(@event.ReportId, x.Location, x.ContactCount, x.PhoneNumberCount))
              .ToList();

            await _reportDetailService.CreateReportDetailsAsync(details);
        }
    }
}
