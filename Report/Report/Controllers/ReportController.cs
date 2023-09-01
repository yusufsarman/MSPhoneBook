using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ReportApi.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/report")]
    [ApiVersion("1.0")]
    public class ReportController:ControllerBase
    {
        public ReportController() { }

        [HttpGet("GetList")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListAsync()
        {
            // Get all reports and return

            return Ok();
        }

        [HttpGet("GetByIdAsync{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            if (id > 0)
            {
                try
                {
                   

                    return Ok(); // 200 OK

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
