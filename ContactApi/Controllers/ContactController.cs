﻿using ContactApi.Model;
using ContactApi.Model.ValidateObjects;
using Microsoft.AspNetCore.Mvc;

namespace ContactApi.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/contacts")]
    public class ContactController : ControllerBase
    {
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var contact = ""; 
            if (contact == null)
            {
                return NotFound(); // 404 Not Found
            }

            //return Ok(contact); // 200 OK
            return Ok(); 
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return validation errors
            }
            // Save contact to database or storage
            return CreatedAtAction(nameof(GetByIdAsync), new { id = contact.Name }, contact); // 201 Created
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