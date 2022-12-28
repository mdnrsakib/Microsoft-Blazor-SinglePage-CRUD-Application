using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Blazor_Create_Update_Delete.Shared.Models;

namespace Blazor_Create_Update_Delete.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentTourPackagesController : ControllerBase
    {
        private readonly TravelTourDbContext _context;

        public AgentTourPackagesController(TravelTourDbContext context)
        {
            _context = context;
        }

        // GET: api/AgentTourPackages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AgentTourPackage>>> GetAgentTourPackages()
        {
          if (_context.AgentTourPackages == null)
          {
              return NotFound();
          }
            return await _context.AgentTourPackages.ToListAsync();
        }

        // GET: api/AgentTourPackages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AgentTourPackage>> GetAgentTourPackage(int id)
        {
          if (_context.AgentTourPackages == null)
          {
              return NotFound();
          }
            var agentTourPackage = await _context.AgentTourPackages.FindAsync(id);

            if (agentTourPackage == null)
            {
                return NotFound();
            }

            return agentTourPackage;
        }

        // PUT: api/AgentTourPackages/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAgentTourPackage(int id, AgentTourPackage agentTourPackage)
        {
            if (id != agentTourPackage.TravelAgentId)
            {
                return BadRequest();
            }

            _context.Entry(agentTourPackage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AgentTourPackageExists(id))
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

        // POST: api/AgentTourPackages
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AgentTourPackage>> PostAgentTourPackage(AgentTourPackage agentTourPackage)
        {
          if (_context.AgentTourPackages == null)
          {
              return Problem("Entity set 'TravelTourDbContext.AgentTourPackages'  is null.");
          }
            _context.AgentTourPackages.Add(agentTourPackage);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AgentTourPackageExists(agentTourPackage.TravelAgentId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetAgentTourPackage", new { id = agentTourPackage.TravelAgentId }, agentTourPackage);
        }

        // DELETE: api/AgentTourPackages/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAgentTourPackage(int id)
        {
            if (_context.AgentTourPackages == null)
            {
                return NotFound();
            }
            var agentTourPackage = await _context.AgentTourPackages.FindAsync(id);
            if (agentTourPackage == null)
            {
                return NotFound();
            }

            _context.AgentTourPackages.Remove(agentTourPackage);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AgentTourPackageExists(int id)
        {
            return (_context.AgentTourPackages?.Any(e => e.TravelAgentId == id)).GetValueOrDefault();
        }
    }
}
