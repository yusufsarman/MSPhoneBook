using ContactApi.Infrastructure.Interfaces;
using ContactApi.Model.ValidateObjects;
using Microsoft.AspNetCore.Mvc;

namespace ContactApi.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/contact")]
    [ApiVersion("1.0")]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;
        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }
        [HttpGet("GetByIdAsync/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            if (id != Guid.Empty)
            {
                try
                {
                    ContactDto contact = await _contactService.GetContactByIdAsync(id);
                    if (contact == null)
                    {
                        return NotFound(); // 404 Not Found
                    }

                    return Ok(contact); // 200 OK

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
                List<ContactDto> data = await _contactService.GetListAsync();
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
        public async Task<IActionResult> CreateAsync([FromBody] ContactCreateDto contact)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState); // Return validation errors
                }
                ContactDto data = await _contactService.CreateContactAsync(contact);
                return CreatedAtAction(nameof(GetByIdAsync), new { id = data.Id }, data); // 201 Created
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            if (id != Guid.Empty)
            {
                try
                {
                    await _contactService.DeleteContactByIdAsync(id);
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
