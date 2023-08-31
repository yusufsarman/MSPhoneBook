using ContactApi.Infrastructure.Interfaces;
using ContactApi.Model.ValidateObjects;
using Microsoft.AspNetCore.Mvc;

namespace ContactDetailApi.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/contactDetail")]
    [ApiVersion("1.0")]
    public class ContactDetailController : ControllerBase
    {
        private readonly IContactDetailService _contactDetailService;
        public ContactDetailController(IContactDetailService contactDetailService)
        {
            _contactDetailService = contactDetailService;
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
                    ContactDetailDto contactDetail = await _contactDetailService.GetContactDetailByIdAsync(id);
                    if (contactDetail == null)
                    {
                        return NotFound(); // 404 Not Found
                    }

                    return Ok(contactDetail); // 200 OK

                }
                catch (Exception ex)
                {

                    return BadRequest(ex);
                }
               
            }
            else
            {
                return BadRequest();
            }
            
             
        }

        [HttpGet("GetListAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListAsync()
        {
            try
            {
                List<ContactDetailDto> data = await _contactDetailService.GetListAsync();
                return Ok(data); // 200 OK
              
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
           
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAsync([FromBody] ContactDetailCreateDto contactDetail)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState); // Return validation errors
                }
                ContactDetailDto data = await _contactDetailService.CreateContactDetailAsync(contactDetail);
                return CreatedAtAction(nameof(GetByIdAsync), new { id = data.Id }, data); // 201 Created
            }
            catch (Exception ex )
            {
                return BadRequest(ex.Message);
            }
            
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (id > 0)
            {
                try
                {
                    await _contactDetailService.DeleteContactDetailByIdAsync(id);
                    return NoContent(); // 204 No Content
                }
                catch (Exception ex)
                {

                    return BadRequest(ex);
                }
                
            }
            else
            {
                return BadRequest();
            }

            
        }
    }
}
