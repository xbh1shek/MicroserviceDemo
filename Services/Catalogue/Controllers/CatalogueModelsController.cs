using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Catalogue;
using Catalogue.Data;

namespace Catalogue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogueModelsController : ControllerBase
    {
        private readonly CatalogueContext _context;

        public CatalogueModelsController(CatalogueContext context)
        {
            _context = context;
        }

        // GET: api/CatalogueModels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CatalogueModel>>> GetCatalogueModel()
        {
          if (_context.CatalogueModel == null)
          {
              return NotFound();
          }
            return await _context.CatalogueModel.ToListAsync();
        }

        // GET: api/CatalogueModels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CatalogueModel>> GetCatalogueModel(int id)
        {
          if (_context.CatalogueModel == null)
          {
              return NotFound();
          }
            var catalogueModel = await _context.CatalogueModel.FindAsync(id);

            if (catalogueModel == null)
            {
                return NotFound();
            }

            return catalogueModel;
        }

        // PUT: api/CatalogueModels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCatalogueModel(int id, CatalogueModel catalogueModel)
        {
            if (id != catalogueModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(catalogueModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CatalogueModelExists(id))
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

        // POST: api/CatalogueModels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CatalogueModel>> PostCatalogueModel(CatalogueModel catalogueModel)
        {
          if (_context.CatalogueModel == null)
          {
              return Problem("Entity set 'CatalogueContext.CatalogueModel'  is null.");
          }
            _context.CatalogueModel.Add(catalogueModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCatalogueModel", new { id = catalogueModel.Id }, catalogueModel);
        }

        // DELETE: api/CatalogueModels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCatalogueModel(int id)
        {
            if (_context.CatalogueModel == null)
            {
                return NotFound();
            }
            var catalogueModel = await _context.CatalogueModel.FindAsync(id);
            if (catalogueModel == null)
            {
                return NotFound();
            }

            _context.CatalogueModel.Remove(catalogueModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CatalogueModelExists(int id)
        {
            return (_context.CatalogueModel?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
