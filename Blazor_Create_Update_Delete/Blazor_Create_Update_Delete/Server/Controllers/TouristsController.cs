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
    public class TouristsController : ControllerBase
    {
        private readonly TravelTourDbContext _context;
        private readonly IWebHostEnvironment env;
        public TouristsController(TravelTourDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            this.env = env;
        }

        // GET: api/Tourists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tourist>>> GetTourists()
        {
            if (_context.Tourists == null)
            {
                return NotFound();
            }
            return await _context.Tourists.ToListAsync();
        }

        // GET: api/Tourists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tourist>> GetTourist(int id)
        {
            if (_context.Tourists == null)
            {
                return NotFound();
            }
            var Tourists = await _context.Tourists.FindAsync(id);
            if (Tourists == null)
            {
                return NotFound();
            }

            return Tourists;
        }
        [HttpGet("Include")]
        public async Task<IEnumerable<TouristDTO>> GetTouristWithPackage()
        {
            var data = await this._context.Tourists.Include(x => x.TourPackages).
                Select(b => new TouristDTO
                {
                    TouristId = b.TouristId,
                    TouristName = b.TouristName,
                    BookingDate = b.BookingDate,
                    TouristOccupation = b.TouristOccupation,
                    TouristPicture = b.TouristPicture,
                    TourPackageId = b.TourPackageId,
                    PackageName = b.TourPackages.PackageName
                })
                .ToListAsync();
            return data;
        }
        // PUT: api/Tourists/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<Tourist>> PutTourist(int id, Tourist Tourist)
        {
            if (id != Tourist.TouristId)
            {
                return BadRequest();
            }

            _context.Entry(Tourist).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TouristExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Tourist;
        }

        // POST: api/Tourists
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Tourist>> PostTourist(Tourist Tourist)
        {
            if (_context.Tourists == null)
            {
                return Problem("Entity set 'TouristDbContext.Tourists'  is null.");
            }
            _context.Tourists.Add(Tourist);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTourist", new { id = Tourist.TouristId }, Tourist);
        }
        [HttpPost("Upload")]
        public async Task<ImageUploadResponse> Upload(IFormFile file)
        {
            var ext = Path.GetExtension(file.FileName);
            var randomFileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
            var storedFileName = randomFileName + ext;
            using FileStream fs = new FileStream(Path.Combine(env.WebRootPath, "Uploads", storedFileName), FileMode.Create);
            await file.CopyToAsync(fs);
            fs.Close();
            return new ImageUploadResponse { FileName = file.FileName, StoredFileName = storedFileName };
        }
        // DELETE: api/Tourists/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTourist(int id)
        {
            if (_context.Tourists == null)
            {
                return NotFound();
            }
            var Tourist = await _context.Tourists.FindAsync(id);
            if (Tourist == null)
            {
                return NotFound();
            }

            _context.Tourists.Remove(Tourist);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TouristExists(int id)
        {
            return (_context.Tourists?.Any(e => e.TouristId == id)).GetValueOrDefault();
        }
    }
}