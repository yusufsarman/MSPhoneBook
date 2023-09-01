using AutoMapper;
using EventBus.Base.Abstraction;
using Microsoft.AspNetCore.Mvc;
using ReportApi.Infrastructure.Interfaces;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        [HttpGet("GetList")]
        [ProducesResponseType(StatusCodes.Status200OK)]
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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            if (id > 0)
            {
                try
                {
                    var report = await _reportService.GetReportByIdAsync(id);
                    if (report == null)
                        return NotFound();

                    return Ok(report);
                   

                }
                catch (Exception ex)
                {

                    return BadRequest(ex);
                }

            }
            else
            {
                return BadRequest("Id must be greater than 0");
            }


        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAsync([FromBody] int contact)
        {
            try
            {
                return Ok();
                //if (!ModelState.IsValid)
                //{
                //    return BadRequest(ModelState); // Return validation errors
                //}
                //ContactDto data = await _contactService.CreateContactAsync(contact);
                //return CreatedAtAction(nameof(GetByIdAsync), new { id = data.Id }, data); // 201 Created
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
