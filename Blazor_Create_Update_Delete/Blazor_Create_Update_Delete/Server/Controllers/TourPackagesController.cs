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
    public class TourPackagesController : ControllerBase
    {
        private readonly TravelTourDbContext _context;

        public TourPackagesController(TravelTourDbContext context)
        {
            _context = context;
        }

        // GET: api/TourPackages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TourPackage>>> GetTourPackages()
        {
          if (_context.TourPackages == null)
          {
              return NotFound();
          }
            return await _context.TourPackages.ToListAsync();
        }
        [HttpGet("DTO")]
        public async Task<ActionResult<IEnumerable<TourPackageDTO>>> GetCustomerDTOs()
        {
            if (_context.TourPackages == null)
            {
                return NotFound();
            }
            return await _context
                .TourPackages.Include(x => x.Tourists)
                .Select(c => new TourPackageDTO
                {
                    TourPackageId = c.TourPackageId,
                    PackageCategory = c.PackageCategory,
                    PackageName = c.PackageName,
                    CostPerPerson = c.CostPerPerson,
                    TourTime = c.TourTime,
                    CanDelete = !c.Tourists.Any()
                })
                .ToListAsync();
        }
        // GET: api/TourPackages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TourPackage>> GetTourPackage(int id)
        {
          if (_context.TourPackages == null)
          {
              return NotFound();
          }
            var tourPackage = await _context.TourPackages.FindAsync(id);

            if (tourPackage == null)
            {
                return NotFound();
            }

            return tourPackage;
        }

        // PUT: api/TourPackages/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTourPackage(int id, TourPackage tourPackage)
        {
            if (id != tourPackage.TourPackageId)
            {
                return BadRequest();
            }

            _context.Entry(tourPackage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TourPackageExists(id))
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

        // POST: api/TourPackages
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TourPackage>> PostTourPackage(TourPackage tourPackage)
        {
          if (_context.TourPackages == null)
          {
              return Problem("Entity set 'TravelTourDbContext.TourPackages'  is null.");
          }
            _context.TourPackages.Add(tourPackage);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTourPackage", new { id = tourPackage.TourPackageId }, tourPackage);
        }

        // DELETE: api/TourPackages/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTourPackage(int id)
        {
            if (_context.TourPackages == null)
            {
                return NotFound();
            }
            var tourPackage = await _context.TourPackages.FindAsync(id);
            if (tourPackage == null)
            {
                return NotFound();
            }

            _context.TourPackages.Remove(tourPackage);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TourPackageExists(int id)
        {
            return (_context.TourPackages?.Any(e => e.TourPackageId == id)).GetValueOrDefault();
        }
    }
}
