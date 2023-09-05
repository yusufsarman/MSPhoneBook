using AutoMapper;
using EventBus.Base.Abstraction;
using Microsoft.AspNetCore.Mvc;
using ReportApi.Infrastructure.Interfaces;
using ReportApi.IntegrationEvents.Events;

namespace ReportApi.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/report")]
    [ApiVersion("1.0")]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly IReportDetailService _reportDetailService;
        private readonly IEventBus _eventBus;
        private readonly IMapper _mapper;
        public ReportController(IReportService reportService,
            IReportDetailService reportDetailService,
            IEventBus eventBus,
            IMapper mapper)
        {
            _reportService = reportService;
            _reportDetailService = reportDetailService;
            _mapper = mapper;
            _eventBus = eventBus;

        }

        [HttpGet("GetListAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetListAsync()
        {
            try
            {
                var data = await _reportService.GetListAsync();

                return Ok(data); // 200 OK
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
            
        }

        [HttpGet("GetByIdAsync/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            if (id != Guid.Empty)
            {
                try
                {
                    var report = await _reportService.GetReportByIdAsync(id);
                    if (report == null)
                        return NoContent();

                    return Ok(report);
                   

                }
                catch (Exception ex)
                {

                    return BadRequest(ex);
                }

            }
            else
            {
                return BadRequest("Id must be a valid Guid");
            }


        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAsync()
        {
            try
            { 
                var report = await _reportService.CreateReportAsync();
                if (report == null)
                    return NoContent();
                var reportStartedEventModel = new ReportStartedIntegrationEvent(report.Id);
                _eventBus.Publish(reportStartedEventModel);

                return CreatedAtAction(nameof(GetByIdAsync), new { id = report.Id }, report); // 201 Created
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
