using ContactApi.Infrastructure.Interfaces;
using ContactApi.Model;
using ContactApi.Model.ValidateObjects;
using Microsoft.AspNetCore.Mvc;

namespace ContactApi.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/contacts")]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;
        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            if (id > 0)
            {
                ContactDto contact =await _contactService.GetContactByIdAsync(id);
                if (contact == null)
                {
                    return NotFound(); // 404 Not Found
                }

                return Ok(contact); // 200 OK
            }
            else
            {
                return BadRequest();
            }
            
             
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListAsync()
        {
            
            //return Ok(contacts); // 200 OK
            return Ok(); // 200 OK
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
            catch (Exception ex )
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] ContactDto contact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return validation errors
            }
            if (contact.Id != id)
            {
                return BadRequest("Id not matched"); 
            }

            // Update contact

            return CreatedAtAction(nameof(GetByIdAsync), new { id = contact.Id }, contact);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
           
            //var contact = ...; 
            //if (contact == null)
            //{
            //    return NotFound(); // 404 Not Found
            //}

            // Delete contact

            return NoContent(); // 204 No Content
        }
    }
}
