using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pollr.Api.Data;
using Pollr.Api.Models.PollDefinitions;

namespace Pollr.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PollDefinitions1Controller : ControllerBase
    {
        private readonly PollrContext _context;

        public PollDefinitions1Controller(PollrContext context)
        {
            _context = context;
        }

        // GET: api/PollDefinitions1
        [HttpGet]
        public IEnumerable<PollDefinition> GetPollDefinitions()
        {
            return _context.PollDefinitions;
        }

        // GET: api/PollDefinitions1/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPollDefinition([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pollDefinition = await _context.PollDefinitions.FindAsync(id);

            if (pollDefinition == null)
            {
                return NotFound();
            }

            return Ok(pollDefinition);
        }

        // PUT: api/PollDefinitions1/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPollDefinition([FromRoute] int id, [FromBody] PollDefinition pollDefinition)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pollDefinition.Id)
            {
                return BadRequest();
            }

            _context.Entry(pollDefinition).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PollDefinitionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/PollDefinitions1
        [HttpPost]
        public async Task<IActionResult> PostPollDefinition([FromBody] PollDefinition pollDefinition)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.PollDefinitions.Add(pollDefinition);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPollDefinition", new { id = pollDefinition.Id }, pollDefinition);
        }

        // DELETE: api/PollDefinitions1/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePollDefinition([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pollDefinition = await _context.PollDefinitions.FindAsync(id);
            if (pollDefinition == null)
            {
                return NotFound();
            }

            _context.PollDefinitions.Remove(pollDefinition);
            await _context.SaveChangesAsync();

            return Ok(pollDefinition);
        }

        private bool PollDefinitionExists(int id)
        {
            return _context.PollDefinitions.Any(e => e.Id == id);
        }
    }
}