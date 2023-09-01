using EventBus.Base.Abstraction;
using Microsoft.Extensions.Logging;
using ReportApi.Entities;
using ReportApi.Infrastructure.Interfaces;
using ReportApi.IntegrationEvents.Events;
using ReportApi.Model.ValidateObjects;

namespace ReportApi.IntegrationEvents.Handlers
{
    public class ReportCreatingEventHandler : IIntegrationEventHandler<ReportCreatingEvent>
    {
        private readonly IReportDetailService _reportDetailService;
        private readonly IReportService _reportService;
        private readonly ILogger<ReportCreatingEvent> _logger;

        public ReportCreatingEventHandler(IReportDetailService reportDetailService,
            IReportService reportService,
            ILogger<ReportCreatingEvent> logger)
        {
            _reportDetailService = reportDetailService;
            _reportService = reportService;
            _logger = logger;
        }
        public async Task Handle(ReportCreatingEvent @event)
        {
            _logger.LogInformation("@ RabbitMQ Event Handling: {IntegrationEventId} at MSPhoneBook.ReportApi - ({@IntegrationEvent})", @event.ReportId, @event);

            var report = await _reportService.GetReportByIdAsync(@event.ReportId);
            if (report == null) return;
            try
            {
                await _reportService.ReportCompletedAsync(report.Id);
                _logger.LogInformation("@ Report Completed : | {IntegrationEventId} | Report Id : " + report.Id);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("@ Report Not Completed : | {IntegrationEventId} | Error is : " + ex.Message.ToString());
            }
            if (@event == null) return;
            var details = @event.ReportDetails
              .Select(x => new ReportDetailCreateDto(@event.ReportId, x.Location, x.EmailCount, x.PhoneCount))
              .ToList();

            await _reportDetailService.CreateReportDetailsAsync(details);
        }
    }
}
