using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Blazor_Create_Update_Delete.Shared.Models;
using Blazor_Create_Update_Delete.Shared.DTO;

namespace Blazor_Create_Update_Delete.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TravelAgentsController : ControllerBase
    {
        private readonly TravelTourDbContext _context;

        public TravelAgentsController(TravelTourDbContext context)
        {
            _context = context;
        }

        // GET: api/TravelAgents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TravelAgent>>> GetTravelAgents()
        {
          if (_context.TravelAgents == null)
          {
              return NotFound();
          }
            return await _context.TravelAgents.ToListAsync();
        }
        [HttpGet("DTO")]
        public async Task<ActionResult<IEnumerable<TravelAgentDTO>>> GetOrderDTOs()
        {
            if (_context.TravelAgents == null)
            {
                return NotFound();
            }
            return await _context.TravelAgents
                .Include(o => o.AgentTourPackages).ThenInclude(o=>o.TourPackages)
                .Select(o =>
                    new TravelAgentDTO
                    {
                        TravelAgentId = o.TravelAgentId,
                        AgentName = o.AgentName,
                        AgentAddress = o.AgentAddress,
                        Email = o.Email,
                    })
                .ToListAsync();
        }
        // GET: api/TravelAgents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TravelAgent>> GetTravelAgent(int id)
        {
          if (_context.TravelAgents == null)
          {
              return NotFound();
          }
            var travelAgent = await _context.TravelAgents.FindAsync(id);

            if (travelAgent == null)
            {
                return NotFound();
            }

            return travelAgent;
        }
        [HttpGet("DTO/{id}")]
        public async Task<ActionResult<TravelAgentViewDTO>> GetTravelAgentViewDTO(int id)
        {
            if (_context.TravelAgents == null)
            {
                return NotFound();
            }
            var travelagents = await _context.TravelAgents
                .Include(o => o.AgentTourPackages).ThenInclude(o => o.TourPackages).FirstOrDefaultAsync(o => o.TravelAgentId == id);

            if (travelagents == null)
            {
                return NotFound();
            }

            return new TravelAgentViewDTO
            {
                TravelAgentId = travelagents.TravelAgentId,
                AgentName = travelagents.AgentName,
                AgentAddress = travelagents.AgentAddress,
                Email = travelagents.Email,

            };
        }
        [HttpGet("Items/{id}")]
        public async Task<ActionResult<IEnumerable<AgentTourPackagesViewDTO>>> Getagenttourpackages(int id)
        {
            if (_context.AgentTourPackages == null)
            {
                return NotFound();
            }
            var AgentTourpackages = await _context.AgentTourPackages.Include(x => x.TourPackages).Where(oi => oi.TravelAgentId == id).ToListAsync();

            if (AgentTourpackages == null)
            {
                return NotFound();
            }

            return AgentTourpackages.Select(oi => new AgentTourPackagesViewDTO { TravelAgentId = oi.TravelAgentId, PackageName = oi.TourPackages.PackageName, CostPerPerson = oi.TourPackages.CostPerPerson }).ToList(); ;
        }
        [HttpGet("OI/{id}")]
        public async Task<ActionResult<IEnumerable<AgentTourPackage>>> GetAgentTourPackageOf(int id)
        {
            if (_context.AgentTourPackages == null)
            {
                return NotFound();
            }
            var agentTourPackages = await _context.AgentTourPackages.Where(oi => oi.TravelAgentId == id).ToListAsync();

            if (agentTourPackages == null)
            {
                return NotFound();
            }

            return agentTourPackages;
        }
        // PUT: api/TravelAgents/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTravelAgent(int id, TravelAgent travelAgent)
        {
            if (id != travelAgent.TravelAgentId)
            {
                return BadRequest();
            }

            _context.Entry(travelAgent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TravelAgentExists(id))
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
        [HttpPut("DTO/{id}")]
        public async Task<IActionResult> PutAgentTourPackage(int id, TravelAgentEditDTO travelagent)
        {
            if (id != travelagent.TravelAgentId)
            {
                return BadRequest();
            }
            var existing = await _context.TravelAgents.Include(x => x.AgentTourPackages).FirstAsync(o => o.TravelAgentId == id);
            _context.AgentTourPackages.RemoveRange(existing.AgentTourPackages);
            existing.TravelAgentId = travelagent.TravelAgentId;
            existing.AgentName = travelagent.AgentName;
            existing.Email = travelagent.Email;
            existing.AgentAddress = travelagent.AgentAddress;
            foreach (var item in travelagent.AgentTourPackages)
            {
                _context.AgentTourPackages.Add(new AgentTourPackage { TravelAgentId = travelagent.TravelAgentId, TourPackageId = item.TourPackageId });
            }


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.InnerException?.Message);

            }

            return NoContent();
        }

        // POST: api/TravelAgents
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TravelAgent>> PostTravelAgent(TravelAgent travelAgent)
        {
          if (_context.TravelAgents == null)
          {
              return Problem("Entity set 'TravelTourDbContext.TravelAgents'  is null.");
          }
            _context.TravelAgents.Add(travelAgent);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTravelAgent", new { id = travelAgent.TravelAgentId }, travelAgent);
        }
        [HttpPost("DTO")]
        public async Task<ActionResult<TravelAgent>> PostOrderDTO(TravelAgentDTO dto)
        {
            if (_context.TravelAgents == null)
            {
                return Problem("Entity set 'ProductDbContext.Orders'  is null.");
            }
            var travelagents = new TravelAgent { TravelAgentId = dto.TravelAgentId, AgentName = dto.AgentName, Email = dto.Email, AgentAddress = dto.AgentAddress };
            foreach (var oi in dto.AgentTourPackages)
            {
                travelagents.AgentTourPackages.Add(new AgentTourPackage { TourPackageId = oi.TourPackageId,});
            }
            _context.TravelAgents.Add(travelagents);
            await _context.SaveChangesAsync();

            return travelagents;
        }
        // DELETE: api/TravelAgents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTravelAgent(int id)
        {
            if (_context.TravelAgents == null)
            {
                return NotFound();
            }
            var travelAgent = await _context.TravelAgents.FindAsync(id);
            if (travelAgent == null)
            {
                return NotFound();
            }

            _context.TravelAgents.Remove(travelAgent);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TravelAgentExists(int id)
        {
            return (_context.TravelAgents?.Any(e => e.TravelAgentId == id)).GetValueOrDefault();
        }
    }
}
